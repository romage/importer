﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.WebApi.Controllers;
using Xunit;
using System.Web.Mvc;
using TNS.Importer.Models;
using TNS.Importer.Interfaces;
using TNS.Importer.Services;
using TNS.Importer.Data;

namespace TNS.Importer.Tests.Acceptance
{
    [Trait("HomeController", "")]
    public class HomeControllerTests
    {
        HomeController _homeController;
        
        public HomeControllerTests()
        {
            ImporterDataContext idc = new ImporterDataContext();

            this._homeController = new HomeController(new HomeService(new HomeRepository(idc), new ExcelParserViaDomService(new FileLoader())));
        }

        [Fact(DisplayName="Check the upload action return correct data model")]
        public void CheckTheUploadActionReturnsCorrectDataModel()
        {
            ControllerContext cc = new ControllerContext();
            var ret = _homeController.Upload() as ViewResult;
            var model = ret.Model;
            
            Assert.IsType<ViewResult>(ret);
            Assert.IsType<Product>(model);
        }

        //[Fact(DisplayName = "Redirected model will contain object graph")]
        //public void RedirectedModelWillContainObjectGraph()
        //{ 
            
        //}


    }
}
