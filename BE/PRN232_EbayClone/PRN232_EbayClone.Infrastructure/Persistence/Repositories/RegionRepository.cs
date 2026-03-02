using PRN232_EbayClone.Domain.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories
{
    public class RegionRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public RegionRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public Task<Region?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            // Implementation to get region by id from the database
            throw new NotImplementedException();
        }


    }
}
