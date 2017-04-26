using PessoalLibrary.Padroes;
using SaucierLibrary.ItemBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.ItemBase
{
    #region Criterias
    public class ItemComponenteItemCriteriaBase : ICriteriaBase
    {
        public Guid ItemId { get; set; }

        public Guid ComponenteId { get; set; }

        public ItemComponenteItemCriteriaBase(Guid componenteId, Guid itemId)
        {
            ComponenteId = componenteId;
            ItemId = itemId;
        }
    }

    public class ItemComponenteItemCriteriaCreateBase : ICriteriaCreateBase
    {
        public Guid ItemId { get; set; }

        public Guid ComponenteId { get; set; }

        public ItemComponenteItemCriteriaCreateBase(Guid componenteId, Guid itemId)
        {
            ComponenteId = componenteId;
            ItemId = itemId;
        }
    }
    #endregion Criterias

    public class ItemComponenteItem : ObjectModel<ItemComponenteItem>
    {
        #region Properties
        public Guid ItemId { get; set; }

        public Guid ComponenteId { get; set; }

        public Item Item { get; set; }

        public Item ItemComponente { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public ItemComponenteItem()
        {
        }

        public new static ItemComponenteItem Empty()
        {
            return New(new ItemComponenteItemCriteriaCreateBase(Guid.Empty, Guid.Empty));
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
                return "ItemComponente";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            ComponenteId = ((ItemComponenteItemCriteriaCreateBase)criteria).ComponenteId;
            ItemId = ((ItemComponenteItemCriteriaCreateBase)criteria).ItemId;
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ComponenteId, "@ComponenteId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ItemId, "@ItemId"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ComponenteId, "@ComponenteId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ItemId, "@ItemId"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.ComponenteId = ConvertBase.ToGuid(reader["ComponenteId"].ToString());
            this.ItemId = ConvertBase.ToGuid(reader["ItemId"].ToString());
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Item = Item.GetByReader(reader);
            ItemComponente = Item.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            Item = Item.Get(new ItemCriteriaBase(ItemId));
            ItemComponente = Item.Get(new ItemCriteriaBase(ComponenteId));
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
