using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Common
{
    public class MyPageBar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="parms"></param>
        /// <param name="parms2"></param>
        /// <returns></returns>
        public static string ShowMyPageBar(int pageIndex, int pageCount, string parms = null, string parms2 = null)
        {
            if (pageCount == 1)
            {
                return string.Empty;
            }
            int start = pageIndex - 5;//计算起始位置，要求页面上有10个数字页码。
            if (start < 1)
            {
                start = 1;
            }
            int end = start + 9;
            if (end > pageCount)
            {
                end = pageCount;
                start = end - 9 > 1 ? end - 9 : 1;
            }
            StringBuilder sb = new StringBuilder();
            if (pageIndex > 1)
            {
                if (string.IsNullOrWhiteSpace(parms2))
                    sb.AppendFormat("<li><a href='?pageIndex=1{0}'>首页</a></li>", parms);
                else
                    sb.AppendFormat("<li><a href='?{0}=1{1}'>首页</a></li>", parms2, parms);
            }
            for (int i = start; i <= end; i++)
            {
                if (i == pageIndex)
                {
                    sb.Append("<li class='am-active'><a href='#'>" + i + "</a></li>");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(parms2))
                        sb.Append(string.Format("<li><a href='?pageIndex={0}{1}'>{0}</a></li>", i, parms));
                    else
                        sb.Append(string.Format("<li><a href='?{0}={1}{2}'>{1}</a></li>", parms2, i, parms));
                }
            }
            if (pageIndex < pageCount)
            {
                if (string.IsNullOrWhiteSpace(parms2))
                    sb.Append(string.Format("<li><a href='?pageIndex={0}{1}'>末页</a></li>", pageCount, parms));
                else
                    sb.Append(string.Format("<li><a href='?{0}={1}{2}'>末页</a></li>", parms2, pageCount, parms));
            }
            return sb.ToString();
        }


     
        public static string ToBase64(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            string returnstr = Convert.ToBase64String(b);
            return returnstr;
        }
    }
}
