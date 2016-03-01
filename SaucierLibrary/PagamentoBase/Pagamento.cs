using PessoalLibrary.Padroes;
using SaucierLibrary.CaixaBase;
using SaucierLibrary.FuncionarioBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.PagamentoBase
{
    #region Criterias
    public class PagamentoCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public PagamentoCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class PagamentoCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class Pagamento : ObjectModel<Pagamento>
    {
        #region Properties
        public System.Guid Id { get; set; }

        public decimal ValorPago { get; set; }

        public decimal ValorRecebido { get; set; }

        public decimal Troco { get; set; }

        [Display(Name = "Data/Hora")]
        public DateTime DataHora { get; set; }

        [Display(Name = "Data/Hora Cancelamento")]
        public DateTime DataHoraCancelamento { get; set; }

        [Display(Name = "Cancelado")]
        public Boolean Cancelado { get; set; }

        [Display(Name = "Motivo")]
        public string Motivo { get; set; }

        public System.Guid ResponsavelId { get; set; }

        public System.Guid ComandaId { get; set; }

        public Guid TipoPagamentoId { get; set; }

        public Comanda Comanda { get; private set; }

        public TipoPagamento TipoPagamento { get; private set; }

        public Funcionario Responsavel { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Pagamento()
        {
        }

        public new static Pagamento Empty()
        {
            return New(new PagamentoCriteriaCreateBase());
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "Pagamento";
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
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ComandaId, "@ComandaId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, TipoPagamentoId, "@TipoPagamentoId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ResponsavelId, "@ResponsavelId"));
            lista.Add(CriarParametro(SqlDbType.Real, ValorPago, "@ValorPago"));
            lista.Add(CriarParametro(SqlDbType.Real, ValorRecebido, "@ValorRecebido"));
            lista.Add(CriarParametro(SqlDbType.Real, Troco, "@Troco"));
            lista.Add(CriarParametro(SqlDbType.DateTime, DataHora, "@DataHora"));
            lista.Add(CriarParametro(SqlDbType.DateTime, DataHoraCancelamento, "@DataHoraCancelamento"));
            lista.Add(CriarParametro(SqlDbType.Bit, Cancelado, "@Cancelado"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Motivo, "@Motivo"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((PagamentoCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.ComandaId = ConvertBase.ToGuid(reader["ComandaId"].ToString());
            this.TipoPagamentoId = ConvertBase.ToGuid(reader["TipoPagamentoId"].ToString());
            this.ResponsavelId = ConvertBase.ToGuid(reader["ResponsavelId"].ToString());
            this.ValorPago = Convert.ToDecimal(reader["ValorPago"].ToString());
            this.ValorRecebido = Convert.ToDecimal(reader["ValorRecebido"].ToString());
            this.Troco = Convert.ToDecimal(reader["Troco"].ToString());
            this.DataHora = Convert.ToDateTime(reader["DataHora"].ToString());
            this.DataHoraCancelamento = Convert.ToDateTime(reader["DataHoraCancelamento"].ToString());
            this.Cancelado = Convert.ToBoolean(reader["Cancelado"]);
            this.Motivo = reader["Motivo"].ToString();
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Comanda = Comanda.GetByReader(reader);
            Responsavel = Funcionario.GetByReader(reader);
            TipoPagamento = TipoPagamento.GetByReader(reader);
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
