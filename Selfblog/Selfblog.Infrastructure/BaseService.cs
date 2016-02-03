using Selfblog.EF.BaseEntity;
using Selfblog.Infrastructure;
using Selfblog.Infrastructure._Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Infrastructure
{
    public partial class BaseService<T> : IBaseService<T> where T : Root
    {
        /// <summary>
        /// EF上下文
        /// </summary>
        public System.Data.Entity.DbContext db
        {
            get { return UnitOfWork.context; }
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return new UnitOfWork();
            }
            set
            {
                UnitOfWork = value;
            }
        }

        /// <summary>
        /// 扩展
        /// </summary>
        public Infrastructure._Interface.IBaseExtension extension
        {
            get { return ContextFactory.GetBaseExtension(); }
        }

        #region CURD_SELECT

        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Where(whereLambda.Compile()).AsQueryable();
        }

        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambad, bool isAsc)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var temp = db.Set<T>().Where<T>(whereLambda.Compile()).AsQueryable();
            totalCount = temp.Count();
            if (isAsc)//升序
            {
                temp = temp.OrderBy<T, s>(orderbyLambad.Compile()).AsQueryable().Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, s>(orderbyLambad.Compile()).AsQueryable().Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }

        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambad, bool isAsc)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var temp = db.Set<T>().Where<T>(whereLambda.Compile()).AsQueryable();

            if (isAsc)//升序
            {
                temp = temp.OrderBy<T, s>(orderbyLambad.Compile()).AsQueryable().Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, s>(orderbyLambad.Compile()).AsQueryable().Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }

        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <typeparam name="s">排序字段</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="whereLambda">条件</param>
        /// <param name="orderbyLambad">排序条件</param>
        /// <param name="isAsc">正序:true  倒序:false </param>
        /// <returns></returns>
        public PageOfItems<T> PageOfItemsLoadPageEntites<s>(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambad, bool isAsc = true)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var info = db.Set<T>().AsNoTracking().Where<T>(whereLambda.Compile()).AsQueryable();

            var list = new PageOfItems<T>()
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                TotalItemCount = info.Count()
            };

            IQueryable<T> orderList = null;
            if (isAsc)
            {
                orderList = info.OrderBy<T, s>(orderbyLambad.Compile()).AsQueryable().Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                orderList = info.OrderByDescending<T, s>(orderbyLambad.Compile()).AsQueryable().Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }


            list.AddRange(orderList.ToList());
            return list;
        }


        #endregion

        #region CURD_UPDATE

        public bool UpdateEntity(T entity)
        {
            UnitOfWork.RegisterModified(entity);
            return UnitOfWork.Commit() > 0;
        }

        public bool UpdatePartProperty(T entity, string[] PropertyNames)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CURD_CREATE

        public T AddEntity(T entity)
        {
            UnitOfWork.RegisterNew<T>(entity);
            UnitOfWork.Commit();
            return entity;
        }

        public int AddEntity(IEnumerable<T> entites)
        {
            foreach (var item in entites)
            {
                UnitOfWork.RegisterNew(item);
            }

            return UnitOfWork.Commit();
        }

        #endregion

        #region CURD_DELETE

        public int DeleteEntity(T entity)
        {
            UnitOfWork.RegisterDeleted(entity);
            return UnitOfWork.Commit();
        }

        public int DeleteEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {

            Func<T, bool> lamda = whereLambda.Compile();
            var del = UnitOfWork.context.Set<T>().Where(lamda);
            foreach (var entity in del)
            {
                UnitOfWork.RegisterDeleted<T>(entity);
            }
            return UnitOfWork.Commit();
        }

        public int DeleteEntity(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                UnitOfWork.RegisterDeleted(item);
            }
            return UnitOfWork.Commit();
        }

        #endregion

    }
}
