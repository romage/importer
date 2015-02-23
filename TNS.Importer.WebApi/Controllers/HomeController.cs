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

            string originalFileName;
            string newFileName;
  
            if (SaveFile(file, out originalFileName, out newFileName))
            {
                product.OriginalFileName = originalFileName;
                product.SystemFileNameWithExtension = newFileName;
                product.CurrentProcessingFolder = ConfigHelper.ToBeProcessedPath;
               
                _svc.ProcessUploadedFile(product);
            }
            return RedirectToAction("Product", product);
        }


        public ActionResult Product(Product product)
        {
            return View(product);
        }

        private bool SaveFile(HttpPostedFileBase file, out string originalFileName, out string newFilename)
        {
            FileInfo fi = new FileInfo(file.FileName);
            newFilename = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
            var physpath = Server.MapPath(Path.Combine(ConfigHelper.UploadFileRoot, ConfigHelper.ToBeProcessedPath, newFilename));
            file.SaveAs(physpath);
            originalFileName = file.FileName;
          
            return true;
        }




    }
}
