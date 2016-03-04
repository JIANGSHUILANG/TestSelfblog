using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.DomainObject
{
    public class categoryDomainObject
    {
        //[DisplayName("文章标题")]
        //[Required(ErrorMessage = "文章标题不能为空")]
        //[StringLength(50, ErrorMessage = "文章标题长度不能超过128个字")]
        public int category_id { get; set; }
        //partent_id：父类ID,顶级为0
        public Nullable<int> partent_id { get; set; }
        [DisplayName("分类名字")]
        [Required(ErrorMessage = "分类名字不能为空")]
        [StringLength(15, ErrorMessage = "分类名字长度不能超过15个字")]
        public string category_name { get; set; }
        public Nullable<int> photo_id { get; set; }

        //文章集合
        public List<articleDomainObject> articleDomainObjects { get; set; }
        //扩展字段
        public object other { get; set; }
    }
}
