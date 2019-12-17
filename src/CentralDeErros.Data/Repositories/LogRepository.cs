using CentralDeErros.Data.Context;
using CentralDeErros.Data.Repositories.Base;
using CentralDeErros.Domain.Interfaces.Repositories;
using CentralDeErros.Domain.Models;

namespace CentralDeErros.Data.Repositories
{
    /// <summary>
    /// Classe Log Repository
    /// </summary>
    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(Contexto context) : base(context)
        {
        }
    }
}
