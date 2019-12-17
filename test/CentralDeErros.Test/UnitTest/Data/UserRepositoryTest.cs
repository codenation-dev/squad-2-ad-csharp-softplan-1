using CentralDeErros.CrossCutting.Constants;
using CentralDeErros.Data.Context;
using CentralDeErros.Data.Repositories;
using CentralDeErros.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using Xunit;

namespace CentralDeErros.Test.UnitTest.Data
{
    public class UserRepositoryTest
    {
        private readonly UserRepository _repository;
        private Mock<Contexto> _fakeContext;
        private IQueryable<User> fakeData;

        public UserRepositoryTest()
        {
            var fakes = new Fakes("ObterDadosUser");
            fakeData = fakes.GetFakeData<User>().AsQueryable();

            var fakeDbSet = new Mock<DbSet<User>>();

            fakeDbSet.As<IQueryable<User>>().Setup(x => x.Provider).Returns(fakeData.Provider);
            fakeDbSet.As<IQueryable<User>>().Setup(x => x.Expression).Returns(fakeData.Expression);
            fakeDbSet.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(fakeData.ElementType);
            fakeDbSet.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(fakeData.GetEnumerator());

            _fakeContext = new Mock<Contexto>();
            _fakeContext.Setup(m => m.Set<User>()).Returns(fakeDbSet.Object);

            _repository = new UserRepository(_fakeContext.Object);
        }

        [Fact]
        public void Dever_Encontrar_Usuario_Pelo_Id()
        {
            var user = fakeData.FirstOrDefault();
            Assert.NotNull(user);

            var changed = _repository.GetById(user.Id);
            Assert.NotNull(changed);
        }

        [Fact]
        public void Dever_Retornar_Todos_Usuarios()
        {
            var changed = _repository.GetAll();
            Assert.NotNull(changed);
        }

        [Fact]
        public void Dever_Encontrar_Usuario_Find()
        {
            var user = fakeData.FirstOrDefault();
            Assert.NotNull(user);

            var actual = _repository.Find(e => e.Email == user.Email);
            Assert.NotNull(actual);

            Assert.Equal(user.Name, actual.FirstOrDefault().Name);
        }

        [Fact]
        public void Deve_Excluir_Usuario()
        {
            User user, userReturn;
            IncluirUsuario(out user, out userReturn);

            RemoverUsuario(userReturn);
        }

        [Fact]
        public void Deve_Incluir_Usuario()
        {
            User user, userReturn;
            IncluirUsuario(out user, out userReturn);

            Assert.Equal(userReturn.Name, user.Name);
            RemoverUsuario(userReturn);
        }

        [Fact]
        public void Deve_Alterar_Usuario()
        {
            User user, userReturn;
            IncluirUsuario(out user, out userReturn);

            var name = userReturn.Name;
            userReturn.Name = $"{userReturn.Name} - alterado";

            _repository.Update(userReturn);
            Assert.NotEqual(name, userReturn.Name);
            RemoverUsuario(userReturn);
        }

        private void IncluirUsuario(out User user, out User userReturn)
        {
            var changed = fakeData.FirstOrDefault();
            Assert.NotNull(changed);

            user = new User
            {
                Name = $"{changed.Name} _teste",
                Email = $"{changed.Email} .br",
                Password = "123456",
                Role = Constants.PERFIL_USUARIO,
                Active = false
            };
            userReturn = _repository.Add(user);
        }

        private void RemoverUsuario(User userReturn)
        {
            var total = _repository.GetAll().Count;

            _repository.Remove(userReturn.Id);
            Assert.True(_repository.GetAll().Count < total);
        }
    }
}