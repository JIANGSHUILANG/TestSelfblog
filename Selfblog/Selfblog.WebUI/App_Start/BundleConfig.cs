using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Selfblog.WebUI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Scripts.Render("~/bundles/amazeuijs")  //页面使用这行就直接添加2条js引用了

            //校验model的js
            bundles.Add(new ScriptBundle("~/bundles/Script/jqueryval").Include(
                      "~/Content/Script/jquery.unobtrusive*",
                      "~/Content/Script/jquery.validate*"));

            /*
             *-------------------------------------后台 Start-------------------------------------
             */

            //添加js
            bundles.Add(new ScriptBundle("~/bundles/Script/amauijquery").Include
                (
                "~/Content/Script/jquery.min.js",
                "~/Content/Script/app.js"
                )
                );

            //添加css
            bundles.Add(new StyleBundle("~/bundles/Css/amazeuicss").Include
                (
                "~/Content/Css/amazeui.min.css",
                "~/Content/Css/admin.css",
                "~/Content/Css/amazeuierror.css"
                )
                );
            /*------------------------------------------End----------------------------------------------*/

            bundles.Add(new StyleBundle("~/bundles/Show/Script/bootstrap-3.3.5-dist/css")
                .Include(
                "~/Areas/Show/Script/bootstrap-3.3.5-dist/css/bootstrap.min.css"
                )
                );

            bundles.Add(new ScriptBundle("~/bundles/Show/Script/bootstrap-3.3.5-dist/js")
                .Include(
                "~/Areas/Show/Script/bootstrap-3.3.5-dist/js/bootstrap.min.js",
                "~/Areas/Show/Script/laydate.dev.js"
                )
                );
        }
    }
}