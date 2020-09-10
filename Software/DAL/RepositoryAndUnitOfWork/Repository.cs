using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Data.Entity;
using System.Linq.Expressions;


namespace DAL
{
    public class Repository<T> : System.Object, IRepository<T> where T : Models.BaseEntity
    {
        public Repository(DatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw (new System.ArgumentNullException("DatabaseCotext"));
            }
            DatabaseContext = databaseContext;
            DbSet = DatabaseContext.Set<T>();

        }
        private DbSet<T> DbSet { get; set; }
        private Models.DatabaseContext DatabaseContext { get; set; }

        public T GetById(Guid id)
        {
            T obj = DbSet.Find(id);
            return (obj);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw (new System.ArgumentNullException("Entity"));
            };
            entity.Id = Guid.NewGuid();
            entity.IsDeleted = false;
            entity.CreationDate = DateTime.Now;
            entity.LastModifiedDate = DateTime.Now;

            DbSet.Add(entity: entity);
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw (new ArgumentNullException("Entity"));
            };
          
            entity.IsDeleted = false;
            entity.LastModifiedDate = DateTime.Now;

            EntityState oEntityState = DatabaseContext.Entry(entity: entity).State;

            if (oEntityState == EntityState.Detached)
            {
                DbSet.Attach(entity: entity);
            }

            oEntityState = DatabaseContext.Entry(entity).State;
            DatabaseContext.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw (new ArgumentNullException("Entity"));
            };
            entity.IsActive = false;
            entity.IsDeleted = true;
            entity.LastModifiedDate = DateTime.Now;
            entity.DeletionDate = DateTime.Now;

            EntityState oEntityState = DatabaseContext.Entry(entity: entity).State;

            if (oEntityState == EntityState.Detached)
            {
                DbSet.Attach(entity: entity);
            }
            oEntityState = DatabaseContext.Entry(entity).State;
            DatabaseContext.Entry(entity).State = EntityState.Modified;
        }
        public void DeletePhysical(T entity)
        {
            if (entity == null)
            {
                throw (new ArgumentNullException("entity"));
            }
            EntityState oEntityState = DatabaseContext.Entry(entity: entity).State;

            if (oEntityState == EntityState.Detached)
            {
                DbSet.Attach(entity: entity);
            }
            oEntityState = DatabaseContext.Entry(entity: entity).State;
            DbSet.Remove(entity: entity);

            oEntityState = DatabaseContext.Entry(entity).State;
        }
        public bool DeleteById(Guid id)
        {
            T entity = GetById(id: id);
            if (entity == null)
            {
                return (false);
            };
            entity.IsDeleted = true;
            entity.LastModifiedDate = DateTime.Now;
            entity.DeletionDate = DateTime.Now;
            entity.IsActive = false;

            EntityState oEntityState = DatabaseContext.Entry(entity: entity).State;
       

            if (oEntityState == EntityState.Detached)
            {
                DbSet.Attach(entity: entity);
            }
            oEntityState =
                         DatabaseContext.Entry(entity).State;
            DatabaseContext.Entry(entity).State = EntityState.Modified;
            return (true);

        }
        public bool DeleteByIdPhysically(Guid id)
        {
            T oEntity = GetById(id: id);
            if (oEntity == null)
            {
                return (false);
            }
            else
            {
                Delete(entity: oEntity);
                return (true);
            }
        }
        public IQueryable<T> Get()
        {
                return (DbSet.Where(current => current.IsDeleted == false).OrderByDescending(current => current.CreationDate));
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return (DbSet.Where(predicate: predicate).Where(current => current.IsDeleted == false).OrderByDescending(current => current.CreationDate));
        }
        public IEnumerable<T> GetWithRawSql
            (string query, params object[] parameters)
        {
            return (DbSet.SqlQuery(sql: query, parameters: parameters).OrderByDescending(current => current.CreationDate).ToList());
        }

        public IQueryable<T> GetWithDeleted()
        {
            return (DbSet);
        }
        //public IQueryable<T> Include(Expression<Func<T, bool>> predicate)
        //{
        //    return (DbSet.Include(current=> predicate).Where(current => current.IsDeleted == false).OrderByDescending(current => current.CreationDate));
        //}

        public IQueryable<T> Include(Expression<Func<T, object>> criteria) 
        {
            return DbSet.Include(criteria);
        }
    }
}
