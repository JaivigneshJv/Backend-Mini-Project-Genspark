using AutoMapper;
using SimpleBankingSystemAPI.Models.DTOs;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
