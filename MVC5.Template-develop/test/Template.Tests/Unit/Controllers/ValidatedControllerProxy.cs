using Template.Controllers;
using Template.Services;
using Template.Validators;
using System.Web.Mvc;

namespace Template.Tests.Unit.Controllers
{
    public class ValidatedControllerProxy : ValidatedController<IValidator, IService>
    {
        protected ValidatedControllerProxy(IValidator validator, IService service)
            : base(validator, service)
        {
        }

        public void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            OnActionExecuting(filterContext);
        }
    }
}
