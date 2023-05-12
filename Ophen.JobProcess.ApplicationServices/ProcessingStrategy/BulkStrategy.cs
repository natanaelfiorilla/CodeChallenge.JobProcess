﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;

namespace Ophen.JobProcess.ApplicationServices.ProcessingStrategy
{
    public class BulkStrategy : BaseStrategy
    {
        public BulkStrategy(IJobItemRep jobItemRep,
                            ILogger<BulkStrategy> logger,
                            IOptions<DictionaryConfig> dictionary,
                            IExternalProcessingService externalProcessingService):base(jobItemRep,
                                                                                        logger,
                                                                                        dictionary,
                                                                                        externalProcessingService)
        { }

        public override async Task Process(IEnumerable<JobItem> jobItems)
        {
            try
            {
                if (jobItems == null || jobItems.ToList().Count <= 0) throw new ArgumentNullException("JobItems");

                foreach (var jobItem in jobItems)
                {
                    await ExecuteExternalProcessing(jobItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToProcessJobItem);
                throw;
            }
        }
    }
}
