using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.ItemBase
{
    #region Criterias
    public class TipoItemCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public TipoItemCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class TipoItemCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class TipoItem : ObjectModel<TipoItem>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo Tipo é obrigatório.")]
        public string Tipo { get; set; }

        public System.Guid ParentId { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Basica; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public TipoItem()
        {
        }

        public new static TipoItem Empty()
        {
            return New(new TipoItemCriteriaCreateBase());
        }
        #endregion Constructors

        #region Data Methods

        #region ClassName
        protected override string TableName
        {
            get
            {
                return "TipoItem";
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
            lista.Add(CriarParametro(SqlDbType.VarChar, Tipo, "@Tipo"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ParentId, "@ParentId"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((TipoItemCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Tipo = reader["Tipo"].ToString();
            this.ParentId = ConvertBase.ToGuid(reader["ParentId"].ToString());
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        { }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
