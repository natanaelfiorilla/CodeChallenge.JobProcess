namespace Ophen.JobProcess.Domain.DTO
{
    public class JobStatusDTO
    {
        public int JobId { get; set; }
        public int TotalNumItems { get; set; }
        public int ProcessedNumItems { get; set; }
        public int FailedNumItems { get; set; }
    }
}
