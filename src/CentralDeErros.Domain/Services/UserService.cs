﻿using CentralDeErros.Domain.Interfaces.Repositories;
using CentralDeErros.Domain.Interfaces.Services;
using CentralDeErros.Domain.Models;
using CentralDeErros.Domain.Services.Base;
using CentralDeErros.CrossCutting.Utils;
using System.Linq;

namespace CentralDeErros.Domain.Services
{
    public class UserService : ServiceBase<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
            : base(userRepository)
        {
            this._userRepository = userRepository;
        }

        public User Login(User obj)
        {
            var usuario = _userRepository.Find(p => p.Email == obj.Email && p.Password == obj.Password.ToHashMD5()).FirstOrDefault();

            if (usuario == default)
                return null;

            return usuario;
        }
    }
}
