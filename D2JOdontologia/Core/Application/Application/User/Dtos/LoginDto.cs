using System.ComponentModel.DataAnnotations;

namespace Application.User.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "E-mail é campo obrigatório para Login")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo {1} caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é campo obrigatório para Login")]
        [StringLength(50, ErrorMessage = "Senha deve ter no máximo {1} caracteres")]
        public string Password { get; set; }
    }
}
