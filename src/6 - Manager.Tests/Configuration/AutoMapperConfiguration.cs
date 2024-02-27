using AutoMapper;
using Manager.Domain.Entities;
using Manager.Services.DTO;

namespace Manager.Tests.Configuration
{
    public class AutoMapperConfiguration
    {
        public static IMapper GetConfiguration() 
        {
            var autoMapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
            });

            return autoMapperConfig.CreateMapper();
        }
    }
}