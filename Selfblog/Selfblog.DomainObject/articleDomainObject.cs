using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selfblog.DomainObject
{
    public class articleDomainObject
    {
        //[DisplayName("ID")]
        public int article_id { get; set; }

        [DisplayName("文章标题")]
        [Required(ErrorMessage = "文章标题不能为空")]
        [StringLength(50, ErrorMessage = "文章标题长度不能超过128个字")]
        public string article_name { get; set; }

        [DisplayName("文章作者")]
        [Required(ErrorMessage = "作者不能为空")]
        [StringLength(50, ErrorMessage = "文章作者长度不能超过50个字")]
        public string article_author { get; set; }

        [DisplayName("文章代码下载地址")]
        [StringLength(200, ErrorMessage = "代码下载地址长度不能超过200个字")]
        public string article_codeurl { get; set; }


        [DisplayName("文章发布时间")]
        public System.DateTime article_time { get; set; }

        [DisplayName("文章点击量")]
        public Nullable<int> article_click { get; set; }

        [DisplayName("文章分类")]
        public Nullable<int> category_id { get; set; }

        //[DisplayName("所属用户")]
        //public Nullable<int> user_id { get; set; }

        [DisplayName("所属栏目")]
        public Nullable<int> type_id { get; set; }

        //:0为私有，1为公开，2为仅好友查看"
        [DisplayName("文章的模式")]
        public Nullable<int> article_type { get; set; }

        [DisplayName("文章内容")]
        [Required(ErrorMessage = "文章内容不能为空")]
        public string article_content { get; set; }

        [DisplayName("是否置顶")] //1:置顶
        public Nullable<int> article_up { get; set; }

        [DisplayName("是否是博主推荐")]
        public Nullable<int> article_support { get; set; }

        [DisplayName("状态")] // 0：删除 1：可用
        public Nullable<int> article_status { get; set; }

        [DisplayName("标题图片ID")] // 0：删除 1：可用
        public Nullable<int> photo_id { get; set; }

        [DisplayName("赞")]
        public Nullable<int> article_goodup { get; set; }
        [DisplayName("踩")]
        public Nullable<int> article_baddown { get; set; }

        /// <summary>
        /// 评论个数
        /// </summary>
        public Nullable<int> comment_count { get; set; }
        //所属分类的图片
        public photoDomainObject photo { get; set; }

        //扩展字段
        public object other { get; set; }

    }
}
