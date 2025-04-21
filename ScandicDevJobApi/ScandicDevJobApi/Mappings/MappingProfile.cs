using AutoMapper;
using ScandicDevJobApi.Models;

namespace ScandicDevJobApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping configuration 
            // Model , DTO
            CreateMap<JobListing, JoblistingDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<User, UserDto>();
            CreateMap<Company, CompanyRequestDto>();

            //CreateMap<Company, CompanyResponseDto>()
            //    .ForMember(dest => dest.LogoUrl,
            //        opt => opt.MapFrom((src, dest, _, context) =>
            //            context.Options.Items["BaseUrl"] + src.BlobStorageLogoName));
        }
    }
}
