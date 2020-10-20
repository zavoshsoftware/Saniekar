using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class InventoryDetailTypeRepository : Repository<Models.InventoryDetailType>, IInventoryDetailTypeRepository
    {
        public InventoryDetailTypeRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
