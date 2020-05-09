using eCommerceForSale.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface IAddressRepository : IRepository<Address>
    {
        void Update(Address address);
    }
}