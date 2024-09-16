using BlazorApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class PaymentDetails
    {
        [Required(ErrorMessage = "Card number is required")]
        [CreditCard(ErrorMessage = "Invalid card number format")]
        [StringLength(16, MinimumLength = 13, ErrorMessage = "Card number should be between 13 and 16 digits")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Card holder name is required")]
        [StringLength(100, ErrorMessage = "Card holder name is too long")]
        public string CardHolderName { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Invalid expiry date format (MM/YY)")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Invalid CVV format")]
        public string CVV { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than zero")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; } = "USD";
    }
}
