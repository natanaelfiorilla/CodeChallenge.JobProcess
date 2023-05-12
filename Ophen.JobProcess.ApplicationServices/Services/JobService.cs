using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.DTO;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;

namespace Ophen.JobProcess.ApplicationServices.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRep _jobRep;
        private readonly ILogger<JobService> _logger;
        private readonly DictionaryConfig _dictionary;
        private readonly IJobDataProcessingService _jobDataProcessingService;

        public JobService(IJobRep jobRep,
                          ILogger<JobService> logger, 
                          IOptions<DictionaryConfig> dictionary,
                          IJobDataProcessingService jobDataProcessingService)
        {
            _jobRep = jobRep;
            _dictionary = dictionary.Value;
            _logger = logger;
            _jobDataProcessingService = jobDataProcessingService;
        }

        public async Task<int> Create(Job job)
        {
            try
            {
                var validationResult = Validate(job);

                if (validationResult.Count > 0) throw new ArgumentException(String.Join(";",validationResult));

                Job newJob = new Job()
                {
                    Type = job.Type,
                };

                foreach (var jobItem in job.Items)
                {
                    JobItem newJobItem = new JobItem()
                    {
                        Data = jobItem.Data
                    };

                    newJob.Items.Add(newJobItem);
                }

                _jobRep.Add(job);

                await _jobRep.SaveChangesAsync();

                await _jobDataProcessingService.ProcessJob(job);

                return job.Id;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToCreateJob);
                throw;
            }
        }

        public async Task<JobStatusDTO> GetJobStatus(int jobId)
        {
            try
            {
                var job = await _jobRep.GetById(jobId);

                if(job == null) throw new ArgumentException(_dictionary.JobNotExists);

                return GetStatusDTO(job);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToGetJob);
                throw;
            }
        }

        private JobStatusDTO GetStatusDTO(Job job)
        {
            return new JobStatusDTO()
            {
                TotalNumItems = job.Items.Count(),
                ProcessedNumItems = job.Items.Count(ji => ji.Status == JobItemStatus.Success),
                FailedNumItems = job.Items.Count(ji => ji.Status == JobItemStatus.Failure)
            };
        }

        private List<string> Validate(Job job)
        {
            List<string> result = new List<string>();

            if (job == null)
            {
                result.Add(_dictionary.JobNull);
                return result;
            }
            if (job.Items == null || job.Items.Count <= 0) result.Add(_dictionary.JobItemNull);
            if (!Enum.IsDefined(typeof(JobType), job.Type)) result.Add(_dictionary.JobTypeNotValid);
            return result;

        }
    }
}
