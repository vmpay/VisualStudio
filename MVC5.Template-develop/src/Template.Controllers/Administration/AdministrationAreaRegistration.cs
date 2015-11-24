using System;
using System.Web.Mvc;

namespace Template.Controllers.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration
    {
        public override String AreaName
        {
            get
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context
                .MapRoute(
                    "AdministrationMultilingual",
                    "{language}/Administration/{controller}/{action}/{id}",
                    new { area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "lt" },
                    new[] { "Template.Controllers.Administration" });

            context
                .MapRoute(
                    "Administration",
                    "Administration/{controller}/{action}/{id}",
                    new { language = "en", area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "en" },
                    new[] { "Template.Controllers.Administration" });
        }
    }
}
