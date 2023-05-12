using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Ophen.JobProcess.ApplicationServices.Services;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;
using Ophen.JobProcess.DomainServices;

namespace Ophen.JobProcess.Test
{
    [TestFixture()]
    public class JobServiceTest
    {
        [Test()]
        public void Create_JobNull_ArgumentException()
        {
            var sut = GetSut(out Mock<IJobRep> jobRepMock,
                                  out Mock<ILogger<JobService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IJobDataProcessingService> jobDataProcessingServiceMock);          


            Assert.ThrowsAsync<ArgumentException>(async () => await sut.Create(null));
        }

        [Test()]
        public void Create_JobInvalid_ArgumentException()
        {
            var sut = GetSut(out Mock<IJobRep> jobRepMock,
                                  out Mock<ILogger<JobService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IJobDataProcessingService> jobDataProcessingServiceMock);

            var jobFake = new Job();

            Assert.ThrowsAsync<ArgumentException>(async () => await sut.Create(jobFake));
        }

        [Test()]
        public void Create_JobRepThrowException_Exception()
        {
            var sut = GetSut(out Mock<IJobRep> jobRepMock,
                                  out Mock<ILogger<JobService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IJobDataProcessingService> jobDataProcessingServiceMock);

            var jobFake = new Job() { Items = new List<JobItem>() { new JobItem() } };

            jobRepMock.Setup(r => r.SaveChangesAsync()).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(async () => await sut.Create(jobFake));
        }

        [Test()]
        public void Create_JobDataProcessingServiceThrowException_Exception()
        {
            var sut = GetSut(out Mock<IJobRep> jobRepMock,
                                  out Mock<ILogger<JobService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IJobDataProcessingService> jobDataProcessingServiceMock);

            var jobFake = new Job() { Items = new List<JobItem>() { new JobItem() } };

            jobRepMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(It.IsAny<int>);

            jobDataProcessingServiceMock.Setup(r => r.ProcessJob(It.IsAny<Job>())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(async () => await sut.Create(jobFake));
        }

        [Test()]
        public void Create_Ok_ReturnInt()
        {
            var sut = GetSut(out Mock<IJobRep> jobRepMock,
                                  out Mock<ILogger<JobService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IJobDataProcessingService> jobDataProcessingServiceMock);

            var jobFake = new Job() { Items = new List<JobItem>() { new JobItem() } };

            jobRepMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(It.IsAny<int>);

            var result = sut.Create(jobFake);

            Assert.IsInstanceOf<int>(result.Result);
        }

        private JobService GetSut(out Mock<IJobRep> jobRepMock,
                                  out Mock<ILogger<JobService>> loggerMock,
                                  out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                  out Mock<IJobDataProcessingService> jobDataProcessingServiceMock)
        {
            jobRepMock = new Mock<IJobRep>();
            loggerMock = new Mock<ILogger<JobService>>();
            dictionaryMock = new Mock<IOptions<DictionaryConfig>>();
            jobDataProcessingServiceMock = new Mock<IJobDataProcessingService>();

            dictionaryMock.Setup(d => d.Value).Returns(new DictionaryConfig());

            return new JobService(jobRepMock.Object,
                                        loggerMock.Object,
                                        dictionaryMock.Object,                                        
                                        jobDataProcessingServiceMock.Object);
        }
    }
}
