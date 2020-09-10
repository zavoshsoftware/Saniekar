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
    public class Account:BaseEntity
    {
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Account))]
        public int Code { get; set; }

        [Display(Name = "Body", ResourceType = typeof(Resources.Models.Account))]
        public string Body { get; set; }

        [Column(TypeName = "Money")]
        [Display(Name = "Bedehkar", ResourceType = typeof(Resources.Models.Account))]
        public decimal Bedehkar { get; set; }

        [Column(TypeName = "Money")]
        [Display(Name = "Bestankar", ResourceType = typeof(Resources.Models.Account))]
        public decimal Bestankar { get; set; }

        [Column(TypeName = "Money")]
        [Display(Name = "Remain", ResourceType = typeof(Resources.Models.Account))]
        public decimal Remain { get; set; }

        [NotMapped]
        [Display(Name = "Bedehkar", ResourceType = typeof(Resources.Models.Account))]
        public string BedehkarStr
        {
            get { return Bedehkar.ToString("N0"); }
        }


        [NotMapped]
        [Display(Name = "Bestankar", ResourceType = typeof(Resources.Models.Account))]
        public string BestankarStr
        {
            get { return Bestankar.ToString("N0"); }
        }


        [NotMapped]
        [Display(Name = "Remain", ResourceType = typeof(Resources.Models.Account))]
        public string RemainStr
        {
            get { return Remain.ToString("N0"); }
        }

        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }

        public Guid  RefrenceId { get; set; }
        internal class configuration : EntityTypeConfiguration<Account>
        {
            public configuration()
            {
                HasRequired(p => p.Branch).WithMany(t => t.Accounts).HasForeignKey(p => p.BranchId);
            }
        }
    }
}
