using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class OrderDetailRepository : Repository<Models.OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }

    }
}
