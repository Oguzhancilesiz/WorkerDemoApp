using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDemoApp.Core.Abstracts;
using WorkerDemoApp.Entity;
using WorkerDemoApp.Services.Abstracts;

namespace WorkerDemoApp.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _email;

        public AuthService(UserManager<AppUser> userManager, IUnitOfWork uow, IEmailService email)
        {
            _userManager = userManager;
            _uow = uow;
            _email = email;
        }

        public async Task<(bool Ok, string? Error)> RegisterAsync(string email, string password, CancellationToken ct)
        {
            var exists = await _userManager.Users.AnyAsync(x => x.Email == email, ct);
            if (exists) return (false, "Email already registered.");

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                EmailConfirmed = false,
                Status = Core.Enums.Status.Active,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

            // 6 haneli kod
            var code = GenerateCode(6);
            var vc = new VerificationCode
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Code = code,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(2),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = Core.Enums.Status.Active
            };

            await _uow.Repository<VerificationCode>().AddAsync(vc);
            await _uow.SaveChangesAsync();

            await _email.SendAsync(email, "Hesabınızı Doğrulayın",
                $"<p>Doğrulama kodunuz: <b>{code}</b></p><p>Bu kod 2 dakika geçerlidir.</p>", ct);

            return (true, null);
        }

        public async Task<(bool Ok, string? Error)> ConfirmAsync(string email, string code, CancellationToken ct)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
            if (user is null) return (false, "User not found.");

            var repo = _uow.Repository<VerificationCode>();
            var vc = (await repo.GetBy(x => x.UserId == user.Id && x.Status == Core.Enums.Status.Active))
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault();

            if (vc is null) return (false, "No verification request.");
            if (vc.UsedAtUtc != null) return (false, "Already confirmed.");
            if (vc.ExpiresAtUtc < DateTime.UtcNow) return (false, "Code expired.");
            if (!string.Equals(vc.Code, code, StringComparison.Ordinal))
                return (false, "Invalid code.");

            vc.UsedAtUtc = DateTime.UtcNow;
            await repo.Update(vc);
            await _uow.SaveChangesAsync();

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return (true, null);
        }

        // Worker çağırır: 10 dakikada bir hatırlatma
        public async Task<int> SendRemindersAsync(CancellationToken ct)
        {
            var vcRepo = _uow.Repository<VerificationCode>();
            var now = DateTime.UtcNow;

            // Kullanıcı e-maili onaylanmamış + kodu hiç kullanılmamış + reminder aralığı 10 dk dolmuş
            var q = await vcRepo.GetBy(x =>
                x.Status == Core.Enums.Status.Active &&
                x.UsedAtUtc == null);

            // include user
            var pending = await q.Include(x => x.User)
                                 .Where(x => x.User.EmailConfirmed == false &&
                                             (x.LastReminderUtc == null || x.LastReminderUtc <= now.AddMinutes(-10)))
                                 .ToListAsync(ct);

            int sent = 0;
            foreach (var item in pending)
            {
                // nazik ama net spam: 10 dk’da bir
                await _email.SendAsync(item.User.Email!, "Hesabınızı Doğrulamadınız",
                    "<p>Hesabınızı henüz doğrulamadınız. Lütfen uygulamadaki doğrulama kodu ekranından işlemi tamamlayın.</p>",
                    ct);

                item.LastReminderUtc = now;
                await vcRepo.Update(item);
                sent++;
            }

            if (sent > 0) await _uow.SaveChangesAsync();
            return sent;
        }

        static string GenerateCode(int len)
        {
            var r = new Random();
            return string.Concat(Enumerable.Range(0, len).Select(_ => r.Next(0, 10))).PadLeft(len, '0');
        }
    }
}
