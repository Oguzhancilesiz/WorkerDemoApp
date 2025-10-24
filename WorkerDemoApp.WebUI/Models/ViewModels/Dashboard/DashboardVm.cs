namespace WorkerDemoApp.WebUI.Models.ViewModels.Dashboard
{
    public class DashboardVm
    {
        public int TotalUsers { get; set; }
        public int ConfirmedUsers { get; set; }
        public int UnconfirmedUsers { get; set; }
        public int PendingCodes { get; set; }
        public int TotalRemindersSent { get; set; }
        public List<DashboardRowVm> Rows { get; set; } = new();
    }
}
