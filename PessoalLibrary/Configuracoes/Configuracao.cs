using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace PessoalLibrary.Configuracoes
{
    public class Configuracao
    {
        private static string _baseId = string.Empty;

        public static string BaseId
        {
            private get { return _baseId; }
            set { _baseId = value; }
        }

        private Configuracao() { }

        public static string ConnectionStringBase
        {
            get { return ConfigurationManager.ConnectionStrings["SDBConnectionString"].ConnectionString; }
        }

        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SDBConnectionString"].ConnectionString; }
        }

        public static string ConnectionStringCliente
        {
            get { return ConfigurationManager.ConnectionStrings["SDBConnectionString"].ConnectionString.Replace("SaucierDB", BaseId); }
        }

        public static string GetPastaDeArquivos
        {
            get
            {
                return ConfigurationManager.AppSettings["PastaDeArquivos"].ToString();
            }
        }

        public static string DateFormat24
        {
            get { return "yyyy-MM-dd HH:mm:ss"; }
        }

        public static string DateFormat24Visualizar
        {
            get { return "dd/MM/yyyy HH:mm:ss"; }
        }

        public static string DateFormatShort
        {
            get { return "dd/MM/yyyy"; }
        }

        public static string DateFormat12
        {
            get { return "yyyy-MM-dd hh:mm:ss"; }
        }

        public static int GetNumeroDePaginasNoGrupo
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["NumeroDePaginasNoGrupo"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 5; }
            }
        }

        public static int GetNumeroDeItensNaPagina
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["NumeroDeItensNaPagina"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 5; }
            }
        }

        public static string UrlPrincipal
        {
            get
            {
                return ConfigurationManager.AppSettings["UrlPrincipal"].ToString();
            }
        }

        public static int PrazoOn
        {
            get
            {
                string pz = ConfigurationManager.AppSettings["PrazoOn"].ToString();
                try
                {
                    return Convert.ToInt32(pz);
                }
                catch
                {
                    return 11;
                }
            }
        }

        public static string ToStringDateTime
        {
            get
            {
                return ConfigurationManager.AppSettings["ToStringDateTime"].ToString();
            }
        }

        public static string AlertaOculto
        {
            get
            {
                return ConfigurationManager.AppSettings["AlertaOculto"].ToString();
            }
        }

        public static string EmailAlertaCoopercargas
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailAlertaCoopercargas"].ToString();
            }
        }

        public static string GetEmailRelatorio
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailRelatorio"].ToString();
            }
        }

        public static string GetEmailAtrela
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailAtrela"].ToString();
            }
        }

        public static int GetTempMinDia
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["TempMinDia"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 0; }
            }
        }

        public static int GetTempMinHora
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["TempMinHora"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 0; }
            }
        }

        public static int GetTempMinMinuto
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["TempMinMinuto"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 5; }
            }
        }

        public static int GetTempMinSegundo
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["TempMinSegundo"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 0; }
            }
        }

        public static int GetVelocidadeLimite
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["VelocidadeLimite"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 5; }
            }
        }

        public static int GetRaioLimite
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["RaioLimite"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 50; }
            }
        }

        public static int GetVariacaoMinMaxParadaLigado
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["VariacaoMinMaxParadaLigado"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 3; }
            }
        }

        public static int GetNumLoop
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["NumLoop"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 1; }
            }
        }

        public static int GetNumLoopCli
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["NumLoopCli"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 1; }
            }
        }

        public static int GetLinhasRelatorio
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["LinhasRelatorio"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 100; }
            }
        }

        public static int GetAtualizarParadas
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["AtualizarParadas"].ToString();
                    return Convert.ToInt32(num);
                }
                catch { return 100; }
            }
        }

        public static long GetLimiteDeleteMaximo
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["LimiteDeleteMaximo"].ToString();
                    return Convert.ToInt64(num);
                }
                catch { return 200; }
            }
        }

        public static long GetLimiteDeleteMinimo
        {
            get
            {
                try
                {
                    string num = ConfigurationManager.AppSettings["LimiteDeleteMinimo"].ToString();
                    return Convert.ToInt64(num);
                }
                catch { return 100; }
            }
        }
    }
}
