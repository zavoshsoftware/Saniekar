using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Branch : BaseEntity
    {
        public Branch()
        {
            Accounts = new List<Account>();
            BranchProductRequests = new List<ProductRequest>();
            SupplierProductRequests = new List<ProductRequest>();
            Orders = new List<Order>();
            InputDocuments = new List<InputDocument>();
            BranchProducts = new List<BranchProduct>();
            Inventories = new List<Inventory>();
            ProductRequestDetailSuppliers = new List<ProductRequestDetailSupplier>();
            Funds = new List<Fund>();
            Users=new List<User>();
        }
        [Display(Name = "شعبه")]
        public string Title { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        [InverseProperty("RequestBranch")]
        public virtual ICollection<ProductRequest> BranchProductRequests { get; set; }
        [InverseProperty("RequestSupplier")]
        public virtual ICollection<ProductRequest> SupplierProductRequests { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<InputDocument> InputDocuments { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<BranchProduct> BranchProducts { get; set; }
        public virtual ICollection<ProductRequestDetailSupplier> ProductRequestDetailSuppliers { get; set; }
        public virtual ICollection<Fund> Funds { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
