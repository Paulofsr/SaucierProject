using SaucierLibrary.ClienteBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaucierWeb.Controllers
{
    public class AutenticatedController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Geral.Logado)
                base.OnActionExecuting(filterContext);
            else
                Response.Redirect("~/Login");
        }
    }
}