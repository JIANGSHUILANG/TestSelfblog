using Selfblog.DomainObject;
using Selfblog.EF;
using Selfblog.Infrastructure;
using Selfblog.IService;
using Selfblog.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Service
{
    public class user_commentService : Service<user_commentDomainObject, user_comment>, Iuser_commentService
    {
        public Infrastructure.PageOfItems<user_commentDomainObject> GetComments(int pageIndex, int pageSize)
        {
            selfblog__Entities context = (selfblog__Entities)base.db;

            var info = from c in context.user_comment
                       join u in context.user
                       on c.user_id equals u.user_id into temp
                       from t in temp.DefaultIfEmpty()
                       join a in context.article
                       on c.article_id equals a.article_id into temp1
                       from t1 in temp1.DefaultIfEmpty()
                       where c.rec_status == 0
                       select new user_commentDomainObject
                       {
                           username = t.user_name,
                           userheadimage = t.user_image_url,
                           article_name = t1.article_name,
                           comment = c.comment,
                           article_id = c.article_id.Value,
                           user_id = c.user_id,
                           rec_status = c.rec_status,
                           comment_time = c.comment_time,
                           c_id = c.c_id,
                           chirden = (from r in context.user_comment
                                      where r.rec_status == 1 && r.rec_id == c.c_id
                                      select new user_commentChirden
                                      {
                                          rec_time = r.comment_time,
                                          rec_comment = r.comment,
                                          username = (from n in context.user where n.user_id == r.user_id select n.user_name).FirstOrDefault(),
                                          userheadimage = t.user_image_url,
                                          rec_id = r.rec_id.Value
                                      }).ToList()
                           //rec_id = c.c_id,
                           //rec_time = (from r in context.user_comment where r.rec_status == 1 && r.rec_id == c.c_id select r.comment_time).FirstOrDefault(),
                           //rec_comment = (from r in context.user_comment where r.rec_status == 1 && r.rec_id == c.c_id select r.comment).FirstOrDefault()
                       };


            var list = new PageOfItems<user_commentDomainObject>()
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                TotalItemCount = info.Count()
            };

            var result = info.OrderByDescending<user_commentDomainObject, DateTime>(c => c.comment_time).Skip<user_commentDomainObject>((pageIndex - 1) * pageSize).Take<user_commentDomainObject>(pageSize);

            list.AddRange(result.ToList());

            return list;

        }
    }
}
