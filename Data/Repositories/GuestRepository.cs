using Domain.Core.Respositories;
using Domain.Entities;

namespace Data.Repositories
{
    public class GuestRepository : RepositoryBase<Guest>, IGuestRepository
    {
        public GuestRepository(IDataFactory dataFactory) : base(dataFactory)
        {

        }
    }
}
