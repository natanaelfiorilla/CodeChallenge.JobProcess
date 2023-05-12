using System.Collections.Generic;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.API.Models
{
    public class JobModel
    {        
        public int Id { get; set; }
        public int Type { get; set; }
        public List<JobItemModel> Items { get; set; }

    }
}
