using System.ComponentModel.DataAnnotations;

namespace BlogApi.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Informe a senha para login")]
        public string Password { get; set; }
    }
}
