using PessoalLibrary.Configuracoes;
using PessoalLibrary.Padroes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaucierLibrary.ClienteOldBase
{
    public class ClienteOld
    {
        #region Property
        public bool Novo { get; private set; }
        public bool Vazio { get; private set; }

        public System.Guid Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Base")]
        public string Base { get; private set; }

        public bool Ativa { get; set; }

        public DateTime CriadoEm { get; set; }

        #endregion Property

        #region Constructors
        private ClienteOld()
        {
        }
        #endregion Constructors

        #region Control Methods
        public static ClienteOld NewClienteOld()
        {
            return Create(new CriteriaCreate());
        }

        public static ClienteOld GetClienteOld(Guid id)
        {
            return Select(new Criteria(id));
        }

        public static void DeleteClienteOld(Guid id)
        {
            Delete(new Criteria(id));
        }

        public void Save()
        {
            if (this.Novo)
                Insert();
            else
                Update();
        }
        #endregion Control Methods

        #region Data Methods

        #region ClassName
        private string ClasseNome
        {
            get
            {
                return "ClienteOld";
            }
        }
        #endregion ClassName

        #region Criteria
        private class Criteria
        {
            public Guid Id { get; set; }

            public Criteria(Guid id)
            {
                Id = id;
            }
        }
        private class CriteriaCreate
        {
        }
        #endregion Criteria

        #region Parameters
        private SqlParameter[] GetParametros()
        {
            List<SqlParameter> lista = new List<SqlParameter>();

            lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, this.Id, "@Id"));
            lista.Add(CriarParametro(SqlDbType.Bit, this.Ativa, "@Ativa"));
            lista.Add(CriarParametro(SqlDbType.VarChar, this.Base, "@Base"));
            lista.Add(CriarParametro(SqlDbType.VarChar, this.Nome, "@Nome"));
            lista.Add(CriarParametro(SqlDbType.DateTime, this.CriadoEm, "@CriadoEm"));

            //lista.Add(CriarParametro(SqlDbType.VarChar, obj.CodigoIndicacao, "@CodigoIndicacao"));
            //lista.Add(CriarParametro(SqlDbType.UniqueIdentifier, obj.Id, "@Id"));
            //lista.Add(CriarParametro(SqlDbType.Bit, obj.Liberado, "@Liberado"));
            //lista.Add(CriarParametro(SqlDbType.DateTime, obj.DataCadastro, "@DataCadastro"));
            //lista.Add(CriarParametro(SqlDbType.Int, obj.Idade, "@Idade"));

            return lista.ToArray();
        }

        private void SetParameters(SqlDataReader reader)
        {
            Novo = false;
            if (!reader.Read())
            {
                Vazio = true;
                return;
            }
            Vazio = false;
            this.Id = ConvertBase.ToGuid(reader["Id"].ToString());
            this.Nome = reader["Nome"].ToString();
            this.CriadoEm = Convert.ToDateTime(reader["CriadoEm"]);
            this.Base = reader["Base"].ToString();
            this.Ativa = Convert.ToBoolean(reader["Ativa"]);
        }

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
        private static ClienteOld Create(CriteriaCreate create)
        {
            ClienteOld obj = new ClienteOld();
            obj.Id = Guid.NewGuid();
            obj.Base = "Saucier" + obj.Id.ToString() + "DB";
            obj.Novo = true;
            obj.Vazio = false;
            obj.CriadoEm = DateTime.Now;
            return obj;
        }
        #endregion Create

        #region Select
        private static ClienteOld Select(Criteria criteria)
        {
            ClienteOld obj = new ClienteOld();
            using (SqlConnection cn = new SqlConnection(Configuracao.ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Get" + obj.ClasseNome;
                    cm.Parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, criteria.Id, "@Id"));

                    cn.Open();

                    obj.SetParameters(cm.ExecuteReader());
                }
            }
            return obj;
        }
        #endregion Select

        #region Insert
        private void Insert()
        {
            using (SqlConnection cn = new SqlConnection(Configuracao.ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Add" + ClasseNome;
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
        private void Update()
        {
            using (SqlConnection cn = new SqlConnection(Configuracao.ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Update" + ClasseNome;
                    cm.Parameters.AddRange(GetParametros());

                    cn.Open();

                    cm.ExecuteNonQuery();
                }
            }
        }
        #endregion Update

        #region Delete
        private static ClienteOld Delete(Criteria criteria)
        {
            ClienteOld obj = new ClienteOld();
            using (SqlConnection cn = new SqlConnection(Configuracao.ConnectionString))
            {
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    cm.CommandText = "Delete" + obj.ClasseNome;
                    cm.Parameters.Add(CriarParametro(SqlDbType.UniqueIdentifier, criteria.Id, "@Id"));

                    cn.Open();

                    cm.ExecuteNonQuery();
                }
            }
            return obj;
        }
        #endregion Delete

        #endregion Data Methods
    }
}
