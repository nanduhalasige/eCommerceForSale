using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);

        void softDelete(Guid id);
    }
}