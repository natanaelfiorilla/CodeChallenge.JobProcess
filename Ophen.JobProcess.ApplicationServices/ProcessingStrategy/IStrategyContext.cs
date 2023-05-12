using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.ApplicationServices.ProcessingStrategy
{
    public interface IStrategyContext
    {
        public void SetStrategy(IStrategy strategy);
        Task ExecuteStrategy(IEnumerable<JobItem> items);
    }
}
