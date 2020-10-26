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
    public class OrderDetail:BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public Guid? ProductColorId { get; set; }
        [Display(Name="تعداد")]
        public int Quantity { get; set; }
        [Display(Name="قیمت فی")]
        public decimal Amount { get; set; }
        [Display(Name="جمع کل")]
        public decimal RowAmount { get; set; }
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

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
        public virtual ProductColor ProductColor { get; set; }



        [Display(Name = "تشک")]
        public Guid? MattressId { get; set; }
        public virtual Mattress Mattress { get; set; }


        internal class configuration : EntityTypeConfiguration<OrderDetail>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.OrderDetails).HasForeignKey(p => p.ProductId);
                HasRequired(p => p.Order).WithMany(t => t.OrderDetails).HasForeignKey(p => p.OrderId);
                HasRequired(p => p.ProductColor).WithMany(t => t.OrderDetails).HasForeignKey(p => p.ProductColorId);
                HasOptional(p => p.Mattress).WithMany(t => t.OrderDetails).HasForeignKey(p => p.MattressId);
            }
        }
    }
}
