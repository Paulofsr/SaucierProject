using SaucierLibrary.ClienteBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaucierWeb.Controllers
{
    public class HomeController : AutenticatedController
    {

        public ActionResult Index()
        {
            //Cliente cliente = Cliente.Get(new ClienteCriteriaBase(new Guid("adfasdfasdfasdfasdf")));
            //Geral.Logar("login", "OigaleaH1@3", new Guid("683BC06E-52EA-444A-97A6-F97E6FAC68C9"));
            //Cliente cliente = Cliente.New(new ClienteCriteriaCreateBase());
            //cliente.Nome = "Novo!! Again!!";
            //cliente.Save();
            //Usuario user = Usuario.New(new UsuarioCriteriaCreateBase(new Guid("B8F21383-8834-486C-98E4-2C09491565FD")));//cliente.Id));
            //user.Email = "teste@teste";
            //user.Nome = "Adm";
            //user.EmailConfirmado = true;
            //if (!user.SetLogin("login"))
            //    throw new Exception("Login já existe.");
            //user.Senha = "OigaleaH1@3";
            //user.ConfirmarSenha = "OigaleaH1@3";
            //user.Save();
            //user = Usuario.ValidarLoginSenha("login", "OigaleaH1@3", new Guid("B8F21383-8834-486C-98E4-2C09491565FD"));
            //if(user.Vazio)
            //    user = Usuario.ValidarLoginSenha("login", "OigaleaH1", cliente.Id);

            //kdjfhkajsdhf

            //if (!Geral.Logado)
            //    Geral.Logar("login", "OigaleaH1@3", new Guid("B8F21383-8834-486C-98E4-2C09491565FD"));
            //Usuario user = Geral.UsuarioLogado;
            //user.MudarSenha(PessoalLibrary.Configuracoes.Criptografia.Descriptografar(user.Senha), "123");
            //user.Save();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}