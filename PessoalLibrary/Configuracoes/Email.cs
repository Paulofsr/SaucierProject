using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Configuration;

namespace PessoalLibrary.Configuracoes
{
    public class Email
    {
        #region Configurações do E-mail de Envio
        private static string EmailSMTP
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailSMTP"];
            }
        }

        private static string EmailConta
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailConta"];
            }
        }

        private static string UsuarioConta
        {
            get
            {
                return ConfigurationManager.AppSettings["UsuarioConta"];
            }
        }

        private static string SenhaConta
        {
            get
            {
                return ConfigurationManager.AppSettings["SenhaConta"];
            }
        }

        private static string Port
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailPorta"];
            }
        }

        private static bool DefaultCredentials
        {
            get
            {
                object obj;
                obj = ConfigurationManager.AppSettings["DefaultCredentials"];
                if (obj == null || ((string)obj) == string.Empty)
                    return false;
                return Convert.ToBoolean(obj);
            }
        }

        private static bool EnableSsl
        {
            get
            {
                object obj;
                obj = ConfigurationManager.AppSettings["EnableSsl"];
                if (obj == null || ((string)obj) == string.Empty)
                    return false;
                return Convert.ToBoolean(obj);
            }
        }

        #region Is Body HTML in...
        public static bool IsBodyHtmlNewsletter
        {
            get
            {
                object obj = ConfigurationManager.AppSettings["IsBodyHtmlNewsletter"];
                if (obj == null || ((string)obj) == string.Empty)
                    return false;
                return Convert.ToBoolean(obj);
            }
        }
        #endregion //Is Body HTML in...

        public static string EmailFaleConosco
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailFaleConosco"];
            }
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
        #endregion

        private static string Remetente
        {
            get
            {
                return "DopSexy.com";
            }
        }

        public static void EnviarEmail(string email, string titulo, string mensagem, bool isBodyHTML)
        {
            List<string> emails = GetEmails(email);
            if (emails.Count > 1)
            {
                List<string> ccs = new List<string>();
                for (int i = 1; i < emails.Count; i++)
                    ccs.Add(emails[i]);
                EnviarEmail(emails[0], titulo, mensagem, isBodyHTML, ccs);
            }
            else
            {
                MailAddress to = new MailAddress(email.Replace(";", ""));
                MailAddress from = new MailAddress(EmailConta, Remetente);
                MailMessage menssege = new MailMessage(from, to);
                menssege.Subject = titulo;
                menssege.IsBodyHtml = isBodyHTML;
                menssege.Body = mensagem;

                // dados para autenticação
                SmtpClient cliente = new SmtpClient(EmailSMTP);
                cliente.UseDefaultCredentials = DefaultCredentials;
                cliente.EnableSsl = EnableSsl;
                cliente.Credentials = new NetworkCredential(UsuarioConta, SenhaConta);
                if (Port != string.Empty)
                    cliente.Port = Convert.ToInt32(Port);

                // envia a mensagem
                
                    cliente.Send(menssege);
            }
        }

        public static void EnviarEmail(string email, string titulo, string mensagem, bool isBodyHTML, string oculto)
        {
            List<string> emails = GetEmails(email);
            List<string> ocultos = GetEmails(oculto);
            if (emails.Count > 1 || ocultos.Count > 1)
            {
                List<string> ccs = new List<string>();
                for (int i = 1; i < emails.Count; i++)
                    ccs.Add(emails[i]);
                EnviarEmail(emails[0], titulo, mensagem, isBodyHTML, ccs, ocultos);
            }
            else
            {
                MailAddress to = new MailAddress(email.Replace(";", ""));
                MailAddress from = new MailAddress(EmailConta, Remetente);
                MailMessage menssege = new MailMessage(from, to);
                menssege.Subject = titulo;
                menssege.IsBodyHtml = isBodyHTML;
                menssege.Body = mensagem;

                if (oculto != string.Empty)
                    menssege.Bcc.Add(oculto);

                // dados para autenticação
                SmtpClient cliente = new SmtpClient(EmailSMTP);
                cliente.UseDefaultCredentials = DefaultCredentials;
                cliente.EnableSsl = EnableSsl;
                cliente.Credentials = new NetworkCredential(UsuarioConta, SenhaConta);
                if (Port != string.Empty)
                    cliente.Port = Convert.ToInt32(Port);

                // envia a mensagem
                cliente.Send(menssege);
            }
        }

        public static void EnviarEmail(string email, string titulo, string mensagem, bool isBodyHTML, List<string> emails)
        {
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress(EmailConta, Remetente);
            MailMessage menssege = new MailMessage(from, to);
            menssege.Subject = titulo;
            menssege.IsBodyHtml = isBodyHTML;
            menssege.Body = mensagem;
            foreach (string cc in emails)
                menssege.CC.Add(cc);

            // dados para autenticação
            SmtpClient cliente = new SmtpClient(EmailSMTP);
            cliente.UseDefaultCredentials = DefaultCredentials;
            cliente.EnableSsl = EnableSsl;
            cliente.Credentials = new NetworkCredential(UsuarioConta, SenhaConta);
            if (Port != string.Empty)
                cliente.Port = Convert.ToInt32(Port);

            // envia a mensagem
            cliente.Send(menssege);
        }

        public static void EnviarEmail(string email, string titulo, string mensagem, bool isBodyHTML, List<string> emails, List<string> ocultos)
        {
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress(EmailConta, Remetente);
            MailMessage menssege = new MailMessage(from, to);
            menssege.Subject = titulo;
            menssege.IsBodyHtml = isBodyHTML;
            menssege.Body = mensagem;
            foreach (string cc in emails)
                menssege.CC.Add(cc);
            foreach (string bcc in ocultos)
                menssege.Bcc.Add(bcc);

            // dados para autenticação
            SmtpClient cliente = new SmtpClient(EmailSMTP);
            cliente.UseDefaultCredentials = DefaultCredentials;
            cliente.EnableSsl = EnableSsl;
            cliente.Credentials = new NetworkCredential(UsuarioConta, SenhaConta);
            if (Port != string.Empty)
                cliente.Port = Convert.ToInt32(Port);

            // envia a mensagem
            cliente.Send(menssege);
        }

        public static void EnviarEmail(string email, string titulo, string mensagem, bool isBodyHTML, Stream file, string foto)
        {
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress(EmailConta, Remetente);
            MailMessage menssege = new MailMessage(from, to);
            Attachment att = new Attachment(file, foto);
            menssege.Subject = titulo;
            menssege.IsBodyHtml = isBodyHTML;
            menssege.Body = mensagem;
            menssege.Attachments.Add(att);

            // dados para autenticação
            SmtpClient cliente = new SmtpClient(EmailSMTP);
            cliente.UseDefaultCredentials = DefaultCredentials;
            cliente.EnableSsl = EnableSsl;
            cliente.Credentials = new NetworkCredential(UsuarioConta, SenhaConta);
            if (Port != string.Empty)
                cliente.Port = Convert.ToInt32(Port);

            // envia a mensagem
            cliente.Send(menssege);
        }

        private static List<string> GetEmails(string email)
        {
            List<string> emails = new List<string>();
            if (email.Contains(",") || email.Contains(";"))
            {
                string[] result;
                List<char> separ = new List<char>();
                separ.Add(',');
                separ.Add(';');
                result = email.Split(separ.ToArray());
                for (int i = 0; i < result.Length; i++)
                    emails.Add(result.GetValue(i).ToString().Replace(" ", ""));
            }
            emails.Remove("");
            emails.Remove(" ");
            return emails;
        }
    }
}
