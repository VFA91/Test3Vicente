using Domain.Core.Respositories;
using Domain.Entities;

namespace Data.Repositories
{
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(IDataFactory dataFactory) : base(dataFactory)
        {

        }
    }
}
