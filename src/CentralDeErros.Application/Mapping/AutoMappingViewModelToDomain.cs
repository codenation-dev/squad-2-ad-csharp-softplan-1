using AutoMapper;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.Domain.Models;

namespace CentralDeErros.Application.Mapping
{
    public class AutoMappingViewModelToDomain : Profile
    {
        /// <summary>
        /// AutoMapping de LogViewModel para Log
        /// </summary>
        public AutoMappingViewModelToDomain()
        {
            CreateMap<LogViewModel, Log>();
            CreateMap<UserViewModel, User>();
        }
    }
}