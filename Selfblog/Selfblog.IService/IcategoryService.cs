using Selfblog.DomainObject;
using Selfblog.EF;
using Selfblog.IService.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.IService
{
  public  interface IcategoryService:IService<categoryDomainObject,category>
    {
       List<categoryDomainObject> GetCategorycoverarticlecount();
    }
}
