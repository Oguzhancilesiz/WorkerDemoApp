using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDemoApp.Services.Abstracts;

namespace WorkerDemoApp.Worker
{
    /// <summary>
    /// e-mail hatırlatma servis
    /// </summary>
    public class EmailReminderWorker : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<EmailReminderWorker> _logger;

        public EmailReminderWorker(IServiceProvider sp, ILogger<EmailReminderWorker> logger)
        {
            _sp = sp;
            _logger = logger;
        }
        /// <summary>
        /// dakika başı hatırlatma e-postası gönderir token stop edilene kadar
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmailReminderWorker started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _sp.CreateScope();
                    var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();
                    var sent = await auth.SendRemindersAsync(stoppingToken);
                    if (sent > 0)
                        _logger.LogInformation("Reminder emails sent: {Count}", sent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Reminder loop failed");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

}
