using BlazorApp.Models;

namespace BlazorApp.Models.Response
{
    public class DishResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Dish Dish { get; set; }
    }
}
