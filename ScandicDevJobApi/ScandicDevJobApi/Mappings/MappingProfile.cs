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
            CreateMap<Company, CompanyDto>();


        }
    }
}
