using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

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
        }

        public ICategoryRepository Category { get; private set; }
        public ISPRepository StoredProc { get; private set; }

        public IProductRepository Product { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public ICompanyRepository Company { get; private set; }

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