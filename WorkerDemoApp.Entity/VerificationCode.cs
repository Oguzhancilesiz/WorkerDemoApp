using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDemoApp.Core.Enums;

namespace WorkerDemoApp.Entity
{
    public class VerificationCode : BaseEntity
    {
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public string Code { get; set; }        // 6 haneli
        public VerificationType Type { get; set; } = VerificationType.EmailConfirm;
        public DateTime ExpiresAtUtc { get; set; }       // +2 dakika
        public DateTime? UsedAtUtc { get; set; }         // null ise kullanılmamış
        public DateTime? LastReminderUtc { get; set; }   // 10 dk spam koruması
    }
}
