using PessoalLibrary.Configuracoes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PessoalLibrary.Padroes
{
    public class Contexto : IDisposable
    {
        [ThreadStatic]
        private static SqlConnection _conexao;
        private static bool _baseId;

        protected static bool BaseId
        {
            set
            {
                if (value)
                {
                    if (!_baseId)
                    {
                        //Conexao.c
                    }
                }
            }
        }

        private bool _disposed;
        private SqlConnection Conexao { get; set; }

        private static SqlConnection ConexaoDS
        {
            get
            {
                if (_conexao == null)
                    _conexao = new SqlConnection(Configuracao.ConnectionString);
                return _conexao;
            }
        }

        private static SqlConnection ConexaoDSBaseId
        {
            get
            {
                if (_conexao == null)
                    _conexao = new SqlConnection(Configuracao.ConnectionStringBase);
                return _conexao;
            }
        }

        public Contexto()
        {
            Conexao = ConexaoDS;
            Thread.BeginThreadAffinity();
            if (Conexao.State == ConnectionState.Closed)
                Conexao.Open();
        }

        public DataTable Select(string consulta, params SqlParameter[] parametros)
        {
            using (var comando = CriarComando(consulta, parametros))
                return ObterDataTable(comando);
        }

        public DataTable ObterDataTable(SqlCommand sql)
        {
            var aseDataAdapter = new SqlDataAdapter() { SelectCommand = sql };
            var dataTable = new DataTable() { Locale = CultureInfo.InvariantCulture };
            aseDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public object SelectEscalar(string consulta, params SqlParameter[] parametros)
        {
            using (var comando = CriarComando(consulta, parametros))

                return comando.ExecuteScalar();
        }


        public void ExecutarProcedure(string Procedure, params SqlParameter[] parametros)
        {
            var comando = CriarComando(Procedure, parametros);
            comando.CommandType = CommandType.StoredProcedure;
            comando.ExecuteNonQuery();
        }

        public void Executar(string sql, params SqlParameter[] parametros)
        {
            using (var comando = CriarComando(sql, parametros))
                comando.ExecuteNonQuery();
        }

        public void ExecutarBulk(string tableName, DataTable table, int batchSize, int timeout, params SqlBulkCopyColumnMapping[] mappings)
        {
            var bulk = new SqlBulkCopy(Conexao)
            {
                DestinationTableName = tableName,
                BatchSize = batchSize,
                BulkCopyTimeout = timeout
            };

            foreach (var mapping in mappings)
                bulk.ColumnMappings.Add(mapping);

            bulk.WriteToServer(table);
        }

        private SqlCommand CriarComando(string consulta, params SqlParameter[] parametros)
        {
            var comando = new SqlCommand(consulta, Conexao) { CommandTimeout = 300 };
            if (parametros != null) comando.Parameters.AddRange(parametros);
            return comando;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                if (Conexao.State == ConnectionState.Executing || Conexao.State == ConnectionState.Fetching || Conexao.State == ConnectionState.Open)
                    Conexao.Close();
                Thread.EndThreadAffinity();
            }
        }

        public SqlParameter CriarParametro(SqlDbType dbType, Object data, string nome)
        {
            return new SqlParameter()
            {
                SqlDbType = dbType,
                Direction = ParameterDirection.Input,
                Value = data ?? DBNull.Value,
                ParameterName = nome
            };
        }
    }
}
