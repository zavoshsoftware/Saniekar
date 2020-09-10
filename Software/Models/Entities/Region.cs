using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Region : BaseEntity
    {
        public Region()
        {
            Orders = new List<Order>();
        }
        [Display(Name = "محله")]
        public string Title { get; set; }
        [Display(Name = "هزینه حمل")]
        public decimal ShipmentAmount { get; set; }

        [Display(Name = "شهر")]
        public Guid CityId { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        internal class Configuration : EntityTypeConfiguration<Region>
        {
            public Configuration()
            {
                HasRequired(p => p.City)
                    .WithMany(j => j.Regions)
                    .HasForeignKey(p => p.CityId);
            }
        }
    }
}
