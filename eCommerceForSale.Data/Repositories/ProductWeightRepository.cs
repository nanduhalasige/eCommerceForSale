using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System.Linq;

namespace eCommerceForSale.Data.Repositories
{
    public class ProductWeightRepository : Repository<ProductWeight>, IProductWeightRepository
    {
        private readonly ApplicationDbContext context;

        public ProductWeightRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(ProductWeight productWeight)
        {
            var productWeightObj = context.ProductWeights.FirstOrDefault(x => x.Id.Equals(productWeight.Id));
            if (productWeightObj != null)
            {
                productWeightObj.Weight = productWeight.Weight;
                productWeightObj.WeightUnit = productWeight.WeightUnit;
            }
        }
    }
}