using AutoMapper;
using Ophen.JobProcess.Domain.DTO;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JobModel, Job>();
            CreateMap<JobItemModel, JobItem>();
            CreateMap<JobStatusDTO, JobStatusModel>();
            CreateMap<JobItem, JobItemLogModel>().ForMember(dest => dest.JobItemId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
