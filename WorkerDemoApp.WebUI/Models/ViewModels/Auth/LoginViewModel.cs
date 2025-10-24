using System.ComponentModel.DataAnnotations;

namespace WorkerDemoApp.WebUI.Models.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
