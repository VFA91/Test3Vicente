using Domain.Core.Respositories;
using Domain.Entities;

namespace Data.Repositories
{
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(IDataFactory dataFactory) : base(dataFactory)
        {

        }
    }
}
