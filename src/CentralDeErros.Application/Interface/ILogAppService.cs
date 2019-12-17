using CentralDeErros.Domain.Models;
using System;
using System.Collections.Generic;
using CentralDeErros.Application.ViewModels;

namespace CentralDeErros.Application.Interface
{
    public interface ILogAppService
    {
        LogViewModel Add(LogViewModel obj);
        void Update(LogViewModel obj);
        void Remove(Guid id);
        LogViewModel GetById(Guid id);
        IList<LogViewModel> GetAll();
        IList<LogViewModel> Find(Func<Log, bool> predicate);

        IList<LogViewModel> GetLogsFilter(LogFilterViewModel? filter);
    }
}
