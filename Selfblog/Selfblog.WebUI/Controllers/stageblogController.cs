using Selfblog.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Selfblog.WebUI.Controllers
{
    public class stageblogController : BaseController
    {

        //
        // GET: /stageblog/
        public ActionResult Index(int addorupdate = 0)
        {
            //分类：
            var list = categoryservice.LoadEntities(c => true).ToList();
            var categorylist = new List<SelectListItem>();
            foreach (var item in list)
            {
                categorylist.Add(new SelectListItem() { Text = item.category_name, Value = item.category_id.ToString() });
            }
            ViewBag.categorylist = categorylist;

            ViewBag.url = BaseUrl;

            ViewBag.addorupdate = addorupdate;
            return View();
        }

        public ActionResult Showupdate(int article_id, int addroupdate)
        {
            var model = articleservice.LoadEntities(c => c.article_id == article_id).FirstOrDefault();
            var list = categoryservice.LoadEntities(c => true).ToList();


            var categorylist = new List<SelectListItem>();
            foreach (var item in list)
            {
                if (model.category_id == item.category_id)
                    categorylist.Add(new SelectListItem() { Text = item.category_name, Value = item.category_id.ToString(), Selected = true });
                else
                    categorylist.Add(new SelectListItem() { Text = item.category_name, Value = item.category_id.ToString() });
            }
            ViewBag.categorylist = categorylist;
            if (model.photo_id != null)
            {
                model.other = photoservice.LoadEntities(c => c.photo_id == model.photo_id).FirstOrDefault().photo_imageurl;
            }
            ViewBag.url = BaseUrl;

            return View(model);
        }


        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddUpdateBlog(articleDomainObject article, int addorupdate = 0)
        {

            if (addorupdate == 0)
            {
                article.category_id = Convert.ToInt32(Request.Form["categorylist"] ?? "1");
                article.article_up = Convert.ToInt32(Request.Form["article_up"] == "on" ? 1 : 0);
                article.article_support = Convert.ToInt32(Request.Form["article_support"] == "on" ? 1 : 0);
                article.article_time = DateTime.Now;
                article.article_click = 0;
                article.type_id = 0;
                article.article_status = 1;
                article.article_goodup = 0;
                article.article_baddown = 0;
                articleservice.AddEntity(article);
            }
            else
            {
                var model = articleservice.LoadEntities(c => c.article_id == article.article_id).FirstOrDefault();
                model.category_id = Convert.ToInt32(Request.Form["categorylist"] ?? "1");
                model.article_up = Convert.ToInt32(Request.Form["article_up"] == "on" ? 1 : 0);
                model.article_support = Convert.ToInt32(Request.Form["article_support"] == "on" ? 1 : 0);
                model.article_content = article.article_content;
                model.article_name = article.article_name;
                model.photo_id = article.photo_id;
                model.article_codeurl = article.article_codeurl;
                model.article_type = article.article_type;
                articleservice.UpdateEntity(model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CT()
        { return View(); }

    }
}
