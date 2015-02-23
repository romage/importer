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
            file.SaveAs(string.Concat(physpath, "newFileName", fi.Extension));
          
            return View();
        }



    }
}
