using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.CaixaBase
{
    #region Criterias
    public class ConfiguracaoCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public ConfiguracaoCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public enum TipoConfiguracao
    {
        Consumo,
        DescontoGeral,
        DescontoItem,
        TaxaGeral,
        TaxaItem,
        TaxaDescontoGeral,
        TaxaDescontoItem
    }

    public class ConfiguracaoCriteriaCreateBase : ICriteriaCreateBase
    {
        public List<ConfiguracaoItem> ConfiguracaoItemList { get; set; }

        public TipoConfiguracao TipoConfiguracao { get; set; }

        public ConfiguracaoCriteriaCreateBase(List<ConfiguracaoItem> configuracaoItemList, TipoConfiguracao tipoConfiguracao)
        {
            ConfiguracaoItemList = configuracaoItemList;
            TipoConfiguracao = tipoConfiguracao;
        }

        public ConfiguracaoCriteriaCreateBase(ConfiguracaoItem configuracaoItem, TipoConfiguracao tipoConfiguracao)
        {
            ConfiguracaoItemList = new List<ConfiguracaoItem>();
            ConfiguracaoItemList.Add(configuracaoItem);
            TipoConfiguracao = tipoConfiguracao;
        }
    }
    #endregion Criterias

    public class Configuracao : ObjectModel<Configuracao>
    {
        #region Properties
        public System.Guid Id { get; set; }

        public System.Guid CaixaId { get; set; }

        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [Display(Name = "Percentual")]
        public Boolean Percentual { get; set; }

        [Display(Name = "Desconto")]
        public Boolean Desconto { get; set; }

        [Display(Name = "Geral")]
        public Boolean Geral { get; set; }

        [Display(Name = "Consumo")]
        public Boolean Consumo { get; set; }

        public Caixa Caixa { get; private set; }

        public List<ConfiguracaoItem> ConfiguracaoItemList { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Configuracao()
        {
        }

        public new static Configuracao Empty()
        {
            return New(new ConfiguracaoCriteriaCreateBase(new List<ConfiguracaoItem>(), TipoConfiguracao.Consumo));
        }

        protected override void SaveChilds()
        {
            foreach (ConfiguracaoItem configuracaoItem in ConfiguracaoItemList)
                configuracaoItem.Save();
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
                return "Configuracao";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = Guid.NewGuid();
            ConfiguracaoItemList = new List<ConfiguracaoItem>();
            foreach (ConfiguracaoItem configuracaoItem in ((ConfiguracaoCriteriaCreateBase)criteria).ConfiguracaoItemList)
                AddConfiguracaoItem(configuracaoItem);
            TipoConfiguracao tipoConfiguracao = ((ConfiguracaoCriteriaCreateBase)criteria).TipoConfiguracao;
            switch(tipoConfiguracao)
            {
                case TipoConfiguracao.Consumo:
                    Consumo = true;
                    Percentual = false;
                    Geral = false;
                    Desconto = false;
                    break;
                case TipoConfiguracao.DescontoGeral:
                    Consumo = false;
                    Percentual = false;
                    Geral = true;
                    Desconto = true;
                    break;
                case TipoConfiguracao.DescontoItem:
                    Consumo = false;
                    Percentual = false;
                    Geral = false;
                    Desconto = true;
                    break;
                case TipoConfiguracao.TaxaDescontoGeral:
                    Consumo = false;
                    Percentual = true;
                    Geral = true;
                    Desconto = true;
                    break;
                case TipoConfiguracao.TaxaDescontoItem:
                    Consumo = false;
                    Percentual = true;
                    Geral = false;
                    Desconto = true;
                    break;
                case TipoConfiguracao.TaxaGeral:
                    Consumo = false;
                    Percentual = true;
                    Geral = true;
                    Desconto = false;
                    break;
                case TipoConfiguracao.TaxaItem:
                    Consumo = false;
                    Percentual = true;
                    Geral = false;
                    Desconto = false;
                    break;
            }
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@Id"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, CaixaId, "@CaixaId"));
            lista.Add(CriarParametro(SqlDbType.Real, Valor, "@Valor"));
            lista.Add(CriarParametro(SqlDbType.Bit, Percentual, "@Percentual"));
            lista.Add(CriarParametro(SqlDbType.Bit, Desconto, "@Desconto"));
            lista.Add(CriarParametro(SqlDbType.Bit, Geral, "@Geral"));
            lista.Add(CriarParametro(SqlDbType.Bit, Consumo, "@Consumo"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((ConfiguracaoCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.CaixaId = ConvertBase.ToGuid(reader["CaixaId"].ToString());
            this.Valor = Convert.ToDecimal(reader["Valor"].ToString());
            this.Percentual = Convert.ToBoolean(reader["Percentual"]);
            this.Desconto = Convert.ToBoolean(reader["Desconto"]);
            this.Geral = Convert.ToBoolean(reader["Geral"]);
            this.Consumo = Convert.ToBoolean(reader["Consumo"]);
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

        public void AddConfiguracaoItem(ConfiguracaoItem configuracaoItem)
        {
            configuracaoItem.ConfiguracaoId = Id;
            ConfiguracaoItemList.Add(configuracaoItem);
        }

        #region Configurações
        private List<Guid> _ids = new List<Guid>();
        private List<ConfiguracaoItem> BuscarConfiguracaoItem()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@ConfiguracaoId"));
            string query = @"
SELECT [Id]
FROM [dbo].[ConfiguracaoItemTB]
where [ConfiguracaoId] = @ConfiguracaoId;";

            ExecuteReaderQuery(query, parameters.ToArray(), "ReturnConfigItemIds");
            ConfiguracaoItemList = new List<ConfiguracaoItem>();
            foreach (Guid id in _ids)
            {
                ConfiguracaoItem configuracaoItem = ConfiguracaoItem.Get(new ConfiguracaoItemCriteriaBase(id));
                if (!configuracaoItem.Vazio)
                    ConfiguracaoItemList.Add(configuracaoItem);
            }
            return ConfiguracaoItemList;
        }

        public void ReturnConfigItemIds(SqlDataReader reader)
        {
            _ids = new List<Guid>();
            while (reader.Read())
                _ids.Add(ConvertBase.ToGuid(reader["Id"].ToString()));
        }
        #endregion Configurações

        #endregion Custom Methods
    }
}
