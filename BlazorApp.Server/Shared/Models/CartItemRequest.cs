using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models
{
    public class CartItemRequest
    {
        public Guid DishId { get; set; }
        public string DishName { get; set; }
        public decimal DishPrice { get; set; }
        public int Quantity { get; set; }

        public ICollection<CartItem>? CartItems { get; set; }
    }
}
