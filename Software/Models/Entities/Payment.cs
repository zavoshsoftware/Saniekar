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
    public class Payment:BaseEntity
    {
        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Payment))]
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        [Display(Name = "PaymentTypeId", ResourceType = typeof(Resources.Models.Payment))]
        public Guid PaymentTypeId { get; set; }

        [Display(Name = "IsDeposit", ResourceType = typeof(Resources.Models.Payment))]
        public bool IsDeposit { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Payment))]
        public string Code { get; set; }

        [Display(Name = "FileAttched", ResourceType = typeof(Resources.Models.Payment))]
        public string FileAttched { get; set; }

        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        internal class configuration : EntityTypeConfiguration<Payment>
        {
            public configuration()
            {
                HasRequired(p => p.PaymentType).WithMany(t => t.Payments).HasForeignKey(p => p.PaymentTypeId);
            }
        }
    }
}
