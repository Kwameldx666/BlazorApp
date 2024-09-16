using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public  class CartItemDto
    {
        public Guid CartId { get; set; }
        public Guid DishId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
