using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.ClienteBase
{
    #region Criterias
    public class CadastroCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public CadastroCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class CadastroCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class Cadastro : ObjectModel<Cadastro>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o seu Nome.")]
        public string Nome { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Informe o seu Email.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Estabelecimento")]
        [Required(ErrorMessage = "Infome o nome do Estabelecimento.")]
        public string Estabelecimento { get; set; }

        [Required(ErrorMessage = "Infome o CNPJ do Estabelecimento.")]
        public string CNPJ { get; set; }

        public DateTime CriadoEm { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Basica; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Cadastro()
        {
        }

        public new static Cadastro Empty()
        {
            return New(new CadastroCriteriaCreateBase());
        }

        protected override void SaveChilds()
        {
        }

        protected override void BeforeSave()
        {
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "Cadastro";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = Guid.NewGuid();
            CriadoEm = DateTime.Now;
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@Id"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Nome, "@Nome"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Email, "@Email"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Senha, "@Senha"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Estabelecimento, "@Estabelecimento"));
            lista.Add(CriarParametro(SqlDbType.VarChar, CNPJ, "@CNPJ"));
            lista.Add(CriarParametro(SqlDbType.DateTime, CriadoEm, "@CriadoEm"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((CadastroCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.Email = reader["Email"].ToString();
            this.Senha = reader["Senha"].ToString();
            this.Estabelecimento = reader["Estabelecimento"].ToString();
            this.CNPJ = reader["CNPJ"].ToString();
            this.CriadoEm = ConvertBase.ToDateTime(reader["CriadoEm"]);
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        { }

        protected override void SetChildren()
        { }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
