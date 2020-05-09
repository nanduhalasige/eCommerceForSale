using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerceForSale.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void softDelete(Guid id)
        {
            var ProductObj = context.Products.FirstOrDefault(x => x.Id.Equals(id));
            if (ProductObj != null)
            {
                ProductObj.IsActive = false;
                context.Update(ProductObj);
            }
        }

        public void Update(Product product)
        {
            var ProductObj = context.Products.FirstOrDefault(x => x.Id.Equals(product.Id));
            if (ProductObj != null)
            {
                ProductObj.ProductName = product.ProductName;
                ProductObj.IsActive = product.IsActive;
                ProductObj.ImagePath = product.ImagePath;
                ProductObj.IsBoxOrPack = product.IsBoxOrPack;
                ProductObj.Price = product.Price;
                ProductObj.Stock = product.Stock;
                ProductObj.Desciption = product.Desciption;
                //context.Update(product);
            }
        }
    }
}