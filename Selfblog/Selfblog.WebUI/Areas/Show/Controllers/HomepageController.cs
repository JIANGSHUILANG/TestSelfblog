using Selfblog.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Selfblog.WebUI.Areas.Show.Controllers
{
    public class HomepageController : FrontBaseController
    {
        //
        // GET: /Show/Homepage/

        //1，得到网页上的链接地址：
        //string matchString = @"<a[^>]+href=\s*(?:'(?<href>[^']+)'|""(?<href>[^""]+)""|(?<href>[^>\s]+))\s*[^>]*>";
        //2，得到网页的标题：
        //string matchString = @"<title>(?<title>.*)</title>";
        //3，去掉网页中的所有的html标记：
        //string temp = Regex.Replace(html, "<[^>]*>", ""); //html是一个要去除html标记的文档

        //4, string matchString = @"<title>([\S\s\t]*?)</title>";
        //5,js去掉所有html标记的函数： 
        //function delHtmlTag(str)
        //{
        //return str.replace(/<[^>]+>/g,"");//去掉所有的html标记


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


        public ActionResult WebMap()
        {
            return View();
        }
    }

}

public static class Extension
{
    /// <summary>
    /// 删除html代码 保留 IMG P BR三个标签
    /// </summary>
    /// <param name="str">所需要删除HTML代码的字符串</param>
    /// <returns></returns>
    public static string ReplaceHtml_IPB(this string str)
    {
        if (str != "" && str != null)
        {
            //删除内含的 样式表代码
            Regex CutStyle = new Regex(@"<style([^>])*>(\w|\W)*?</style([^>])*>", RegexOptions.IgnoreCase);
            String TempStr = CutStyle.Replace(str, "");

            //<([^>]+)> 不过滤 img标签
            TempStr = TempStr.Replace("</p>", "[/p]");
            TempStr = TempStr.Replace("</P>", "[/p]");
            TempStr = TempStr.Replace("<p>", "[p]");
            TempStr = TempStr.Replace("<P>", "[p]");


            Regex BrHtml = new Regex("<br(.*?)>", RegexOptions.IgnoreCase);
            TempStr = BrHtml.Replace(TempStr, "[br/]");
            Regex SpanHtml1 = new Regex("<span", RegexOptions.IgnoreCase);
            TempStr = SpanHtml1.Replace(TempStr, "[span");
            Regex SpanHtml2 = new Regex("</span>", RegexOptions.IgnoreCase);
            TempStr = SpanHtml2.Replace(TempStr, "[/span]");
            Regex ImgHtml = new Regex("<img", RegexOptions.IgnoreCase);
            TempStr = ImgHtml.Replace(TempStr, "[img");
            Regex CutHtml = new Regex("<([^>]+)>", RegexOptions.IgnoreCase);
            TempStr = CutHtml.Replace(TempStr, "");
            //TempStr = TempStr.Replace ("/>" , ">");
            //Regex ImgHtml=new Regex("<img",RegexOptions.IgnoreCase);
            //格式化现有代码
            //TempStr = HttpUtility.HtmlEncode(TempStr);


            TempStr = TempStr.Replace("[img", "<img");
            TempStr = TempStr.Replace("[span", "<span");
            TempStr = TempStr.Replace("[p]", "<p>");
            TempStr = TempStr.Replace("[/p]", "</p>");
            TempStr = TempStr.Replace("[br/]", "<br/>");
            TempStr = TempStr.Replace("[/span]", "</span>");
            return TempStr;

        }
        else
        {
            return "";
        }
    }
}
