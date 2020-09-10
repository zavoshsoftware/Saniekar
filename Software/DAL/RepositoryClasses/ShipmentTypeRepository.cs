using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ShipmentTypeRepository : Repository<Models.ShipmentType>, IShipmentTypeRepository
    {
        public ShipmentTypeRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
        }
    }
}
