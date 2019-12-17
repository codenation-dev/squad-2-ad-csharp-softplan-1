using AutoMapper;
using CentralDeErros.Domain.Interfaces.Services;
using CentralDeErros.Domain.Models;
using System;
using System.Collections.Generic;
using CentralDeErros.Application.Interface;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.Application.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Application.ApplicationServices
{
    public class LogAppService : ILogAppService
    {
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicialização de LogAppService classe
        /// </summary>
        /// <param name="logService">ILogService</param>
        /// <param name="mapper">IMapper</param>
        public LogAppService(ILogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }

        /// <summary>
        /// Método para incluir
        /// </summary>
        /// <param name="logViewModel"></param>
        public LogViewModel Add(LogViewModel logViewModel)
        {
            if (logViewModel.CreatedAt == null || logViewModel.CreatedAt < DateTime.Now)
                logViewModel.CreatedAt = DateTime.Now;
            var model = _mapper.Map<Log>(logViewModel);
            var log = _logService.Add(model);
            return _mapper.Map<LogViewModel>(log);
        }

        /// <summary>
        /// Método para pesquisar
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>LogViewModel</returns>
        public IList<LogViewModel> Find(Func<Log, bool> predicate)
        {
            var modelLogs = _logService.Find(predicate);
            return _mapper.Map<List<LogViewModel>>(modelLogs);
        }

        /// <summary>
        /// Método para listar todos
        /// </summary>
        /// <returns>LogViewModel</returns>
        public IList<LogViewModel> GetAll()
        {
            var modelLogs = _logService.GetAll();
            return _mapper.Map<List<LogViewModel>>(modelLogs);
        }

        /// <summary>
        /// Método para buscar LogViewModel especifico
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>LogViewModel</returns>
        public LogViewModel GetById(Guid id)
        {
            var modelLog = _logService.GetById(id);
            return _mapper.Map<LogViewModel>(modelLog);
        }

        /// <summary>
        /// Método para remover
        /// </summary>
        /// <param name="id">Identificador</param>
        public void Remove(Guid id)
        {
            _logService.Remove(id);
        }

        /// <summary>
        /// Método para atualizar
        /// </summary>
        /// <param name="logViewModel">LogViewModel a ser atualizado</param>
        public void Update(LogViewModel logViewModel)
        {
            var modelLog = _mapper.Map<Log>(logViewModel);
            _logService.Update(modelLog);
        }

        /// <summary>
        /// Método para realizar a pesquisa do log utilizando filtro - GraphQl
        /// </summary>
        /// <param name="filter"> Filtro da pesquisa</param>
        /// <returns>LogViewModel</returns>
        [ExcludeFromCodeCoverage]
        public IList<LogViewModel> GetLogsFilter(LogFilterViewModel? filter)
        {
            Func<Log, bool> predicate = p => true;
            return Find(predicate.LogFilter(filter));
        }
    }
}