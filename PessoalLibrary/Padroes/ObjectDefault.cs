using System;
using System.Data;
using System.Data.SqlClient;

namespace PessoalLibrary.Padroes
{
    public class ObjectDefault
    {
        public bool IsEmpty { get; protected set; }

        protected ObjectDefault()
        {
            IsEmpty = true;
        }
    }
}
