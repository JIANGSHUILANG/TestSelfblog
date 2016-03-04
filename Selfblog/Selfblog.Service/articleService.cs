using Selfblog.DomainObject;
using Selfblog.EF;
using Selfblog.Infrastructure;
using Selfblog.IService;
using Selfblog.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Service
{
    public class articleService : Service<articleDomainObject, article>, IarticleService
    {


        //http://www.tuicool.com/articles/AZR3ua2   :好的Linq文章
        /// <summary>
        /// 获取文章信息以及相关信息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="categroy_id">文章分类ID</param>
        /// <returns></returns>
        public Infrastructure.PageOfItems<articleDomainObject> Getarticles(int pageIndex, int pageSize, int categroy_id = 0, string author = null, string click = null, string comment = null)
        {

            //string.IsNullOrWhiteSpace(click)
            //    string.IsNullOrWhiteSpace(comment)

            selfblog__Entities context = (selfblog__Entities)base.db;

            #region LINQ TO SQL

            var info = (from c in context.article
                        select new articleDomainObject
                        {
                            article_author = c.article_author,
                            article_id = c.article_id,
                            article_name = c.article_name,
                            article_time = c.article_time,
                            article_click = c.article_click,
                            category_id = c.category_id.Value,
                            article_up = c.article_up,
                            article_type = c.article_type.Value,
                            article_support = c.article_support,
                            article_content = c.article_content,
                            article_goodup = c.article_goodup.Value,
                            article_baddown = c.article_baddown.Value,
                            comment_count = (from u in context.user_comment where c.article_id == u.article_id select u).Count(),
                            photo = (from p in context.photo
                                     join g in context.category
                                     on p.photo_id equals g.photo_id into temp
                                     from t in temp.DefaultIfEmpty()
                                     where c.category_id == t.category_id
                                     select new photoDomainObject
                                     {
                                         photo_id = p.photo_id,
                                         photo_imageurl = p.photo_imageurl,
                                         photo_typeid = p.photo_typeid,
                                         other = t.category_name
                                     }).FirstOrDefault()

                        });

            #endregion

            var list = new PageOfItems<articleDomainObject>()
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                TotalItemCount = info.Count()
            };


            IQueryable<articleDomainObject> result = null;

            #region 筛选条件以及排序条件查询

            if (categroy_id != 0) //分类查询
            {
                result = info.Where(c => c.category_id == categroy_id);
                list.TotalItemCount = result.Count();
                result = result.OrderByDescending<articleDomainObject, int?>(c => c.article_click.Value).Skip<articleDomainObject>((pageIndex - 1) * pageSize).Take<articleDomainObject>(pageSize);
            }
            else if (!string.IsNullOrWhiteSpace(author))//按作者查询
            {
                result = info.Where(c => c.article_author.Contains(author));
                list.TotalItemCount = result.Count();
                result = result.OrderByDescending<articleDomainObject, DateTime>(c => c.article_time).Skip<articleDomainObject>((pageIndex - 1) * pageSize).Take<articleDomainObject>(pageSize);
            }
            else if (!string.IsNullOrWhiteSpace(click))//按点击量查询
            {
                result = info.OrderByDescending<articleDomainObject, int?>(c => c.article_click.Value).Skip<articleDomainObject>((pageIndex - 1) * pageSize).Take<articleDomainObject>(pageSize);
            }
            else if (!string.IsNullOrWhiteSpace(comment))//按评论量查询
            {
                result = info.OrderByDescending<articleDomainObject, int?>(c => c.comment_count.Value).Skip<articleDomainObject>((pageIndex - 1) * pageSize).Take<articleDomainObject>(pageSize);
            }
            else//All
            {
                result = info.OrderByDescending<articleDomainObject, DateTime>(c => c.article_time).Skip<articleDomainObject>((pageIndex - 1) * pageSize).Take<articleDomainObject>(pageSize);
            }

            #endregion

            list.AddRange(result.ToList());

            return list;
        }


        public articleDomainObject GetarticleInfo(Expression<Func<articleDomainObject, bool>> whereT)
        {
            selfblog__Entities context = (selfblog__Entities)base.db;

            #region LINQ TO SQL

            var info = (from c in context.article
                        select new articleDomainObject
                        {
                            article_author = c.article_author,
                            article_id = c.article_id,
                            article_name = c.article_name,
                            article_time = c.article_time,
                            article_click = c.article_click,
                            category_id = c.category_id.Value,
                            article_up = c.article_up,
                            article_type = c.article_type.Value,
                            article_support = c.article_support,
                            article_content = c.article_content,
                            article_goodup = c.article_goodup.Value,
                            article_baddown = c.article_baddown.Value,
                            article_status = c.article_status,
                            comment_count = (from u in context.user_comment where c.article_id == u.article_id select u).Count(),
                            photo = (from p in context.photo
                                     join g in context.category
                                     on p.photo_id equals g.photo_id into temp
                                     from t in temp.DefaultIfEmpty()
                                     where c.category_id == t.category_id
                                     select new photoDomainObject
                                     {
                                         photo_id = p.photo_id,
                                         photo_imageurl = p.photo_imageurl,
                                         photo_typeid = p.photo_typeid,
                                         other = t.category_name
                                     }).FirstOrDefault()

                        }).Where(whereT.Compile());

            #endregion

            return info.FirstOrDefault();
        }
    }
}
