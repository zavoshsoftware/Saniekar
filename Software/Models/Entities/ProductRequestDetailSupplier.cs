using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class ProductRequestDetailSupplier : BaseEntity
    {
        public Guid ProductRequestDetailId { get; set; }
        public ProductRequestDetail ProductRequestDetail { get; set; }

        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }

        [Display(Name = "تعداد")]
        public int Quantity { get; set; }

        [Display(Name = "دریافت شد؟")]
        public bool IsReceived { get; set; }

        internal class configuration : EntityTypeConfiguration<ProductRequestDetailSupplier>
        {
            public configuration()
            {
                HasRequired(p => p.ProductRequestDetail).WithMany(t => t.ProductRequestDetailSuppliers).HasForeignKey(p => p.ProductRequestDetailId);
                HasRequired(p => p.Branch).WithMany(t => t.ProductRequestDetailSuppliers).HasForeignKey(p => p.BranchId);
            }
        }
    }
}
