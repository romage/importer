using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using TNS.Importer.WebApi.App_Start;

[assembly: OwinStartup(typeof(TNS.Importer.WebApi.Startup))]

namespace TNS.Importer.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutofacConfig.Register(app);

            ConfigureAuth(app);
        }
    }
}
