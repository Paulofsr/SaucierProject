using System;
using System.Data;
using System.Data.SqlClient;

namespace PessoalLibrary.Padroes
{
    public class ObjectInfoDefault<T>
    {
        public bool IsEmpty { get; protected set; }

        protected ObjectInfoDefault()
        {
            IsEmpty = true;
        }
    }
}
