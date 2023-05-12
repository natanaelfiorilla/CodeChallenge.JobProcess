using System.ComponentModel.DataAnnotations;

namespace Ophen.JobProcess.Domain.Entities
{
    public class JobItem
    {
        [Key]
        public int Id { get; set; }
        public Job Job { get; set; }
        public int JobId { get; set; }
        public string Data { get; set; }
        public string Log { get; set; }
        public JobItemStatus Status { get; set; }

        public JobItem() => this.Status = JobItemStatus.NotProcessed;
    }
}
