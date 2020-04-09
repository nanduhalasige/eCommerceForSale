using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceForSale.Data.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string isIncludeProperties = null
            );

        T GetFirstOfDefault(
            Expression<Func<T, bool>> filter = null,
            string isIncludeProperties = null
            );

        void Add(T entity);

        void AddRange(IQueryable<T> entities);

        void Remove(int id);

        void RemoveRange(IQueryable<T> entities);
    }
}