using PessoalLibrary.Padroes;
using SaucierLibrary.ItemBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.CaixaBase
{
    #region Criterias
    public class ConfiguracaoItemCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public ConfiguracaoItemCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class ConfiguracaoItemCriteriaCreateBase : ICriteriaCreateBase
    {
        public List<ConfiguracaoCondicional> ConfiguracaoCondicionalList { get; set; }

        public ConfiguracaoItemCriteriaCreateBase(List<ConfiguracaoCondicional> configuracaoCondicionalList)
        {
            ConfiguracaoCondicionalList = configuracaoCondicionalList;
        }

        public ConfiguracaoItemCriteriaCreateBase(ConfiguracaoCondicional configuracaoCondicional)
        {
            ConfiguracaoCondicionalList = new List<ConfiguracaoCondicional>();
            ConfiguracaoCondicionalList.Add(configuracaoCondicional);
        }
    }
    #endregion Criterias

    public class ConfiguracaoItem : ObjectModel<ConfiguracaoItem>
    {
        #region Properties
        public System.Guid Id { get; set; }

        public System.Guid ConfiguracaoId { get; set; }

        public System.Guid ItemId { get; set; }

        [Display(Name = "Condicional")]
        public Boolean Condicional { get; set; }

        public Configuracao Configuracao { get; private set; }

        public Item Item { get; set; }

        public List<ConfiguracaoCondicional> ConfiguracaoCondicionalList { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public ConfiguracaoItem()
        {
        }

        public new static ConfiguracaoItem Empty()
        {
            return New(new ConfiguracaoItemCriteriaCreateBase(new List<ConfiguracaoCondicional>()));
        }

        protected override void SaveChilds()
        {
            foreach (ConfiguracaoCondicional configuracaoCondicional in ConfiguracaoCondicionalList)
                configuracaoCondicional.Save();
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
                return "ConfiguracaoItem";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = Guid.NewGuid();
            ConfiguracaoCondicionalList = new List<ConfiguracaoCondicional>();
            foreach (ConfiguracaoCondicional configuracaoCondicional in ((ConfiguracaoItemCriteriaCreateBase)criteria).ConfiguracaoCondicionalList)
                AddConfiguracaoCondicional(configuracaoCondicional);
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@Id"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ConfiguracaoId, "@ConfiguracaoId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ItemId, "@ItemId"));
            lista.Add(CriarParametro(SqlDbType.Bit, Condicional, "@Condicional"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((ConfiguracaoItemCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.ConfiguracaoId = ConvertBase.ToGuid(reader["ConfiguracaoId"].ToString());
            this.ItemId = ConvertBase.ToGuid(reader["ItemId"].ToString());
            this.Condicional = Convert.ToBoolean(reader["Condicional"]);
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Configuracao = Configuracao.GetByReader(reader);
            Item = Item.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            Configuracao = Configuracao.Get(new ConfiguracaoCriteriaBase(ConfiguracaoId));
            Item = Item.Get(new ItemCriteriaBase(ItemId));
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        public void AddConfiguracaoCondicional(ConfiguracaoCondicional configuracaoCondicional)
        {
            configuracaoCondicional.ConfiguracaoItemId = Id;
            ConfiguracaoCondicionalList.Add(configuracaoCondicional);
        }

        #endregion Custom Methods
    }
}
