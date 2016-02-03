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
    public class userService : Service<userDomainObject, user>, IuserService
    {
    }
}
