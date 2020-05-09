using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System;
using System.Linq;

namespace eCommerceForSale.Data.Repositories
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        private readonly ApplicationDbContext context;

        public CouponRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void softDelete(Guid id)
        {
            var couponObj = context.Coupon.FirstOrDefault(x => x.Id.Equals(id));
            if (couponObj != null)
            {
                couponObj.IsActive = false;
            }
        }

        public void Update(Coupon coupon)
        {
            var couponObj = context.Coupon.FirstOrDefault(x => x.Id.Equals(coupon.Id));
            if (couponObj != null)
            {
                couponObj.CouponCode = coupon.CouponCode;
                couponObj.ValidTill = coupon.ValidTill;
                couponObj.IsPercent = coupon.IsPercent;
                couponObj.Discount = coupon.Discount;
            }
        }
    }
}