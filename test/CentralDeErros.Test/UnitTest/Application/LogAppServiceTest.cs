using AutoMapper;
using CentralDeErros.Application.ApplicationServices;
using CentralDeErros.Application.Mapping;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.Data.Context;
using CentralDeErros.Data.Repositories;
using CentralDeErros.Domain.Interfaces.Repositories;
using CentralDeErros.Domain.Models;
using CentralDeErros.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace CentralDeErros.Test.UnitTest.Application
{
    public class LogAppServiceTest
    {
        private readonly ILogRepository _repository;
        protected readonly IMapper _mapper;
        private readonly Mock<Contexto> _fakeContext;
        private readonly LogService service;
        private readonly LogAppService appService;
        private readonly IQueryable<Log> fakeLogs;


        public LogAppServiceTest()
        {
            var fakes = new Fakes("ObterDadosLog");

            fakeLogs = fakes.GetFakeData<Log>().AsQueryable();
            var fakeDbSet = new Mock<DbSet<Log>>();

            fakeDbSet.As<IQueryable<Log>>().Setup(x => x.Provider).Returns(fakeLogs.Provider);
            fakeDbSet.As<IQueryable<Log>>().Setup(x => x.Expression).Returns(fakeLogs.Expression);
            fakeDbSet.As<IQueryable<Log>>().Setup(x => x.ElementType).Returns(fakeLogs.ElementType);
            fakeDbSet.As<IQueryable<Log>>().Setup(x => x.GetEnumerator()).Returns(fakeLogs.GetEnumerator());

            _fakeContext = new Mock<Contexto>();
            _fakeContext.Setup(m => m.Set<Log>()).Returns(fakeDbSet.Object);

            _repository = new LogRepository(_fakeContext.Object);

            _mapper = new Mapper(
            new MapperConfiguration(
                configure =>
                {
                    configure.AddProfile<AutoMappingDomainToViewModel>();
                    configure.AddProfile<AutoMappingViewModelToDomain>();
                }
            ));

            service = new LogService(_repository);
            appService = new LogAppService(service, _mapper);
        }

        [Fact]
        public void Dever_Encontrar_Log_Pelo_Id()
        {
            var changed = appService.GetById(fakeLogs.FirstOrDefault().Id);
            Assert.NotNull(changed);
        }

        [Fact]
        public void Dever_Retornar_Todos_Log()
        {
            var changed = appService.GetAll();
            Assert.NotNull(changed);
        }

        [Fact]
        public void Dever_Encontrar_Log_Find()
        {
            var log = fakeLogs.FirstOrDefault();
            var actual = appService.Find(e => e.Title == log.Title);
            Assert.NotNull(actual);
            Assert.Equal(log.Title, actual.FirstOrDefault().Title);
        }

        [Fact]
        public void Deve_Excluir_Log()
        {
            var log = fakeLogs.FirstOrDefault();
            var changed = appService.GetById(log.Id);
            Assert.NotNull(changed);

            var total = appService.GetAll().Count;
            
            appService.Remove(changed.Id);
            Assert.True(appService.GetAll().Count < total);
        }

        [Fact]
        public void Deve_Incluir_Log()
        {
            var log = fakeLogs.FirstOrDefault();
            var changed = appService.GetById(log.Id);
            Assert.NotNull(changed);

            var logLocal = new LogViewModel
            {
                Title = $"{changed.Title} _teste",
                Detail = $"{changed.Detail} _teste",
                Environment = $"{changed.Detail} _teste",
                Event = changed.Event + 1,
                Enabled = true,
                Level = "1",
                Token = Guid.NewGuid()
            };
            var userReturn = appService.Add(logLocal);
            Assert.Equal(userReturn.Title, logLocal.Title);

        }

        [Fact]
        public void Deve_Alterar_Log()
        {
            var log = fakeLogs.FirstOrDefault();
            var changed = appService.GetById(log.Id);
            Assert.NotNull(changed);

            var name = changed.Title;
            changed.Title = "teste";

            appService.Update(changed);
            Assert.NotEqual(name, changed.Title);

        }
    }
}