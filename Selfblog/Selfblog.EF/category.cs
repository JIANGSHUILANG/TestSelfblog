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
    
    public partial class category:Selfblog.EF.BaseEntity.Root
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> partent_id { get; set; }
        public Nullable<int> photo_id { get; set; }
    }
}
