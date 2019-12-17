using CentralDeErros.Domain.Models;
using System;
using System.Collections.Generic;
using CentralDeErros.Application.ViewModels;

namespace CentralDeErros.Application.Interface
{
    public interface IUserAppService
    {
        UserViewModel Add(UserViewModel obj);
        void Update(UserViewModel obj);
        void Remove(Guid id);
        UserViewModel GetById(Guid id);
        IList<UserViewModel> GetAll();
        IList<UserViewModel> Find(Func<User, bool> predicate);
        
        UserViewModel Login(LoginViewModel obj);
        IList<UserViewModel> GetUsersFilter(UserFilterViewModel? filter);
    }
}
