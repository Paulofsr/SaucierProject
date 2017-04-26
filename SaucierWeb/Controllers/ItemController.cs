using SaucierLibrary.ItemBase;
using SaucierWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaucierWeb.Controllers
{
    [CustomAuthorize]
    public class ItemController : Controller
    {
        // GET: Item
        public ActionResult Index()
        {
            return View(Item.ToList());
        }

        // GET: Item/Details/5
        public ActionResult Details(string id)
        {
            return View(GetItem(id));
        }

        private Item GetItem(string id)
        {
            id = string.IsNullOrEmpty(id) ? Guid.Empty.ToString() : id;
            return Item.Get(new ItemCriteriaBase(new Guid(id)));
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            return View(Item.New(new ItemCriteriaCreateBase()));
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(Item model)
        {
            return Save(model);
        }

        private ActionResult Save(Item model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Save();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Erro ao Salvar", ex.Message);
            }
            return View(model);
        }

        // GET: Item/Edit/5
        public ActionResult Edit(string id)
        {
            return View(GetItem(id));
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Edit(Item model)
        {
            return Save(model);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(string id)
        {
            return View(GetItem(id));
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(Item model)
        {
            try
            {
                Item.Delete(new ItemCriteriaBase(model.Id));
                return RedirectToAction("Index");
            }
            catch
            {
            }
            return View(model);
        }
    }
}
