using CentralDeErros.Data.Context;
using System;
using Xunit;

namespace CentralDeErros.Test.UnitTest.Domain
{
    public class UserModelTest : ModelBaseTest
    {
        //private readonly IUserService _userService;

        public UserModelTest()//IUserService userService)
                    : base(new Contexto())
        {
            Model = "CentralDeErros.Domain.Models.User";
            Table = "User";
           // _userService = userService;
        }

        [Fact]
        public void Deve_Retornar_Definicao_Tabela_Valida()
        {
            AssertTable();
        }

        [Fact]
        public void Deve_Retornar_Chave_Primaria_Valida()
        {
            AssertPrimaryKeys("token");
        }

        [Theory]
        [InlineData("token", false, typeof(Guid), null)]
        [InlineData("name", false, typeof(string), 255)]
        [InlineData("email", false, typeof(string), 255)]
        [InlineData("password", false, typeof(string), 50)]
        [InlineData("role", true, typeof(string), 50)]
        public void Deve_Retornar_Campos_Validos(string fieldName, bool isNullable, Type fieldType, int? fieldSize)
        {
            AssertField(fieldName, isNullable, fieldType, fieldSize);
        }

        [Theory]
        [InlineData("Log")]        
        public void Deve_Retornar_Navegacao_Valida(string childrenTable)
        {
            AssertChildrenNavigation(childrenTable);
        }

        //[Theory]
        //[InlineData("admin@mail.com")]
        //public async void Deve_Retornar_OK_Ao_Efetuar_Login(string email)
        //{
        //    var fakes = new Fakes();
        //    var user = fakes.Get<User>().Find(x => x.Email == email);
        //    Assert.NotNull(user);

        //    user = _userService.Login(user);
        //    Assert.NotNull(user);
        //}

    }
}