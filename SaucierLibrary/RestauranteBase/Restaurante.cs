using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.RestauranteBase
{
    #region Criterias
    public class RestauranteCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public RestauranteCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class RestauranteCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class Restaurante : ObjectModel<Restaurante>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Campo CNPJ é obrigatório.")]
        public string CNPJ { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Restaurante()
        {
        }

        public new static Restaurante Empty()
        {
            return New(new RestauranteCriteriaCreateBase());
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
                return "Restaurante";
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
            lista.Add(CriarParametro(SqlDbType.VarChar, Nome, "@Nome"));
            lista.Add(CriarParametro(SqlDbType.VarChar, CNPJ, "@CNPJ"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((RestauranteCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.CNPJ = reader["CNPJ"].ToString();
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        { }

        protected override void SetChildren()
        { }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        public static Guid GetRestauranteDefaultId()
        {
            Restaurante restaurante = Restaurante.Empty();
            return restaurante.RestauranteDefaultId();
        }

        private Guid RestauranteDefaultId()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string query = @"
SELECT [Id]
  FROM [dbo].[RestauranteTB];";

            ExecuteReaderQuery(query, parameters.ToArray(), "ReturnRestauranteDefaultId");
            return Id;
        }

        public void ReturnRestauranteDefaultId(SqlDataReader reader)
        {
            if (reader.Read())
                Id = ConvertBase.ToGuid(reader["Id"].ToString());
        }

        #endregion Custom Methods
    }
}
