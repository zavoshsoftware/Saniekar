using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Infrastructure
{
    public class BaseControllerWithUnitOfWork : BaseController
    {
        public BaseControllerWithUnitOfWork()
        {
        }
        public BaseControllerWithUnitOfWork(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private IUnitOfWork _unitOfWork;
        protected virtual IUnitOfWork UnitOfWork
        {
            get
            {
                ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();

                ViewBag.unSentOrder = Helpers.GetUserInfo.GetBranchUnSentOrders();
                ViewBag.earlyOrder = Helpers.GetUserInfo.GetEarlyOrdersCount();
                ViewBag.proRecieve = Helpers.GetUserInfo.GetBranchRecieveCount();
                ViewBag.orderByFactory = Helpers.GetUserInfo.GetFactoryOrderCount();
                ViewBag.productRequestFactory = Helpers.GetUserInfo.GetFactoryProductRequestCount();

                if (_unitOfWork == null)
                {
                    _unitOfWork =
                    new UnitOfWork();
                }
                return (_unitOfWork);
            }
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
                _unitOfWork = null;
            }
        }
    }
}