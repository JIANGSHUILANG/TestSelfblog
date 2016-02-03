using Selfblog.Common;
using Selfblog.Infrastructure;
using Selfblog.IService;
using Selfblog.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Selfblog.WebUI.Areas.Allapis.Controllers;
namespace Selfblog.WebUI.Controllers
{
    public partial class BaseController : Controller
    {
        public IarticleService articleservice = new articleService();
        public IcategoryService categoryservice = new categoryService();
        public Iuser_commentService usercommentservice = new user_commentService();
        public IphotoService photoservice = new photoService();
        public string BaseUrl = ConfigurationManager.AppSettings["HostUrl"];
    }

    public partial class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //错误日志只有Manager能查看
            base.OnActionExecuting(filterContext);
        }

        //type=0 :全部  type=1:数据库错误  type=2 MVC错误
        public ActionResult ErrorPage(int pageIndex = 1, int type = 0)
        {
            Expression<Func<ErrorModel, bool>> expression = null;
            if (type == 0)
                expression = c => true;
            else
                expression = c => c.TypeValue == (type == 1 ? "MVC框架错误" : "数据库错误");
            MongoHelper<ErrorModel> helper = new MongoHelper<ErrorModel>("Server=127.0.0.1:27017", "errordatabase", "errorinfo");
            int pageCount = 0; //总页数
            List<ErrorModel> list = helper.LoadPageEnities<DateTime?>(pageIndex, 20, out pageCount, expression, c => c.Time, false);
            if (list != null)
            {
                ViewBag.pageIndex = pageIndex;

                ViewBag.pageCount = (int)Math.Ceiling((double)pageCount / 20); // pageCount;
            }
            return View(list);
        }

        public ActionResult DefaultPage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            return RedirectToAction("Index", "Home");
        }

        public string SaveImage(string base64value, int photo_type)
        {
            if (base64value.Length <= 50) return null;
            var length = base64value.IndexOf(',');
            var temp = base64value.Substring(length + 1);
            string format = string.Format("base64str={0}&photo_type={1}", temp, photo_type);
            string result = WebCommon.SendPost(string.Format("{0}/api/Image/PostSaveImage", ConfigurationManager.AppSettings["ApiUrl"]), format);

            return result;
        }
    }
}
