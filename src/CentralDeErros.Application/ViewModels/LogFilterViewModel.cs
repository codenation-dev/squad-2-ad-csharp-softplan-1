using System;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Application.ViewModels
{
    /// <summary>
    /// Classe de Log View Model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LogFilterViewModel
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public int? Event { get; set; }
        public string? Level { get; set; }
        public string? Environment { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Ip { get; set; }
        public Guid? Token { get; set; }
    }
}
