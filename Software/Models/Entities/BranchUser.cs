using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BranchUser:BaseEntity
    {
        [Display(Name = "UserId", ResourceType = typeof(Resources.Models.BranchUser))]
        public Guid UserId { get; set; }
        public Guid BranchId { get; set; }
        [Display(Name = "IsManager", ResourceType = typeof(Resources.Models.BranchUser))]
        public bool IsManager { get; set; }
        public virtual User User { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
