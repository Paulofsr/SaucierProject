using PessoalLibrary.Padroes;
using SaucierLibrary.RestauranteBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.ItemBase
{
    #region Criterias
    public class RestauranteItemAdicionalCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public RestauranteItemAdicionalCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class RestauranteItemAdicionalCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class RestauranteItemAdicional : ObjectModel<RestauranteItemAdicional>
    {
        #region Properties
        public System.Guid Id { get; set; }

        public Guid RestauranteId { get; set; }

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

        public DateTime DataCriacao { get; set; }

        public Restaurante Restaurante { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public RestauranteItemAdicional()
        {
        }

        public new static RestauranteItemAdicional Empty()
        {
            return New(new RestauranteItemAdicionalCriteriaCreateBase());
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "RestauranteItemAdicional";
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
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, RestauranteId, "@RestauranteId"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Nome, "@Nome"));
            lista.Add(CriarParametro(SqlDbType.Real, Custo, "@Custo"));
            lista.Add(CriarParametro(SqlDbType.Real, Preco, "@Preco"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Descricao, "@Descricao"));
            lista.Add(CriarParametro(SqlDbType.DateTime, DataCriacao, "@DataCriacao"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((RestauranteItemAdicionalCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.Descricao = reader["Descricao"].ToString();
            this.Custo = Convert.ToDecimal(reader["Custo"].ToString());
            this.Preco = Convert.ToDecimal(reader["Preco"].ToString());
            this.RestauranteId = ConvertBase.ToGuid(reader["RestauranteId"].ToString());
            this.DataCriacao = Convert.ToDateTime(reader["DataCriacao"].ToString());
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            Restaurante = Restaurante.GetByReader(reader);
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
