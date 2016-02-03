using Selfblog.EF.BaseEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Infrastructure._Interface
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext context { get; }
        bool IsCommitted { get; set; }
        int Commit();
        void Rollback();

        /// <summary>
        /// 将聚合跟的状态标记为新建，但EF上下文此时并未提交
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        TEntity RegisterNew<TEntity>(TEntity obj)
            where TEntity : Root;

        /// <summary>
        /// 将聚合根的状态标记为修改，但EF上下文此时并未提交
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        void RegisterModified<TEntity>(TEntity obj)
            where TEntity : Root;

        /// <summary>
        /// 将聚合根的状态标记为操作，但EF上下文此时并未提交
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="obj"></param>
        void RegisterDeleted<TEntity>(TEntity obj)
            where TEntity : Root;
    }
}
