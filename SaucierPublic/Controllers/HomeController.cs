using SaucierLibrary.ClienteBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaucierPublic.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(Cadastro.New(new CadastroCriteriaCreateBase()));
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

        public ActionResult Cadastrar()
        {
            return Redirect("~/");
        }

        [HttpPost]
        public ActionResult Cadastrar(Cadastro model)
        {
            if (ModelState.IsValid)
            {
                model.Save();
                TempData["sucess"] = "Cadastro realizado com sucesso!";
                return Redirect("~/");
            }
            return View("Index", model);
        }
    }
}