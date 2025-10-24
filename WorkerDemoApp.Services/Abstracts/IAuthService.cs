using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDemoApp.Services.Abstracts
{
    public interface IAuthService
    {
        Task<(bool Ok, string? Error)> RegisterAsync(string email, string password, CancellationToken ct);
        Task<(bool Ok, string? Error)> ConfirmAsync(string email, string code, CancellationToken ct);
        Task<int> SendRemindersAsync(CancellationToken ct); // worker çağırır
    }
}
