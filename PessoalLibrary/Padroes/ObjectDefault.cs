using System;
using System.Data;
using System.Data.SqlClient;

namespace PessoalLibrary.Padroes
{
    public class ObjectDefault<T> : Base
    {
        public bool IsNew { get; set; }
        public bool IsEmpty { get; protected set; }

        protected ObjectDefault() : base()
        {
            IsNew = true;
            IsEmpty = false;
        }

        protected SqlParameter CriarParametro(SqlDbType dbType, Object data, string nome)
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
