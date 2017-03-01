using Domain.Core;
using Domain.Core.Respositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(IDataFactory dataFactory) : base(dataFactory)
        {

        }
    }
}
