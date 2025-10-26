// HomeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WorkerDemoApp.DAL;
using WorkerDemoApp.WebUI.Models;
using WorkerDemoApp.WebUI.Models.ViewModels.Dashboard;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BaseContext _db;

    public HomeController(ILogger<HomeController> logger, BaseContext db)
    {
        _logger = logger;
        _db = db;
    }
    /// <summary>
    /// kullanýcý ve doðrulama kodu metrikleri ile dashboard'ý gösterir
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        // Metrikler
        var totalUsers = await _db.Users.CountAsync();
        var confirmed = await _db.Users.CountAsync(u => u.EmailConfirmed);
        var unconfirmed = totalUsers - confirmed;

        var pendingCodes = await _db.VerificationCodes
            .CountAsync(v => v.UsedAtUtc == null);

        var totalReminders = await _db.VerificationCodes
            .SumAsync(v => (int?)v.ReminderCount) ?? 0;

        // Her kullanýcý için son code (varsa)
        var rows = await _db.Users
            .Select(u => new
            {
                u.Email,
                u.EmailConfirmed,
                u.CreatedDate,
                LastCode = _db.VerificationCodes
                    .Where(v => v.UserId == u.Id)
                    .OrderByDescending(v => v.CreatedDate)
                    .Select(v => new
                    {
                        v.Code,
                        v.ExpiresAtUtc,
                        v.UsedAtUtc,
                        v.LastReminderUtc,
                        v.ReminderCount
                    })
                    .FirstOrDefault()
            })
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();

        var vm = new DashboardVm
        {
            TotalUsers = totalUsers,
            ConfirmedUsers = confirmed,
            UnconfirmedUsers = unconfirmed,
            PendingCodes = pendingCodes,
            TotalRemindersSent = totalReminders,
            Rows = rows.Select(x => new DashboardRowVm
            {
                Email = x.Email ?? "",
                EmailConfirmed = x.EmailConfirmed,
                CreatedUtc = x.CreatedDate,
                Code = x.LastCode?.Code,
                ExpiresAtUtc = x.LastCode?.ExpiresAtUtc,
                UsedAtUtc = x.LastCode?.UsedAtUtc,
                LastReminderUtc = x.LastCode?.LastReminderUtc,
                ReminderCount = x.LastCode?.ReminderCount ?? 0
            }).ToList()
        };

        return View(vm);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
