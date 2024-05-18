using AutoMapper;
using DatingAPI.DTO;
using DatingAPI.Extensions;
using DatingAPI.Models;

namespace DatingAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, MemberDto>()
                //map photoUrl in MemberDto with url of photo in a list of photo whose IsMain = true
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(photo => photo.IsMain).Url))
                // Map age in MemberDto with age calculated by dob in User
                .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotoDto>();
            CreateMap<MemberUpdateDto, User>();
            CreateMap<RegisterDto, User>();
        }
    }
}
