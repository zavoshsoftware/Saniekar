using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InventoryDetail:BaseEntity
    {
        public Guid InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }

        public Guid InventoryDetailTypeId { get; set; }
        public virtual InventoryDetailType InventoryDetailType { get; set; }

        public string Title { get; set; }
        public int Quantity { get; set; }
        public int Remail { get; set; }

        public Guid? EntityId { get; set; }
    }
}
