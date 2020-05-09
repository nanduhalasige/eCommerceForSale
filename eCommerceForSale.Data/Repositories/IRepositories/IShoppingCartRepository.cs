using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart cart);
    }
}