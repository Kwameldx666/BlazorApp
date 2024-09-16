using System;
using System.ComponentModel.DataAnnotations;
using BlazorApp.Models.Enums;

namespace BlazorApp.Models
{
    public class UserData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); // Set default value if not provided

        public Guid CartId { get; set; }
        public virtual Cart Cart { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public Role Roles { get; set; }

        public DateTime FirstRegisterTime { get; set; } = DateTime.Now;

        public DateTime FirstLoginTime { get; set; }

        public string? IP { get; set; }

        public string? CookieValue { get; set; }

        public string? Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        public Guid DeliveryId { get; set; }
        public virtual DeliveryAddress Delivery { get; set; }
    }

}
