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
    public class JobRep : BaseRep, IJobRep
    {
        private readonly ILogger<JobRep> _logger;
        private readonly DictionaryConfig _dictionary;

        public JobRep(JobProcessContext context,
                        IOptions<DictionaryConfig> dictionary,
                        ILogger<JobRep> logger) : base(context)
        {
            _logger = logger;
            _dictionary = dictionary.Value;
        }

        public void Add(Job newObject)
        {
            try
            {
                this._context.Jobs.Add(newObject);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToCreateJob);
                throw;
            }
        }

        public int Count()
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Job> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Job> GetById(int id)
        {
            try
            {
                return await (from j in _context.Jobs
                              where j.Id == id
                              select j)
                              .Include(j => j.Items)
                              .FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, _dictionary.FailToGetJob);
                throw;
            }
        }

        public void Update(Job newObject)
        {
            throw new System.NotImplementedException();
        }
    }
}
