using System.Web.Routing;

namespace Template.Controllers
{
    public interface IRouteConfig
    {
        void RegisterRoutes(RouteCollection routes);
    }
}
