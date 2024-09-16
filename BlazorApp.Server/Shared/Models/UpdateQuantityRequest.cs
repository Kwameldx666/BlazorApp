using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models
{
    public class UpdateQuantityRequest
    {
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
    }
}
