using CentralDeErros.Data.Context;
using System;
using Xunit;

namespace CentralDeErros.Test.UnitTest.Domain
{
    public class LogModelTest : ModelBaseTest
    {
        public LogModelTest()
                    : base(new Contexto())
        {
            Model = "CentralDeErros.Domain.Models.Log";
            Table = "Log";
        }

        [Fact]
        public void Deve_Retornar_Definicao_Tabela_Valida()
        {
            AssertTable();
        }

        [Fact]
        public void Deve_Retornar_Chave_Primaria_Valida()
        {
            AssertPrimaryKeys("id");
        }

        [Theory]
        [InlineData("id", false, typeof(Guid), null)]
        [InlineData("title", false, typeof(string), 255)]
        [InlineData("detail", false, typeof(string), 4000)]
        [InlineData("event", false, typeof(int), null)]
        [InlineData("level", false, typeof(string), 1)]
        [InlineData("environment", false, typeof(string), 1)]
        [InlineData("ip", true, typeof(string), 20)]
        public void Deve_Retornar_Campos_Validos(string fieldName, bool isNullable, Type fieldType, int? fieldSize)
        {
            AssertField(fieldName, isNullable, fieldType, fieldSize);
        }
    }
}
