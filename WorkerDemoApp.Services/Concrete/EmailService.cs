using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDemoApp.Core.Extensions;
using WorkerDemoApp.Services.Abstracts;

namespace WorkerDemoApp.Services.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _opt;
        public EmailService(IOptions<SmtpOptions> opt) { _opt = opt.Value; }
        /// <summary>
        /// verilen parametrelere göre email gönderir
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
        {
            using var msg = MailHelper.MailCreate(new[] { to }, subject, htmlBody);
            await MailHelper.SendAsync(msg, _opt, ct);
        }
    }
}
