using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface IPriceForWeightRepository : IRepository<PriceForWeight>
    {
        void Update(List<PriceForWeight> priceForWeight);
    }
}