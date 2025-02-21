using BlazorApp.Models.Enums;
using BlazorApp.Models;

namespace BlazorApp.Server.Builder
{
    public class Director
    {
        public Dish CreateDefaultDish()
        {
            return new DishBuilder()
                .SetName("Standard Dish")
                .SetDescription("Default Description")
                .SetPrice(9.99m)
                .SetCategory("General")
                .SetImageUrl("default.jpg")
                .Build();
        }

        public Cart CreateCartWithItem()
        {
            var user = new UserDataBuilder()
                .SetUsername("JohnDoe")
                .SetEmail("john@example.com")
                .Build();

            return new CartBuilder()
                .SetUser(user)
                .AddCartItem(new CartItem { DishId = Guid.NewGuid(), Name = "Burger", Price = 5.99m, Quantity = 2 })
                .Build();
        }

        public UserData CreateAdminUser()
        {
            return new UserDataBuilder()
                .SetUsername("AdminUser")
                .SetEmail("admin@example.com")
                .SetPassword("admin123")
                .SetRole(Role.Admin)
                .Build();
        }
    }
}
