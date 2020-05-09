using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        void Update(Coupon coupon);

        void softDelete(Guid id);
    }
}