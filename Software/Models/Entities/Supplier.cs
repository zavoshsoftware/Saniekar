using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Supplier:BaseEntity
    {
        public Supplier()
        {
            InputDocuments = new List<InputDocument>();
        }
        public string Title { get; set; }
        public virtual ICollection<InputDocument> InputDocuments { get; set; }

    }
}
