using CentralDeErros.Data.Context;
using CentralDeErros.Data.Repositories;
using CentralDeErros.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using Xunit;

namespace CentralDeErros.Test.UnitTest.Data
{
    public class LogRepositoryTest
    {
        private readonly LogRepository _repository;
        private Mock<Contexto> _fakeContext;
        private IQueryable<Log> fakeData;

        public LogRepositoryTest()
        {
            var fakes = new Fakes("ObterDadosLog");
            fakeData = fakes.GetFakeData<Log>().AsQueryable();

            var fakeDbSet = new Mock<DbSet<Log>>();

            fakeDbSet.As<IQueryable<Log>>().Setup(x => x.Provider).Returns(fakeData.Provider);
            fakeDbSet.As<IQueryable<Log>>().Setup(x => x.Expression).Returns(fakeData.Expression);
            fakeDbSet.As<IQueryable<Log>>().Setup(x => x.ElementType).Returns(fakeData.ElementType);
            fakeDbSet.As<IQueryable<Log>>().Setup(x => x.GetEnumerator()).Returns(fakeData.GetEnumerator());

            _fakeContext = new Mock<Contexto>();
            _fakeContext.Setup(m => m.Set<Log>()).Returns(fakeDbSet.Object);

            _repository = new LogRepository(_fakeContext.Object);
        }

        [Fact]
        public void Dever_Encontrar_Log_Pelo_Id()
        {
            var log = fakeData.FirstOrDefault();
            Assert.NotNull(log);

            var changed = _repository.GetById(log.Id);
            Assert.NotNull(changed);
        }

        [Fact]
        public void Dever_Retornar_Todos_Logs()
        {
            var changed = _repository.GetAll();
            Assert.NotNull(changed);
        }

        [Fact]
        public void Dever_Encontrar_Log_Find()
        {
            var log = fakeData.FirstOrDefault();
            Assert.NotNull(log);

            var actual = _repository.Find(e => e.Title == log.Title);
            Assert.NotNull(actual);

            Assert.Equal(log.Title, actual.FirstOrDefault().Title);
        }

        [Fact]
        public void Deve_Excluir_Log()
        {
            Log log, logReturn;
            IncluirLog(out log, out logReturn);

            RemoverLog(logReturn);
        }

        [Fact]
        public void Deve_Incluir_Log()
        {
            Log log, logReturn;
            IncluirLog(out log, out logReturn);

            Assert.Equal(logReturn.Title, log.Title);
            RemoverLog(logReturn);
        }

        [Fact]
        public void Deve_Alterar_Log()
        {
            Log log, logReturn;
            IncluirLog(out log, out logReturn);

            var name = logReturn.Title;
            logReturn.Title = $"{logReturn.Title} - alterado";

            _repository.Update(logReturn);
            Assert.NotEqual(name, logReturn.Title);
            RemoverLog(logReturn);
        }

        private void IncluirLog(out Log log, out Log logReturn)
        {
            var changed = fakeData.FirstOrDefault();
            Assert.NotNull(changed);

            log = new Log
            {
                Title = $"{changed.Title}-Teste",
                Detail = $"{changed.Detail}",
                Event = changed.Event,
                Enabled = changed.Enabled,
                Level = changed.Level,
                Ip = $"{ changed.Ip }",
                Environment = $"{ changed.Environment }",
                Token = changed.Token

            };
            logReturn = _repository.Add(log);
        }

        private void RemoverLog(Log logReturn)
        {
            var total = _repository.GetAll().Count;

            _repository.Remove(logReturn.Id);
            Assert.True(_repository.GetAll().Count < total);
        }
    }
}