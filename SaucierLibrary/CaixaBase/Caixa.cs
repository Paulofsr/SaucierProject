using PessoalLibrary.Padroes;
using SaucierLibrary.RestauranteBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.CaixaBase
{
    #region Criterias
    public class CaixaCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public CaixaCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class CaixaCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class Caixa : ObjectModel<Caixa>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Data Abertura")]
        public DateTime DataAbertura { get; set; }

        [Display(Name = "Data Fechamento")]
        public DateTime DataFechamento { get; set; }

        [Display(Name = "Data Cancelamento")]
        public DateTime DataCancelamento { get; set; }

        [Display(Name = "Motivo Cancelamento")]
        public string MotivoCancelamento { get; set; }

        public System.Guid RestauranteId { get; set; }

        public Restaurante Restaurante { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Caixa()
        {
        }

        public new static Caixa Empty()
        {
            return New(new CaixaCriteriaCreateBase());
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "Caixa";
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
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, RestauranteId, "@RestauranteId"));
            lista.Add(CriarParametro(SqlDbType.DateTime, DataAbertura, "@DataAbertura"));
            lista.Add(CriarParametro(SqlDbType.DateTime, DataFechamento, "@DataFechamento"));
            lista.Add(CriarParametro(SqlDbType.DateTime, DataCancelamento, "@DataCancelamento"));
            lista.Add(CriarParametro(SqlDbType.VarChar, MotivoCancelamento, "@MotivoCancelamento"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((CaixaCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.DataAbertura = Convert.ToDateTime(reader["DataAbertura"].ToString());
            this.DataFechamento = Convert.ToDateTime(reader["DataFechamento"].ToString());
            this.DataCancelamento = Convert.ToDateTime(reader["DataCancelamento"].ToString());
            this.RestauranteId = ConvertBase.ToGuid(reader["RestauranteId"].ToString());
            this.MotivoCancelamento = reader["MotivoCancelamento"].ToString();
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Restaurante = Restaurante.GetByReader(reader);
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
