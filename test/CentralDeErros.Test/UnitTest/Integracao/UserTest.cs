using CentralDeErros.Api;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace CentralDeErros.Test.Integracao
{
    public class UserTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly LoginTest _loginTest;

        public UserTest(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _loginTest = new LoginTest(factory);
        }

        [Fact]
        public async void Deve_Retornar_OK_Ao_Efetuar_Get_Users()
        {
            await DefinirTokenHeader();

            var response = await _client.GetAsync("/api/User/");

            ValidarStatusCode(HttpStatusCode.OK, (int)response.StatusCode);
        }

        [Fact]
        public async void Deve_Retornar_OK_Ao_Efetuar_Get_User_Especifico()
        {
            await DefinirTokenHeader();

            var newUser = await IncluirUser();

            var response = await _client.GetAsync("/api/User/" + newUser.Id);

            ValidarStatusCode(HttpStatusCode.OK, (int)response.StatusCode);

            await ExcluirUser(newUser);
        }

        [Fact]
        public async void Deve_Retornar_NoContent_Ao_Efetuar_Get_User_Especifico()
        {
            await DefinirTokenHeader();

            var id = Guid.NewGuid();

            var response = await _client.GetAsync("/api/User/" + id);

            ValidarStatusCode(HttpStatusCode.NoContent, (int)response.StatusCode);
        }

        [Fact]
        public async void Deve_Retornar_Created_Ao_Efetuar_Post_User()
        {
            await DefinirTokenHeader();

            var newUser = await IncluirUser();

            await ExcluirUser(newUser);
        }

        [Fact]
        public async void Deve_Retornar_BadRequest_Ao_Efetuar_Post_User()
        {
            await DefinirTokenHeader();

            await IncluirUser(true, HttpStatusCode.BadRequest);

        }

        private async Task<UserViewModel> IncluirUser(bool emailReptido = false,
            HttpStatusCode statusCode = HttpStatusCode.Created)
        {
            var user = ObterDadosUserFake();

            var email = user.Email + new Random(99999).Next().ToString();

            if (emailReptido)
                email = "admin@mail.com";

            var userViewModel = new UserViewModel
            {
                Name = $"{user.Name}-Teste",
                Email = email,
                Password = user.Password,
                Active = user.Active,
                Role = user.Role

            };

            var byteContent = DefinirContent(userViewModel);

            var response = await _client.PostAsync("/api/User", byteContent);

            ValidarStatusCode(statusCode, (int)response.StatusCode);

            if (statusCode == HttpStatusCode.BadRequest)
                return userViewModel;

            var result = response.Content.ReadAsStringAsync().Result;
            var jsonResult = JObject.Parse(result);

            userViewModel.Id = Guid.Parse(jsonResult["id"].ToString());

            return userViewModel;

        }

        [Fact]
        public async void Deve_Retornar_Created_Ao_Efetuar_Put_User()
        {
            await DefinirTokenHeader();

            var newUser = await IncluirUser();

            newUser.Name = "Test User Updated - alterado";

            var byteContent = DefinirContent(newUser);

            var response = await _client.PutAsync("/api/User/" + newUser.Id, byteContent);

            ValidarStatusCode(HttpStatusCode.OK, (int)response.StatusCode);

            await ExcluirUser(newUser);
        }

        [Fact]
        public async void Deve_Retornar_BadRequest_Ao_Efetuar_Put_User()
        {
            await DefinirTokenHeader();

            var newUser = await IncluirUser();

            newUser.Name = "Test User Updated - alterado";

            var id = newUser.Id;
            newUser.Id = Guid.NewGuid();

            var byteContent = DefinirContent(newUser);

            var response = await _client.PutAsync("/api/User/" + id, byteContent);

            ValidarStatusCode(HttpStatusCode.BadRequest, (int)response.StatusCode);

            newUser.Id = id;
            await ExcluirUser(newUser);
        }

        private static ByteArrayContent DefinirContent(UserViewModel newUser)
        {
            var myContent = JsonConvert.SerializeObject(newUser);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.Clear();
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }

        [Fact]
        public async void Deve_Retornar_Ok_Ao_Efetuar_Delete_User()
        {
            await DefinirTokenHeader();

            var newUser = await IncluirUser();

            await ExcluirUser(newUser);
        }

        private async Task ExcluirUser(UserViewModel newUser)
        {
            var response = await _client.DeleteAsync("/api/User/" + newUser.Id);

            ValidarStatusCode(HttpStatusCode.OK, (int)response.StatusCode);
        }

        private async Task<string> GetBearerToken(string user)
        {
            var contentString = _loginTest.ObterDadosUsuario(true, user, false);

            var loginResponse = await _loginTest.EfetuarLogin(contentString);

            var result = loginResponse.Content.ReadAsStringAsync().Result;

            JObject jsonResult = JObject.Parse(result);

            string bearerToken = jsonResult["accessToken"].ToString();

            return bearerToken;
        }
        private async Task DefinirTokenHeader()
        {
            string bearerToken = await GetBearerToken("admin@mail.com");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", bearerToken);
        }

        private void ValidarStatusCode(HttpStatusCode statusCode, int responseStatusCode)
        {
            Assert.Equal((int)statusCode, responseStatusCode);
        }

        public static User ObterDadosUserFake()
        {
            var fakes = new Fakes("ObterDadosUser");
            var user = fakes.GetFakeData<User>().FirstOrDefault();
            Assert.NotNull(user);
            return user;
        }
    }
}
