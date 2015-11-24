using Template.Components.Security;
using System.Web.Mvc;

namespace Template.Tests.Unit.Components.Security
{
    public class GlobalizedAuthorizeAttributeProxy : GlobalizedAuthorizeAttribute
    {
        public void BaseHandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HandleUnauthorizedRequest(filterContext);
        }
    }
}
