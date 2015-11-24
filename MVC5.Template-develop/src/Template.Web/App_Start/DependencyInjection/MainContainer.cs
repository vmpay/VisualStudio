using LightInject;
using Template.Components.Logging;
using Template.Components.Mail;
using Template.Components.Mvc;
using Template.Components.Security;
using Template.Controllers;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Services;
using Template.Validators;
using System.Data.Entity;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Template.Web.DependencyInjection
{
    public class MainContainer : ServiceContainer
    {
        public virtual void RegisterServices()
        {
            Register<DbContext, Context>();
            Register<IUnitOfWork, UnitOfWork>();

            Register<ILogger, Logger>();
            Register<IAuditLogger, AuditLogger>();

            Register<IHasher, BCrypter>();
            Register<IMailClient, SmtpMailClient>();

            Register<IRouteConfig, RouteConfig>();
            Register<IBundleConfig, BundleConfig>();
            Register<IExceptionFilter, ExceptionFilter>();

            Register<IMvcSiteMapParser, MvcSiteMapParser>();
            Register<IMvcSiteMapProvider>(factory => new MvcSiteMapProvider(
                 HostingEnvironment.MapPath("~/Mvc.sitemap"), factory.GetInstance<IMvcSiteMapParser>()));

            Register<IGlobalizationProvider>(factory =>
                new GlobalizationProvider(HostingEnvironment.MapPath("~/Globalization.xml")));
            RegisterInstance<IAuthorizationProvider>(new AuthorizationProvider(typeof(BaseController).Assembly));

            Register<IRoleService, RoleService>();
            Register<IAccountService, AccountService>();

            Register<IRoleValidator, RoleValidator>();
            Register<IAccountValidator, AccountValidator>();
        }
    }
}
