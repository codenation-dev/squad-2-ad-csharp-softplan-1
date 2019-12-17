using AutoMapper;
using CentralDeErros.Application.ViewModels;
using CentralDeErros.Domain.Models;

namespace CentralDeErros.Application.Mapping
{
    public class AutoMappingDomainToViewModel : Profile
    {
        /// <summary>
        /// AutoMapping de Log para LogViewModel
        /// </summary>
        public AutoMappingDomainToViewModel()
        {
            CreateMap<Log, LogViewModel>();
            CreateMap<User, UserViewModel>();
        }
    }
}
