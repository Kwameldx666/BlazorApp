using BlazorApp.DbModel;
using BlazorApp.Models;
using BlazorApp.Interfaces;

namespace BlazorApp.Factory
{
    // Abstract class defining a factory for creating cart services
    public abstract class CartFactory
    {
        // Database context to be used by the cart services
        protected readonly ApplicationDbContext _applicationDbContext;

        // Constructor initializing the database context
        protected CartFactory(ApplicationDbContext dbContext)
        {
            _applicationDbContext = dbContext;
        }

        // Abstract method for creating a cart service
        public abstract ICartService CreateCart();
    }

    // Factory class for creating VIP user cart services
    public class VipFactoryCart : CartFactory
    {
        // Constructor initializing the database context
        public VipFactoryCart(ApplicationDbContext dbContext) : base(dbContext) { }

        // Method to create and return a VIP user cart service
        public override ICartService CreateCart()
        {
            return new VIPUserCartService(_applicationDbContext);
        }
    }

    // Factory class for creating regular user cart services
    public class RegularUserCart : CartFactory
    {
        // Constructor initializing the database context
        public RegularUserCart(ApplicationDbContext dbContext) : base(dbContext) { }

        // Method to create and return a regular user cart service
        public override ICartService CreateCart()
        {
            return new RegularUserCartService(_applicationDbContext);
        }
    }
}
