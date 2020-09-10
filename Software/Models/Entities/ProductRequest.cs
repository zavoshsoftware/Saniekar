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
    public class ProductRequest : BaseEntity
    {

        public ProductRequest()
        {
            ProductRequestDetails=new List<ProductRequestDetail>();
        }

        [Display(Name = "کد درخواست")]
        public int Code { get; set; }
        [Display(Name = "تاریخ درخواست")]
        public DateTime RequestDate { get; set; }

        [ForeignKey("RequestBranch")]
        public Guid? RequestBranchId { get; set; }

        [ForeignKey("RequestSupplier")]
        public Guid? RequestSupplierId { get; set; }

        [Display(Name = "جمع کل")]
        public decimal Total { get; set; }

        [NotMapped]
        [Display(Name = "جمع کل")]
        public string  TotalStr
        {
            get { return Total.ToString("N0"); }
        }

        public virtual Branch RequestBranch { get; set; }

        public virtual Branch RequestSupplier { get; set; }

        public virtual ICollection<ProductRequestDetail> ProductRequestDetails { get; set; }
        internal class Configuration : EntityTypeConfiguration<ProductRequest>
        {
            public Configuration()
            {
                HasOptional(p => p.RequestBranch).WithMany(j => j.BranchProductRequests).HasForeignKey(p => p.RequestBranchId).WillCascadeOnDelete(false);
                HasOptional(p => p.RequestSupplier).WithMany(j => j.SupplierProductRequests).HasForeignKey(p => p.RequestSupplierId).WillCascadeOnDelete(false);
            }
        }


        [NotMapped]
        [Display(Name = "تاریخ درخواست")]
        public string RequestDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(CreationDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(CreationDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(CreationDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day) + " " + CreationDate.ToString("HH:mm:ss");


            }
        }
    }
}
