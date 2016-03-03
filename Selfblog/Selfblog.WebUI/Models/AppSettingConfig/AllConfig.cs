using Selfblog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selfblog.WebUI.Models.AppSettingConfig
{
    public class AllConfig
    {
        public static string QiNiuConfig(string key, string value)
        {
            return string.Format("{0}{1}",key, value);
        }
    }
}