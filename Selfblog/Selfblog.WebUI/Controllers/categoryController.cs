using Selfblog.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selfblog.WebUI.Controllers
{
    public class categoryController : BaseController
    {
        //
        // GET: /category/

        public ActionResult Index()
        {
            var categories = categoryservice.LoadEntities(c => c.partent_id == 0);
            var categorylist = new List<SelectListItem>();
            foreach (var item in categories)
            {
                categorylist.Add(new SelectListItem() { Text = item.category_name, Value = item.category_id.ToString() });
            }
            ViewBag.categorylist = categorylist;
            ViewBag.addorupdate = 0;
            return View();
        }

        public ActionResult AddorUpdateCateory(categoryDomainObject cateory,int addorupdate=0)
        {
            //添加
          
            var categories = Request.Form["categorylist"];
            if (categories == null)
                cateory.partent_id = 0;
            else
                cateory.partent_id = Convert.ToInt32(categories);

            if (addorupdate == 0)
                categoryservice.AddEntity(cateory);
            else
                categoryservice.UpdateEntity(cateory);


            return RedirectToAction("Index", "Home");
        }
    }
}
