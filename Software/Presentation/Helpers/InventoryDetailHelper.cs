using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Helpers
{
    public class InventoryDetailHelper : Infrastructure.BaseControllerWithUnitOfWork
    {
        /// <summary>
        /// entitycode can be null just when typeName is start
        /// </summary>
        public InventoryDetail Insert(Guid inventoryId, string typeName, int quantity, int oldRemain,int? entityCode,Guid? entityId)
        {
            try
            {
                InventoryDetailType inventoryDetailType = GetInventoryDetailTypeByName(typeName);

                InventoryDetail inventoryDetail = new InventoryDetail()
                {
                    InventoryDetailTypeId = inventoryDetailType.Id,
                    InventoryId = inventoryId,
                    IsActive = true,
                    Quantity = quantity,
                    Remain = oldRemain + quantity,
                    Title = GetInventoryDetailTitle(entityCode, inventoryDetailType.Title),
                    EntityId = entityId
                };

                return inventoryDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GetInventoryDetailTitle(int? code,string typeTitle)
        {
            if (code != null)
                return typeTitle + " - کد: " + code;

            return typeTitle;
        }

        public InventoryDetailType GetInventoryDetailTypeByName(string name)
        {
            return UnitOfWork.InventoryDetailTypeRepository.Get(c => c.Name == name).FirstOrDefault();
        }
    }
}