using Selfblog.EF.BaseEntity;
using Selfblog.Infrastructure;
using Selfblog.Infrastructure._Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.IService.Base
{
    public interface IService<DomainObject, TEntity>
        where DomainObject : class,new()
        where TEntity : Root
    {
        /// EF上下文
        /// </summary>
        DbContext db { get; }

        #region CURD__Select

        IQueryable<DomainObject> LoadEntities(System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda);

        IQueryable<DomainObject> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda, System.Linq.Expressions.Expression<Func<DomainObject, s>> orderbyLambad, bool isAsc);

        IQueryable<DomainObject> LoadPageEntities<s>(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda, System.Linq.Expressions.Expression<Func<DomainObject, s>> orderbyLambad, bool isAsc);

        PageOfItems<DomainObject> PageOfItemsLoadPageEntites<s>(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda, System.Linq.Expressions.Expression<Func<DomainObject, s>> orderbyLambad, bool isAsc = true);



        #endregion

        #region CURD_Update

        bool UpdateEntity(DomainObject entity);

        bool UpdatePartProperty(DomainObject entity, string[] PropertyNames);

        #endregion

        #region CURD_Create

        DomainObject AddEntity(DomainObject entity);

        int AddEntity(IEnumerable<DomainObject> entites);

        #endregion

        #region CURD_Delete

        int DeleteEntity(DomainObject entity);

        int DeleteEntity(System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda);

        int DeleteEntity(IEnumerable<DomainObject> entities);

        #endregion
    }
}
