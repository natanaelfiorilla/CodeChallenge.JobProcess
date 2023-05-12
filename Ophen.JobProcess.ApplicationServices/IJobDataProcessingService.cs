using System;
using System.Threading.Tasks;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.DomainServices
{
    public interface IJobDataProcessingService
    {
        Task ProcessJob(Job job);
    }
}
