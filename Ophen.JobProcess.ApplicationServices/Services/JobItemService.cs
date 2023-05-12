using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;

namespace Ophen.JobProcess.ApplicationServices.Services
{
    public class JobItemService : IJobItemService
    {
        private readonly IJobItemRep _jobItemRep;
        private readonly ILogger<JobItemService> _logger;
        private readonly DictionaryConfig _dictionary;
        private readonly IJobDataProcessingService _jobDataProcessingService;

        public JobItemService(IJobItemRep jobItemRep,
                          ILogger<JobItemService> logger,
                          IOptions<DictionaryConfig> dictionary,
                          IJobDataProcessingService jobDataProcessingService)
        {
            _jobItemRep = jobItemRep;
            _dictionary = dictionary.Value;
            _logger = logger;
            _jobDataProcessingService = jobDataProcessingService;
        }

        public async Task<IEnumerable<JobItem>> GetJobItems(int jobId)
        {
            try
            {
                var jobItems = await _jobItemRep.GetAllByJob(jobId);

                if (jobItems == null
                    || jobItems.ToList().Count() < 1) throw new ArgumentException(_dictionary.JobNotExists);

                return jobItems;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToGetJobItems);
                throw;
            }
        }
    }
}
