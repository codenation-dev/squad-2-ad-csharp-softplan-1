using System.ComponentModel.DataAnnotations;

namespace CentralDeErros.Application.ViewModels
{
    /// <summary>
    /// Classe de Login View Model
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
