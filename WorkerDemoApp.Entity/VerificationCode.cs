using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDemoApp.Core.Enums;

namespace WorkerDemoApp.Entity
{
    /// <summary>
    /// verification code entity
    /// </summary>
    public class VerificationCode : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public string Code { get; set; }       
        public VerificationType Type { get; set; } = VerificationType.EmailConfirm;
        public DateTime ExpiresAtUtc { get; set; }   
        public DateTime? UsedAtUtc { get; set; }     
        public DateTime? LastReminderUtc { get; set; }

        public int ReminderCount { get; set; } = 0;
        public DateTime? FirstReminderUtc { get; set; }
    }
}
