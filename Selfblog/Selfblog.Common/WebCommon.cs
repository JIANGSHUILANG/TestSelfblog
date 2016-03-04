using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Selfblog.Common
{
    public class WebCommon
    {
        public static string SendGet(string url, string parm)
        {
            WebClient client = new WebClient();
            string result = client.DownloadString(new Uri(url + parm));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">url:请求地址</param>
        /// <param name="key">拼接出来的参数如：pageindex=1&pagesize=6&uid=b597b35b-24dc-493f-baa0-8daafd6ef79d</param>
        /// <returns></returns>
        //public static string SendPost(string url, string key)
        //{
        //    string resultValue = string.Empty;
        //    var param = Enctyption.Encrypt(key);//加密
        //    param = System.Web.HttpUtility.HtmlEncode(param);//url编码
        //    var butter = System.Text.Encoding.UTF8.GetBytes(param);

        //    Uri Url = new Uri(url);
        //    var request = (HttpWebRequest)WebRequest.Create(Url);
        //    request.Method = "post";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.Accept = "*/*";
        //    request.UseDefaultCredentials = true;
        //    request.ContentLength = butter.Length;
        //    using (var stream = request.GetRequestStream())
        //    {
        //        stream.Write(butter, 0, butter.Length);
        //    }
        //    try
        //    {
        //        var response = (HttpWebResponse)request.GetResponse();
        //        var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //        var str = stream.ReadToEnd();//返回值
        //        resultValue = System.Web.HttpUtility.HtmlDecode(Enctyption.Decrypt(str));
        //    }
        //    catch (WebException ex)
        //    {
        //        var webResponse = ex.Response as HttpWebResponse;
        //        var myread = new StreamReader(webResponse.GetResponseStream());
        //        resultValue = myread.ReadToEnd();
        //        myread.Close();
        //    }

        //    return resultValue;
        //}

        /// <summary>
        /// 不加密Post请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string SendPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string Get_AppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }


    }
}
