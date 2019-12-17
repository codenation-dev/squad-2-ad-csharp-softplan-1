using System;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Application.ViewModels
{
    /// <summary>
    /// Classe de Usuário View Model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserFilterViewModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        public string? Email { get; set; }
        public bool? Active { get; set; }
        public string? Role { get; set; }
    }
}
