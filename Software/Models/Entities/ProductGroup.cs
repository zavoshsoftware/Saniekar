 
namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;

    public class ProductGroup : BaseEntity
    {
        public ProductGroup()
        {
            Products = new List<Product>();
        }

        [Display(Name="گروه محصول")]
        public string Title { get; set; }

        [Display(Name="کد گروه محصول")]
        public string Code { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
