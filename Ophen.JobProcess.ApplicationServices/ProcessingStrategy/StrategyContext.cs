using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.ApplicationServices.ProcessingStrategy
{
    public class StrategyContext : IStrategyContext
    {
        private IStrategy _strategy;
        public StrategyContext(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task ExecuteStrategy(IEnumerable<JobItem> items)
        {
            await _strategy.Process(items);
        }

        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }
    }
}
