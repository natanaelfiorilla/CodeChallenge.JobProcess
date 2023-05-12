using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ophen.JobProcess.ApplicationServices.ProcessingStrategy;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;

namespace Ophen.JobProcess.ApplicationServices.Services
{
    public class JobDataProcessingService : IJobDataProcessingService
    {
        private readonly IStrategyContext _strategyContext;
        private readonly ILogger<JobDataProcessingService> _logger;
        private readonly DictionaryConfig _dictionary;
        private readonly IServiceProvider _serviceProvider;

        public JobDataProcessingService(IStrategyContext strategyContext,
                                        ILogger<JobDataProcessingService> logger,
                                        IOptions<DictionaryConfig> dictionary,
                                        IServiceProvider serviceProvider)
        {
            _strategyContext = strategyContext;
            _dictionary = dictionary.Value;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task ProcessJob(Job job)
        {
            try
            {
                Validate(job);

                IStrategy strategy = DefineStrategy(job);

                if (strategy == null) throw new Exception(_dictionary.StrategyNotFound);

                _strategyContext.SetStrategy(strategy);

                await _strategyContext.ExecuteStrategy(job.Items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToCreateJob);
                throw;
            }
        }

        private IStrategy DefineStrategy(Job job)
        {
             switch (job.Type)
            {
                case JobType.Batch:
                    return (IStrategy)_serviceProvider.GetService(typeof(BatchStrategy));

                case JobType.Bulk:
                    return (IStrategy)_serviceProvider.GetService(typeof(BulkStrategy));

                default:
                    return null;
            }
        }

        private void Validate(Job job)
        {
            if (job == null) throw new ArgumentNullException("Job");
            if (job.Items == null || job.Items.Count <= 0) throw new ArgumentNullException("JobItems");
        }
    }
}
