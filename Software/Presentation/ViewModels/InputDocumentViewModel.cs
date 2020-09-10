using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class InputDocumentViewModel
    {
        public DateTime InputDate { get; set; }
    }

    public class InputDocumentInsertViewModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal RowAmount { get; set; }
        public decimal Amount { get; set; }

        public Guid? ColorId { get; set; }
        public Guid? MattressId { get; set; }
    }
}