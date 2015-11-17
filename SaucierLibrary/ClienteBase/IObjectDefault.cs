using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaucierLibrary.ClienteBase
{
    public interface IObjectDefault
    {
        #region Data Methods

        #region ClassName
        string TableName { get; }
        #endregion ClassName

        #region Parameters
        SqlParameter[] GetParametros();
        SqlParameter[] GetIdParametros(ICriteriaBase criteria);
        void SetParameters(SqlDataReader reader);
        #endregion Parameters

        #endregion Data Methods
    }
}
