//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Selfblog.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class article:Selfblog.EF.BaseEntity.Root
    {
        public int article_id { get; set; }
        public string article_name { get; set; }
        public System.DateTime article_time { get; set; }
        public Nullable<int> article_click { get; set; }
        public Nullable<int> category_id { get; set; }
        public Nullable<int> type_id { get; set; }
        public Nullable<int> article_type { get; set; }
        public string article_content { get; set; }
        public Nullable<int> article_up { get; set; }
        public Nullable<int> article_support { get; set; }
        public string article_author { get; set; }
        public string article_codeurl { get; set; }
        public Nullable<int> article_status { get; set; }
        public Nullable<int> photo_id { get; set; }
        public Nullable<int> article_goodup { get; set; }
        public Nullable<int> article_baddown { get; set; }
    }
}
