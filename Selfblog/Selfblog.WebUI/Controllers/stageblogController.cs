using Qiniu.IO;
using Qiniu.RS;
using Selfblog.Common;
using Selfblog.DomainObject;
using Selfblog.WebUI.Models.AppSettingConfig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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

        //获取Token
        private string GetToken()
        {

            Qiniu.Conf.Config.ACCESS_KEY = "ZZdUQg0-Ww5ngJTQ8F6i-1khKtLKUEYyuZP5alGv";
            Qiniu.Conf.Config.SECRET_KEY = "SZiQqnmOh0yP4n-Sh1gE_gXOtsDZFtV6pHUKUGbJ";
            var policy = new PutPolicy("jiangshuilang", 3600);
            return policy.Token();
        }

        //富文本编辑器上传图片,上传至七牛
        public string ckeditorUploadImage()
        {
            var file = Request.Files["upload"];
            var filename = string.Empty;
            var errCount = 0;
            var err = string.Empty;
            var imagepath = string.Empty;
            try
            {
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (file.ContentLength / 1024 > 5120)
                    {
                        err = "上传文件大于5MB";
                        errCount++;
                    }
                    else if (ext.ToLower() == ".jpg" || ext.ToLower() == ".gif" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".png")
                    {

                        byte[] bytes = new byte[file.InputStream.Length];
                        file.InputStream.Read(bytes, 0, bytes.Length);
                        file.InputStream.Seek(0, SeekOrigin.Begin);


                        #region 七牛操作 —— 上传图片

                        var token = GetToken();
                        PutExtra extra = new PutExtra();
                        Qiniu.IO.IOClient client = new IOClient();
                        var date = DateTime.Now;
                        var key = date.Year.ToString() + (date.Month + 1).ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.Millisecond.ToString() + ".jpg";
                        PutRet ret = client.Put(token, key, file.InputStream, extra);
                        imagepath = AllConfig.QiNiuConfig(WebCommon.Get_AppSetting("QiNiuUrl"), ret.key);              
                        #endregion

                        #region 上传至WCF

                        //Upload.UploadServiceClient client = new Upload.UploadServiceClient();
                        //imagepath = client.UploadImage1(bytes, 0, ext.Split('.')[1]);  //后缀转换base64获取 
                        #endregion
                    }
                    else
                    {
                        err = "只允许上传jpg，jpeg，png，gif格式";
                        errCount++;
                    }
                }
                else
                {
                    err = "请上传文件";
                    errCount++;
                }
            }
            catch (Exception ex)
            {
                err = "请上传文件";
                errCount++;
            }

            var jsel = new JavaScriptSerializer();

            return jsel.Serialize(new
            {
                error = errCount,
                url = err == string.Empty ? imagepath : string.Empty,
                message = err
            });

        }

    }
}
