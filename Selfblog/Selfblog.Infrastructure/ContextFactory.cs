using Selfblog.EF;
using Selfblog.Infrastructure._Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Infrastructure
{
    public class ContextFactory
    {
        public static DbContext GetDbContext()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext == null)
            {
                dbContext = new SelfBlogEntities(); //Release :状态使用
                CallContext.SetData("dbContext", dbContext);
            }
            return dbContext;
        }

        public static IBaseExtension GetBaseExtension()
        {
            IBaseExtension CURD = (IBaseExtension)CallContext.GetData("BaseExtension");
            if (CURD == null)
            {
                CURD = new BaseExtension();
                CallContext.SetData("BaseExtension", CURD);
            }
            return CURD;
        }
    }
}
