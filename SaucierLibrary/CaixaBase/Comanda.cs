using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.CaixaBase
{
    #region Criterias
    public class ComandaCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public ComandaCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class ComandaCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class Comanda : ObjectModel<Comanda>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Informação")]
        public string Informacao { get; set; }

        [Display(Name = "Data Abertura")]
        public DateTime DataAbertura { get; set; }

        [Display(Name = "Pago")]
        public Boolean Pago { get; set; }

        public System.Guid CaixaId { get; set; }

        public Caixa Caixa { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Comanda()
        {
        }

        public new static Comanda Empty()
        {
            return New(new ComandaCriteriaCreateBase());
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "Comanda";
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
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, CaixaId, "@CaixaId"));
            lista.Add(CriarParametro(SqlDbType.DateTime, DataAbertura, "@DataAbertura"));
            lista.Add(CriarParametro(SqlDbType.Bit, Pago, "@Pago"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Informacao, "@Informacao"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((ComandaCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.DataAbertura = Convert.ToDateTime(reader["DataAbertura"].ToString());
            this.Informacao = reader["Informacao"].ToString();
            this.Pago = Convert.ToBoolean(reader["Pago"]);
            this.CaixaId = ConvertBase.ToGuid(reader["CaixaId"].ToString());
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Caixa = Caixa.GetByReader(reader);
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
