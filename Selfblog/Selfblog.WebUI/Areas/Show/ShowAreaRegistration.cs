using System.Web.Mvc;

namespace Selfblog.WebUI.Areas.Show
{
    public class ShowAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Show";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Show_default",
                "Show/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
