using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Presentation.Controllers
{
    public class PaymentsController : Infrastructure.BaseControllerWithUnitOfWork
    {

        public ActionResult Index(Guid id)
        {
            List<Payment> payments = UnitOfWork.PaymentRepository.Get(current => current.OrderId == id).ToList();

            PutAmount(id);

            return View(payments);
        }

        public void PutAmount(Guid orderId)
        {
            Order order = UnitOfWork.OrderRepository.GetById(orderId);

            ViewBag.total = order.TotalAmountStr;
            ViewBag.payment = order.PaymentAmountStr;
            ViewBag.remain = order.RemainAmountStr;
            ViewBag.orderDate = order.OrderDateStr;
            ViewBag.code = order.Code;

        }
        public ActionResult Create(Guid id)
        {
            Payment depositePayment = UnitOfWork.PaymentRepository
                .Get(current => current.OrderId == id && current.IsDeposit).FirstOrDefault();

            if (depositePayment != null)
                ViewBag.hasDeposite = true;
            else
                ViewBag.hasDeposite = false;


            PutAmount(id);

            ViewBag.PaymentTypeId = new SelectList(UnitOfWork.PaymentTypeRepository.Get(), "Id", "Title");

            ViewBag.id = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payment payment, Guid id, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                if (UpdateOrderPayment(id, payment.Amount))
                {
                    #region Upload and resize image if needed
                    string newFilenameUrl = string.Empty;
                    if (fileupload != null)
                    {
                        string filename = Path.GetFileName(fileupload.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFilenameUrl = "/Uploads/paymentAttachment/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);

                        fileupload.SaveAs(physicalFilename);

                        payment.FileAttched = newFilenameUrl;
                    }
                    #endregion


                    payment.OrderId = id;
                    payment.IsActive = true;
                    UnitOfWork.PaymentRepository.Insert(payment);

                    UnitOfWork.Save();

                    return RedirectToAction("Index", new { id = id });
                }
                ModelState.AddModelError("highAmount", "مبلغ وارد شده بیشتر از باقی مانده مبلغ سفارش می باشد");
            }
            PutAmount(id);

            ViewBag.PaymentTypeId = new SelectList(UnitOfWork.PaymentTypeRepository.Get(), "Id", "Title", payment.PaymentTypeId);
            return View(payment);
        }

        public bool UpdateOrderPayment(Guid orderId, decimal payment)
        {
            Order order = UnitOfWork.OrderRepository.GetById(orderId);

            if (payment > order.RemainAmount)
                return false;

            order.PaymentAmount = order.PaymentAmount + payment;

            order.RemainAmount = order.RemainAmount - payment;

            if (order.PaymentAmount == order.RemainAmount)
                order.IsPaid = true;

            UnitOfWork.OrderRepository.Update(order);

            return true;
        }

        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Payment payment = UnitOfWork.PaymentRepository.GetById(id.Value);

        //    if (payment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    PutAmount(payment.OrderId);

        //    ViewBag.PaymentTypeId = new SelectList(UnitOfWork.PaymentTypeRepository.Get(), "Id", "Title", payment.PaymentTypeId);
        //    ViewBag.id = payment.OrderId;

        //    return View(payment);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Payment payment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (payment.IsDeposit)
        //        {
        //            Payment depositePayment = UnitOfWork.PaymentRepository
        //                .Get(current =>
        //                    current.OrderId == payment.OrderId && current.IsDeposit && current.Id != payment.Id)
        //                .FirstOrDefault();

        //            if (depositePayment == null)
        //            {
        //                payment.IsDeleted = false;
        //                UnitOfWork.PaymentRepository.Update(payment);

        //                UpdatePaymentOnEdit(payment);

        //                UnitOfWork.Save();
        //                return RedirectToAction("Index", new {id = payment.OrderId});
        //            }
        //            ModelState.AddModelError("hasdeposit",
        //                "این سفارش پیش پرداخت دیگری دارد. ابتدا از لیست پرداخت ها، پیش پرداخت قبلی را حذف نمایید");

        //        }
        //        else
        //        {
        //            payment.IsDeleted = false;
        //            UnitOfWork.PaymentRepository.Update(payment);
        //            UpdatePaymentOnEdit(payment);

        //            UnitOfWork.Save();
        //            return RedirectToAction("Index", new { id = payment.OrderId });
        //        }

        //    }
        //    PutAmount(payment.OrderId);

        //    ViewBag.PaymentTypeId = new SelectList(UnitOfWork.PaymentTypeRepository.Get(), "Id", "Title", payment.PaymentTypeId);
        //    return View(payment);
        //}

        //public void UpdatePaymentOnEdit(Payment payment)
        //{
        //    Payment oPayment = UnitOfWork.PaymentRepository.GetById(payment.Id);

        //    if (oPayment.Amount != payment.Amount)
        //    {
        //        Order order = UnitOfWork.OrderRepository.GetById(payment.OrderId);

        //        order.PaymentAmount = order.PaymentAmount - oPayment.Amount;
        //        order.RemainAmount = order.RemainAmount + oPayment.Amount;

        //        UpdateOrderPayment(payment.OrderId, payment.Amount);
        //    }
        //}

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = UnitOfWork.PaymentRepository.GetById(id.Value);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = payment.OrderId;

            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Payment entity)
        {
            entity = UnitOfWork.PaymentRepository.GetById(entity.Id);
            UnitOfWork.PaymentRepository.Delete(entity);
            UpdateOrderPaymentOnDelete(entity.OrderId, entity.Amount);
            UnitOfWork.Save();
            return RedirectToAction("Index", new { id = entity.OrderId });
        }


        public bool UpdateOrderPaymentOnDelete(Guid orderId, decimal payment)
        {
            Order order = UnitOfWork.OrderRepository.GetById(orderId);

            if (payment > order.RemainAmount)
                return false;

            order.PaymentAmount = order.PaymentAmount - payment;

            order.RemainAmount = order.RemainAmount + payment;

            UnitOfWork.OrderRepository.Update(order);

            return true;
        }

    }
}
