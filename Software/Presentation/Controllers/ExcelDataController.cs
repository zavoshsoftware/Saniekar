using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using Helpers;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class ExcelDataController : Infrastructure.BaseControllerWithUnitOfWork
    {
        CodeGenerator codeGenerator = new CodeGenerator();

        public ActionResult Import()
        {
            UploadFile uploadFile = new UploadFile();
            return View(uploadFile);
        }

        [HttpPost]
        public ActionResult Import(UploadFile UploadFile)
        {
            if (ModelState.IsValid)
            {
                if (UploadFile.ExcelFile.ContentLength > 0)
                {
                    if (UploadFile.ExcelFile.FileName.EndsWith(".xlsx") || UploadFile.ExcelFile.FileName.EndsWith(".xls"))
                    {
                        XLWorkbook Workbook;
                        try
                        {
                            Workbook = new XLWorkbook(UploadFile.ExcelFile.InputStream);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(String.Empty, $"Check your file. {ex.Message}");
                            return View();
                        }
                        IXLWorksheet WorkSheet = null;
                        try
                        {
                            WorkSheet = Workbook.Worksheet("sheet1");

                        }
                        catch
                        {
                            ModelState.AddModelError(String.Empty, "sheet not found!");
                            return View();
                        }
                        WorkSheet.FirstRow().Delete();//if you want to remove ist row
                        int newCode = 0;
                        int i = 1;
                        foreach (var row in WorkSheet.RowsUsed())
                        {

                            UpdateRow(row.Cell(1).Value.ToString(), 
                                row.Cell(4).Value.ToString(),
                                DecimalConvertor(row.Cell(5).Value.ToString()),
                                DecimalConvertor(row.Cell(6).Value.ToString()),
                                DecimalConvertor(row.Cell(7).Value.ToString()),
                                DecimalConvertor(row.Cell(8).Value.ToString()),
                                DecimalConvertor(row.Cell(9).Value.ToString()),
                                DecimalConvertor(row.Cell(10).Value.ToString()),
                                ConvertMattresVal(row.Cell(11).Value.ToString()));
                            i++;
                        }

                        UnitOfWork.Save();
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Only .xlsx and .xls files are allowed");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Not a valid file");
                    return View();
                }
            }
            return View();
        }

        public bool ConvertMattresVal(string hasMattres)
        {
            if (hasMattres.ToLower() == "y")
                return true;

            return false;
        }
        public decimal DecimalConvertor(string amount)
        {
            try
            {
                if (string.IsNullOrEmpty(amount))
                    return 0;

                return Convert.ToDecimal(amount);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void UpdateRow(string parentCode, string childTitle, decimal customerAmount, decimal storeAmount, decimal factoryAmount, decimal colorAdditiveAmount, decimal colorAdditiveStoreeAmount, decimal colorAdditiveFactoryAmount,bool hasMattress)
        {
            Product product = UnitOfWork.ProductRepository.Get(current => current.Code == parentCode).FirstOrDefault();


            if (product != null)
            {
                if (string.IsNullOrEmpty(childTitle))
                {
                    product.Amount = customerAmount;
                    product.StoreAmount = storeAmount;
                    product.FactoryAmount = factoryAmount;
                    product.HasMattress = hasMattress;

                    UnitOfWork.ProductRepository.Update(product);
                    UnitOfWork.Save();

                }

                else
                {
                    product.StoreAmount = storeAmount;
                    product.FactoryAmount = factoryAmount;
                    product.Amount = customerAmount;
                    product.HasMattress = hasMattress;
                    UnitOfWork.ProductRepository.Update(product);

                    ProductColor productColor = UnitOfWork.ProductColorRepository
                        .Get(current => current.ProductId == product.Id && current.Title == childTitle)
                        .FirstOrDefault();

                    if (productColor != null)
                    {
                        productColor.Amount = colorAdditiveAmount;
                        productColor.FactoryAmount = colorAdditiveFactoryAmount;
                        productColor.StoreAmount = colorAdditiveStoreeAmount;

                        UnitOfWork.ProductColorRepository.Update(productColor);
                    }
                    else
                    {
                        ProductColor newChildProduct = new ProductColor
                        {
                            Id = Guid.NewGuid(),
                            ProductId = product.Id,
                            Title = childTitle,
                            Amount = colorAdditiveAmount,
                            IsDeleted = false,
                            IsActive = true,
                            FactoryAmount = colorAdditiveFactoryAmount,
                            StoreAmount = colorAdditiveStoreeAmount,
                        };

                        UnitOfWork.ProductColorRepository.Insert(newChildProduct);

                    }

                    UnitOfWork.Save();
                }
            }
            //else
            //{
            //    Product oProduct=new Product()
            //    {
            //        Title = 
            //    };
            //}
        }
    }
}