using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var physpath = Server.MapPath("/Uploads/ToBeProcessed/");
            FileInfo fi = new FileInfo(file.FileName);
            file.SaveAs(string.Concat(physpath, "newFileName", fi.Extension));


            return View();
        }



    }
}
