namespace WorkerDemoApp.WebUI.Models.ViewModels.Dashboard
{
    public class DashboardRowVm
    {
        public string Email { get; set; } = "";
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedUtc { get; set; }

        public string? Code { get; set; }
        public DateTime? ExpiresAtUtc { get; set; }
        public DateTime? UsedAtUtc { get; set; }
        public DateTime? LastReminderUtc { get; set; }
        public int ReminderCount { get; set; }
    }
}
