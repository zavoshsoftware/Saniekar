using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class ShipmentType:BaseEntity
    {
        public ShipmentType()
        {
            Orders=new List<Order>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ShipmentType))]
        public string Title { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
