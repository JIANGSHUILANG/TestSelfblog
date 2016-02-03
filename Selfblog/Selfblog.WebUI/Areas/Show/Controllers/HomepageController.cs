using Selfblog.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selfblog.WebUI.Areas.Show.Controllers
{
    public class HomepageController : FrontBaseController
    {
        //
        // GET: /Show/Homepage/

        public ActionResult Index(
            int pageIndex = 1,
            int categroy_id = 0,
            string author = null,
            string click = null,
            string comment = null)
        {
            //获取分类，以及分类中包含的文章
            var categorylist = categoryservice.GetCategorycoverarticlecount();
            if (categorylist != null)
                ViewBag.categorylist = categorylist;

            //所有文章
            var allarticlelist = articleservice.Getarticles(pageIndex, 4, categroy_id, author, click, comment);
            if (allarticlelist != null)
            {
                ViewBag.allarticlelist = allarticlelist;
                ViewBag.AllCount = allarticlelist.TotalItemCount;
                ViewBag.pageIndex = pageIndex;
                ViewBag.pageCount = allarticlelist.TotalPageCount;
            }


            var articlelist = articleservice.LoadEntities(c => c.article_status == 1).OrderByDescending(c => c.article_click).Take(8).ToList();
            if (articlelist != null)
                ViewBag.hotarticlelist = articlelist;
            else
                ViewBag.hotarticlelist = articleservice.LoadEntities(c => c.article_status == 1 && c.article_support == 1).OrderByDescending(c => c.article_time).Take(8).ToList();

            ViewBag.newcommentlist = usercommentservice.LoadEntities(c => c.status == 1 && c.rec_status == 0).OrderByDescending(c => c.comment_time).Take(8).ToList();



            return View();
        }
        /// <summary>
        /// Info信息
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ActionResult articleinfo(int article_id)
        {
            var model = articleservice.GetarticleInfo(c => c.article_id == article_id);
            if (model != null)
            {
                model.article_click += 1;
                articleservice.UpdateEntity(model);
            }
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="article_id"></param>
        /// <param name="clicktype">1:好评   -1：差评</param>
        /// <returns></returns>

        [HttpPost]
        public string clickrticleup_down(int article_id, int clicktype)
        {
            //一个用户只能点一次赞和踩一次
            var model = articleservice.LoadEntities(c => c.article_id == article_id).FirstOrDefault();
            if (model != null)
            {
                if (clicktype == 1)
                    model.article_goodup += 1;
                else
                    model.article_baddown += 1;
                var temp = articleservice.UpdateEntity(model);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { type = 1,clicktype=clicktype,message = "操作成功 !" });
        }

    }
}
