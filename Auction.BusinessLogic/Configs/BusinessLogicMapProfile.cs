using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Mappers;

namespace Auction.BusinessLogic.Configs
{
    public class BusinessLogicMapProfile : Profile
    {
        public BusinessLogicMapProfile()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddConditionalObjectMapper().Where((s, d) => s.Name == d.Name + "DTO");
            });
        }
    }
}
