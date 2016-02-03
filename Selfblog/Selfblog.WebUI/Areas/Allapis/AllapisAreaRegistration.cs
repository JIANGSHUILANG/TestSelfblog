using System.Web.Mvc;

namespace Selfblog.WebUI.Areas.Allapis
{
    public class AllapisAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Allapis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Allapis_default",
                "Allapis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
