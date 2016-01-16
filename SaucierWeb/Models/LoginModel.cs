using System.ComponentModel.DataAnnotations;

namespace SaucierWeb.Models
{
    public class LoginModels
    {

        private string _mensagem = string.Empty;

        [Display(Name = "ERRO")]
        public string Mensagem
        {
            get { return _mensagem; }
            set { _mensagem = value; }
        }

        private string _login = string.Empty;

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Informe o Login.")]
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        private string _senha = string.Empty;

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Informe a Senha.")]
        public string Senha
        {
            get { return _senha; }
            set { _senha = value; }
        }
    }
}