using System.Collections.Generic;
using System.Threading.Tasks;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.DomainServices
{
    public interface IJobItemRep : IRep<JobItem>
    {
        Task<IEnumerable<JobItem>> GetAllByJob(int jobId);
    }
}
