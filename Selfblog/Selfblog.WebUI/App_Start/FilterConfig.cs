using Selfblog.WebUI.MyAttribute;
using System.Web;
using System.Web.Mvc;

namespace Selfblog.WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MyExceptionAttribute());
        }
    }
}