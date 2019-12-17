using System;

namespace CentralDeErros.Domain.Models.Base
{
    /// <summary>
    /// Classe Base
    /// </summary>
    public class ModelBase
    {
        public Guid Id { get; set; }

        public ModelBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
