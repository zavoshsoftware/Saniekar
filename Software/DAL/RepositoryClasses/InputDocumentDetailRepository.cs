using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class InputDocumentDetailRepository : Repository<Models.InputDocumentDetail>, IInputDocumentDetailRepository
    {
        public InputDocumentDetailRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
        }
    }
}
