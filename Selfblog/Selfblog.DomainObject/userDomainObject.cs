using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.DomainObject
{
    public class userDomainObject
    {
        public int user_id { get; set; }
        public Nullable<int> group_id { get; set; }
        public string user_name { get; set; }
        public string user_pwd { get; set; }
        public string user_phone { get; set; }
        public string user_sex { get; set; }
        public Nullable<int> user_qq { get; set; }
        public string user_email { get; set; }
        public string user_address { get; set; }
        public Nullable<int> user_mark { get; set; }
        public Nullable<int> user_rank_id { get; set; }
        public string user_last_login_ip { get; set; }
        public Nullable<int> user_birthday { get; set; }
        public string user_description { get; set; }
        public string user_image_url { get; set; }
        public string user_school { get; set; }
        public Nullable<int> user_register_time { get; set; }
        public string user_register_ip { get; set; }
        public Nullable<int> user_last_update_time { get; set; }
        public string user_weibo { get; set; }
        public string user_blood_type { get; set; }
        public string user_says { get; set; }
        public Nullable<int> user_lock { get; set; }
        public Nullable<int> user_freeze { get; set; }
        public string user_power { get; set; }
        //扩展字段
        public object other { get; set; }
    }
}
