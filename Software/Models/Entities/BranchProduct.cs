using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BranchProduct : BaseEntity
    {
        public Guid BranchId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual Product Product { get; set; }

        internal class configuration : EntityTypeConfiguration<BranchProduct>
        {
            public configuration()
            {
                HasRequired(p => p.Branch).WithMany(t => t.BranchProducts).HasForeignKey(p => p.BranchId);
                HasRequired(p => p.Product).WithMany(t => t.BranchProducts).HasForeignKey(p => p.ProductId);
            }
        }
    }
}
