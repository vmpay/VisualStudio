using System.Collections.Generic;
using System.Web.Mvc;

namespace Template.Components.Mvc
{
    public interface IMvcSiteMapProvider
    {
        IEnumerable<MvcSiteMapNode> GetAuthorizedMenus(ViewContext context);
        IEnumerable<MvcSiteMapNode> GetBreadcrumb(ViewContext context);
    }
}
