using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class InventoryRepository : Repository<Models.Inventory>, IInventoryRepository
    {
        public InventoryRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
        }
    }
}
