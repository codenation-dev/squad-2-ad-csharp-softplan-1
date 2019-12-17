using CentralDeErros.CrossCutting.Exceptions;
using CentralDeErros.CrossCutting.Utils;
using System.Collections.Generic;
using Xunit;

namespace CentralDeErros.Test.UnitTest.CrossCutting
{
    public class CrossCuttingTest
    {

        [Fact]
        public void Deve_Retornar_HashMD5_Valido()
        {
            Assert.Equal("e10adc3949ba59abbe56e057f20f883e", "123456".ToHashMD5());
        }

        [Fact]
        public void Deve_Retornar_HashMD5_Invalido()
        {
            Assert.NotEqual("e10adc3949ba59abbe56e057f20f883e", "123".ToHashMD5());
        }

        [Fact]
        public void Nao_Deve_Validar_Model_Exception()
        {
            Assert.ThrowsAny<ModelValidationException>(() => throw new ModelValidationException("erro"));
        }

        [Fact]
        public void Nao_Deve_Validar_Model_Exception_Lista()
        {
            Assert.ThrowsAny<ModelValidationException>(() => throw new ModelValidationException(new List<string> { "erro", "erro 2" }));
        }

    }
}
