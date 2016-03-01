using PessoalLibrary.Padroes;
using SaucierLibrary.ItemBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.RestauranteBase
{
    #region Criterias
    public class RestauranteItemCriteriaBase : ICriteriaBase
    {
        public Guid RestauranteId { get; set; }

        public Guid ItemId { get; set; }

        public RestauranteItemCriteriaBase(Guid restauranteId, Guid itemId)
        {
            RestauranteId = restauranteId;
            ItemId = itemId;
        }
    }

    public class RestauranteItemCriteriaCreateBase : ICriteriaCreateBase
    {
        public Guid RestauranteId { get; set; }

        public Guid ItemId { get; set; }

        public RestauranteItemCriteriaCreateBase(Guid restauranteId, Guid itemId)
        {
            RestauranteId = restauranteId;
            ItemId = itemId;
        }
    }
    #endregion Criterias

    public class RestauranteItem : ObjectModel<RestauranteItem>
    {
        #region Properties
        public Guid RestauranteId { get; set; }

        public Guid ItemId { get; set; }

        [Display(Name = "Publicar no Cardápio")]
        public bool Cardapio { get; set; }

        public Restaurante Restaurante { get; set; }

        public Item Item { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public RestauranteItem()
        {
        }

        public new static RestauranteItem Empty()
        {
            return New(new RestauranteItemCriteriaCreateBase(Guid.Empty, Guid.Empty));
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "RestauranteItem";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            RestauranteId = ((RestauranteItemCriteriaCreateBase)criteria).RestauranteId;
            ItemId = ((RestauranteItemCriteriaCreateBase)criteria).ItemId;
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, RestauranteId, "@RestauranteId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ItemId, "@ItemId"));
            lista.Add(CriarParametro(SqlDbType.Bit, Cardapio, "@Cardapio"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, RestauranteId, "@RestauranteId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ItemId, "@ItemId"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.RestauranteId = ConvertBase.ToGuid(reader["RestauranteId"].ToString());
            this.ItemId = ConvertBase.ToGuid(reader["ItemId"].ToString());
            this.Cardapio = Convert.ToBoolean(reader["Cardapio"]);
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Restaurante = Restaurante.GetByReader(reader);
            Item = Item.GetByReader(reader);
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
