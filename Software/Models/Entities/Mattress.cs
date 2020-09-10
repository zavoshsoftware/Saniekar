using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Mattress : BaseEntity
    {
        public Mattress()
        {
            OrderDetails=new List<OrderDetail>();
            ProductRequestDetails = new List<ProductRequestDetail>();
        }
        [Display(Name="تشک")]
        public string Title { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductRequestDetail> ProductRequestDetails { get; set; }
        public virtual ICollection<InputDocumentDetail> InputDocumentDetails { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }

    }
}
