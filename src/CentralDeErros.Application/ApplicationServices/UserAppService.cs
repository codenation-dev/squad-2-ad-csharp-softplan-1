using AutoMapper;
using CentralDeErros.Application.Extensions;
using CentralDeErros.Domain.Interfaces.Services;
using CentralDeErros.Domain.Models;
using System;
using System.Collections.Generic;
using CentralDeErros.Application.Interface;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.CrossCutting.Utils;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Application.ApplicationServices
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicialização da classe UserAppService
        /// </summary>
        /// <param name="userService">IUserService</param>
        /// <param name="mapper">IMapper</param>
        public UserAppService(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Método para realizar a inclusão de um novo Usuário
        /// </summary>
        /// <param name="userViewModel"></param>
        public UserViewModel Add(UserViewModel userViewModel)
        {
            var model = _mapper.Map<User>(userViewModel);
            model.Password = model.Password.ToHashMD5();
            var user = _userService.Add(model);
            return _mapper.Map<UserViewModel>(user);
        }
        
    /// <summary>
    /// Método para realizar a listagem de um Usuário
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns>UserViewModel</returns>
    public IList<UserViewModel> Find(Func<User, bool> predicate)
        {
            var modelUsers = _userService.Find(predicate);
            return _mapper.Map<List<UserViewModel>>(modelUsers);
        }

        /// <summary>
        /// Método para realizar a listagem de todos os Usuários
        /// </summary>
        /// <returns>UserViewModel</returns>
        public IList<UserViewModel> GetAll()
        {
            var modelUsers = _userService.GetAll();
            return _mapper.Map<List<UserViewModel>>(modelUsers);
        }

        /// <summary>
        /// Método para realizar a busca de um Usuário pelo Id
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>UserViewModel</returns>
        public UserViewModel GetById(Guid id)
        {
            var modelUsers = _userService.GetById(id);
            return _mapper.Map<UserViewModel>(modelUsers);
        }

        /// <summary>
        /// Método para realizar a remoção de um Usuário pelo Id
        /// </summary>
        /// <param name="id">Identificador</param>
        public void Remove(Guid id)
        {
            _userService.Remove(id);
        }

        /// <summary>
        /// Método para realizar a atualização de um Usuário
        /// </summary>
        /// <param name="userViewModel">UserViewModel a ser atualizado</param>
        public void Update(UserViewModel userViewModel)
        {
            var modelUser = _mapper.Map<User>(userViewModel);
            _userService.Update(modelUser);
        }

        /// <summary>
        /// Método para realizar o login do usuário
        /// </summary>
        /// <param name="obj">LoginViewModel para autenticação do usuário</param>
        /// <returns>UserViewModel</returns>
        public UserViewModel Login(LoginViewModel obj)
        {
            var usuario = _userService.Login(new User { Email = obj.Email, Password = obj.Password });
            return _mapper.Map<UserViewModel>(usuario);
        }

        /// <summary>
        /// Método para realizar a pesquisa do usuário utilizando filtro - GraphQl
        /// </summary>
        /// <param name="filter"> Filtro da pesquisa</param>
        /// <returns>UserViewModel</returns>
        [ExcludeFromCodeCoverage]
        public IList<UserViewModel> GetUsersFilter(UserFilterViewModel? filter)
        {
            Func<User, bool> predicate = p => true;
            return Find(predicate.UserFilter(filter));
        }
    }
}
