using SaucierLibrary.CaixaBase;
using SaucierLibrary.ItemBase;
using SaucierWeb.Models;
using SaucierWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaucierWeb.Controllers
{
    [CustomAuthorizeAttribute]
    public class CaixaController : Controller
    {
        // GET: Caixa
        public ActionResult Index()
        {
            return View(Caixa.GetCaixaAberto());
        }

        public ActionResult Open()
        {
            return View(Geral.AbrirCaixa());
        }

        public ActionResult Refresh()
        {
            Caixa.GetCaixaAberto().Refresh();
            return Redirect("Index");
        }

        public ActionResult Close()
        {
            try
            {
                if (Geral.FecharCaixa())
                {
                    TempData["Msg"] = "Caixa fechado com sucesso!";
                    return RedirectToAction("Index");
                }
                TempData["ErroMsg"] = "Caixa não pode ser fechado, existe comanda em aberto.";
                return RedirectToAction("Erro");
            }
            catch(Exception ex)
            {
                TempData["ErroMsg"] = ex.Message;
                return RedirectToAction("Erro");
            }
        }

        public ActionResult Erro()
        {
            return View();
        }

        public ActionResult New(string info)
        {
            Caixa caixa = Caixa.GetCaixaAberto();
            Comanda comanda = caixa.NewComanda(info);
            return PartialView("_comandas", caixa.ToListComandaAtivas());
        }

        public ActionResult GetComanda(string id)
        {
            Comanda comanda = Comanda.Get(new ComandaCriteriaBase(new Guid(id)));
            return PartialView("_comanda", comanda);
        }

        public ActionResult GetCaixaComanda(string id)
        {
            Comanda comanda = Comanda.Get(new ComandaCriteriaBase(new Guid(id)));
            return PartialView("_caixaComanda", comanda);
        }

        public ActionResult SearchItem(string filter)
        {
            return PartialView("_tensFilter", Item.ToListByFilter(filter));
        }
        
        
        [HttpPost]
        public ActionResult AddItem(string comandaId, string itemId, string quantidade)
        {
            if (!string.IsNullOrEmpty(comandaId) && !string.IsNullOrEmpty(itemId) && !string.IsNullOrEmpty(quantidade))
            {
                Comanda comanda = Comanda.Get(new ComandaCriteriaBase(new Guid(comandaId)));
                Item item = Item.Get(new ItemCriteriaBase(new Guid(itemId)));
                if (!comanda.Vazio && !item.Vazio)
                {
                    if (comanda.AddItem(item.Id, Geral.UsuarioLogado.Id, Convert.ToDecimal(quantidade)))
                        comanda.Save();
                    return PartialView("_comanda", comanda);
                }
            }
            throw new Exception("Dados inválidos.");
        }

    }
}