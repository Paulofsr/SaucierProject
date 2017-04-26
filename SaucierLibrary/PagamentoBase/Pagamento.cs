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
        public System.Guid ResponsavelId { get; set; }

        public System.Guid ComandaId { get; set; }

        public Guid TipoPagamentoId { get; set; }

        public PagamentoCriteriaCreateBase(Guid responsavelId, Guid comandaId, Guid tipoPagamentoId)
        {
            ResponsavelId = responsavelId;
            ComandaId = comandaId;
            TipoPagamentoId = tipoPagamentoId;
        }
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
        public DateTime DataHoraCancelamento { get; private set; }

        [Display(Name = "Cancelado")]
        public Boolean Cancelado { get; private set; }

        [Display(Name = "Motivo")]
        public string Motivo { get; private set; }

        [Display(Name = "Informação")]
        public string Informacao { get; set; }

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
            return New(new PagamentoCriteriaCreateBase(Guid.Empty, Guid.Empty, Guid.Empty));
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
                return "Pagamento";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = Guid.NewGuid();
            ResponsavelId = ((PagamentoCriteriaCreateBase)criteria).ResponsavelId;
            ComandaId = ((PagamentoCriteriaCreateBase)criteria).ComandaId;
            TipoPagamentoId = ((PagamentoCriteriaCreateBase)criteria).TipoPagamentoId;
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
            lista.Add(CriarParametro(SqlDbType.VarChar, Informacao, "@Informacao"));

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
            this.DataHora = ConvertBase.ToDateTime(reader["DataHora"].ToString());
            this.DataHoraCancelamento = ConvertBase.ToDateTime(reader["DataHoraCancelamento"].ToString());
            this.Cancelado = Convert.ToBoolean(reader["Cancelado"]);
            this.Motivo = reader["Motivo"].ToString();
            this.Informacao = reader["Informacao"].ToString();
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Comanda = Comanda.GetByReader(reader);
            Responsavel = Funcionario.GetByReader(reader);
            TipoPagamento = TipoPagamento.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            Comanda = Comanda.Get(new ComandaCriteriaBase(ComandaId));
            Responsavel = Funcionario.Get(new FuncionarioCriteriaBase(ResponsavelId));
            TipoPagamento = TipoPagamento.Get(new TipoPagamentoCriteriaBase(TipoPagamentoId));
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #region Cancelar Pagamento
        public void CancelarPagamento(string motivo)
        {
            DataHoraCancelamento = DateTime.Now;
            Motivo = motivo;
            Cancelado = true;
            Save();
        }
        #endregion Cancelar Pagamento

        #endregion Custom Methods
    }
}
