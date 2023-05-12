using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ophen.JobProcess.Domain.Entities
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        public JobType Type { get; set; }
        public List<JobItem> Items { get; set; }

        public Job()
        {
            Items = new List<JobItem>();
        }
    }
}