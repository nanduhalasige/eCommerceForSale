using eCommerceForSale.Data.Data;
using eCommerceForSale.Data.Repositories.IRepositories;
using eCommerceForSale.Entity.Models;
using System.Linq;

namespace eCommerceForSale.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void softDelete(int id)
        {
            var CategoryObj = context.Categories.FirstOrDefault(x => x.Id.Equals(id));
            if (CategoryObj != null)
            {
                CategoryObj.IsActive = false;
            }
        }

        public void Update(Category category)
        {
            var CategoryObj = context.Categories.FirstOrDefault(x => x.Id.Equals(category.Id));
            if (CategoryObj != null)
            {
                CategoryObj.CategoryName = category.CategoryName;
                CategoryObj.IsActive = category.IsActive;
            }
        }
    }
}