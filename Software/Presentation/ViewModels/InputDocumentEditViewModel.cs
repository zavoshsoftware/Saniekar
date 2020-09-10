using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class InputDocumentEditViewModel
    {
        //public Guid BranchId { get; set; }
        //public Guid SupplierId { get; set; }
        //public string Code { get; set; }
        //public DateTime InputDate { get; set; }
        //public decimal SubTotal { get; set; }
        //public decimal AddedAmount { get; set; }
        //public decimal DecreaseAmount { get; set; }
        //public decimal Total { get; set; }
        public InputDocument InputDocument { get; set; }
        public List<InputDocumentDetail> InputDocumentDetails { get; set; }
    }

}