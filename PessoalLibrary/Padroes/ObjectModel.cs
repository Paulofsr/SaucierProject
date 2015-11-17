using PessoalLibrary.Configuracoes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PessoalLibrary.Padroes
{
    public enum TiboBase
    {
        Basica,
        Pessoal
    }

    #region Criteria
    public interface ICriteriaBase
    {
    }
    public interface ICriteriaCreateBase
    {
    }
    #endregion Criteria

    public abstract class ObjectModel<T> where T : ObjectModel<T>
    {
        #region Property
        public bool Novo { get; protected set; }
        public bool Vazio { get; protected set; }

        protected abstract TiboBase BaseSelected { get; }

        private string ConnectionString
        {
            get
            {
                if (BaseSelected == TiboBase.Basica)
                    return Configuracao.ConnectionStringBase;
                return Configuracao.ConnectionStringCliente;
            }
        }
        #endregion Property

        #region Constructors
        protected ObjectModel()
        {
            Novo = true;
            Vazio = true;
        }
        #endregion Constructors

        #region Control Methods
        /// <summary>
        /// ATTENTION: This class should be renewed
        /// </summary>
        public static T Empty()
        {
            throw new NotImplementedException();
        }

        public static T New(ICriteriaCreateBase criteria)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.Create(criteria);
            return obj;
        }

        public static T Get(ICriteriaBase criteria)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.SelectData(criteria);
            return obj;
        }

        public static void Delete(ICriteriaBase criteria)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.DeleteData(criteria);
        }

        protected static T GetByReader(SqlDataReader reader)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.SetDefaultParameters(reader);
            return obj;
        }

        public void Save()
        {
            if (this.Novo)
                InsertData();
            else
                UpdateData();
        }
        #endregion Control Methods

        #region Data Methods

        #region TableName
        protected virtual string TableName { get; }
        #endregion TableName

        #region Parameters
        protected abstract SqlParameter[] GetParametros();
        protected abstract SqlParameter[] GetIdParametros(ICriteriaBase criteria);
        protected abstract void SetParameters(SqlDataReader reader);
        protected abstract void SetParentAndChildren(SqlDataReader reader);

        protected static SqlParameter CriarParametro(SqlDbType dbType, Object data, string nome, ParameterDirection direcao = ParameterDirection.Input)
        {
            return new SqlParameter()
            {
                SqlDbType = dbType,
                Direction = direcao,
                Value = data ?? DBNull.Value,
                ParameterName = nome
            };
        }

        protected static SqlParameter CriarParametro(SqlDbType dbType, Object data, string nome)
        {
            return new SqlParameter()
            {
                SqlDbType = dbType,
                Direction = ParameterDirection.Input,
                Value = data ?? DBNull.Value,
                ParameterName = nome
            };
        }
        #endregion Parameters

        #region Create
        protected abstract void Create(ICriteriaCreateBase create);
        #endregion Create

        #region Select
        private void SelectData(ICriteriaBase criteria)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Get" + TableName;
                    cm.Parameters.AddRange(GetIdParametros(criteria));

                    cn.Open();

                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        SetDefaultParameters(reader);
                        if (reader.NextResult())
                            SetParentAndChildren(reader);
                    }
                    
                }
            }
        }

        private void SetDefaultParameters(SqlDataReader reader)
        {
            Novo = false;
            if (!reader.Read())
            {
                Vazio = true;
                return;
            }
            Vazio = false;
            SetParameters(reader);
        }
        #endregion Select

        #region Insert
        private void InsertData()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Add" + TableName;
                    cm.Parameters.AddRange(GetParametros());

                    cn.Open();

                    cm.ExecuteNonQuery();
                }
            }
            ConfirmInsert();
        }

        private void ConfirmInsert()
        {
            Novo = false;
            Vazio = false;
        }
        #endregion Insert

        #region Update
        private void UpdateData()
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Update" + TableName;
                    cm.Parameters.AddRange(GetParametros());

                    cn.Open();

                    cm.ExecuteNonQuery();
                }
            }
        }
        #endregion Update

        #region Delete
        private void DeleteData(ICriteriaBase criteria)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Delete" + TableName;
                    cm.Parameters.AddRange(GetIdParametros(criteria));

                    cn.Open();

                    cm.ExecuteNonQuery();
                }
            }
        }
        #endregion Delete

        #region Executes
        /// <summary>
        /// Executa a procedure retornando a lista de resposta Reader. A resposta irá retornar ao método informado pelo parâmentro.
        /// CUIDADO: A execução estará com a conexão aberta
        /// PADRÃO: O método deverá ter uma variável do tipo SqlDataReader
        /// </summary>
        /// <param name="procedure">Procedure que irá executar na base</param>
        /// <param name="parameters">Parâmetros necessários para executar a procedure</param>
        /// <param name="methodNameInvoke">Método que será invocado após a execução da Procedure repassando a resposta Reader do resultado</param>
        protected void ExecuteReaderProcedure(string procedure, SqlParameter[] parameters, string methodNameInvoke)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = procedure;
                    cm.Parameters.AddRange(parameters);

                    cn.Open();
                    Invoke(methodNameInvoke, new List<SqlDataReader>(1) { cm.ExecuteReader() }.ToArray());
                }
            }
        }

        protected void ExecuteProcedure(string procedure, SqlParameter[] parameters)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = procedure;
                    cm.Parameters.AddRange(parameters);

                    cn.Open();

                    cm.ExecuteNonQuery();
                }
            }
        }

        protected int ExecuteQuery(string query)
        {
            return ExecuteQuery(query, new List<SqlParameter>().ToArray());
        }

        protected int ExecuteQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandText = query;
                    cm.Parameters.AddRange(parameters);

                    cn.Open();

                    return cm.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executa a Query retornando a lista de resposta Reader. A resposta irá retornar ao método informado pelo parâmentro.
        /// CUIDADO: A execução estará com a conexão aberta
        /// PADRÃO: O método deverá ter uma variável do tipo SqlDataReader
        /// </summary>
        /// <param name="query">Query que irá executar na base</param>
        /// <param name="methodNameInvoke">Método que será invocado após a execução da Query repassando a resposta Reader do resultado</param>
        protected void ExecuteReaderQuery(string query, string methodNameInvoke)
        {
            ExecuteReaderQuery(query, new List<SqlParameter>().ToArray(), methodNameInvoke);
        }

        /// <summary>
        /// Executa a Query retornando a lista de resposta Reader. A resposta irá retornar ao método informado pelo parâmentro.
        /// CUIDADO: A execução estará com a conexão aberta
        /// PADRÃO: O método deverá ter uma variável do tipo SqlDataReader
        /// </summary>
        /// <param name="query">Query que irá executar na base</param>
        /// <param name="parameters">Parâmetros necessários para executar a query</param>
        /// <param name="methodNameInvoke">Método que será invocado após a execução da Query repassando a resposta Reader do resultado</param>
        protected void ExecuteReaderQuery(string query, SqlParameter[] parameters, string methodNameInvoke)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.CommandText = query;
                    cm.Parameters.AddRange(parameters);

                    cn.Open();
                    Invoke(methodNameInvoke, new List<SqlDataReader>(1) { cm.ExecuteReader() }.ToArray());
                }
            }
        }
        #endregion Executes

        #endregion Data Methods

        #region Private Methods
        private void Invoke(string methodNameInvoke, object[] parameters)
        {
            MethodInfo met = this.GetType().GetMethod(methodNameInvoke);
            met.Invoke(this, parameters);
        }
        #endregion Private Methods
    }
}
