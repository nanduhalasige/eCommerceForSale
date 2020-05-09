using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System.Linq;

namespace eCommerceForSale.Data.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            var shoppingCartObj = context.ShoppingCarts.FirstOrDefault(x => x.Id.Equals(shoppingCart.Id));
            if (shoppingCartObj != null)
            {
                shoppingCartObj.Count = shoppingCart.Count;
                shoppingCartObj.PriceForWeight = shoppingCart.PriceForWeight;
            }
        }
    }
}