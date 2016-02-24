using Selfblog.IService;
using Selfblog.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selfblog.WebUI.Areas.Show.Controllers
{
    public class FrontBaseController : Controller
    {
        public IarticleService articleservice = new articleService();
        public IcategoryService categoryservice = new categoryService();
        public Iuser_commentService usercommentservice = new user_commentService();
    }
}
