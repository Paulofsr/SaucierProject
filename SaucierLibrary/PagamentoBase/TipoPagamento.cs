using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.PagamentoBase
{
    #region Criterias
    public class TipoPagamentoCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public TipoPagamentoCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class TipoPagamentoCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class TipoPagamento : ObjectModel<TipoPagamento>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo Tipo é obrigatório.")]
        public string Tipo { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Basica; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public TipoPagamento()
        {
        }

        public new static TipoPagamento Empty()
        {
            return New(new TipoPagamentoCriteriaCreateBase());
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "TipoPagamento";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = Guid.NewGuid();
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@Id"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Tipo, "@Tipo"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((TipoPagamentoCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Tipo = reader["Tipo"].ToString();
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        { }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
