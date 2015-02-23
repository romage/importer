using Autofac;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.Owin;
using Autofac.Integration.Mvc;

using TNS.Importer.Data;
using TNS.Importer.Services;
using TNS.Importer.Interfaces;
using TNS.Importer.Models;
using TNS.Importer.ModelInterfaces;
using System.Web.Mvc;
using System.Web.Http;



namespace TNS.Importer.WebApi.App_Start
{
    public class AutofacConfig
    {
        public static void Register(IAppBuilder app)
        { 
            var builder = new ContainerBuilder();
            var WebApiApplication = typeof(TNS.Importer.WebApi.WebApiApplication).Assembly;
            var Services = typeof(TNS.Importer.Services.HomeService).Assembly;

            var config = new HttpConfiguration();

            builder
                .RegisterType(typeof(ImporterDataContext))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>));

            builder
                .RegisterType(typeof(UnitOfWork))
                .As(typeof(IUnitOfWork));

            builder.RegisterAssemblyTypes(Services)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder
                .RegisterType<HomeRepository>()
                .As<IHomeRepository>();

            builder.RegisterControllers(WebApiApplication);
            builder.RegisterApiControllers(WebApiApplication);
            
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            app.UseAutofacWebApi(config);
            //.UseAutofacMvc();

        }
    }
}