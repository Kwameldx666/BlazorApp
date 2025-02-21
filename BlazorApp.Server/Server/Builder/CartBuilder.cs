using BlazorApp.Models;

namespace BlazorApp.Server.Builder
{
    public class CartBuilder : IBuilder<Cart>
    {
        private Cart _cart = new()
        {
            CartId = Guid.NewGuid(),
            CartItems = new List<CartItem>()
        };

        public CartBuilder SetUserId(Guid userId)
        {
            _cart.UserId = userId;
            return this;
        }

        public CartBuilder SetUser(UserData user)
        {
            _cart.User = user;
            return this;
        }

        public CartBuilder AddCartItem(CartItem item)
        {
            _cart.CartItems.Add(item);
            return this;
        }

        public Cart Build()
        {
            var result = _cart;
            Reset(); // Сброс строителя после создания объекта
            return result;
        }

        public void Reset()
        {
            _cart = new Cart { CartId = Guid.NewGuid(), CartItems = new List<CartItem>() };
        }
    }
}
