using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System.Linq;

namespace eCommerceForSale.Data.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext context;

        public OrderHeaderRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(OrderHeader orderHeader)
        {
            var orderHeaderObj = context.OrderHeaders.FirstOrDefault(x => x.Id.Equals(orderHeader.Id));
            if (orderHeaderObj != null)
            {
                orderHeaderObj.Carrier = orderHeader.Carrier;
                orderHeaderObj.TrackingNumber = orderHeader.TrackingNumber;
                orderHeaderObj.OrderStatus = orderHeader.OrderStatus;
            }
        }
    }
}