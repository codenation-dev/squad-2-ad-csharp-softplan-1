using System;

namespace CentralDeErros.Application.ViewModels.Base
{
    /// <summary>
    /// Classe View Model Base
    /// </summary>
    public class ViewModelBase
    {
        public Guid Id { get; set; }

        public ViewModelBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
