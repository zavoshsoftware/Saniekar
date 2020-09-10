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