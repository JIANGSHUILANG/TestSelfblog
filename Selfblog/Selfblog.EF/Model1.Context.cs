﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SelfBlogEntities : DbContext
    {
        public SelfBlogEntities()
            : base("name=SelfBlogEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<category> category { get; set; }
        public DbSet<user_comment> user_comment { get; set; }
        public DbSet<friendly_link> friendly_link { get; set; }
        public DbSet<user> user { get; set; }
        public DbSet<visitor> visitor { get; set; }
        public DbSet<photo> photo { get; set; }
        public DbSet<article> article { get; set; }
    }
}
