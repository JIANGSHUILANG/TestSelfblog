using Selfblog.DomainObject;
using Selfblog.EF;
using Selfblog.Infrastructure;
using Selfblog.IService.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.IService
{
    public interface IarticleService : IService<articleDomainObject, article>
    {
        PageOfItems<articleDomainObject> Getarticles(int pageIndex, int pageSize, int categroy_id = 0, string author = null,
            string click = null,
            string comment = null);

        articleDomainObject GetarticleInfo(Expression<Func<articleDomainObject, bool>> whereT);
    }
}
