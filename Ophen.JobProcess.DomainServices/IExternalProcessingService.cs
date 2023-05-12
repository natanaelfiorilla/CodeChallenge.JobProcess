using System.Threading.Tasks;

namespace Ophen.JobProcess.DomainServices
{
    public interface IExternalProcessingService
    {
        Task<IExternalProcessingStatus> Process(string data);
    }

    public interface IExternalProcessingStatus
    {
        public bool Sucess { get; set; }

        public string Message { get; set; }
    }
}
