using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selfblog.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index(int pageIndexBlog = 1, int pageIndexComment = 1)
        {

            var result = articleservice.LoadEntities(c => c.article_status == 1 && c.article_time.Value.Day == DateTime.Now.Day);
            ViewBag.blogcount = result.Count();
            ViewBag.allclick = result.Sum(c => c.article_click);
            var list = articleservice.Getarticles(pageIndexBlog, 5, 0);
            if (list != null)
            {
                ViewBag.list = list;
                ViewBag.AllCount = list.TotalItemCount;
                ViewBag.pageIndex = pageIndexBlog;
                ViewBag.pageCount = list.TotalPageCount;
            }

            ViewBag.commentcount = usercommentservice.LoadEntities(c => c.status == 1 && c.comment_time.Value.Day == DateTime.Now.Day).Count();

            var commentlist = usercommentservice.GetComments(pageIndexComment, 2);
            if (commentlist != null)
            {
                ViewBag.commentlist = commentlist;
                ViewBag.commentAllCount = commentlist.TotalItemCount;
                ViewBag.commentpageIndex = pageIndexComment;
                ViewBag.commentpageCount = commentlist.TotalPageCount;
            }

            return View();
        }

        public ActionResult Isarticle_up_support(string typeIsup_support, int type, int? Article_ID)
        {
            if (!string.IsNullOrEmpty(typeIsup_support))
            {
                var temp = articleservice.LoadEntities(c => c.article_id == Article_ID).FirstOrDefault();


                if (typeIsup_support == "artice_up")
                {
                    temp.article_up = type;
                    articleservice.UpdateEntity(temp);
                }
                else if (typeIsup_support == "article_support")
                {
                    temp.article_support = type;
                    articleservice.UpdateEntity(temp);
                }
            }
            return RedirectToAction("Index");
        }

        //删除文章
        public ActionResult Deletearticle(int ArticleID)
        {
            var temp = articleservice.LoadEntities(c => c.article_id == ArticleID).FirstOrDefault();
            if (temp != null)
            {
                articleservice.DeleteEntity(temp);
            }
            return RedirectToAction("Index");
        }

        public ActionResult CommentLook(int pageIndex = 1)
        {
            return View();
        }

    }
}
