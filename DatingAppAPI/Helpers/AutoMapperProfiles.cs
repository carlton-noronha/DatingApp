using AutoMapper;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extentions;

namespace DatingAppAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>().ForMember(
                dest => dest.PhotoUrl, 
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(photo => photo.IsMain).Url)
            ).
            ForMember(
                dest => dest.Age, 
                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())
            );
            CreateMap<Photo, PhotoDTO>();
        }
    }
}