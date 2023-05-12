using System.Collections.Generic;
using System.Threading.Tasks;
using Ophen.JobProcess.Domain.DTO;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.ApplicationServices
{
    public interface IJobService
    {
        Task<int> Create(Job job);

        Task<JobStatusDTO> GetJobStatus(int jobId);
    }
}
