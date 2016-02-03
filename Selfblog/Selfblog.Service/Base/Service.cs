using AutoMapper;
using AutoMapper.QueryableExtensions;
using Selfblog.EF.BaseEntity;
using Selfblog.Infrastructure;
using Selfblog.Infrastructure._Interface;
using Selfblog.IService.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Service.Base
{
    public class Service<DomainObject, TEntity> : IService<DomainObject, TEntity>
        where DomainObject : class,new()
        where TEntity : Root
    {
        IBaseService<TEntity> Baseservice = new BaseService<TEntity>();

        public DbContext db
        {
            get { return Baseservice.db; }
        }

        #region SELECT
        private Expression<Func<TEntity, S>> GetTEntityLamda<S>(System.Linq.Expressions.Expression<Func<DomainObject, S>> whereLambda)
        {
            //Func<DomainObject, bool> func = whereLambda.Compile();
            Func<DomainObject, S> func = whereLambda.Compile();

            Func<TEntity, DomainObject> mapper = Extensions.CreateMapExpression<TEntity, DomainObject>(Mapper.Engine).Compile();

            //得到领域Model相关的lamada
            Expression<Func<TEntity, S>> lamada = ef_t => func(mapper(ef_t));

            return lamada;
        }
        //http://www.tuicool.com/articles/qq2q6fA :还可使用链接 ：AutoMapper扩展
        public virtual IQueryable<DomainObject> LoadEntities(System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda)
        {
            var expression = GetTEntityLamda<bool>(whereLambda);
            var list = Baseservice.LoadEntities(expression).ToList();
            //var list = Baseservice.LoadEntities(expression).Project().To<DomainObject>().ToList();
            return Mapper.Map<List<TEntity>, List<DomainObject>>(list).AsQueryable();

        }

        public virtual IQueryable<DomainObject> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda, System.Linq.Expressions.Expression<Func<DomainObject, s>> orderbyLambad, bool isAsc)
        {
            var expressionwherelamda = GetTEntityLamda<bool>(whereLambda);
            var expressionorderlamda = GetTEntityLamda<s>(orderbyLambad);
            List<TEntity> list = Baseservice.LoadPageEntities<s>(pageIndex, pageSize, out totalCount, expressionwherelamda, expressionorderlamda, isAsc).ToList();
            return Mapper.Map<List<TEntity>, List<DomainObject>>(list).AsQueryable();
        }

        public virtual IQueryable<DomainObject> LoadPageEntities<s>(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda, System.Linq.Expressions.Expression<Func<DomainObject, s>> orderbyLambad, bool isAsc)
        {
            var expressionwherelamda = GetTEntityLamda<bool>(whereLambda);
            var expressionorderlamda = GetTEntityLamda<s>(orderbyLambad);
            List<TEntity> list = Baseservice.LoadPageEntities<s>(pageIndex, pageSize, expressionwherelamda, expressionorderlamda, isAsc).ToList();
            return Mapper.Map<List<TEntity>, List<DomainObject>>(list).AsQueryable();
        }

        public virtual PageOfItems<DomainObject> PageOfItemsLoadPageEntites<s>(int pageIndex, int pageSize, System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda, System.Linq.Expressions.Expression<Func<DomainObject, s>> orderbyLambad, bool isAsc = true)
        {
            var expressionwherelamda = GetTEntityLamda<bool>(whereLambda);
            var expressionorderlamda = GetTEntityLamda<s>(orderbyLambad);
            PageOfItems<TEntity> list = Baseservice.PageOfItemsLoadPageEntites<s>(pageIndex, pageSize, expressionwherelamda, expressionorderlamda, isAsc);

            PageOfItems<DomainObject> result = Mapper.Map<PageOfItems<TEntity>, PageOfItems<DomainObject>>(list);
            result.PageNumber = list.PageNumber;
            result.PageSize = list.PageSize;
            result.TotalItemCount = list.TotalItemCount;
            return result;
        }
        #endregion

        #region UPDATE

        public virtual bool UpdateEntity(DomainObject entity)
        {
            var temp = AutoMapper.Mapper.Map<DomainObject, TEntity>(entity);
            return Baseservice.UpdateEntity(temp);
        }

        public virtual bool UpdatePartProperty(DomainObject entity, string[] PropertyNames)
        {
            var temp = AutoMapper.Mapper.Map<DomainObject, TEntity>(entity);
            return Baseservice.UpdatePartProperty(temp, PropertyNames);
        }

        #endregion

        #region CREATE

        public virtual DomainObject AddEntity(DomainObject entity)
        {
            var temp = Baseservice.AddEntity(AutoMapper.Mapper.Map<DomainObject, TEntity>(entity));
            return AutoMapper.Mapper.Map<TEntity, DomainObject>(temp);
        }

        public virtual int AddEntity(IEnumerable<DomainObject> entites)
        {
            List<TEntity> list = new List<TEntity>();
            foreach (var item in entites)
            {
                var temp = AutoMapper.Mapper.Map<DomainObject, TEntity>(item);
                list.Add(temp);
            }

            return Baseservice.AddEntity(list.AsEnumerable());
        }

        #endregion

        #region DELETE

        public virtual int DeleteEntity(DomainObject entity)
        {
            return Baseservice.DeleteEntity(AutoMapper.Mapper.Map<DomainObject, TEntity>(entity));
        }

        public virtual int DeleteEntity(System.Linq.Expressions.Expression<Func<DomainObject, bool>> whereLambda)
        {
            var expriession = GetTEntityLamda<bool>(whereLambda);
            return Baseservice.DeleteEntity(expriession);
        }

        public virtual int DeleteEntity(IEnumerable<DomainObject> entities)
        {
            List<TEntity> list = new List<TEntity>();
            foreach (var item in entities)
            {
                var temp = AutoMapper.Mapper.Map<DomainObject, TEntity>(item);
                list.Add(temp);
            }

            return Baseservice.DeleteEntity(list.AsEnumerable());
        }
        #endregion
    }
}
