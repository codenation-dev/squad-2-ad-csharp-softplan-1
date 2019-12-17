﻿using CentralDeErros.Domain.Interfaces.Base;
using CentralDeErros.Domain.Models;

namespace CentralDeErros.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface usuário Service
    /// </summary>
    public interface IUserService : IServiceBase<User>
    {
        User Login(User obj);
    }
}
