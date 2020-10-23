using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InventoryDetail : BaseEntity
    {
        public Guid InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }

        public Guid InventoryDetailTypeId { get; set; }
        public virtual InventoryDetailType InventoryDetailType { get; set; }

        [Display(Name = "شرح")]
        public string Title { get; set; }
        [Display(Name = "تعداد")]
        public int Quantity { get; set; }
        [Display(Name = "باقی مانده")]
        public int Remain { get; set; }

        public Guid? EntityId { get; set; }


        internal class configuration : EntityTypeConfiguration<InventoryDetail>
        {
            public configuration()
            {
                HasRequired(p => p.Inventory).WithMany(t => t.InventoryDetails).HasForeignKey(p => p.InventoryId);
                HasRequired(p => p.InventoryDetailType).WithMany(t => t.InventoryDetails).HasForeignKey(p => p.InventoryDetailTypeId);
            }
        }

    }
}
