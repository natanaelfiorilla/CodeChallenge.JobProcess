namespace Ophen.JobProcess.API.Models
{
    public class JobStatusModel
    {
        public int JobId { get; set; }
        public int TotalNumItems { get; set; }
        public int ProcessedNumItems { get; set; }
        public int FailedNumItems { get; set; }
    }
}
