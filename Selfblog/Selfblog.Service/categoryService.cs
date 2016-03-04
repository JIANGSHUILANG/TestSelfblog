using AutoMapper;
using Selfblog.DomainObject;
using Selfblog.EF;
using Selfblog.IService;
using Selfblog.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Service
{
    public class categoryService : Service<categoryDomainObject, category>, IcategoryService
    {
        public List<categoryDomainObject> GetCategorycoverarticlecount()
        {
            selfblog__Entities context = (selfblog__Entities)base.db;
            var result = (from c in context.category
                          select new categoryDomainObject
                          {
                              category_id = c.category_id,
                              category_name = c.category_name,
                              partent_id=c.partent_id,
                              articleDomainObjects = (from a in context.article
                                                      where c.category_id == a.category_id
                                                      select new articleDomainObject
                                                      {
                                                          article_id = a.article_id,
                                                          article_name = a.article_name,
                                                          article_author = a.article_author,
                                                          article_codeurl = a.article_codeurl,
                                                          article_time = a.article_time,
                                                          article_click = a.article_click,
                                                          category_id = a.category_id.Value,
                                                          type_id = a.type_id,
                                                          article_type = a.article_type.Value,
                                                          article_content = a.article_content,
                                                          article_up = a.article_up,
                                                          article_support = a.article_support,
                                                          article_status = a.article_status,
                                                          comment_count = (from u in context.user_comment where a.article_id == u.article_id select u).Count()
                                                      }).ToList()
                          }).ToList();
            return result;

        }
    }
}
