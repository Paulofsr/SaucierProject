using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.ItemBase
{
    #region Criterias
    public class ItemCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public ItemCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class ItemCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class Item : ObjectModel<Item>
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

        public TipoItem TipoItem { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Item()
        {
        }

        public new static Item Empty()
        {
            return New(new ItemCriteriaCreateBase());
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
                return "Item";
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
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, TipoItemId, "@TipoItemId"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Nome, "@Nome"));
            lista.Add(CriarParametro(SqlDbType.Real, Custo, "@Custo"));
            lista.Add(CriarParametro(SqlDbType.Real, Preco, "@Preco"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Descricao, "@Descricao"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((ItemCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.Descricao = reader["Descricao"].ToString();
            this.Custo = Convert.ToDecimal(reader["Custo"].ToString());
            this.Preco = Convert.ToDecimal(reader["Preco"].ToString());
            this.TipoItemId = ConvertBase.ToGuid(reader["TipoItemId"].ToString());
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            TipoItem = TipoItem.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            TipoItem = TipoItem.Get(new TipoItemCriteriaBase(TipoItemId));
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods


        #region ToList By Filter
        public static List<Item> ToListByFilter(string filter)
        {
            List<Item> result = new List<Item>();
            if (filter.Length > 0)
            {
                List<SqlParameter> lista = new List<SqlParameter>();
                lista.Add(CriarParametro(SqlDbType.VarChar, filter + "%", "@filter"));
                result.AddRange(ToList("ByFilter", lista));
                lista = new List<SqlParameter>();
                lista.Add(CriarParametro(SqlDbType.VarChar, "%" + filter, "@filter"));
                result.AddRange(ToList("ByFilter", lista));
            }
            return result;
        }
        #endregion ToList By Filter
        #endregion Custom Methods
    }
}
