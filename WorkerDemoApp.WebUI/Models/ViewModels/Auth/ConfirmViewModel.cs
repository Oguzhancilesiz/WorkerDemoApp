using System.ComponentModel.DataAnnotations;

namespace WorkerDemoApp.WebUI.Models.ViewModels.Auth
{
    public class ConfirmViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = null!;
    }
}
