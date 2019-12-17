using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CentralDeErros.Application.Interface;
using CentralDeErros.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CentralDeErros.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    #pragma warning disable CS1591
    public class UserController : ControllerBase
    {
        /// <summary>
        /// IUserAppService variável readonly
        /// </summary>
        private readonly IUserAppService _userAppService;

        /// <summary>
        /// Inicialização da classe UserController
        /// </summary>
        /// <param name="userAppService"></param>
        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }
        
        /// <summary>
        /// Método para listar todos os Usuários
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     GET /user
        ///
        /// </remarks>
        /// <returns>Lista contendo todos os usuários</returns>
        /// <response code="200">Pesquisa realizada com sucesso</response>
        /// <response code="400">Erro na requisição</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  
        public ActionResult GetAllUsers()
        {
            try
            {
                var users = _userAppService.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao buscar os usuários." +
                    $"\nErro: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Método para buscar Usuário pelo Id
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     GET /user/id
        ///
        /// </remarks>
        /// <param name="id">Identificador do usuário</param>
        /// <returns>Dados do usuário pesquisado</returns>
        /// <response code="200">Pesquisa realizada com sucesso</response>
        /// <response code="201">Usuário não localizado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserViewModel> GetById(Guid id)
        {
            try
            {
                var user = _userAppService.GetById(id);

                if (user != default)
                    return Ok(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao buscar o usuário de código: {id}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Método para incluir o usuário
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     POST /user/id
        ///
        /// </remarks>
        /// <param name="userViewModel">Dados do usuário a ser incluído (body da msg)</param>
        /// <returns></returns>
        /// <response code="200">Inclusão realizada com sucesso</response>
        /// <response code="400">Erro na requisição</response>        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post([FromBody()] UserViewModel userViewModel)
        {
            try
            {
                if(_userAppService.Find(p => p.Email == userViewModel.Email).Count > 0)
                    return BadRequest(new
                    {
                        Message = "O e-mail informado já existe na base de dados."
                    });

                userViewModel = _userAppService.Add(userViewModel);

                return Created($"{Request.Path.Value}/{userViewModel.Id}", userViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao incluir o usuário de código: {userViewModel.Id}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Método para atualizar o usuário
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     PUT /user/id
        ///
        /// </remarks>
        /// <param name="id">Identificador do usuário</param>
        /// <param name="userViewModel">Dados do usuário a ser alterado (body da msg)</param>
        /// <returns></returns>
        /// <response code="200">Alteração realizada com sucesso</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(Guid id, [FromBody()] UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
                return BadRequest(new
                {
                    Message = $"Identificador do usuário diferente dos dados informados: {id}."
                });
            try
            {
                _userAppService.Update(userViewModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao alterar o usuário de código: {id}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }


        /// <summary>
        /// Método para remover Usuário por Id
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     DELETE /user/id
        ///
        /// </remarks>
        /// <param name="id">Identificador do usuário</param>
        /// <returns></returns>
        /// <response code="200">Exclusão realizada com sucesso</response>
        /// <response code="400">Erro na requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteUserById(Guid id)
        {
            try
            {
                _userAppService.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao excluir o usuário de código: {id}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }
    }
}