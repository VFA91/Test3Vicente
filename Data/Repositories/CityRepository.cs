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
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(IDataFactory dataFactory) : base(dataFactory)
        {

        }
    }
}
