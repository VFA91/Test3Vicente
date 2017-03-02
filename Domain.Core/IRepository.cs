using System.Linq;

namespace Domain.Core
{
    public interface IRepository<TModel> where TModel : class
    {
        IQueryable<TModel> GetAll();
        void Add(TModel entityToAdd);
        void Update(TModel entityToUpdate);
        void Delete(TModel entityToDelete);
        TModel GetById(long id);
        TModel GetById(string id);
    }
}
