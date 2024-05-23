using AutoMapper;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;

namespace SimpleBankingSystemAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserProfileDto>();
            CreateMap<UpdateUserProfileRequest, User>();
        }
    }
}
