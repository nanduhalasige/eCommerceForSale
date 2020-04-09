using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        //void Update(ApplicationUser applicationUser);

        void softDelete(int id);
    }
}