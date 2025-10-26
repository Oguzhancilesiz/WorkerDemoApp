using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDemoApp.Services.Abstracts
{
    public interface IEmailService
    {
        /// <summary>
        /// sistemden email gönderir
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    }
}
