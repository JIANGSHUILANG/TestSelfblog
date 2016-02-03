using AutoMapper;
using Selfblog.DomainObject;
using Selfblog.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Infrastructure
{
    public class AutoMapperConfiguration
    {
        public static void Initiaztion()
        {

            Mapper.CreateMap<categoryDomainObject, category>();
            Mapper.CreateMap<category, categoryDomainObject>();

            Mapper.CreateMap<articleDomainObject, article>();
            Mapper.CreateMap<article, articleDomainObject>();

            Mapper.CreateMap<user_commentDomainObject, user_comment>();
            Mapper.CreateMap<user_comment, user_commentDomainObject>();

            Mapper.CreateMap<photoDomainObject, photo>();
            Mapper.CreateMap<photo, photoDomainObject>();
           
            //  Mapper.CreateMap<articleDomainObject, article>()
            //      .ForMember(dest=>dest.)
            //      ;


            //Mapper.CreateMap<article, articleDomainObject>();


            //        apper.CreateMap<DummySource, DummyDestination>()
            //.ForMember(dest => dest.Dummy, opt => opt.MapFrom(src => (int?)src.Dummy));
        }
    }
}
