using Selfblog.DomainObject;
using Selfblog.EF;
using Selfblog.Infrastructure;
using Selfblog.IService.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.IService
{
    public interface Iuser_commentService : IService<user_commentDomainObject, user_comment>
    {
        PageOfItems<user_commentDomainObject> GetComments(int pageIndex, int pageSize); 
    }
}
