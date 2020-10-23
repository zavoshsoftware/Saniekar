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
    public class Inventory : BaseEntity
    {
        public Inventory()
        {
            InventoryDetails=new List<InventoryDetail>();
        }
        [Display(Name = "ProductId", ResourceType = typeof(Resources.Models.Inventory))]
        public Guid ProductId { get; set; }

        [Display(Name = "BranchId", ResourceType = typeof(Resources.Models.Inventory))]
        public Guid BranchId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Branch Branch { get; set; }

        [Display(Name = "Stock", ResourceType = typeof(Resources.Models.Inventory))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Stock { get; set; }

        [Display(Name = "OrderPoint", ResourceType = typeof(Resources.Models.Inventory))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int OrderPoint { get; set; }

        [Display(Name = "رنگ")]
        public Guid? ProductColorId { get; set; }
        public virtual ProductColor ProductColor { get; set; }

        [Display(Name = "تشک")]
        public Guid? MattressId { get; set; }
        public virtual Mattress Mattress { get; set; }

        public virtual ICollection<InventoryDetail> InventoryDetails { get; set; }


        internal class configuration : EntityTypeConfiguration<Inventory>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.Inventories).HasForeignKey(p => p.ProductId);
                HasRequired(p => p.Branch).WithMany(t => t.Inventories).HasForeignKey(p => p.BranchId);
                HasOptional(p => p.ProductColor).WithMany(t => t.Inventories).HasForeignKey(p => p.ProductColorId);
                HasOptional(p => p.Mattress).WithMany(t => t.Inventories).HasForeignKey(p => p.MattressId);
            }
        }
    }
}
