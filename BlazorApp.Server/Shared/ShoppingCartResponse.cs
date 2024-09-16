using BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class ShoppingCartResponse
    {
         public decimal Total { get; set; }
        public int ItemsCount { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
