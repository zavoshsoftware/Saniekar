using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Helpers
{
    public class AmountCalculator
    {
        public string GetAmountStrByType(Product product, string type)
        {
            if (type == "factory")
                return product.FactoryAmountStr;

            if (type == "store")
                return product.StoreAmountStr;

            return product.AmountStr;
        }


        public decimal GetAmountByType(Product product, string type)
        {
            if (type == "factory")
                return (decimal)product.FactoryAmount;

            if (type == "store")
                return (decimal)product.StoreAmount;

            return (decimal)product.Amount;
        }
    }
}