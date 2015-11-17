using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PessoalLibrary.Padroes
{
    public static class ConvertBase 
    {
        public static Guid ToGuid(string value)
        {
            
            try 
            {
                return new Guid(value);
            }
            catch { }
            return Guid.Empty;
        }

        public static Guid ToGuid(object value)
        {

            try
            {
                return ToGuid(value.ToString());
            }
            catch { }
            return Guid.Empty;
        }
    }
}
