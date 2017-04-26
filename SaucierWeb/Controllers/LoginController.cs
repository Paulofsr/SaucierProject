using SaucierWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaucierWeb.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        public ActionResult Index(string url)
        {
            return View(new LoginModel(url));
        }

        public ActionResult Entrar()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Entrar(LoginModel model)
        {
            if(ModelState.IsValid && Geral.Logar(model.Email, model.Senha, Request.Url.DnsSafeHost))
            {
                model.LastUrl = string.IsNullOrEmpty(model.LastUrl) ? "Home/Index" : model.LastUrl;
                return Redirect("~/" + model.LastUrl);
            }
            return View("Index", model);
        }
    }
}