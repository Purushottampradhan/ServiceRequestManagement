using AutoMapper;
using ServiceRequestAPI.DTOs;
using ServiceRequestAPI.Models;

namespace ServiceRequestAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ServiceRequest, ServiceRequestDto>().ReverseMap();
            CreateMap<CreateServiceRequestDto, ServiceRequest>();
        }
    }
}