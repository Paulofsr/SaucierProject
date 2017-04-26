using PessoalLibrary.Padroes;
using SaucierLibrary.ClienteBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace SaucierLibrary.FuncionarioBase
{
    #region Criterias
    public class FuncionarioCriteriaBase : ICriteriaBase
    {
        public Guid Id { get; set; }

        public FuncionarioCriteriaBase(Guid id)
        {
            Id = id;
        }
    }

    public class FuncionarioCriteriaCreateBase : ICriteriaCreateBase
    {
        public Guid UsuarioId { get; set; }

        public FuncionarioCriteriaCreateBase(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }
    }
    #endregion Criterias

    public class Funcionario : ObjectModel<Funcionario>
    {
        #region Properties
        public System.Guid Id { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Display(Name = "RG")]
        public string RG { get; set; }

        public System.Guid TipoId { get; set; }

        public System.Guid UsuarioId { get; set; }

        public bool Ativo { get; set; }

        public TipoFuncionario TipoFuncionario { get; private set; }

        public Usuario Usuario { get; private set; }

        #region Overrides Properties
        protected override TiboBase BaseSelected { get { return TiboBase.Pessoal; } }
        #endregion Overrides Properties
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Call statics methods: New, Get, Delete
        /// </summary>
        public Funcionario()
        {
        }

        public new static Funcionario Empty()
        {
            return New(new FuncionarioCriteriaCreateBase(Guid.Empty));
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
                return "Funcionario";
            }
        }
        #endregion ClassName

        #region Create
        protected override void Create(ICriteriaCreateBase criteria)
        {
            Id = ((FuncionarioCriteriaCreateBase)criteria).UsuarioId;
            UsuarioId = ((FuncionarioCriteriaCreateBase)criteria).UsuarioId;
        }
        #endregion Create

        #region Parameters
        protected override SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, Id, "@Id"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, TipoId, "@TipoId"));
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, UsuarioId, "@UsuarioId"));
            lista.Add(CriarParametro(SqlDbType.VarChar, Nome, "@Nome"));
            lista.Add(CriarParametro(SqlDbType.VarChar, CPF, "@CPF"));
            lista.Add(CriarParametro(SqlDbType.VarChar, RG, "@RG"));
            lista.Add(CriarParametro(SqlDbType.Bit, Ativo, "@Ativo"));

            return lista.ToArray();
        }

        protected override SqlParameter[] GetIdParametros(ICriteriaBase criteria)
        {
            List<SqlParameter> lista = new List<SqlParameter>();
            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, ((FuncionarioCriteriaBase)criteria).Id, "@Id"));
            return lista.ToArray();
        }

        protected override void SetParameters(SqlDataReader reader)
        {
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.TipoId = ConvertBase.ToGuid(reader["TipoId"].ToString());
            this.UsuarioId = ConvertBase.ToGuid(reader["UsuarioId"].ToString());
            this.Ativo = Convert.ToBoolean(reader["Ativo"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.CPF = reader["CPF"].ToString();
            this.RG = reader["RG"].ToString();
        }

        protected override void SetParentAndChildren(SqlDataReader reader)
        {
            TipoFuncionario = TipoFuncionario.GetByReader(reader);
            Usuario = Usuario.GetByReader(reader);
        }

        protected override void SetChildren()
        {
            TipoFuncionario = TipoFuncionario.Get(new TipoFuncionarioCriteriaBase(TipoId));
            Usuario = Usuario.Get(new UsuarioCriteriaBase(UsuarioId));
        }
        #endregion Parameters

        #endregion Data Methods

        #region Custom Methods

        #endregion Custom Methods
    }
}
