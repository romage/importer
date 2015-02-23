using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNS.Importer.Models;
using TNS.Importer.Interfaces;
using TNS.Importer.Services;

namespace TNS.Importer.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            var model = new Product();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Upload(HttpPostedFileBase file, [Bind(Include = "ProductName,Scorer,ScorerEmail")] Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            var physpath = Server.MapPath("/Uploads/ToBeProcessed/");
            FileInfo fi = new FileInfo(file.FileName);
            string systemFileName = string.Concat(physpath, Guid.NewGuid(), fi.Extension);
            file.SaveAs(systemFileName);

            FileLoader fl = new FileLoader();
            ExcelParserViaDomService parser = new ExcelParserViaDomService(fl);
            Product newProduct =  parser.Parse(systemFileName);

            //TODO: refactor this depending on whether product data is got from the spreadsheet. may be better to pass in the product to the parser,  or just get the scores out of the spreadsheet
            newProduct.DateOfScoreInput = DateTime.Today;
            //newProduct.ProcessState = ProcessStateEnum.ToBeProcessed;
            newProduct.OriginalFileName = fi.Name;
            newProduct.SystemFileName = systemFileName;
            newProduct.Scorer = product.Scorer;
            newProduct.ProductName = product.ProductName;
            newProduct.ScorerEmail = product.ScorerEmail;

            newProduct.ProcessState = ProcessStateEnum.FinishedProcessing;
            
            //TODO: Move to ioc container, and constructor
            HomeService hs = new HomeService();
            hs.SaveProduct(newProduct);
          
            return View();
        }




    }
}
