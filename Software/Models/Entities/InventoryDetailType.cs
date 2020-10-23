
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InventoryDetailType : BaseEntity
    {
        public InventoryDetailType()
        {
            InventoryDetails=new List<InventoryDetail>();
        }
        [StringLength(50)]
        [Display(Name="نوع رکورد")]
        public string Title { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
        public virtual ICollection<InventoryDetail> InventoryDetails { get; set; }
         
    }
}
