using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class InputDocument : BaseEntity
    {
        public InputDocument()
        {
            InputDocumentDetails = new List<InputDocumentDetail>();
        }
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.InputDocument))]
        public string Code { get; set; }

        [Display(Name = "InputDate", ResourceType = typeof(Resources.Models.InputDocument))]
        public DateTime InputDate { get; set; }


        [Display(Name = "BranchId", ResourceType = typeof(Resources.Models.InputDocument))]
        public Guid BranchId { get; set; }

        [Display(Name = "SupplierId", ResourceType = typeof(Resources.Models.InputDocument))]
        public Guid SupplierId { get; set; }

        [Display(Name = "SubTotal", ResourceType = typeof(Resources.Models.InputDocument))]
        public decimal SubTotal { get; set; }

        [Display(Name = "AddedAmount", ResourceType = typeof(Resources.Models.InputDocument))]
        public decimal AddedAmount { get; set; }

        [Display(Name = "DecreaseAmount", ResourceType = typeof(Resources.Models.InputDocument))]
        public decimal DecreaseAmount { get; set; }

        [Display(Name = "Total", ResourceType = typeof(Resources.Models.InputDocument))]
        public decimal Total { get; set; }

        [NotMapped]
        [Display(Name = "Total", ResourceType = typeof(Resources.Models.InputDocument))]
        public string TotalStr
        {
            get { return Total.ToString("N0"); }
        }

        [NotMapped]
        [Display(Name = "DecreaseAmount", ResourceType = typeof(Resources.Models.InputDocument))]
        public string DecreaseAmountStr
        {
            get { return DecreaseAmount.ToString("N0"); }
        }


        [NotMapped]
        [Display(Name = "AddedAmount", ResourceType = typeof(Resources.Models.InputDocument))]
        public string AddedAmountStr
        {
            get { return AddedAmount.ToString("N0") ; }
        }

        [NotMapped]
        [Display(Name = "SubTotal", ResourceType = typeof(Resources.Models.InputDocument))]
        public string SubTotalStr
        {
            get { return SubTotal.ToString("N0"); }
        }


        public virtual Branch Branch { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<InputDocumentDetail> InputDocumentDetails { get; set; }

        internal class configuration : EntityTypeConfiguration<InputDocument>
        {
            public configuration()
            {
                HasRequired(p => p.Branch).WithMany(t => t.InputDocuments).HasForeignKey(p => p.BranchId);
                HasRequired(p => p.Supplier).WithMany(t => t.InputDocuments).HasForeignKey(p => p.SupplierId);
            }
        }
    }
}
