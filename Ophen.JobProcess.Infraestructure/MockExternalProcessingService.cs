using System;
using System.Threading.Tasks;
using Ophen.JobProcess.DomainServices;

namespace Ophen.JobProcess.Infraestructure
{
    public class MockExternalProcessingService: IExternalProcessingService
    {
        public async Task<IExternalProcessingStatus> Process(string data)
        {
            var rand = new Random();

            await Task.Delay(500);

            switch (rand.Next(11))
            {
                case < 5:
                    return new ExternalProcessingStatus() { Sucess = false, Message = "Data Error" };                    
                case < 10:                     
                    return new ExternalProcessingStatus() { Sucess = true, Message = string.Empty };
                default:
                    throw new Exception();
            }
        }
    }
    public class ExternalProcessingStatus: IExternalProcessingStatus
    {
        public bool Sucess { get; set; }

        public string Message { get; set; }
    }
}
