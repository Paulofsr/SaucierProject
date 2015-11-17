using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.ClienteBase
{
    #region Criterias
    public class ClienteCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public ClienteCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class ClienteCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class Cliente : ObjectModel<Cliente>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Base")]
        public string Base { get; private set; }

        public bool Ativa { get; set; }

        public DateTime CriadoEm { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Basica; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Cliente() 
        {
        }

        public new static Cliente Empty()
        {
            return New(new ClienteCriteriaCreateBase());
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "Cliente";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = Guid.NewGuid();
            Base = "Saucier" + Id.ToString() + "DB";
            CriadoEm = DateTime.Now;
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@Id"));
            lista.Add(CriarParametro(SqlDbType.Bit, Ativa, "@Ativa"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Base, "@Base"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Nome, "@Nome"));
            lista.Add(CriarParametro(SqlDbType.DateTime, CriadoEm, "@CriadoEm"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((ClienteCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            Novo = false;
            if (!reader.Read())
            {
                Vazio = true;
                return;
            }
            Vazio = false;
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.CriadoEm = Convert.ToDateTime(reader["CriadoEm"]);
            this.Base = reader["Base"].ToString();
            this.Ativa = Convert.ToBoolean(reader["Ativa"]);
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        { }
        #endregion Parameters

        #endregion Data Methods
    }
}
