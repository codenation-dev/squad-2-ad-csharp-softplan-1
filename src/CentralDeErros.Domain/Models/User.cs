using CentralDeErros.Domain.Models.Base;
using System.Collections.Generic;

namespace CentralDeErros.Domain.Models
{
    /// <summary>
    /// Classe de usuário
    /// </summary>
    public class User : ModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
