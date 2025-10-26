using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDemoApp.Services.Abstracts
{
    public interface IAuthService
    {
        /// <summary>
        /// sisteme kayıt olur
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<(bool Ok, string? Error)> RegisterAsync(string email, string password, CancellationToken ct);
        /// <summary>
        /// aktivasyon kodu ile email doğrulaması yapar
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<(bool Ok, string? Error)> ConfirmAsync(string email, string code, CancellationToken ct);
        /// <summary>
        /// worker tarafından çağrılır
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<int> SendRemindersAsync(CancellationToken ct);
    }
}
