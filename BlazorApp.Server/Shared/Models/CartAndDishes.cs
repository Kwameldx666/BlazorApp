using BlazorApp.Models;

namespace BlazorApp.Models
{
    public class CartAndDishes
    {
        public IEnumerable<Dish> dish { get; set; }
        public Cart cart { get; set; }
    }
}
