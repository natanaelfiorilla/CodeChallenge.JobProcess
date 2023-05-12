using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Ophen.JobProcess.ApplicationServices.ProcessingStrategy;
using Ophen.JobProcess.ApplicationServices.Services;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;

namespace Ophen.JobProcess.Test
{
    [TestFixture()]
    public class JobDataProcessingServiceTest
    {
        [Test()]
        public void ProcessJob_JobNull_ArgumentException()
        {
            var sut = GetSut(out Mock<IStrategyContext> strategyContextMock,
                                  out Mock<ILogger<JobDataProcessingService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IServiceProvider> serviceProviderMock);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ProcessJob(null));
        }

        [Test()]
        public void ProcessJob_JobItemEmpty_ArgumentException()
        {
            var sut = GetSut(out Mock<IStrategyContext> strategyContextMock,
                                  out Mock<ILogger<JobDataProcessingService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IServiceProvider> serviceProviderMock);

            var jobFake = new Job();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ProcessJob(jobFake));
        }

        [Test()]
        public void ProcessJob_StrategyNull_Exception()
        {
            var sut = GetSut(out Mock<IStrategyContext> strategyContextMock,
                                  out Mock<ILogger<JobDataProcessingService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IServiceProvider> serviceProviderMock);

            var jobFake = new Job() { Items = new List<JobItem>() { new JobItem() } };

            serviceProviderMock.Setup(s => s.GetService(It.IsAny<Type>())).Returns(null);

            Assert.ThrowsAsync<Exception>(async () => await sut.ProcessJob(jobFake));           
        }


        [Test()]
        public void ProcessJob_Ok()
        {
            var sut = GetSut(out Mock<IStrategyContext> strategyContextMock,
                                  out Mock<ILogger<JobDataProcessingService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IServiceProvider> serviceProviderMock);

            var jobFake = new Job() { Items = new List<JobItem>() { new JobItem() } };

            var jobItemRepMock = new Mock<IJobItemRep>();                       
            var externalProcessingServiceMock = new Mock<IExternalProcessingService>();
            var loggerStrategyMock = new Mock<ILogger<BulkStrategy>>();
            var mockStrategy = new Mock<BulkStrategy>(jobItemRepMock.Object,
                                                      loggerStrategyMock.Object,
                                                      dictionaryMock.Object,
                                                      externalProcessingServiceMock.Object);

            serviceProviderMock.Setup(s => s.GetService(It.IsAny<Type>())).Returns(mockStrategy.Object);

            Assert.DoesNotThrowAsync(async () => await sut.ProcessJob(jobFake));
        }

        private JobDataProcessingService GetSut(out Mock<IStrategyContext> strategyContextMock,
                                  out Mock<ILogger<JobDataProcessingService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IServiceProvider> serviceProviderMock)
        {
            strategyContextMock = new Mock<IStrategyContext>();
            loggerMock = new Mock<ILogger<JobDataProcessingService>>();
            dictionaryMock = new Mock<IOptions<DictionaryConfig>>();
            serviceProviderMock = new Mock<IServiceProvider>();

            dictionaryMock.Setup(d => d.Value).Returns(new DictionaryConfig());

            return new JobDataProcessingService(strategyContextMock.Object,
                                                loggerMock.Object,
                                                dictionaryMock.Object,
                                                serviceProviderMock.Object);
        }
    }
}
