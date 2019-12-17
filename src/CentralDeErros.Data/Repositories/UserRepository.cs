using CentralDeErros.Data.Context;
using CentralDeErros.Data.Repositories.Base;
using CentralDeErros.Domain.Interfaces.Repositories;
using CentralDeErros.Domain.Models;

namespace CentralDeErros.Data.Repositories
{
    /// <summary>
    /// Classe Usuário Repository
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(Contexto context) : base(context)
        {

        }
    }
}
