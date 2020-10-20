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
    public class ProductColor:BaseEntity
    {
        [Display(Name="رنگ")]
        public string Title { get; set; }

        [Display(Name="مبلغ اضافه شونده")]
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }


        [NotMapped]
        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Product))]
        public string AmountStr
        {
            get { return Amount.ToString("N0") + " تومان"; }
        }

        [Display(Name="مبلغ اضافه شونده به قیمت فروشگاه")]
        [Column(TypeName = "Money")]
        public decimal StoreAmount { get; set; }


        [NotMapped]
        public string StoreAmountStr
        {
            get { return StoreAmount.ToString("N0") + " تومان"; }
        }


        [Display(Name="مبلغ اضافه شونده به قیمت کارخانه")]
        [Column(TypeName = "Money")]
        public decimal FactoryAmount { get; set; }


        [NotMapped]
        public string FactoryAmountStr
        {
            get { return FactoryAmount.ToString("N0") + " تومان"; }
        }


        [Display(Name="محصول")]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductRequestDetail> ProductRequestDetails { get; set; }
        public virtual ICollection<InputDocumentDetail> InputDocumentDetails { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }


        internal class configuration : EntityTypeConfiguration<ProductColor>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.ProductColors).HasForeignKey(p => p.ProductId);
            }
        }
    }
}
