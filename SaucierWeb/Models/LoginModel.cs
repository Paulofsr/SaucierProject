using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaucierWeb.Models
{
    public class LoginModel
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Informe o seu email.")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Informe a sua senha.")]
        public string Senha { get; set; }

        public string LastUrl { get; set; }

        public LoginModel(string url)
        {
            this.LastUrl = url;
        }

        public LoginModel() { }
    }
}