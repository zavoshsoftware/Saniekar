using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : Models.BaseEntity
    {
        T GetById(System.Guid id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeletePhysical(T entity);
        bool DeleteById(System.Guid id);
        bool DeleteByIdPhysically(System.Guid id);
        System.Linq.IQueryable<T> Get();
        System.Linq.IQueryable<T> Get
            (System.Linq.Expressions.Expression<System.Func<T, bool>> predicate);
        System.Collections.Generic.IEnumerable<T> GetWithRawSql
            (string query, params object[] parameters);
        System.Linq.IQueryable<T> GetWithDeleted();

        System.Linq.IQueryable<T> Include(System.Linq.Expressions.Expression<System.Func<T, object>> criteria);
    }
}
