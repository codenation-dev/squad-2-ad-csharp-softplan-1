using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using CentralDeErros.Api;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.Domain.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CentralDeErros.Test.Integracao
{
    public class LoginTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public LoginTest(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        //[InlineData("UserController")]
        [InlineData("LogsController")]
        public void Deve_Retornar_OK_Ao_Verificar_Atributo_Authorize_Dos_Controllers(string controller)
        {
            var attributes = Type.GetType($"CentralDeErros.Api.Controllers.{controller}, CentralDeErros.Api").
                GetCustomAttributes(false).Select(x => x.GetType().Name).ToList();
            Assert.Contains("AuthorizeAttribute", attributes);
        }

        [Theory]
        //[InlineData("/api/user")]
        [InlineData("/api/logs")]
        public void Deve_Retornar_Unauthorized_Ao_Efetuar_Login_Sem_Token(string endpoint)
        {
            var actual = _client.GetAsync(endpoint).Result;
            ValidarStatusCode(HttpStatusCode.Unauthorized, (int)actual.StatusCode);
        }

        [Theory]
        [InlineData("Henderson@mail.com")]
        public async void Deve_Retornar_Unauthorized_Ao_Efetuar_Login(string email)
        {
            var contentString = ObterDadosUsuario(true, email, true);

            var response = await EfetuarLogin(contentString);

            ValidarStatusCode(HttpStatusCode.Unauthorized, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("admin@mail.com")]
        public async void Deve_Retornar_OK_Ao_Efetuar_Login(string email)
        {
            var contentString = ObterDadosUsuario(true, email, false);

            var response = await EfetuarLogin(contentString);
            
            ValidarStatusCode(HttpStatusCode.OK, (int)response.StatusCode);
        }                

        [Fact]
        public async void Deve_Retornar_BadRequest_Ao_Efetuar_Login()
        {
            var contentString = ObterDadosUsuario(false, string.Empty);
            
            var response = await EfetuarLogin(contentString);

            ValidarStatusCode(HttpStatusCode.BadRequest, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("usuario_inativo@mail.com")]
        public async void Deve_Retornar_Unauthorized_Ao_Efetuar_Login_Usuario_Inativo(string email)
        {
            var contentString = ObterDadosUsuario(true, email);

            var response = await EfetuarLogin(contentString);

            ValidarStatusCode(HttpStatusCode.Unauthorized, (int)response.StatusCode);
        }

        private void ValidarStatusCode(HttpStatusCode statusCode, int responseStatusCode)
        {
            Assert.Equal((int)statusCode, responseStatusCode);
        }

        public StringContent ObterDadosUsuario(bool carregarDeArquivo, string email, bool usuarioInvalido = false)
        {
            var usuario = new LoginViewModel();

            if (carregarDeArquivo && email != default)
            {
                var fakes = new Fakes("ObterDadosUsuario");
                var user = fakes.GetFakeData<User>().Find(x => x.Email == email);
                Assert.NotNull(user);

                usuario.Email = user.Email;
                usuario.Password = usuarioInvalido ? $"{user.Password}2" : user.Password;
            }

            var jsonContent = JsonConvert.SerializeObject(usuario);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return contentString;
        }

        public async Task<HttpResponseMessage> EfetuarLogin(StringContent contentString)
        {
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PostAsync("/api/Login", contentString);
            return response;
        }

    }
}
