using PessoalLibrary.Configuracoes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PessoalLibrary.Padroes
{

    #region Criteria
    public interface ICriteriaBaseInfo
    {
    }
    #endregion Criteria

    public abstract class ObjectModelInfo<T> where T : ObjectModelInfo<T>
    {
        #region Property
        public bool Vazio { get; protected set; }

        protected abstract TiboBase BaseSelected { get; }

        private List<T> List { get; set; }

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
        protected ObjectModelInfo()
        {
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

        public static T Get(ICriteriaBaseInfo criteria)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.SelectData(criteria);
            return obj;
        }

        public static List<T> ToList()
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.SelectDataList(string.Empty, new List<SqlParameter>());
            return obj.List;
        }

        protected static List<T> ToList(string sufix, List<SqlParameter> parameters)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.SelectDataList(sufix, parameters);
            return obj.List;
        }

        protected static T GetByReader(SqlDataReader reader)
        {
            if (!reader.NextResult())
                return null;
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.SetDefaultParameters(reader);
            return obj;
        }
        #endregion Control Methods

        #region Data Methods

        #region TableName
        protected virtual string ViewName { get; }
        #endregion TableName

        #region Parameters
        protected abstract SqlParameter[] GetFilterParameters(ICriteriaBaseInfo criteria);
        protected abstract void SetParameters(SqlDataReader reader);
        protected abstract void SetParentAndChildren(SqlDataReader reader);
        protected abstract void SetChildren();

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
            if (dbType != SqlDbType.Date && dbType != SqlDbType.DateTime)
                return new SqlParameter()
                {
                    SqlDbType = dbType,
                    Direction = ParameterDirection.Input,
                    Value = data ?? DBNull.Value,
                    ParameterName = nome
                };

            return new SqlParameter()
            {
                SqlDbType = dbType,
                Direction = ParameterDirection.Input,
                Value = GetValuDate(data),
                ParameterName = nome
            };
        }

        private static object GetValuDate(object data)
        {
            if (((DateTime)data) != DateTime.MinValue && ((DateTime)data) != DateTime.MaxValue)
                return data;
            return DBNull.Value;
        }
        #endregion Parameters

        #region Select
        private void SelectData(ICriteriaBaseInfo criteria)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Select" + ViewName;
                    cm.Parameters.AddRange(GetFilterParameters(criteria));

                    OpenConnection(cn);

                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        SetDefaultParameters(reader);
                        //if (reader.NextResult())
                            //SetParentAndChildren(reader);
                    }
                    
                }
            }
        }

        private void SetDefaultParameters(SqlDataReader reader)
        {
            if (!reader.Read())
            {
                Vazio = true;
                return;
            }
            Vazio = false;
            SetParameters(reader);
            SetParentAndChildren(reader);
        }

        private void SetListtParameters(SqlDataReader reader)
        {
            Vazio = false;
            SetParameters(reader);
        }

        #region Select List
        protected void SelectDataList(string sufix, List<SqlParameter> parameters)
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "SelectList" + ViewName + sufix;
                    cm.Parameters.AddRange(parameters.ToArray());

                    OpenConnection(cn);

                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        SetList(reader);
                    }

                }
            }
            MontarChildren();
        }

        protected void SetList(SqlDataReader reader)
        {
            List = new List<T>();
            while (reader.Read())
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                obj.SetListtParameters(reader);
                List.Add(obj);
            }
        }

        private void MontarChildren()
        {
            foreach(T obj in List)
            {
                obj.SetChildren();
            }
        }
        #endregion Select List
        #endregion Select

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

                    OpenConnection(cn);
                    Invoke(methodNameInvoke, new List<SqlDataReader>(1) { cm.ExecuteReader() }.ToArray());
                }
            }
        }

        private void OpenConnection(SqlConnection cn)
        {
            bool abriu = false;
            for(int i = 0; i < 100 && !abriu; i++)
            {
                try
                {
                    cn.Open();
                    abriu = true;
                }
                catch { }
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

                    OpenConnection(cn);

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

                    OpenConnection(cn);

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
