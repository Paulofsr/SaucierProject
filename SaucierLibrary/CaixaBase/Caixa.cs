using PessoalLibrary.Padroes;
using SaucierLibrary.ClienteBase;
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
        public List<Configuracao> ConfiguracaoList { get; set; }

        public CaixaCriteriaCreateBase(List<Configuracao> configuracaoList)
        {
            ConfiguracaoList = configuracaoList;
        }

        public CaixaCriteriaCreateBase(Configuracao configuracao)
        {
            ConfiguracaoList = new List<Configuracao>();
            ConfiguracaoList.Add(configuracao);
        }
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

        public List<Configuracao> ConfiguracaoList { get; set; }

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
            return New(new CaixaCriteriaCreateBase(new List<Configuracao>()));
        }

        protected override void SaveChilds()
        {
            foreach (Configuracao configuracao in ConfiguracaoList)
                configuracao.Save();
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
                return "Caixa";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = Guid.NewGuid();
            ConfiguracaoList = new List<Configuracao>();
            foreach (Configuracao configuracao in ((CaixaCriteriaCreateBase)criteria).ConfiguracaoList)
                AddConfiguracao(configuracao);
            DataCancelamento = DateTime.MinValue;
            DataFechamento = DateTime.MinValue;
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
            this.DataAbertura = ConvertBase.ToDateTime(reader["DataAbertura"].ToString());
            this.DataFechamento = ConvertBase.ToDateTime(reader["DataFechamento"].ToString());
            this.DataCancelamento = ConvertBase.ToDateTime(reader["DataCancelamento"].ToString());
            this.RestauranteId = ConvertBase.ToGuid(reader["RestauranteId"].ToString());
            this.MotivoCancelamento = reader["MotivoCancelamento"].ToString();
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            ConfiguracaoList = new List<Configuracao>();
            Restaurante = Restaurante.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            ConfiguracaoList = new List<Configuracao>();
            Restaurante = Restaurante.Get(new RestauranteCriteriaBase(RestauranteId));
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods
        private List<Guid> _ids;

        public void AddConfiguracao(Configuracao configuracao)
        {
            configuracao.CaixaId = Id;
            ConfiguracaoList.Add(configuracao);
        }

        #region Abrir Caixa
        public static Caixa AbrirCaixa(Usuario usuario, List<Configuracao> configuracoes)
        {
            return InstanceCaixa(usuario, configuracoes);
        }

        public static Caixa GetCaixaAberto()
        {
            return Get(new CaixaCriteriaBase(GetCaixaAbertoId()));
        }

        private static Caixa InstanceCaixa(Usuario usuario, List<Configuracao> configuracoes)
        {
            Guid id = GetCaixaAbertoId();
            if (id == Guid.Empty)
                return NovoCaixa(usuario, configuracoes);
            return Get(new CaixaCriteriaBase(id));
        }

        private static Caixa NovoCaixa(Usuario usuario, List<Configuracao> configuracoes)
        {
            Caixa caixa = Caixa.New(new CaixaCriteriaCreateBase(configuracoes));
            caixa.DataAbertura = DateTime.Now;
            caixa.RestauranteId = Restaurante.GetRestauranteDefaultId();
            caixa.Save();
            return caixa;
        }
        #endregion Abrir Caixa

        #region Verificar Caixa Aberto
        public static bool ExisteCaixaAberto()
        {
            return GetCaixaAbertoId() != Guid.Empty;
        }

        private static Guid GetCaixaAbertoId()
        {
            Caixa caixa = Empty();
            caixa.GetOpened();
            return caixa.Id;
        }

        private void GetOpened()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string query = @"
SELECT [Id]
FROM [dbo].[CaixaTB]
where [DataCancelamento] is null and [DataFechamento] is null;";

            ExecuteReaderQuery(query, parameters.ToArray(), "ReturnOpened");
        }

        public void ReturnOpened(SqlDataReader reader)
        {
            if (reader.Read())
                Id = ConvertBase.ToGuid(reader["Id"].ToString());
            else
                Id = Guid.Empty;
        }
        #endregion Verificar Caixa Aberto

        #region Fechar Caixa 
        public bool FecharCaixa()
        {
            if(PodeFechar())
            {
                DataFechamento = DateTime.Now;
                Save();
                return true;
            }
            return false;
        }
        #endregion Fechar Caixa 

        #region Verificar Caixa para Fechar
        private List<Comanda> _comandas;
        private bool PodeFechar()
        {
            bool result = true;
            _comandas = BuscarComandas();
            foreach(Comanda comanda in _comandas)
            {
                if (!comanda.Pago)
                    result = false;
            }
            return result;
        }
        
        private List<Comanda> BuscarComandas()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@CaixaId"));
            string query = @"
