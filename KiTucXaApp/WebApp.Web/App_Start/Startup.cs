using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Data;
using WebApp.Data.Infrastructure;
using WebApp.Data.Repositories;
using WebApp.Model.Models;
using WebApp.Service.Services;

[assembly: OwinStartup(typeof(WebApp.Web.App_Start.Startup))]

namespace WebApp.Web.App_Start
{
    public partial class Startup
    {      
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureAuth(app);
            ConfigAutofac(app);
        }

        private void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // --- Commons ---
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<WebAppDbContext>().AsSelf().InstancePerRequest();

            // --- Identity ---
            builder.RegisterType<AppUserStore>().As<IUserStore<AppUser>>().InstancePerRequest();
            builder.RegisterType<AppUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<AppSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(m => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(m => app.GetDataProtectionProvider()).InstancePerRequest();

            // --- Respositories ---
            builder.RegisterAssemblyTypes(typeof(IAppUserRepository).Assembly)
            .Where(m => m.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerRequest();

            // --- Services ---
            builder.RegisterAssemblyTypes(typeof(IAppUserService).Assembly)
            .Where(m => m.Name.EndsWith("Service"))
            .AsImplementedInterfaces().InstancePerRequest();

            // ---  ---
            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);

        }
    }
}
