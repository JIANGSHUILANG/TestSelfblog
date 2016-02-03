using Selfblog.Common;
using Selfblog.EF.BaseEntity;
using Selfblog.Infrastructure._Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public bool IsCommitted { get; set; }

        public int Commit()
        {
            if (IsCommitted) return 0;
            try
            {
                int result = context.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (Exception ex)
            {
                MongoHelper<ErrorModel> helper = new MongoHelper<ErrorModel>("Server=127.0.0.1:27017", "errordatabase", "errorinfo");
                ErrorModel error = new ErrorModel();
                error.UID = Guid.NewGuid().ToString();
                error.Time = DateTime.Now;
                error.TypeValue = "数据库错误";
                error.Message = ex.Message;
                helper.Insert(error);
                throw ex;
            }
        }

        public void Rollback()
        {
            IsCommitted = false;
        }

        public DbContext context
        {
            get { return ContextFactory.GetDbContext(); }
        }

        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }

            context.Dispose();
        }

        public TEntity RegisterNew<TEntity>(TEntity obj) where TEntity : Root
        {
            if (context.Entry(obj).State == EntityState.Detached)
            {
                context.Entry(obj).State = EntityState.Added;
            }

            IsCommitted = false;

            return obj;
        }

        public void RegisterModified<TEntity>(TEntity obj) where TEntity : Root
        {
            if (context.Entry(obj).State == EntityState.Detached)
            {
                context.Set<TEntity>().Attach(obj);
            }
            context.Entry(obj).State = EntityState.Modified;
            IsCommitted = false;
        }

        public void RegisterDeleted<TEntity>(TEntity obj) where TEntity : Root
        {
            context.Entry(obj).State = EntityState.Deleted;
            IsCommitted = false;
        }
    }
}
