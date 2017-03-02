using Data.Model;
using Domain.Core;
using System;
using System.Data.Entity;
using System.Linq;

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

        public virtual TEntity GetById(long id)
        {
            return _dbset.Find(id);
        }

        public virtual TEntity GetById(string id)
        {
            return _dbset.Find(id);
        }

        public void Dispose()
        {

        }
    }
}
