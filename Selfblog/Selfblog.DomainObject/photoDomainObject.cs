using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.DomainObject
{
    public class photoDomainObject
    {
        public int photo_id { get; set; }
        public string photo_imageurl { get; set; }
        [DisplayName("图片分类ID")] // 0：category的图片 1：article标题图片 2：article内容图片
        public Nullable<int> photo_typeid { get; set; }
        //扩展字段
        public object other { get; set; }
    }
}
