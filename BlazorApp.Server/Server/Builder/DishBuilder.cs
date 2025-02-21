using BlazorApp.Models;

namespace BlazorApp.Server.Builder
{
    public class DishBuilder : IBuilder<Dish>
    {
        private Dish _dish = new();
        public void Reset()
        {
            _dish = new Dish();
        }

        public DishBuilder SetName(string name)
        {
            _dish.Name = name;
            return this;
        }

        public DishBuilder SetDescription(string description)
        {
            _dish.Description = description;
            return this;
        }

        public DishBuilder SetPrice(decimal price)
        {
            _dish.Price = price;
            return this;
        }

        public DishBuilder SetCategory(string category)
        {
            _dish.Category = category;
            return this;
        }

        public DishBuilder SetImageUrl(string imageUrl)
        {
            _dish.ImageUrl = imageUrl;
            return this;
        }

        public Dish Build()
        {
            _dish.CreatedAt = DateTime.Now;
            _dish.UpdatedAt = DateTime.Now;
            var result = _dish;
            Reset(); // Сбрасываем состояние строителя
            return result;

          
        }

    }
}
