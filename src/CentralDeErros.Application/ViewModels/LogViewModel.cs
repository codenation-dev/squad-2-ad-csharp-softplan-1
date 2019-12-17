using System;
using CentralDeErros.Application.ViewModels.Base;

namespace CentralDeErros.Application.ViewModels
{
    /// <summary>
    /// Classe de Log View Model
    /// </summary>
    /// 
    public class LogViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public int Event { get; set; }
        public string Level { get; set; }
        public string Environment { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Ip { get; set; }
        public virtual Guid Token { get; set; }
        public virtual UserViewModel User { get; set; }
    }
}
