using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.API.Models
{
    public class JobItemLogModel
    {
        public int JobItemId { get; set; }
        public int JobId { get; set; }
        public string Log { get; set; }
        public JobItemStatus Status { get; set; }
    }
}