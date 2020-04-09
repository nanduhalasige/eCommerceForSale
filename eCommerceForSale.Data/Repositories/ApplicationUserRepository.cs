using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eCommerceForSale.Data.Repositories
{
    internal class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository

    {
        private readonly ApplicationDbContext context;

        public ApplicationUserRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void softDelete(int id)
        {
            var CategoryObj = context.Categories.FirstOrDefault(x => x.Id.Equals(id));
            if (CategoryObj != null)
            {
                CategoryObj.IsActive = false;
                context.Update(CategoryObj);
            }
        }

        //public void Update(ApplicationUser applicationUser)
        //{
        //    var CategoryObj = context.Categories.FirstOrDefault(x => x.Id.Equals(applicationUser.Id));
        //    if (CategoryObj != null)
        //    {
        //        CategoryObj.CategoryName = applicationUser.CategoryName;
        //        CategoryObj.IsActive = applicationUser.IsActive;
        //        context.Update(CategoryObj);
        //    }
        //}
    }
}