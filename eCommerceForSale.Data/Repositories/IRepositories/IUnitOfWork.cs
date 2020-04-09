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

        void Save();
    }
}