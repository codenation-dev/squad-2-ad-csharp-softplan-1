using System;
using System.Diagnostics.CodeAnalysis;

namespace CentralDeErros.Domain.Filters
{
    [ExcludeFromCodeCoverage]
    public class UserFilter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
    }
}
