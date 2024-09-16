using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class RegisterData
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Имя должно содержать от 3 до 100 символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must agree to the terms")]
        [Compare(nameof(TrueValue), ErrorMessage = "You must agree to the terms")]
        public bool AgreedToTerms { get; set; }

        public bool TrueValue { get; } = true;

    }
}

