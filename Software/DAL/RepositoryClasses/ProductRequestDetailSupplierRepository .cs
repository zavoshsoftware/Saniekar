using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductRequestDetailSupplierRepository : Repository<Models.ProductRequestDetailSupplier>, IProductRequestDetailSupplierRepository
    {
        public ProductRequestDetailSupplierRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
        }
    }
}
