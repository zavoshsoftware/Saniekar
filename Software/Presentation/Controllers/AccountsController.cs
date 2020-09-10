using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class AccountsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Accounts
        public ActionResult Index(Guid id)
        {
            var accounts = UnitOfWork.AccountRepository.Get(a => a.BranchId == id && a.IsDeleted == false).OrderByDescending(a => a.Code);
            return View(accounts.ToList());
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public DateTime GetGrDate(DateTime datetime)
        {
            System.Globalization.PersianCalendar c = new System.Globalization.PersianCalendar();

            DateTime date = c.ToDateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0);

            return date;
        }
        public ActionResult TransferAmount()
        {
            ViewBag.FromBranchId = new SelectList(UnitOfWork.BranchRepository.Get(), "Id", "Title");
            ViewBag.ToBranchId = new SelectList(UnitOfWork.BranchRepository.Get(), "Id", "Title");


            return View();
        }

        public decimal GetBranchAmount(Guid branchId)
        {
            return 0;
        }

        private Guid refId = new Guid("afc2eb41-1103-4309-9285-c3b055dbe4c4");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransferAmount(TransferAccountViewModel transferAccount)
        {
            CodeGenerator codeGenerator = new CodeGenerator();

            if (ModelState.IsValid)
            {
                int code = codeGenerator.ReturnAccountCode();
                Account account = new Account()
                {
                    BranchId = transferAccount.FromBranchId,
                    Bestankar = transferAccount.Amount,
                    Bedehkar = 0,
                    Code = code,
                    Body = transferAccount.Title,
                    CreationDate = transferAccount.TransferDate,
                    Remain = GetBranchAmount(transferAccount.FromBranchId),
                    RefrenceId = refId,

                    IsDeleted = false,
                    IsActive = true,
                };

                UnitOfWork.AccountRepository.Insert(account);
               

                account = new Account()
                {
                    BranchId = transferAccount.ToBranchId,
                    Bestankar = 0,
                    Bedehkar = transferAccount.Amount,
                    Code = code + 1,
                    Body = transferAccount.Title,
                    CreationDate = transferAccount.TransferDate,
                    Remain = GetBranchAmount(transferAccount.FromBranchId),
                    RefrenceId = refId,

                    IsDeleted = false,
                    IsActive = true,
                };

                UnitOfWork.AccountRepository.Insert(account);

               UnitOfWork.Save();
                return RedirectToAction("Index",new{id=transferAccount.FromBranchId});
            }

            ViewBag.FromBranchId = new SelectList(UnitOfWork.BranchRepository.Get(), "Id", "Title");
            ViewBag.ToBranchId = new SelectList(UnitOfWork.BranchRepository.Get(), "Id", "Title");

            return View(transferAccount);
        }
    }
}
