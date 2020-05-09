using System;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ISPRepository StoredProc { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ICompanyRepository Company { get; }
        IPriceForWeightRepository PriceForWeight { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IProductWeightRepository ProductWeights { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeader { get; }
        ICouponRepository Coupon { get; }

        IAddressRepository Address { get; }

        void Save();
    }
}