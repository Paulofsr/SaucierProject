using System.ComponentModel.DataAnnotations;

namespace SaucierWeb.Models
{
    public class LogarModel
    {

        private string _mensagem = string.Empty;

        [Display(Name = "ERRO")]
        public string Mensagem
        {
            get { return _mensagem; }
            set { _mensagem = value; }
        }

        [Display(Name = "Login ou Email")]
        [Required(ErrorMessage = "Informe o seu Login ou Email.")]
        public string Email { get; set; }

        private string _senha = string.Empty;

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Informe a sua Senha.")]
        public string Senha
        {
            get { return _senha; }
            set { _senha = value; }
        }

        [Display(Name = "Cliente")]
        public string ClienteId { get; set; }
    }
}