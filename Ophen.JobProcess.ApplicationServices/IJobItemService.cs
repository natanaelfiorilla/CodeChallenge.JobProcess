using System.Collections.Generic;
using System.Threading.Tasks;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.ApplicationServices
{
    public interface IJobItemService
    {
        Task<IEnumerable<JobItem>> GetJobItems(int jobId);
    }
}
