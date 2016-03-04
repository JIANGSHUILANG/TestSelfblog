using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.DomainObject
{
    public class user_commentDomainObject
    {
        public int c_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> type_id { get; set; }
        public Nullable<int> article_id { get; set; }
        public string comment { get; set; }
        public DateTime comment_time { get; set; }
        public Nullable<int> rec_status { get; set; } //0：未回复   1：已回复
        public Nullable<int> status { get; set; }
        public string article_name { get; set; } //评论文章的名字
        public string username { get; set; }
        public string userheadimage { get; set; }
        //public int rec_id { get; set; }
        //public Nullable<DateTime> rec_time { get; set; } //回复时间
        //public string rec_comment { get; set; } //回复内容
        
       
        public List<user_commentChirden> chirden { get; set; }
        //扩展字段
        public object other { get; set; }
    }

    public class user_commentChirden
    {
        public string rec_comment { get; set; } //回复内容
        public string username { get; set; }//回复人的名字
        public string userheadimage { get; set; }
        public Nullable<DateTime> rec_time { get; set; } //回复时间
        public int rec_id { get; set; }

    }
}
