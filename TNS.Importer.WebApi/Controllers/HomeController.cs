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
        HomeService _svc = new HomeService();
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
                product.SystemFileName = newFileName;
                _svc.ProcessUploadedFile(product);
            }

            

          
          
            return View();
        }

        private bool SaveFile(HttpPostedFileBase file, out string originalFileName, out string newFilename)
        {
            var physpath = Server.MapPath("/Uploads/ToBeProcessed/");
            FileInfo fi = new FileInfo(file.FileName);
            string systemFileName = string.Concat(physpath, Guid.NewGuid(), fi.Extension);
            file.SaveAs(systemFileName);
            originalFileName = file.FileName;
            newFilename = systemFileName;
            return true;
        }




    }
}
