using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
 

namespace Models
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductRequestDetails = new List<ProductRequestDetail>();
            OrderDetails = new List<OrderDetail>();
            InputDocumentDetails=new List<InputDocumentDetail>();
            BranchProducts = new List<BranchProduct>();
            Inventories=new List<Inventory>();
            ProductColors=new List<ProductColor>();
        }
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Product))]
        public string Code { get; set; }

        [Display(Name = "ProductTypeId", ResourceType = typeof(Resources.Models.Product))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public Guid ProductGroupId { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Product))]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Title { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Product))]
        [Column(TypeName = "Money")]
        public decimal? Amount { get; set; }


        [Display(Name = "FactoryAmount", ResourceType = typeof(Resources.Models.Product))]
        [Column(TypeName = "Money")]
        public decimal? FactoryAmount { get; set; }


        [Display(Name = "StoreAmount", ResourceType = typeof(Resources.Models.Product))]
        [Column(TypeName = "Money")]
        public decimal? StoreAmount { get; set; }


        [Display(Name = "ImageUrl", ResourceType = typeof(Resources.Models.Product))]
        public string ImageUrl { get; set; }

        [Display(Name="تشک دارد؟")]
        public bool HasMattress { get; set; }

        [NotMapped]
        [Display(Name = "StoreAmount", ResourceType = typeof(Resources.Models.Product))]
        public string StoreAmountStr
        {
            get { return StoreAmount?.ToString("N0")+" تومان"; }
        }
        [NotMapped]
        [Display(Name = "FactoryAmount", ResourceType = typeof(Resources.Models.Product))]
        public string FactoryAmountStr
        {
            get { return FactoryAmount?.ToString("N0")+" تومان"; }
        }
        [NotMapped]
        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Product))]
        public string AmountStr
        {
            get { return Amount?.ToString("N0")+" تومان"; }
        }
       
        public virtual ICollection<ProductColor> ProductColors { get; set; }

        public virtual ProductGroup ProductGroup { get; set; }

        public virtual ICollection<ProductRequestDetail> ProductRequestDetails { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<InputDocumentDetail> InputDocumentDetails { get; set; }
        public virtual ICollection<BranchProduct> BranchProducts { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }

        internal class configuration : EntityTypeConfiguration<Product>
        {
            public configuration()
            {
                HasRequired(p => p.ProductGroup).WithMany(t => t.Products).HasForeignKey(p => p.ProductGroupId);
            }
        }
    }
}
