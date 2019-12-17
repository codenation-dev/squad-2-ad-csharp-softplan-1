﻿using CentralDeErros.Domain.Interfaces.Repositories;
using CentralDeErros.Domain.Interfaces.Services;
using CentralDeErros.Domain.Models;
using CentralDeErros.Domain.Services.Base;

namespace CentralDeErros.Domain.Services
{
    public class LogService : ServiceBase<Log>, ILogService
    {
        public LogService(ILogRepository logRepository)
            : base(logRepository)
        {

        }
    }
}
