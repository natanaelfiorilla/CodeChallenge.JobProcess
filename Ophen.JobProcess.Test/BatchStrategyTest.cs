using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Ophen.JobProcess.ApplicationServices.ProcessingStrategy;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;
using Ophen.JobProcess.Infraestructure;

namespace Ophen.JobProcess.Test
{
    [TestFixture()]
    public class BatchStrategyTest
    {
        [Test()]
        public void Process_JobItemsNull_ArgumentException()
        {
            var sut = GetSut(out Mock<IJobItemRep> jobItemRepMock,
                                    out Mock<ILogger<BatchStrategy>> loggerMock,
                                    out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                    out Mock<IExternalProcessingService> externalProcessingServiceMock);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.Process(null));
        }

        [Test()]
        public void Process_JobItemsEmpty_ArgumentException()
        {
            var sut = GetSut(out Mock<IJobItemRep> jobItemRepMock,
                                    out Mock<ILogger<BatchStrategy>> loggerMock,
                                    out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                    out Mock<IExternalProcessingService> externalProcessingServiceMock);

            var jobItemsFake = new List<JobItem>();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.Process(jobItemsFake));
        }

        [Test()]
        public async Task ExecuteExternalProcessing_JobItemNoData_NotCallingExternalService()
        {
            var sut = GetSut(out Mock<IJobItemRep> jobItemRepMock,
                                    out Mock<ILogger<BatchStrategy>> loggerMock,
                                    out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                    out Mock<IExternalProcessingService> externalProcessingServiceMock);

            var jobItemsFake = new List<JobItem>() { new JobItem() };

            externalProcessingServiceMock.Setup(e => e.Process(It.IsAny<string>())).Verifiable();

            await sut.Process(jobItemsFake);

            externalProcessingServiceMock.Verify(e => e.Process(It.IsAny<string>()), Times.Never);      
        }

        [Test()]
        public async Task ExecuteExternalProcessing_BatchProcessingFails_CallExternalServiceOnlyOnce()
        {
            var sut = GetSut(out Mock<IJobItemRep> jobItemRepMock,
                                    out Mock<ILogger<BatchStrategy>> loggerMock,
                                    out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                    out Mock<IExternalProcessingService> externalProcessingServiceMock);

            var jobItemsFake = new List<JobItem>() { new JobItem() { Data = "xxxxxxxx" }, new JobItem() { Data = "xxxxxxxx" } };

            var externalProcessingStatusFake = new ExternalProcessingStatus() { Sucess = false };

            externalProcessingServiceMock.Setup(e => e.Process(It.IsAny<string>())).ReturnsAsync(externalProcessingStatusFake);

            await sut.Process(jobItemsFake);

            externalProcessingServiceMock.Verify(e => e.Process(It.IsAny<string>()), Times.Once);
        }

        [Test()]
        public async Task ExecuteExternalProcessing_JobItemWithData_CallExternalService()
        {
            var sut = GetSut(out Mock<IJobItemRep> jobItemRepMock,
                                    out Mock<ILogger<BatchStrategy>> loggerMock,
                                    out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                    out Mock<IExternalProcessingService> externalProcessingServiceMock);

            var jobItemsFake = new List<JobItem>() { new JobItem() {Data = "xxxxxxxx" } };

            externalProcessingServiceMock.Setup(e => e.Process(It.IsAny<string>())).Verifiable();

            await sut.Process(jobItemsFake);

            externalProcessingServiceMock.Verify(e => e.Process(It.IsAny<string>()), Times.AtLeastOnce);
        }

        private BatchStrategy GetSut(out Mock<IJobItemRep> jobItemRepMock,
                                    out Mock<ILogger<BatchStrategy>> loggerMock,
                                    out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                    out Mock<IExternalProcessingService> externalProcessingServiceMock)
        {
            jobItemRepMock = new Mock<IJobItemRep>();
            loggerMock = new Mock<ILogger<BatchStrategy>>();
            dictionaryMock = new Mock<IOptions<DictionaryConfig>>();
            externalProcessingServiceMock = new Mock<IExternalProcessingService>();

            dictionaryMock.Setup(d => d.Value).Returns(new DictionaryConfig());

            return new BatchStrategy(jobItemRepMock.Object,
                                                loggerMock.Object,
                                                dictionaryMock.Object,
                                                externalProcessingServiceMock.Object);
        }
    }
}
