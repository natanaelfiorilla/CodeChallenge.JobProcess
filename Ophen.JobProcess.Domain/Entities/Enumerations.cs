namespace Ophen.JobProcess.Domain.Entities
{
    public enum JobType
    {
        Bulk,
        Batch
    }

    public enum JobItemStatus
    {
        Success,
        Failure,
        NotProcessed
    }
}
