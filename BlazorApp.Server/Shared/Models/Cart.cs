using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlazorApp.Models;

namespace BlazorApp.Models
{

    public class Cart
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; } 
        public UserData User { get; set; }

        public decimal Total
        {
            get
            {
                return CartItems.Sum(item => item.Price * item.Quantity);
            }
        }

    }

}
