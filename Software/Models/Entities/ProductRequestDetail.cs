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
    public class ProductRequestDetail : BaseEntity
    {
        public ProductRequestDetail()
        {
            ProductRequestDetailSuppliers=new List<ProductRequestDetailSupplier>();
        }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal RowAmount { get; set; }
        public Guid ProductRequestId { get; set; }
        public virtual ProductRequest ProductRequest { get; set; }
        public int TotalSupplied { get; set; }
        public Guid? ProductColorId { get; set; }
        public virtual ProductColor ProductColor { get; set; }

        [Display(Name = "تشک")]
        public Guid? MattressId { get; set; }
        public virtual Mattress Mattress { get; set; }
        public virtual ICollection<ProductRequestDetailSupplier> ProductRequestDetailSuppliers { get; set; }

        [NotMapped]
        public string AmountStr
        {
            get { return Amount.ToString("N0") + " تومان"; }
        }

        [NotMapped]
        public string RowAmountStr
        {
            get { return RowAmount.ToString("N0") + " تومان"; }
        }
        internal class configuration : EntityTypeConfiguration<ProductRequestDetail>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.ProductRequestDetails).HasForeignKey(p => p.ProductId);
                HasRequired(p => p.ProductRequest).WithMany(t => t.ProductRequestDetails).HasForeignKey(p => p.ProductRequestId);
                HasRequired(p => p.ProductColor).WithMany(t => t.ProductRequestDetails).HasForeignKey(p => p.ProductColorId);
                HasOptional(p => p.Mattress).WithMany(t => t.ProductRequestDetails).HasForeignKey(p => p.MattressId);
            }
        }
    }
}
