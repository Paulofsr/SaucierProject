using PessoalLibrary.Padroes;
using SaucierLibrary.ItemBase;
using SaucierLibrary.PagamentoBase;
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
        public Guid CaixaId { get; set; }

        public ComandaCriteriaCreateBase(Guid caixaId)
        {
            CaixaId = caixaId;
        }
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

        public decimal Consumacao { get; set; }

        public System.Guid CaixaId { get; set; }

        public Caixa Caixa { get; private set; }

        [Display(Name ="Custos")]
        public decimal TotalCusto
        {
            get
            {
                return GetTotalCusto();
            }
        }

        [Display(Name = "Pago")]
        public decimal TotalPago
        {
            get
            {
                return GetTotalPagamento();
            }
        }

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
            return New(new ComandaCriteriaCreateBase(Guid.Empty));
        }

        protected override void SaveChilds()
        {
        }

        protected override void BeforeSave()
        {
            Refresh();
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
            CaixaId = ((ComandaCriteriaCreateBase)criteria).CaixaId;
            Consumacao = 0;
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
            lista.Add(CriarParametro(SqlDbType.Real, Consumacao, "@Consumacao"));

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
            this.DataAbertura = ConvertBase.ToDateTime(reader["DataAbertura"].ToString());
            this.Informacao = reader["Informacao"].ToString();
            this.Pago = Convert.ToBoolean(reader["Pago"]);
            this.CaixaId = ConvertBase.ToGuid(reader["CaixaId"].ToString());
            this.Consumacao = Convert.ToDecimal(reader["Consumacao"].ToString());
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Caixa = Caixa.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            Caixa = Caixa.Get(new CaixaCriteriaBase(CaixaId));
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #region Update Status
        private void Refresh()
        {
            Pago = GetTotalCusto() == GetTotalPagamento();
        }
        #endregion Update Status

        #region Add Item
        public bool AddItem(Guid itemId, Guid usuarioId, decimal quantidade)
        {
            if (quantidade == 0)
                return false;
            ComandaItem comandaItem = ComandaItem.New(new ComandaItemCriteriaCreateBase(usuarioId, Id));
            comandaItem.DataHora = DateTime.Now;
            comandaItem.Quantidade = quantidade;
            Item item = Item.Get(new ItemCriteriaBase(itemId));
            if(!item.Vazio)
            {
                comandaItem.ItemId = itemId;
                comandaItem.Save();
                return true;
            }
            RestauranteItemAdicional itemAdicional = RestauranteItemAdicional.Get(new RestauranteItemAdicionalCriteriaBase(itemId));
            if (!item.Vazio)
            {
                comandaItem.ItemAdicionalId = itemId;
                comandaItem.Save();
                return true;
            }
            return false;
        }

        public bool AddItemAdicional(Guid usuarioId, decimal quantidade, decimal valor, string nome)
        {
            RestauranteItemAdicional itemAdicional = RestauranteItemAdicional.New(new RestauranteItemAdicionalCriteriaCreateBase());
            itemAdicional.Custo = valor;
            itemAdicional.DataCriacao = DateTime.Now;
            itemAdicional.Descricao = nome;
            itemAdicional.Nome = nome;
            itemAdicional.Preco = valor;
            itemAdicional.RestauranteId = Caixa.RestauranteId;
            itemAdicional.Save();
            return AddItem(itemAdicional.Id, usuarioId, quantidade);
        }
        #endregion Add Item

        #region Add Pagamento
        public void AddPagamento(Guid usuarioId, Guid tipoPagamentoId, decimal valorPago, decimal valorRecebido, decimal troco)
        {
            Pagamento pagamento = Pagamento.New(new PagamentoCriteriaCreateBase(usuarioId, Id, tipoPagamentoId));
            pagamento.DataHora = DateTime.Now;
            pagamento.ValorPago = valorPago;
            pagamento.ValorRecebido = valorRecebido;
            pagamento.Troco = troco;
            pagamento.Save();
        }
        #endregion

        #region Itens
        private List<ComandaItem> _itens;
        private List<Guid> _idsItens;
        private List<ComandaItem> BuscarItens()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@ComandaId"));
            string query = @"
SELECT [Id]
FROM [dbo].[ComandaItemTB]
where [ComandaId] = @ComandaId;";

            ExecuteReaderQuery(query, parameters.ToArray(), "ReturnItemIds");
            _itens = new List<ComandaItem>();
            foreach (Guid id in _idsItens)
            {
                ComandaItem item = ComandaItem.Get(new ComandaItemCriteriaBase(id));
                if (!item.Vazio)
                    _itens.Add(item);
            }
            return _itens;
        }

        public void ReturnItemIds(SqlDataReader reader)
        {
            _idsItens = new List<Guid>();
            while (reader.Read())
                _idsItens.Add(ConvertBase.ToGuid(reader["Id"].ToString()));
        }

        public decimal GetTotalItem()
        {
            BuscarItens();
            decimal total = 0;
            foreach (ComandaItem item in _itens)
                if (!item.Cancelado)
                    total += item.ValorTotal();
            return total;
        }

        [Display(Name = "Custo")]
        public decimal GetTotalCusto()
        {
            decimal total = GetTotalItem();
            if (Consumacao <= total)
                return total;
            return Consumacao;
        }
        #endregion Itens

        #region Pagamentos
        private List<Pagamento> _pagamentos;
        private List<Guid> _idsPagamentos;
        private List<Pagamento> BuscarPagamentos()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@ComandaId"));
            string query = @"
SELECT [Id]
FROM [dbo].[PagamentoTB]
where [ComandaId] = @ComandaId;";

            ExecuteReaderQuery(query, parameters.ToArray(), "ReturnPaymentIds");
            _pagamentos = new List<Pagamento>();
            foreach (Guid id in _idsPagamentos)
            {
                Pagamento pagamento = Pagamento.Get(new PagamentoCriteriaBase(id));
                if (!pagamento.Vazio)
                    _pagamentos.Add(pagamento);
            }
            return _pagamentos;
        }

        public void ReturnPaymentIds(SqlDataReader reader)
        {
            _idsPagamentos = new List<Guid>();
            while (reader.Read())
                _idsPagamentos.Add(ConvertBase.ToGuid(reader["Id"].ToString()));
        }

        [Display(Name = "Pago")]
        public decimal GetTotalPagamento()
        {
            BuscarPagamentos();
            decimal total = 0;
            foreach (Pagamento pagamento in _pagamentos)
                if (!pagamento.Cancelado)
                    total += pagamento.ValorPago;
            return total;
        }
        #endregion Pagamentos

        #region ToList By Caixa
        public static List<Comanda> ToListByCaixa(Guid caixaId)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, caixaId, "@CaixaId"));
            return ToList("ByCaixa", lista);
        }

        public static List<Comanda> ToListActivedByCaixa(Guid caixaId)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, caixaId, "@CaixaId"));
            return ToList("ActivedByCaixa", lista);
        }
        #endregion ToList By Caixa

        #region Itens
        public List<ComandaItem> ToListComandaItem()
        {
            return ComandaItem.ToListGroupByComanda(Id);
        }

        public List<ComandaItem> ToListComandaItemAtivas()
        {
            return ComandaItem.ToListGroupActivedByComanda(Id);
        }
        #endregion Comandas

        #endregion Custom Methods
    }
}
