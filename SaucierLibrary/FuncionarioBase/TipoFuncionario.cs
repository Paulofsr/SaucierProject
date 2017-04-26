using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.FuncionarioBase
{
    #region Criterias
    public class TipoFuncionarioCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public TipoFuncionarioCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class TipoFuncionarioCriteriaCreateBase : ICriteriaCreateBase
    {

    }
    #endregion Criterias

    public class TipoFuncionario : ObjectModel<TipoFuncionario>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo Tipo é obrigatório.")]
        public string Tipo { get; set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public TipoFuncionario()
        {
        }

        public new static TipoFuncionario Empty()
        {
            return New(new TipoFuncionarioCriteriaCreateBase());
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
                return "TipoFuncionario";
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

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((TipoFuncionarioCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Tipo = reader["Tipo"].ToString();
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        { }

        protected override void SetChildren()
        { }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
