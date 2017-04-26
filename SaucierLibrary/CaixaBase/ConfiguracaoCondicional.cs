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
    public class ConfiguracaoCondicionalCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public ConfiguracaoCondicionalCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class ConfiguracaoCondicionalCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class ConfiguracaoCondicional : ObjectModel<ConfiguracaoCondicional>
    {
        #region Properties
        public System.Guid Id { get; set; }

        public System.Guid ConfiguracaoItemId { get; set; }

        public System.Guid ItemId { get; set; }

        [Display(Name = "Recusivo")]
        public Boolean Recusivo { get; set; }

        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        public ConfiguracaoItem ConfiguracaoItem { get; private set; }

        public Item Item { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public ConfiguracaoCondicional()
        {
        }

        public new static ConfiguracaoCondicional Empty()
        {
            return New(new ConfiguracaoCondicionalCriteriaCreateBase());
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
                return "ConfiguracaoCondicional";
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
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ConfiguracaoItemId, "@ConfiguracaoItemId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ItemId, "@ItemId"));
            lista.Add(CriarParametro(SqlDbType.Int, Quantidade, "@Quantidade"));
            lista.Add(CriarParametro(SqlDbType.Bit, Recusivo, "@Recusivo"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((ConfiguracaoCondicionalCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.ConfiguracaoItemId = ConvertBase.ToGuid(reader["ConfiguracaoItemId"].ToString());
            this.ItemId = ConvertBase.ToGuid(reader["ItemId"].ToString());
            this.Quantidade = Convert.ToInt32(reader["Quantidade"]);
            this.Recusivo = Convert.ToBoolean(reader["Recusivo"]);
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            ConfiguracaoItem = ConfiguracaoItem.GetByReader(reader);
            Item = Item.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            ConfiguracaoItem = ConfiguracaoItem.Get(new ConfiguracaoItemCriteriaBase(ConfiguracaoItemId));
            Item = Item.Get(new ItemCriteriaBase(ItemId));
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
