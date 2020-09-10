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
    public class InputDocumentDetail : BaseEntity
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal RowAmount { get; set; }
        public Guid InputDocumentId { get; set; }
        public virtual InputDocument InputDocument { get; set; }
        public virtual Product Product { get; set; }

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
        public Guid? ProductColorId { get; set; }
        public virtual ProductColor ProductColor { get; set; }

        [Display(Name = "تشک")]
        public Guid? MattressId { get; set; }
        public virtual Mattress Mattress { get; set; }
        internal class configuration : EntityTypeConfiguration<InputDocumentDetail>
        {
            public configuration()
            {
                HasRequired(p => p.InputDocument).WithMany(t => t.InputDocumentDetails).HasForeignKey(p => p.InputDocumentId);
                HasRequired(p => p.Product).WithMany(t => t.InputDocumentDetails).HasForeignKey(p => p.ProductId);
                HasRequired(p => p.ProductColor).WithMany(t => t.InputDocumentDetails).HasForeignKey(p => p.ProductColorId);
                HasOptional(p => p.Mattress).WithMany(t => t.InputDocumentDetails).HasForeignKey(p => p.MattressId);
            }
        }
    }
}
