using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkerDemoApp.Entity;
using WorkerDemoApp.Services.Abstracts;
using WorkerDemoApp.WebUI.Models.ViewModels.Auth;

namespace WorkerDemoApp.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService auth, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _auth = auth;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        /// <summary>
        /// mesaj tipi ve içeriğini TempData'ya ekler
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        private void Alert(string type, string message)
        {
            TempData["MessageType"] = type;
            TempData["Message"] = message;
        }
        /// <summary>
        /// register sayfası
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register() => View();
        /// <summary>
        /// kayıt işlemi yapılır ve doğrulama kodu e-posta olarak gönderilir
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                Alert("warning", "Lütfen gerekli alanları doldurun.");
                return View(model);
            }

            var (ok, err) = await _auth.RegisterAsync(model.Email, model.Password, ct);
            if (!ok)
            {
                Alert("error", err ?? "Kayıt başarısız.");
                return View(model);
            }

            Alert("success", "Doğrulama kodu e-postanıza gönderildi.");
            return RedirectToAction(nameof(Confirm), new { email = model.Email });
        }
        /// <summary>
        /// confirm sayfası e-posta ile doğrulama kodu girilir
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Confirm(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                Alert("warning", "E-posta adresi eksik.");
                return RedirectToAction(nameof(Register));
            }
            return View(new ConfirmViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                Alert("warning", "Lütfen kodu girin.");
                return View(model);
            }

            var (ok, err) = await _auth.ConfirmAsync(model.Email, model.Code, ct);
            if (!ok)
            {
                Alert("error", err ?? "Doğrulama başarısız.");
                return View(model);
            }

            Alert("success", "Hesabınız doğrulandı. Şimdi giriş yapabilirsiniz.");
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login() => View();
        /// <summary>
        /// login işlemi yapılır ve kullanıcı ana sayfaya yönlendirilir
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Alert("warning", "Lütfen e-posta ve şifre girin.");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                Alert("error", "Kullanıcı bulunamadı.");
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                Alert("warning", "Lütfen önce e-posta doğrulaması yapın.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (!result.Succeeded)
            {
                Alert("error", "Geçersiz giriş bilgisi.");
                return View(model);
            }

            Alert("success", "Giriş başarılı, hoş geldiniz!");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Alert("info", "Oturum sonlandırıldı.");
            return RedirectToAction(nameof(Login));
        }
    }
}
