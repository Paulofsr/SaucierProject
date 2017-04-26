using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.ItemBase
{
    #region Criterias
    public class ItemComandaInfoCriteriaBase : ICriteriaBaseInfo
    {
        public Guid Id { get; set; }

        public ItemComandaInfoCriteriaBase(Guid id)
        {
            Id = id;
        }
    }
    #endregion Criterias

    public class ItemComandaInfo : ObjectModelInfo<ItemComandaInfo>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Custo")]
        public decimal Custo { get; set; }

        [Display(Name = "Preço")]
        [Required(ErrorMessage = "Campo Preço é obrigatório.")]
        public decimal Preco { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Selecione um tipo para esse item.")]
        public System.Guid TipoItemId { get; set; }

        [Display(Name = "Data/Hora")]
        public DateTime DataHora { get; set; }

        [Display(Name = "Quantidade")]
        public decimal Quantidade { get; set; }

        [Display(Name = "Cancelado")]
        public Boolean Cancelado { get; private set; }

        public System.Guid ComandaId { get; private set; }

        public TipoItem TipoItem { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public ItemComandaInfo()
        {
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string ViewName
        {
            get
            {
                return "ItemComandaVW";
            }
        }
        #endregion ClassName

        #region Parameters
        //protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        //{
        //    List<SqlParameter> lista = new List<SqlParameter>();
        //    lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((ItemCriteriaBase)criteria).Id, "@Id"));
        //    return lista.ToArray();
        //}

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.Descricao = reader["Descricao"].ToString();
            this.Custo = Convert.ToDecimal(reader["Custo"].ToString());
            this.Preco = Convert.ToDecimal(reader["Preco"].ToString());
            this.TipoItemId = ConvertBase.ToGuid(reader["TipoItemId"].ToString());
            this.ComandaId = ConvertBase.ToGuid(reader["ComandaId"].ToString());;
            this.DataHora = ConvertBase.ToDateTime(reader["DataHora"].ToString());
            this.Cancelado = Convert.ToBoolean(reader["Cancelado"]);
            this.Quantidade = Convert.ToDecimal(reader["Quantidade"].ToString());
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            TipoItem = TipoItem.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            TipoItem = TipoItem.Get(new TipoItemCriteriaBase(TipoItemId));
        }

        protected override SqlParameter[] GetFilterParameters(ICriteriaBaseInfo criteria)
        {
            throw new NotImplementedException();
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods
        #region ToList Actived
        public static List<ItemComandaInfo> ToListByComanda(Guid comandaId)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, comandaId, "@ComandaId"));
            return ToList("ByComanda", lista);
        }

        public static List<ItemComandaInfo> ToListActivedByComanda(Guid comandaId)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, comandaId, "@ComandaId"));
            return ToList("ActivedByComanda", lista);
        }
        #endregion ToList Actived

        #endregion Custom Methods
    }
}
