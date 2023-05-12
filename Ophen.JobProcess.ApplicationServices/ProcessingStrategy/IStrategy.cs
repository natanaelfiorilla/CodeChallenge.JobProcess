using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.ApplicationServices.ProcessingStrategy
{
    public interface IStrategy
    {        
        Task Process(IEnumerable<JobItem> items);
    }
}
