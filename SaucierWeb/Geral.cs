using PessoalLibrary.Configuracoes;
using SaucierLibrary.CaixaBase;
using SaucierLibrary.ClienteBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaucierWeb
{
    public static class Geral
    {
        #region User Login Session
        private static object ObjectLoginSession
        {
            get { return HttpContext.Current.Session["objectLoginSession"]; }
            set { HttpContext.Current.Session["objectLoginSession"] = value; }
        }

        public static Usuario UsuarioLogado
        {
            get
            {
                object obj = ObjectLoginSession;
                if (obj != null && obj is Usuario)
                    return (Usuario)obj;
                return Usuario.Empty();
            }
        }

        public static bool Logado
        {
            get
            {
                if (ObjectLoginSession != null && ObjectLoginSession is Usuario)
                {
                    if (PessoalLibrary.Configuracoes.Configuracao.ClientBaseIsEmpty) // É preciso atualizar, pois ao recarregar a session estará limpo a propriedade.
                        PessoalLibrary.Configuracoes.Configuracao.BaseId = UsuarioLogado.Cliente.Base;
                    return true;
                }
                return false;
            }
        }

        public static bool Logar(string login, string senha, string url)
        {
            Usuario usuario = Usuario.ValidarLoginSenha(login, senha, Cliente.GetId(url));
            if (!usuario.Vazio)
            {
                ObjectLoginSession = usuario;
                PessoalLibrary.Configuracoes.Configuracao.BaseId = usuario.Cliente.Base;
                return true;
            }
            return false;
        }

        public static void Sair()
        {
            ObjectLoginSession = null;
        }

        #endregion User Login Session

        #region Caixa
        public static Caixa AbrirCaixa()
        {
            return Caixa.AbrirCaixa(UsuarioLogado, new List<SaucierLibrary.CaixaBase.Configuracao>());
        }

        public static bool FecharCaixa()
        {
            Caixa caixa = Caixa.GetCaixaAberto();
            if (!caixa.Vazio)
                return caixa.FecharCaixa();
            throw new Exception("Não há caixa aberto.");
        }

        public static bool CaixaAberto()
        {
            return Caixa.ExisteCaixaAberto();
        }
        #endregion Caixa

        #region Session
        public static object URLSession
        {
            get { return HttpContext.Current.Session["uRLSession"]; }
            set { HttpContext.Current.Session["uRLSession"] = value; }
        }
        #endregion Session
    }
}