using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using TNS.Importer.WebApi.App_Start;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;

[assembly: OwinStartup(typeof(TNS.Importer.WebApi.Startup))]

namespace TNS.Importer.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutofacConfig.Register(app);

            ConfigureAuth(app);

            var config = GlobalConfiguration.Configuration;
            var xmlFormatter = config.Formatters.XmlFormatter;
            config.Formatters.Remove(xmlFormatter);

            var jsonFormatter = config.Formatters.JsonFormatter;
            var JsonmediaTypeformatter = System.Net.Http.Formatting.MediaTypeFormatter.GetDefaultValueForType(typeof(JsonMediaTypeFormatter));

            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            jsonFormatter.SerializerSettings = jsonSettings;


        }
    }
}
