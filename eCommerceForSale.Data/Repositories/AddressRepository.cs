using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System.Linq;

namespace eCommerceForSale.Data.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly ApplicationDbContext context;

        public AddressRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(Address address)
        {
            var addressObj = context.Addresses.FirstOrDefault(x => x.Id.Equals(address.Id));
            if (addressObj != null)
            {
                addressObj.FullName = addressObj.FullName;
                addressObj.AddressLine1 = addressObj.AddressLine1;
                addressObj.AddressLine2 = addressObj.AddressLine2;
                addressObj.PostCode = addressObj.AddressLine1;
                addressObj.MobileNumber = addressObj.MobileNumber;
                addressObj.IsDefault = addressObj.IsDefault;
            }
        }
    }
}