using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;

namespace Ophen.JobProcess.ApplicationServices.ProcessingStrategy
{
    public abstract class BaseStrategy : IStrategy
    {
        protected readonly DictionaryConfig _dictionary;
        protected readonly IJobItemRep _jobItemRep;
        protected readonly ILogger<IStrategy> _logger;
        protected readonly IExternalProcessingService _externalProcessingService;


        protected BaseStrategy (IJobItemRep jobItemRep,
                            ILogger<IStrategy> logger,
                            IOptions<DictionaryConfig> dictionary,
                            IExternalProcessingService externalProcessingService)
        {            
            _jobItemRep = jobItemRep;
            _logger = logger;
            _dictionary = dictionary.Value;
            _externalProcessingService = externalProcessingService;
        }

        protected void SetFailureStatus(JobItem jobItem, string resultDescription)
        {
            jobItem.Status = JobItemStatus.Failure;
            jobItem.Log = resultDescription;
        }

        protected void SetSuccessStatus(JobItem jobItem)
        {
            jobItem.Status = JobItemStatus.Success;
        }

        protected void SetNoDataStatus(JobItem jobItem)
        {
            jobItem.Status = JobItemStatus.Failure;
            jobItem.Log = _dictionary.NoDataToProcess;
        }

        protected async Task ExecuteExternalProcessing(JobItem jobItem)
        {
            try
            {
                if (string.IsNullOrEmpty(jobItem.Data))
                {
                    SetNoDataStatus(jobItem);
                }
                else
                {
                    var resultDescription = string.Empty;
                    var result = await _externalProcessingService.Process(jobItem.Data);

                    if (result == null) throw new ArgumentNullException();

                    if (result.Sucess) SetSuccessStatus(jobItem);
                    else SetFailureStatus(jobItem, result.Message);
                }

                _jobItemRep.Update(jobItem);
                await _jobItemRep.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var failure = $"{_dictionary.FailToProcessJobItem} Id: {jobItem.Id}";
                _logger.LogError(ex, failure);

                SetFailureStatus(jobItem, failure);
                _jobItemRep.Update(jobItem);
                await _jobItemRep.SaveChangesAsync();
            }
        }

        public abstract Task Process(IEnumerable<JobItem> items);
    }
}
