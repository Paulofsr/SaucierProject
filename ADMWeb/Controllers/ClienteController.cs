using SaucierLibrary.ClienteBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADMWeb.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View(Cliente.ToList());
        }

        public ActionResult Delete(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                Cliente.Delete(new ClienteCriteriaBase(new Guid(id)));
            }
            return RedirectToAction("Index");
        }
    }
}