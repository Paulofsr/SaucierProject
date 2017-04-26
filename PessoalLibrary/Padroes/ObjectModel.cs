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
        public bool Novo { get; set; }
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

        public static void Delete(ICriteriaBase criteria)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.DeleteData(criteria);
        }

        public static List<NameItem> ToNameList(string info)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            return obj.SelectDataNameList(info);
        }

        public List<NameItem> NameList(string info)
        {
            return SelectDataNameList(info);
        }

        public static T GetByReader(SqlDataReader reader)
        {
            if (!reader.NextResult())
                return null;
            T obj = (T)Activator.CreateInstance(typeof(T));
            obj.SetDefaultParameters(reader);
            return obj;
        }

        protected abstract void BeforeSave();

        public void Save()
        {
            BeforeSave();
            if (this.Novo)
                InsertData();
            else
                UpdateData();
            SaveChilds();
        }

        protected abstract void SaveChilds();
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

                    //foreach (var prop in this.GetType().GetProperties())
                    //{
                    //    //Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(foo, null));
                    //    //lista.Add(CriarParametro(SqlDbType.Bit, , "@Consumo"));
                    //}

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
            Novo = false;
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
            Novo = false;
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
                    cm.CommandText = "List" + TableName + sufix;
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

        #region Select Name List
        private List<NameItem> SelectDataNameList(string info)
        {
            List<NameItem> list = new List<NameItem>();
            if (!string.IsNullOrEmpty(info))
                list.Add(new NameItem()
                {
                    Key = Guid.Empty,
                    Value = info
                });
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "NameList" + TableName;

                    OpenConnection(cn);

                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(new NameItem()
                            {
                                Key = ConvertBase.ToGuid(reader["KeyId"].ToString()),
                                Value = reader["Value"].ToString()
                            });
                    }

                }
            }
            return list;
        }
        #endregion Select Name List
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

                    OpenConnection(cn);

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

                    OpenConnection(cn);

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

                    OpenConnection(cn);

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
