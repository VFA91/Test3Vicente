using Data.Model;
using Domain.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    /// <summary>
    /// Repositorio base de la base de datos
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        private EventManagementEntities _mainContext;
        private readonly IDbSet<TEntity> _dbset;

        protected IDataFactory DataFactory
        {
            get;
            private set;
        }
        protected EventManagementEntities MainContext
        {
            get { return _mainContext ?? (_mainContext = DataFactory.GetMainContext()); }
        }
        protected RepositoryBase(IDataFactory dataFactory)
        {
            DataFactory = dataFactory;
            _dbset = MainContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbset.AsNoTracking().AsQueryable();
        }

        public virtual IQueryable<TEntity> GetAllAsNoTracking()
        {
            return _dbset.AsQueryable();
        }

        public virtual void Add(TEntity entityToAdd)
        {
            _dbset.Add(entityToAdd);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbset.Attach(entityToUpdate);
            _mainContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            _dbset.Remove(entityToDelete);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> objects = _dbset.Where<TEntity>(where).AsEnumerable();
            foreach (TEntity obj in objects)
                _dbset.Remove(obj);
        }

        public virtual TEntity GetById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual TEntity GetById(string id)
        {
            return _dbset.Find(id);
        }

        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return _dbset.AsNoTracking().Where(where).AsQueryable();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault<TEntity>();
        }
        public void Dispose()
        {

        }
    }
}
