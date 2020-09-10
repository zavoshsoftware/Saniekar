using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Helpers
{
    public class CodeGenerator : Infrastructure.BaseControllerWithUnitOfWork
    {
        public int ReturnOrderCode()
        {

            Order order = UnitOfWork.OrderRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();

            if (order != null)
            {
                return order.Code + 1;
            }
            return 1;
        }
        public int ReturnAccountCode()
        {

            Account account = UnitOfWork.AccountRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();

            if (account != null)
            {
                return account.Code + 1;
            }
            return 1;
        }

        public int ReturnUserCode()
        {
            User user = UnitOfWork.UserRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();

            if (user != null)
            {
                return user.Code + 1;
            }
            return 1000;
        }

        public int ReturnProductRequestCode()
        {
            ProductRequest productRequest = UnitOfWork.ProductRequestRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();

            if (productRequest != null)
            {
                return Convert.ToInt32(productRequest.Code) + 1;
            }
            return 1;
        }


        public int ReturnProductGroupCode()
        {
            ProductGroup productGroup = UnitOfWork.ProductGroupRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();

            if (productGroup != null)
            {
                return Convert.ToInt32(productGroup.Code) + 1;
            }
            return 100;
        }

        public string ReturnProductCode(string productGroupCode)
        {
            ProductGroup productGroup = UnitOfWork.ProductGroupRepository
                .Get(current => current.Code == productGroupCode).FirstOrDefault();

            Product product = UnitOfWork.ProductRepository.Get(current => current.ProductGroupId == productGroup.Id).OrderByDescending(current => current.Code).FirstOrDefault();

            if (product != null)
            {
                string sustringCode = product.Code.Substring(3, 4);
                int code = Convert.ToInt32(sustringCode) + 1;
                return productGroupCode + code.ToString();
            }

            return productGroupCode + "1000";
        }

        public string ReturnChildProductCode(Product parentProduct)
        {

            Product product = UnitOfWork.ProductRepository.Get(/*current=>current.ParentId== parentProduct.Id*/).OrderByDescending(current => current.Code).FirstOrDefault();

            if (product != null)
            {
                string sustringCode = product.Code.Substring(7, 3);
                int code = Convert.ToInt32(sustringCode) + 1;
                return parentProduct.Code + code.ToString();
            }

            return parentProduct.Code + "100";
        }
    }
}