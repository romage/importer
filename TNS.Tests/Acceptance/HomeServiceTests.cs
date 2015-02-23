using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNS.Importer.Interfaces;
using Xunit;

namespace TNS.Importer.Tests.Acceptance
{
    [Trait("Home Service Tests", "")]
    public class HomeServiceTests
    {
        IHomeService iHomeService;
        public HomeServiceTests()
        {
            // integration test to be handled separately
            iHomeService.ProcessUploadedFile(new Models.Product(), "");
            
            // integration test to be handled separately
            iHomeService.SaveProduct(new Models.Product());

            
        }

        [Fact(DisplayName = "IT:: Process uploaded File")]
        public void ProcessUploadedFile()
        {
            Assert.True(false);
        }

        [Fact(DisplayName = "IT:: Save Product")]
        public void SaveProduct()
        {
            Assert.True(false);
        }

    }
}
