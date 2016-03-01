using PessoalLibrary.Configuracoes;
using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.ClienteBase
{
    #region Criterias
    public class UsuarioCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public UsuarioCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class UsuarioCriteriaCreateBase : ICriteriaCreateBase
    {
        public Guid ClienteId { get; set; }
        public UsuarioCriteriaCreateBase(Guid clienteId)
        {
            ClienteId = clienteId;
        }
    }
    #endregion Criterias

    public class Usuario : ObjectModel<Usuario>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Login")]
        public string Login { get; private set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Campo Senha é obrigatório.")]
        public string Senha { get; private set; }

        public System.Guid ClienteId { get; set; }

        private string _email = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Campo Email é obrigatório.")]
        public string Email
        {
            get
            { return _email; }
            set
            {
                if (_email != value)
                    EmailConfirmado = false;
                _email = value;
            }
        }


        public bool EmailConfirmado { get; set; }

        public DateTime CriadoEm { get; set; }

        public Cliente Cliente { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Basica; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Usuario()
        {
        }

        public new static Usuario Empty()
        {
            return New(new UsuarioCriteriaCreateBase(Guid.Empty));
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "Usuario";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = Guid.NewGuid();
            ClienteId = ((UsuarioCriteriaCreateBase)criteria).ClienteId;
            CriadoEm = DateTime.Now;
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@Id"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Nome, "@Nome"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Login, "@Login"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Senha, "@Senha"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ClienteId, "@ClienteId"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Email, "@Email"));
            lista.Add(CriarParametro(SqlDbType.Bit, EmailConfirmado, "@EmailConfirmado"));
            lista.Add(CriarParametro(SqlDbType.DateTime, CriadoEm, "@CriadoEm"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((UsuarioCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.Login = reader["Login"].ToString();
            this.Senha = reader["Senha"].ToString();
            this.ClienteId = ConvertBase.ToGuid(reader["ClienteId"].ToString());
            this.Email = reader["Email"].ToString();
            this.EmailConfirmado = Convert.ToBoolean(reader["EmailConfirmado"]);
            this.CriadoEm = Convert.ToDateTime(reader["CriadoEm"]);
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Cliente = Cliente.GetByReader(reader);
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #region Validar Login Senha
        public static Usuario ValidarLoginSenha(string login, string senha, Guid ClienteId)
        {
            Usuario usuario = Empty();
            usuario.GetByLoginSenha(login, senha, ClienteId);
            if (usuario.Id != Guid.Empty)
                usuario = Get(new UsuarioCriteriaBase(usuario.Id));
            return usuario;
        }

        private void GetByLoginSenha(string login, string senha, Guid ClienteId)
        {
            Senha = senha;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(CriarParametro(SqlDbType.VarChar, login, "@Login"));
            parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, ClienteId, "@ClienteId"));
            ExecuteReaderProcedure("ValidarLoginUsuario", parameters.ToArray(), "ReturnByLoginSenha");
        }

        public void ReturnByLoginSenha(SqlDataReader reader)
        {
            if (reader.Read())
            {
                string senha = Criptografia.Descriptografar(reader["Senha"].ToString());
                if (senha == Senha)
                {
                    Id = ConvertBase.ToGuid(reader["Id"].ToString());
                }
            }
        }
        #endregion Validar Login Senha

        #region SetLogin
        public bool SetLogin(string login)
        {
            Login = login;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(CriarParametro(SqlDbType.VarChar, Login, "@Login"));
            parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, ClienteId, "@ClienteId"));
            parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@Id"));
            string query = @"
SELECT [Id]
      ,[Nome]
      ,[Login]
      ,[Senha]
      ,[ClienteId]
      ,[Email]
      ,[EmailConfirmado]
      ,[CriadoEm]
  FROM [SaucierDB].[dbo].[UsuarioTB]
where [Login] = @Login and [ClienteId] = @ClienteId and [Id] != @Id;";

            ExecuteReaderQuery(query, parameters.ToArray(), "ReturnLogin");
            return !string.IsNullOrEmpty(Login);
        }

        public void ReturnLogin(SqlDataReader reader)
        {
            if (reader.Read())
                Login = string.Empty;
        }
        #endregion SetLogin

        #region Change Password
        public bool MudarSenha(string senhaAtual, string novaSenha)
        {
            if(String.IsNullOrEmpty(Senha) || senhaAtual.Equals(Criptografia.Descriptografar(Senha)))
            {
                Senha = Criptografia.Criptografar(novaSenha);
            }
            return false;
        }
        #endregion Change Password

        #endregion Custom Methods
    }
}
