using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.DomainObject
{
    public class categoryDomainObject
    {
        public int category_id { get; set; }
        //partent_id：父类ID,顶级为0
        public Nullable<int> partent_id { get; set; }
        public string category_name { get; set; }
        public Nullable<int> photo_id { get; set; }

        //文章集合
        public List<articleDomainObject> articleDomainObjects { get; set; }
        //扩展字段
        public object other { get; set; }
    }
}
