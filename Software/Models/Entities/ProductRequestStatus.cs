using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
  public  class ProductRequestStatus : BaseEntity
    {
        public ProductRequestStatus()
        {
            ProductRequests = new List<ProductRequest>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductRequestStatus))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Title { get; set; }


        public int Code { get; set; }

        public virtual ICollection<ProductRequest> ProductRequests { get; set; }
    }
}
