using Selfblog.Infrastructure;
using Selfblog.WebUI.MyAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using Selfblog.WebUI.App_Start;
namespace Selfblog.WebUI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            AreaRegistration.RegisterAllAreas();
            log4net.Config.XmlConfigurator.Configure();
            ErrorLog.WriteErrorToLog();
            AutoMapperConfiguration.Initiaztion();

            //这里注册Scripts.Render()这个使用类（）
            BundleConfig.RegisterBundles(BundleTable.Bundles); 

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //删除xml的解析 当返回值是string 时 直接返回string不是json对象(调用Api时，)
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear(); 
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication obj = sender as HttpApplication;
            string browsername = obj.Request.Browser.Browser; //浏览器名
            string IPv4 = GetClientIPv4();
        }



        public string GetUserIp()
        {

            string realRemoteIP = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                realRemoteIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
            }
            if (string.IsNullOrEmpty(realRemoteIP))
            {
                realRemoteIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(realRemoteIP))
            {
                realRemoteIP = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            return realRemoteIP;

        }


        /// <summary>
        /// 此方法获取的是：Ipv6的IP地址
        /// </summary>
        /// <returns></returns>
        private string GetIp()
        {
            string ip = string.Empty;
            if (Context.Request.ServerVariables["HTTP_VIA"] != null) // using proxy 
            {
                ip = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString(); // Return real client IP. 
            }
            else// not using proxy or can't get the Client IP 
            {
                ip = Context.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP. 
            }
            return ip;
        }

        /// <summary>
        /// 此方法获取的是：Ipv6的IP地址
        /// </summary>
        /// <returns></returns>
        public string GetClientIPv4()
        {
            string ipv4 = String.Empty;
            foreach (IPAddress ip in Dns.GetHostAddresses(GetIp()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != String.Empty)
            {
                return ipv4;
            }

            foreach (IPAddress ip in Dns.GetHostEntry(GetIp()).AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }
            return ipv4;
        }

    }
}