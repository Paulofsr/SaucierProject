using PessoalLibrary.Padroes;
using SaucierLibrary.FuncionarioBase;
using SaucierLibrary.ItemBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.CaixaBase
{
    #region Criterias
    public class ComandaItemCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public ComandaItemCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class ComandaItemCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class ComandaItem : ObjectModel<ComandaItem>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Data/Hora")]
        public DateTime DataHora { get; set; }

        [Display(Name = "Quantidade")]
        public decimal Quantidade { get; set; }

        [Display(Name = "Cancelado")]
        public Boolean Cancelado { get; set; }

        public System.Guid ComandaId { get; set; }

        public System.Guid ItemId { get; set; }

        public System.Guid ItemAdicionalId { get; set; }

        public System.Guid ResponsavelId { get; set; }

        public Comanda Comanda { get; private set; }

        public Item Item { get; private set; }

        public RestauranteItemAdicional ItemAdicional { get; private set; }

        public Funcionario Responsavel { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public ComandaItem()
        {
        }

        public new static ComandaItem Empty()
        {
            return New(new ComandaItemCriteriaCreateBase());
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "ComandaItem";
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
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ItemAdicionalId, "@ItemAdicionalId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ItemId, "@ItemId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ResponsavelId, "@ResponsavelId"));
            lista.Add(CriarParametro(SqlDbType.DateTime, DataHora, "@DataHora"));
            lista.Add(CriarParametro(SqlDbType.Bit, Cancelado, "@Cancelado"));
            lista.Add(CriarParametro(SqlDbType.Decimal, Quantidade, "@Quantidade"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((ComandaItemCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.ComandaId = ConvertBase.ToGuid(reader["ComandaId"].ToString());
            this.ItemAdicionalId = ConvertBase.ToGuid(reader["ItemAdicionalId"].ToString());
            this.ItemId = ConvertBase.ToGuid(reader["ItemId"].ToString());
            this.ResponsavelId = ConvertBase.ToGuid(reader["ResponsavelId"].ToString());
            this.DataHora = Convert.ToDateTime(reader["DataHora"].ToString());
            this.Cancelado = Convert.ToBoolean(reader["Cancelado"]);
            this.Quantidade = Convert.ToDecimal(reader["Quantidade"].ToString());
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Comanda = Comanda.GetByReader(reader);
            Item = Item.GetByReader(reader);
            ItemAdicional = RestauranteItemAdicional.GetByReader(reader);
            Responsavel = Funcionario.GetByReader(reader);
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
