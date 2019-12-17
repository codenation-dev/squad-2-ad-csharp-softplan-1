using AutoMapper;
using CentralDeErros.Application.ApplicationServices;
using CentralDeErros.Application.Mapping;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.CrossCutting.Constants;
using CentralDeErros.Data.Context;
using CentralDeErros.Data.Repositories;
using CentralDeErros.Domain.Interfaces.Repositories;
using CentralDeErros.Domain.Models;
using CentralDeErros.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using Xunit;

namespace CentralDeErros.Test.UnitTest.Application
{
    public class UserAppServiceTest
    {
        private readonly IUserRepository _repository;
        protected readonly IMapper _mapper;
        private readonly Mock<Contexto> _fakeContext;
        private readonly IQueryable<User> fakeData;
        private readonly UserService service;
        private readonly UserAppService appService;

        public UserAppServiceTest()
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

            _mapper = new Mapper(
            new MapperConfiguration(
                configure =>
                {
                    configure.AddProfile<AutoMappingDomainToViewModel>();
                    configure.AddProfile<AutoMappingViewModelToDomain>();
                }
            ));
            service = new UserService(_repository);
            appService = new UserAppService(service, _mapper);
        }

        [Fact]
        public void Dever_Encontrar_Usuario_Pelo_Id()
        {
            var user = fakeData.FirstOrDefault();
            Assert.NotNull(user);

            var changed = appService.GetById(user.Id);
            Assert.NotNull(changed);
        }

        [Fact]
        public void Dever_Retornar_Todos_Usuarios()
        {
            var service = new UserService(_repository);
            var appService = new UserAppService(service, _mapper);

            var changed = appService.GetAll();
            Assert.NotNull(changed);
        }

        [Fact]
        public void Dever_Encontrar_Usuario_Find()
        {
            var service = new UserService(_repository);
            var appService = new UserAppService(service, _mapper);

            var user = fakeData.FirstOrDefault();
            Assert.NotNull(user);

            var actual = appService.Find(e => e.Email == user.Email);
            Assert.NotNull(actual);

            Assert.Equal(user.Name, actual.FirstOrDefault().Name);
        }

        [Fact]
        public void Deve_Excluir_Usuario()
        {
            var service = new UserService(_repository);
            var appService = new UserAppService(service, _mapper);

            UserViewModel user, userReturn;
            IncluirUsuario(appService, out user, out userReturn);

            RemoverUsuario(appService, userReturn);            
        }        

        [Fact]
        public void Deve_Incluir_Usuario()
        {
            var service = new UserService(_repository);
            var appService = new UserAppService(service, _mapper);

            UserViewModel user, userReturn;
            IncluirUsuario(appService, out user, out userReturn);

            Assert.Equal(userReturn.Name, user.Name);
            RemoverUsuario(appService, userReturn);
        }

        [Fact]
        public void Deve_Alterar_Usuario()
        {
            var service = new UserService(_repository);
            var appService = new UserAppService(service, _mapper);

            UserViewModel user, userReturn;
            IncluirUsuario(appService, out user, out userReturn);
            
            var name = userReturn.Name;
            userReturn.Name = $"{userReturn.Name} - alterado";
                            
            appService.Update(userReturn);
            Assert.NotEqual(name, userReturn.Name);
            RemoverUsuario(appService, userReturn);
        }

        private void IncluirUsuario(UserAppService appService, out UserViewModel user, out UserViewModel userReturn)
        {
            var changed = fakeData.FirstOrDefault();
            Assert.NotNull(changed);

            user = new UserViewModel
            {
                Name = $"{changed.Name} _teste",
                Email = $"{changed.Email} .br",
                Password = "123456",
                Role = Constants.PERFIL_USUARIO,
                Active = false
            };
            userReturn = appService.Add(user);
        }

        private static void RemoverUsuario(UserAppService appService, UserViewModel userReturn)
        {
            var total = appService.GetAll().Count;

            appService.Remove(userReturn.Id);
            Assert.True(appService.GetAll().Count < total);
        }
    }
}