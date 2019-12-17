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
    public class LogTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly LoginTest _loginTest;

        public LogTest(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _loginTest = new LoginTest(factory);
        }

        [Fact]
        public async void Deve_Retornar_OK_Ao_Efetuar_Get_Logs()
        {
            await DefinirTokenHeader();

            var response = await _client.GetAsync("/api/Logs");

            ValidarStatusCode(HttpStatusCode.OK, (int)response.StatusCode);
        }

        [Fact]
        public async void Deve_Retornar_OK_Ao_Efetuar_Get_Log_Especifico()
        {
            await DefinirTokenHeader();

            var newLog = await IncluirLog();

            var response = await _client.GetAsync("/api/Logs/" + newLog.Id);

            ValidarStatusCode(HttpStatusCode.OK, (int)response.StatusCode);

            await ExcluirLog(newLog);
        }

        [Fact]
        public async void Deve_Retornar_NoContent_Ao_Efetuar_Get_Log_Especifico()
        {
            await DefinirTokenHeader();

            var id = Guid.NewGuid();

            var response = await _client.GetAsync("/api/Logs/" + id);

            ValidarStatusCode(HttpStatusCode.NoContent, (int)response.StatusCode);
        }
        
        [Fact]
        public async void Deve_Retornar_Created_Ao_Efetuar_Post_Log()
        {
            await DefinirTokenHeader();
            
            var newLog = await IncluirLog();

            await ExcluirLog(newLog);
        }

        private async Task<LogViewModel> IncluirLog()
        {
            var log = ObterDadosLogFake();

            var logViewModel = new LogViewModel
            {
                Title = $"{log.Title}-Teste",
                Detail = $"{log.Detail}",
                Event = log.Event,
                Enabled = log.Enabled,
                Level = log.Level,
                Ip = $"{ log.Ip }",
                Environment = $"{ log.Environment }",
                Token = log.Token
            };

            var byteContent = DefinirContent(logViewModel);

            var response = await _client.PostAsync("/api/Logs", byteContent);
            
            ValidarStatusCode(HttpStatusCode.Created, (int)response.StatusCode);

            var result = response.Content.ReadAsStringAsync().Result;
            var jsonResult = JObject.Parse(result);
            logViewModel.Id = Guid.Parse(jsonResult["id"].ToString());

            return logViewModel;
        }

        [Fact]
        public async void Deve_Retornar_Created_Ao_Efetuar_Put_Log()
        {
            await DefinirTokenHeader();

            var newLog = await IncluirLog();

            newLog.Title = "Test Log Updated - alterado";

            var byteContent = DefinirContent(newLog);

            var response = await _client.PutAsync("/api/Logs/" + newLog.Id, byteContent);

            ValidarStatusCode(HttpStatusCode.OK, (int)response.StatusCode);

            await ExcluirLog(newLog);
        }

        [Fact]
        public async void Deve_Retornar_BadRequest_Ao_Efetuar_Put_Log()
        {
            await DefinirTokenHeader();

            var newLog = await IncluirLog();

            newLog.Title = "Test Log Updated - alterado";

            var id = newLog.Id;
            newLog.Id = Guid.NewGuid();

            var byteContent = DefinirContent(newLog);

            var response = await _client.PutAsync("/api/Logs/" + id, byteContent);

            ValidarStatusCode(HttpStatusCode.BadRequest, (int)response.StatusCode);
            
            newLog.Id = id;
            await ExcluirLog(newLog);
        }

        private static ByteArrayContent DefinirContent(LogViewModel newLog)
        {
            var myContent = JsonConvert.SerializeObject(newLog);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.Clear();
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }

        [Fact]
        public async void Deve_Retornar_Ok_Ao_Efetuar_Delete_Log()
        {
            await DefinirTokenHeader();

            var newLog = await IncluirLog();

            await ExcluirLog(newLog);
        }

        private async Task ExcluirLog(LogViewModel newLog)
        {
            var response = await _client.DeleteAsync("/api/Logs/" + newLog.Id);

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

        public static Log ObterDadosLogFake()
        {
            var fakes = new Fakes("ObterDadosLog");
            var log = fakes.GetFakeData<Log>().FirstOrDefault();
            Assert.NotNull(log);
            return log;
        }
    }
}