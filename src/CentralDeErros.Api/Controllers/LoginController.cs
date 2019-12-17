using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.Application.Interface;
using Microsoft.Extensions.Options;
using CentralDeErros.CrossCutting.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CentralDeErros.Api.Controllers
{
    [Route("api/login")]
    [ApiController]
    [Produces("application/json")]
#pragma warning disable CS1591
    public class LoginController : ControllerBase
    {
        private readonly IUserAppService _usuarioAppService;
        private readonly AppSettings _appSettings;

        public LoginController(IUserAppService usuarioAppService, IOptions<AppSettings> appSettings)
        {
            this._usuarioAppService = usuarioAppService;
            this._appSettings = appSettings.Value;
        }

        /// <summary>
        /// Endpoint do Login do usuário
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     POST /login
        ///     {
        ///        "email": "mail@mail.com",
        ///        "password": "123"
        ///     }
        ///
        /// </remarks>
        /// <param name="usuario">Dados o usuário a ser autenticado</param>
        /// <returns>Retornar token e o usuário logado</returns>
        /// <response code="200">Usuário autorizado</response>
        /// <response code="400">Erro na requisição</response> 
        /// <response code="401">Usuário não autorizado</response> 
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        [HttpPost()]
        public object Post([FromBody] LoginViewModel usuario)
        {
            try
            {
                var usuarioLocal = _usuarioAppService.Login(usuario);

                if (usuarioLocal == default)
                    return Unauthorized("Usuário ou senha não conferem");

                if (!usuarioLocal.Active)
                    return Unauthorized("Usuário inativo.");

                usuarioLocal.AccessToken = GerarJWT(usuarioLocal);
                usuarioLocal.Password = null;

                return Ok(usuarioLocal);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao efetuar login do usuário: {usuario.Email}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }

        private string GerarJWT(UserViewModel usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKeyJWT);
            var minutes = Convert.ToInt32(_appSettings.Minutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Name),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Role),
                    new Claim("id", usuario.Id.ToString()),
                }),

                Expires = DateTime.UtcNow.AddMinutes(minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}