using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Selfblog.Infrastructure;
using log4net;
using Selfblog.Common;
//using bobostore.Infrastructure;
namespace Selfblog.WebUI.MyAttribute
{
    public class MyExceptionAttribute : HandleErrorAttribute
    {
        //异常队列
        public static Queue<ErrorModel> ExceptionQueue = new Queue<ErrorModel>();
        public override void OnException(ExceptionContext filterContext)
        {
            //#if DEBUG
            //将异常加入队列
            ErrorModel error = new ErrorModel();
            error.UID = Guid.NewGuid().ToString();
            error.Time = DateTime.Now;
            error.TypeValue = "MVC框架错误";
            error.ControllName = filterContext.Controller.ControllerContext.Controller.ToString();
            error.Message = filterContext.Exception.Message;
            error.StackTrace = filterContext.Exception.StackTrace;
            ExceptionQueue.Enqueue(error);
            MongoHelper<ErrorModel> helper = new MongoHelper<ErrorModel>("Server=127.0.0.1:27017", "errordatabase", "errorinfo");
            helper.Insert(error);
            //filterContext.HttpContext.Request.Path; 报错url路径
            filterContext.HttpContext.Response.Redirect("/Base/ErrorPage?message=" + error.Message);
            base.OnException(filterContext);

        }
    }

    //错误日志记录
    public class ErrorLog
    {
        private static object obj = new object();
        public static void WriteErrorToLog()
        {
            lock (obj)
            {
                WriteLog(() => { return MyExceptionAttribute.ExceptionQueue; });//写入MVC错误日志
            }
            lock (obj)
            {
                WriteLog(() => { return DBerrorLogMessage.DBExceptionQueue; });//写入数据库错误日志
            }
        }

        public static void WriteLog(Func<Queue<ErrorModel>> queue)
        {

            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    if (queue().Count > 0)
                    {
                        ErrorModel ex = queue().Dequeue();
                        if (ex != null)
                        {
                            ILog logger = LogManager.GetLogger("error");
                            logger.Error(string.Format("{0}\r\n ,{1}\r\n,{2}\r\n,{3}\r\n \r\n \r\n \r\n", ex.TypeValue, ex.UID, ex.Message, ex.StackTrace));
                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
            });
        }

    }
}