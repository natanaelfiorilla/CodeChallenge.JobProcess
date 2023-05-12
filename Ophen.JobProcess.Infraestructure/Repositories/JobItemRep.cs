using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace Ophen.JobProcess.Infraestructure.Repositories
{
    public class JobItemRep : BaseRep, IJobItemRep
    {
        private readonly ILogger<JobItemRep> _logger;
        private readonly DictionaryConfig _dictionary;

        public JobItemRep(JobProcessContext context,
                        IOptions<DictionaryConfig> dictionary,
                        ILogger<JobItemRep> logger) : base(context)
        {
            _logger = logger;
            _dictionary = dictionary.Value;
        }

        public void Add(JobItem newObject)
        {
            throw new System.NotImplementedException();
        }

        public int Count()
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<JobItem> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Update(JobItem newObject)
        {
            try
            {
                this._context.JobItems.Update(newObject);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToCreateJob);
                throw;
            }
        }

        public Task<JobItem> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<JobItem>> GetAllByJob(int jobId)
        {
            try
            {
                return await(from j in _context.JobItems
                             where j.JobId == jobId
                             select j)
                              .ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToGetJobItems);
                throw;
            }
        }
    }
}
