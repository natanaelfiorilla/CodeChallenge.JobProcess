using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Ophen.JobProcess.API.Controllers;
using Ophen.JobProcess.API.Models;
using Ophen.JobProcess.ApplicationServices;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.Test
{
    [TestFixture()]
    public class JobsControlerTest
    {
        [Test()]
        public void Post_EmptyModel_BadRequest()
        {
            var sut = GetSut(out Mock<IJobService> jobServiceMock,
                                out Mock<IJobItemService> jobItemServiceMock,
                                out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                out Mock<ILogger<JobsController>> loggerMock,
                                out Mock<IMapper> mapperMock);

            var fakeModel = new JobModel();
            
            var postResponse = sut.Post(fakeModel);

            Assert.IsInstanceOf<BadRequestObjectResult>(postResponse.Result.Result);
        }

        [Test()]
        public void Post_JobServiceThrowArgumentException_BadRequest()
        {
            var sut = GetSut(out Mock<IJobService> jobServiceMock,
                                out Mock<IJobItemService> jobItemServiceMock,
                                out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                out Mock<ILogger<JobsController>> loggerMock,
                                out Mock<IMapper> mapperMock);

            jobServiceMock.Setup(j => j.Create(It.IsAny<Job>())).Throws(new ArgumentException());
            mapperMock.Setup(m => m.Map<JobModel, Job>(It.IsAny<JobModel>())).Returns(It.IsAny<Job>());

            var fakeModel = new JobModel() { Items = new List<JobItemModel>() { new JobItemModel() { Data = "data" } } };

            var postResponse = sut.Post(fakeModel);

            Assert.IsInstanceOf<BadRequestObjectResult>(postResponse.Result.Result);
        }

        [Test()]
        public void Post_JobServiceThrowException_Exception()
        {
            var sut = GetSut(out Mock<IJobService> jobServiceMock,
                                out Mock<IJobItemService> jobItemServiceMock,
                                out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                out Mock<ILogger<JobsController>> loggerMock,
                                out Mock<IMapper> mapperMock);

            jobServiceMock.Setup(j => j.Create(It.IsAny<Job>())).Throws(new Exception());
            mapperMock.Setup(m => m.Map<JobModel, Job>(It.IsAny<JobModel>())).Returns(It.IsAny<Job>());

            var fakeModel = new JobModel() { Items = new List<JobItemModel>() { new JobItemModel() { Data = "data" } } };

            Assert.ThrowsAsync<Exception>(async () => await sut.Post(fakeModel));
        }

        [Test()]
        public void Post_JobServiceReturnInt_Ok()
        {
            var sut = GetSut(out Mock<IJobService> jobServiceMock,
                                out Mock<IJobItemService> jobItemServiceMock,
                                out Mock<IOptions<DictionaryConfig>> dictionaryMock,
                                out Mock<ILogger<JobsController>> loggerMock,
                                out Mock<IMapper> mapperMock);

            jobServiceMock.Setup(j => j.Create(It.IsAny<Job>())).ReturnsAsync(It.IsAny<int>());
            mapperMock.Setup(m => m.Map<JobModel, Job>(It.IsAny<JobModel>())).Returns(It.IsAny<Job>());

            var fakeModel = new JobModel() { Items = new List<JobItemModel>() { new JobItemModel() { Data = "data" } } };

            var postResponse = sut.Post(fakeModel);

            Assert.IsInstanceOf<int>(postResponse.Result.Value);
        }


        private JobsController GetSut(out Mock<IJobService> jobServiceMock, out Mock<IJobItemService> jobItemServiceMock, out Mock<IOptions<DictionaryConfig>> dictionaryMock, out Mock<ILogger<JobsController>> loggerMock, out Mock<IMapper> mapperMock)
        {
            jobServiceMock = new Mock<IJobService>();
            jobItemServiceMock = new Mock<IJobItemService>();
            dictionaryMock = new Mock<IOptions<DictionaryConfig>>();
            loggerMock = new Mock<ILogger<JobsController>>();
            mapperMock = new Mock<IMapper>();

            dictionaryMock.Setup(d => d.Value).Returns(new DictionaryConfig());

            return new JobsController(jobServiceMock.Object,
                                        jobItemServiceMock.Object,
                                        dictionaryMock.Object,
                                        loggerMock.Object,
                                        mapperMock.Object);
        }
    }
}