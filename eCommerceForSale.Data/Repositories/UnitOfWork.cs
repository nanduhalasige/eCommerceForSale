using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;

namespace eCommerceForSale.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext _context)
        {
            context = _context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            StoredProc = new SPRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
            Company = new CompanyRepository(context);
            PriceForWeight = new PriceForWeightRepository(context);
            ShoppingCart = new ShoppingCartRepository(context);
            ProductWeights = new ProductWeightRepository(context);
            OrderDetails = new OrderDetailsRepository(context);
            OrderHeader = new OrderHeaderRepository(context);
            Coupon = new CouponRepository(context);
            Address = new AddressRepository(context);
        }

        public IAddressRepository Address { get; private set; }

        public ICategoryRepository Category { get; private set; }
        public ISPRepository StoredProc { get; private set; }

        public IProductRepository Product { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public IPriceForWeightRepository PriceForWeight { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }

        public IProductWeightRepository ProductWeights { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public ICouponRepository Coupon { get; private set; }

        public void Dispose()
        {
            context.Dispose();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}