 
namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;

    public class City : BaseEntity
    {
        public City()
        {
            Orders=new List<Order>();
            Regions=new List<Region>();
        }

        [Display(Name = "استان")]
        public Guid ProvinceId { get; set; }

        [StringLength(100)]
        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Title { get; set; }

        public bool IsCenter { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Region> Regions { get; set; }

        internal class Configuration : EntityTypeConfiguration<City>
        {
            public Configuration()
            {
                HasRequired(p => p.Province)
                    .WithMany(j => j.Cities)
                    .HasForeignKey(p => p.ProvinceId);
            }
        }
    }
}
