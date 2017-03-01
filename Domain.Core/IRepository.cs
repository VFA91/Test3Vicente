using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Core
{
    public interface IRepository<TModel> where TModel : class
    {
        IQueryable<TModel> GetAll();
        IQueryable<TModel> GetAllAsNoTracking();
        void Add(TModel entityToAdd);
        void Update(TModel entityToUpdate);
        void Delete(TModel entityToDelete);
        void Delete(Expression<Func<TModel, bool>> where);
        TModel GetById(long id);
        TModel GetById(string id);
        TModel Get(Expression<Func<TModel, bool>> where);
        IQueryable<TModel> GetMany(Expression<Func<TModel, bool>> where);

    }
}
