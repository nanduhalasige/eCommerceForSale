using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerceForSale.Data.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository

    {
        private readonly ApplicationDbContext context;

        public CompanyRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(Company company)
        {
            var companyObj = context.Company.FirstOrDefault(x => x.Id.Equals(company.Id));
            if (companyObj != null)
            {
                companyObj.CompanyName = company.CompanyName;
                companyObj.CompanyEmail = company.CompanyEmail;
                companyObj.LogoPath = company.LogoPath;
                companyObj.OwenerName = company.OwenerName;
                companyObj.TaxNumber = company.TaxNumber;
            }
        }
    }
}