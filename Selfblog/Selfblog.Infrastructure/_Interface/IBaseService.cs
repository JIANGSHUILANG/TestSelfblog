using Selfblog.EF.BaseEntity;
using Selfblog.Infrastructure;
using Selfblog.Infrastructure._Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Infrastructure._Interface
{
    public partial interface IBaseService<T> where T : Root
    {
        /// <summary>
        /// EF上下文
        /// </summary>
        DbContext db { get; }

        /// <summary>
        /// 扩展
        /// </summary>
        IBaseExtension extension { get; }

        #region CURD__Select

        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);

        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambad, bool isAsc);

        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambad, bool isAsc);

        PageOfItems<T> PageOfItemsLoadPageEntites<s>(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambad, bool isAsc = true);



        #endregion

        #region CURD_Update

        bool UpdateEntity(T entity);

        bool UpdatePartProperty(T entity, string[] PropertyNames);

        #endregion

        #region CURD_Create

        T AddEntity(T entity);

        int AddEntity(IEnumerable<T> entites);

        #endregion

        #region CURD_Delete

        int DeleteEntity(T entity);

        int DeleteEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);

        int DeleteEntity(IEnumerable<T> entities);

        #endregion

        #region SQL

        //int GetMaxId(string TableName, string IdName);
        //bool Update(string Table, string parm, string where);
        //bool Delete(string Table, string where);
        #endregion



        #region CURD Extention
        //T GetByKey(object key);
        //int Delete(IEnumerable<T> entities);
        //int Delete(System.Linq.Expressions.Expression<Func<T, bool>> express);
        //decimal GetNextVal(string seqName);
        #endregion

        //string GetError(int type = 0);
    }
}
