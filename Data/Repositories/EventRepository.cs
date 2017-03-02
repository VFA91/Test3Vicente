using Domain.Core.Respositories;
using Domain.Entities;

namespace Data.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(IDataFactory dataFactory) : base(dataFactory)
        {

        }
    }
}
