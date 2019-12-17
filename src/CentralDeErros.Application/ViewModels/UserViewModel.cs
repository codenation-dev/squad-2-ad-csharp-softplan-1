using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CentralDeErros.Application.ViewModels.Base;

namespace CentralDeErros.Application.ViewModels
{
    /// <summary>
    /// Classe de Usuário View Model
    /// </summary>
    public class UserViewModel : ViewModelBase
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        public string AccessToken { get; set; }

        public bool Active { get; set; }

        [Required]
        public string Role { get; set; }
        public virtual ICollection<LogViewModel> Logs { get; set; }
    }
}
