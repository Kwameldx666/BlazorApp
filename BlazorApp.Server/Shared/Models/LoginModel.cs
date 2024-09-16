using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Write name or Email")]
        public string Credential { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
