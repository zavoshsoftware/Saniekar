using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductRequestDetailRepository : Repository<Models.ProductRequestDetail>, IProductRequestDetailRepository
    {
        public ProductRequestDetailRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
