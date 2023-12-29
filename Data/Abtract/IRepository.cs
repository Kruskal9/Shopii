using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abtract
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }

        Task Commit();
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> expression);
        void Delete(IEnumerable<T> entities);
        Task<T> GetById(object id);
        Task<IEnumerable<T>> GetData(Expression<Func<T, bool>> expression = null);
        Task Insert(IEnumerable<T> entities);
        Task<T> Insert(T entity);
        void Update(T entity);
    }
}
