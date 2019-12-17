using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CentralDeErros.Application.Interface;
using CentralDeErros.Application.ViewModels;
using System;
using Microsoft.AspNetCore.Http;

namespace CentralDeErros.Api.Controllers
{
    [ApiController]
    [Route("api/logs")]
    [Authorize("Bearer")]
    [Produces("application/json")]
#pragma warning disable CS1591
    public class LogsController : ControllerBase
    {
        /// <summary>
        /// ILogAppService variável
        /// </summary>
        private readonly ILogAppService _logAppService;

        /// <summary>
        /// Inicialização de LogsController classe
        /// </summary>
        /// <param name="logAppService"></param>
        public LogsController(ILogAppService logAppService)
        {
            _logAppService = logAppService;
        }

        /// <summary>
        /// Método para listar todos os Logs
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     GET /logs
        ///
        /// </remarks>
        /// <returns>Lista contendo todos os logs</returns>
        /// <response code="200">Pesquisa realizada com sucesso</response>
        /// <response code="400">Erro na requisição</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Get()
        {
            try
            {
                var logs = _logAppService.GetAll();
                return Ok(logs);

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao buscar os logs." +
                    $"\nErro: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Método para buscar log pelo Id
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     GET /logs/id
        ///
        /// </remarks>
        /// <param name="id">Identificador do log</param>
        /// <returns>Dados do log pesquisado</returns>
        /// <response code="200">Pesquisa realizada com sucesso</response>
        /// <response code="201">Log não localizado</response>
        /// <response code="400">Erro na requisição</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<LogViewModel> Get(Guid id)
        {
            try
            {
                var log = _logAppService.GetById(id);

                if (log != default)
                    return Ok(log);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao buscar o log de código: {id}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Método para incluir o log
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     POST /logs/id
        ///
        /// </remarks>
        /// <param name="logViewModel">Dados do log a ser incluído (body da msg)</param>
        /// <returns></returns>
        /// <response code="200">Inclusão realizada com sucesso</response>
        /// <response code="400">Erro na requisição</response>        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post([FromBody()] LogViewModel logViewModel)
        {
            try
            {
                logViewModel = _logAppService.Add(logViewModel);

                return Created($"{Request.Path.Value}/{logViewModel.Id}", logViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao incluir o log de código: {logViewModel.Id}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Método para atualizar o log
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     PUT /logs/id
        ///
        /// </remarks>
        /// <param name="id">Identificador do log</param>
        /// <param name="logViewModel">Dados do log a ser alterado (body da msg)</param>
        /// <returns></returns>
        /// <response code="200">Alteração realizada com sucesso</response>
        /// <response code="400">Erro na requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(Guid id, [FromBody()] LogViewModel logViewModel)
        {
            if (id != logViewModel.Id)
                return BadRequest(new
                {
                    Message = $"Identificador do log diferente dos dados informados: {id}."
                });
            try
            {
                _logAppService.Update(logViewModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao alterar o log de código: {id}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Método para remover log por Id
        /// </summary>
        /// <remarks>
        /// Exemplo de requisção:
        ///
        ///     DELETE /logs/id
        ///
        /// </remarks>
        /// <param name="id">Identificador do log</param>
        /// <returns></returns>
        /// <response code="200">Exclusão realizada com sucesso</response>
        /// <response code="400">Erro na requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Delete(Guid id)
        {            
            try
            {
                _logAppService.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = $"Ocorreu um erro ao excluir o log de código: {id}." +
                    $"\nErro: {ex.Message}"
                });
            }
        }
    }
}