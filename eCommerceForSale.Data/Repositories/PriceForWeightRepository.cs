using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System.Collections.Generic;
using System.Linq;

namespace eCommerceForSale.Data.Repositories
{
    public class PriceForWeightRepository : Repository<PriceForWeight>, IPriceForWeightRepository
    {
        private readonly ApplicationDbContext context;

        public PriceForWeightRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(List<PriceForWeight> priceForWeight)
        {
            foreach (var productWeight in priceForWeight)
            {
                var priceForWeightObj = context.PriceForWeights.FirstOrDefault(x => x.Id.Equals(productWeight.Id));
                if (priceForWeightObj != null)
                {
                    priceForWeightObj.Price = productWeight.Price;
                    priceForWeightObj.Stock = productWeight.Stock;
                }
            }
        }
    }
}