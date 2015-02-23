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
        IHomeService _svc;

        public HomeController(IHomeService svc)
        {
            this._svc = svc;
        }
        
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

            product.OriginalFileName = file.FileName;
            product.ProcessState = ProcessStateEnum.ToBeProcessed;
            product.CurrentProcessingFolder = ConfigHelper.ToBeProcessedPath;
            
            string UploadRootPhysical = Server.MapPath(Path.Combine("/", ConfigHelper.UploadFileRoot));

            if (SaveFile(file, product, UploadRootPhysical))
            {
                _svc.ProcessUploadedFile(product, UploadRootPhysical);
            }
            return RedirectToAction("Product", product);
        }


        public ActionResult Product(Product product)
        {
            return View(product);
        }

        private bool SaveFile(HttpPostedFileBase file, Product product, string uploadRootPhysical)
        {
            FileInfo fi = new FileInfo(file.FileName);
            product.SystemFileNameWithExtension = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
            var physpath = FileHelper.GetPhysicalFilePath(uploadRootPhysical, product);
            file.SaveAs(physpath);
          
            return true;
        }




    }
}
