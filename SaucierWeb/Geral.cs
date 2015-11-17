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
                return (ObjectLoginSession != null && ObjectLoginSession is Usuario);
            }
        }

        public static bool Logar(string login, string senha, Guid clienteId)
        {
            Usuario usuario = Usuario.ValidarLoginSenha(login, senha, clienteId);
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
    }
}