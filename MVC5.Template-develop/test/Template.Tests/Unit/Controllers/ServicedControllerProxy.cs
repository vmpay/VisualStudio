using Template.Controllers;
using Template.Services;
using System.Web.Mvc;

namespace Template.Tests.Unit.Controllers
{
    public class ServicedControllerProxy : ServicedController<IService>
    {
        public ServicedControllerProxy(IService service) : base(service)
        {
        }

        public void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            OnActionExecuting(filterContext);
        }
    }
}
