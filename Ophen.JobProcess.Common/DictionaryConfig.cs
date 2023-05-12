using System;
namespace Ophen.JobProcess.Common
{
    public class DictionaryConfig
    {
        public string EmptyRequest { get; set; } = "Empty Request.";
        public string FailToCreateJob { get; set; } = "Failed to create a job.";
        public string InternalError { get; set; } = "Internal Server Error.";
        public string JobNull { get; set; } = "Job cannot be null";
        public string JobItemNull { get; set; } = "JobItems cannot be null or empty";
        public string StrategyNotFound { get; set; } = "Unable to detect a strategy to process job.";
        public string NoDataToProcess { get; set; } = "No Data to process.";
        public string FailToProcessJobItem { get; set; } = "Failed to process job Item.";
        public string JobTypeNotValid { get; set; } = "Job Type not valid.";
        public string FailToGetJob { get; set; } = "Fail to get job information.";
        public string JobNotExists { get; set; } = "Non-existent job.";
        public string FailToGetJobItems { get; set; } = "Fail to get job items information.";
    }
}
