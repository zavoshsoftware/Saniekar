using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductRequestStatusRepository : Repository<Models.ProductRequestStatus>, IProductRequestStatusRepository
    {
        public ProductRequestStatusRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
