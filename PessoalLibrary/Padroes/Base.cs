using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PessoalLibrary.Padroes
{
    public abstract class Base : IDisposable
    {
        private bool _disposed;
        private Contexto _contexto;

        protected Contexto Contexto
        {
            get
            {
                if (_contexto == null)
                    _contexto = new Contexto();
                return _contexto;
            }
        }

        //protected Base(bool baseId) { _baseId = baseId; }

        protected object SelectEscalar(string sql, params SqlParameter[] parametros)
        {
            object scalar = Contexto.SelectEscalar(sql, parametros);
            return scalar == DBNull.Value ? null : scalar;
        }

        protected DataTable Select(string sql)
        {
            return Select(sql, null);
        }

        protected DataTable Select(string sql, params SqlParameter[] parametros)
        {
            return Contexto.Select(sql, parametros);
        }

        protected void Executar(string sql, params SqlParameter[] parametros)
        {
            Contexto.Executar(sql, parametros);
        }

        protected void ExecutarBulk(string tableName, DataTable table, int batchSize, int timeout, params SqlBulkCopyColumnMapping[] mappings)
        {
            Contexto.ExecutarBulk(tableName, table, batchSize, timeout, mappings);
        }

        protected void ExecutarProcedure(string nomeProcedure, SqlParameter[] parametros)
        {
            Contexto.ExecutarProcedure(nomeProcedure, parametros);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Contexto.Dispose();
            }
        }
    }
}
