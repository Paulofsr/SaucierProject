using SaucierLibrary.ClienteBase;
using SaucierWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaucierWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Geral.Sair();
            return View(new LogarModel());
        }

        public ActionResult Entrar()
        {
            ViewBag.Title = "Entrar";
            Geral.Sair();

            return View();
        }

        [HttpPost]
        public ActionResult Entrar(LogarModel model)
        {
            if (ModelState.IsValid)
            {
                if (Geral.Logar(model.Email, model.Senha, Cliente.GetId("reservamineira")))
                {
                    return Redirect("~/Home");
                }
                else
                    ModelState.AddModelError("Invalido", "Login e/ou Senha inválido.");
            }

            return View();
        }
    }
}