SELECT [Id]
FROM [dbo].[ComandaTB]
where [CaixaId] = @CaixaId;";

            ExecuteReaderQuery(query, parameters.ToArray(), "ReturnCommandIds");
            _comandas = new List<Comanda>();
            foreach(Guid id in _ids)
            {
                Comanda comanda = Comanda.Get(new ComandaCriteriaBase(id));
                if (!comanda.Vazio)
                    _comandas.Add(comanda);
            }
            return _comandas;
        }

        public void ReturnCommandIds(SqlDataReader reader)
        {
            _ids = new List<Guid>();
            while (reader.Read())
                _ids.Add(ConvertBase.ToGuid(reader["Id"].ToString()));
        }
        #endregion Verificar Caixa para Fechar

        #region Add Comanda
        public bool AddComanda(ref Comanda comanda, string informacao)
        {
            comanda = Comanda.Empty();
            if(Aberto)
            {
                comanda = Comanda.New(new ComandaCriteriaCreateBase(Id));
                comanda.DataAbertura = DateTime.Now;
                comanda.Informacao = informacao;
                VerificarConfiguracoesBefore(ref comanda);
                comanda.Save();
                return true;
            }
            return false;
        }

        public Comanda NewComanda(string informacao)
        {
            Comanda comanda = Comanda.Empty();
            if (AddComanda(ref comanda, informacao))
                return comanda;
            throw new Exception("Caixa está fechado, não pode adicionar comanda.");
        }

        private void VerificarConfiguracoesBefore(ref Comanda comanda)
        {
            BuscarConfiguracoes();
            foreach (Configuracao configuracao in ConfiguracaoList)
            {
                if (configuracao.Consumo)
                    comanda.Consumacao += configuracao.Valor;
                else
                {
                    if (!configuracao.Percentual && !configuracao.Geral)
                    {
                        foreach(ConfiguracaoItem item in configuracao.ConfiguracaoItemList)
                        { }
                    }
                }
            }
        }
        #endregion Add Comanda

        #region Commandas
        public List<Comanda> ToListComanda()
        {
            return Comanda.ToListByCaixa(Id);
        }

        public List<Comanda> ToListComandaAtivas()
        {
            return Comanda.ToListActivedByCaixa(Id);
        }
        #endregion Comandas

        #region Configurações

        private List<Configuracao> BuscarConfiguracoes()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@CaixaId"));
            string query = @"
SELECT [Id]
FROM [dbo].[ConfiguracaoTB]
where [CaixaId] = @CaixaId;";

            ExecuteReaderQuery(query, parameters.ToArray(), "ReturnConfigIds");
            ConfiguracaoList = new List<Configuracao>();
            foreach (Guid id in _ids)
            {
                Configuracao configuracao = Configuracao.Get(new ConfiguracaoCriteriaBase(id));
                if (!configuracao.Vazio)
                    ConfiguracaoList.Add(configuracao);
            }
            return ConfiguracaoList;
        }

        public void ReturnConfigIds(SqlDataReader reader)
        {
            _ids = new List<Guid>();
            while (reader.Read())
                _ids.Add(ConvertBase.ToGuid(reader["Id"].ToString()));
        }
        #endregion Configurações
        
        #region Verify Caixa
        public bool Aberto
        {
            get
            {
                return DataFechamento == DateTime.MinValue && DataCancelamento == DateTime.MinValue;
            }
        }
        #endregion Verify Caixa

        #region Refresh
        public void Refresh()
        {
            List<Comanda> comandas = ToListComanda();
            foreach(Comanda comanda in comandas)
            {
                comanda.Save();
            }
        }
        #endregion Refresh

        #endregion Custom Methods
    }
}
