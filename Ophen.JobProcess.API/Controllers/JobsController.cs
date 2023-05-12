using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ophen.JobProcess.API.Models;
using Ophen.JobProcess.ApplicationServices;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.DTO;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IJobItemService _jobItemService;
        private readonly DictionaryConfig _dictionary;
        private readonly IMapper _mapper;
        private readonly ILogger<JobsController> _logger;
        
        public JobsController(IJobService jobService,
                              IJobItemService jobItemService,
                                IOptions<DictionaryConfig> dictionary,
                                ILogger<JobsController> logger,
                                IMapper mapper)
        {
            _jobService = jobService;
            _jobItemService = jobItemService;
            _dictionary = dictionary.Value;
            _logger = logger;
            _mapper = mapper;
        }

        // POST: api/Jobs
        /// <summary>
        /// START A TYPE OF JOB  so that the provided data can be processed.
        /// </summary>
        /// <returns>JobId:int</returns>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] JobModel jobModel)
        {
            var result = ValidateModel(jobModel);

             if (result != null) return result;

            int? jobId = null;

            try
            {
                var job = _mapper.Map<JobModel, Job>(jobModel);
                jobId = await _jobService.Create(job);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, _dictionary.FailToCreateJob);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToCreateJob);
                throw new Exception(_dictionary.InternalError);
            }

            return Ok(new {JobId = jobId});
        }

        private ActionResult ValidateModel(JobModel jobModel)
        {
            if (jobModel == null ||
                jobModel.Items == null ||
                jobModel.Items.Count <= 0)
            {
                return BadRequest(_dictionary.EmptyRequest);
            }

            return null;
        }

        // GET: api/Jobs/5/status        
        /// <summary>
        /// CHECK THE STATUS OF A JOB.
        /// </summary>
        /// <returns>JobStatus</returns>
        [HttpGet("{id}/status")]
        public async Task<ActionResult<JobStatusModel>> GetStatus(int id)
        {
            JobStatusModel result;

            try
            {
                var dto = await _jobService.GetJobStatus(id);
                result = _mapper.Map<JobStatusDTO, JobStatusModel>(dto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, _dictionary.FailToGetJob);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToGetJob);
                throw new Exception(_dictionary.InternalError);
            }

            if (result == null) return BadRequest();

            return Ok(result);
        }

        // GET: api/Jobs/5/logs      
        /// <summary>
        ///  GET LOGS FROM A JOB.
        /// </summary>
        /// <returns>JobStatus</returns>
        [HttpGet("{id}/logs")]
        public async Task<ActionResult<JobLogModel>> GetLogs(int id)
        {
            JobLogModel result;

            try
            {
                var jobItems = await _jobItemService.GetJobItems(id);
                result = new JobLogModel()
                {
                    Logs = _mapper.Map<List<JobItem>, List<JobItemLogModel>>(jobItems.ToList())
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, _dictionary.FailToGetJobItems);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToGetJobItems);
                throw new Exception(_dictionary.InternalError);
            }

            if (result == null) return BadRequest();

            return Ok(result);
        }
    }
}
