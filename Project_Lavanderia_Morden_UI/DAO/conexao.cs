using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lavanderia_Morden_UI.DAO
{
    class Conexao
    {
        private SqlConnection con;
        private string connectionString = ConfigurationManager.ConnectionStrings["Lavanderia"].ConnectionString;

        public void Insert(String nome, String telefone, String endereco, String email, String bairro, String cidade, String estado, String cep)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder();

                        sql.Append("INSERT INTO TB_CLIENTE(");
                        sql.Append("NOME");
                        sql.Append(",TELEFONE");
                        sql.Append(",ENDERECO");
                        sql.Append(",EMAIL");
                        sql.Append(",DT_CADASTRO");
                        sql.Append(",BAIRRO");
                        sql.Append(",CIDADE");
                        sql.Append(",ESTADO");
                        sql.Append(",CEP)");

                        sql.Append("VALUES(");
                        sql.Append("@NOME");
                        sql.Append(",@TELEFONE");
                        sql.Append(",@ENDERECO");
                        sql.Append(",@EMAIL");
                        sql.Append(",FORMAT(GETDATE(), 'dd/MM/yyyy')");
                        sql.Append(",@BAIRRO");
                        sql.Append(",@CIDADE");
                        sql.Append(",@ESTADO");
                        sql.Append(",@CEP)");

                        cmd.Parameters.AddWithValue("@NOME", nome);
                        cmd.Parameters.AddWithValue("@TELEFONE", telefone);
                        cmd.Parameters.AddWithValue("@ENDERECO", endereco);
                        cmd.Parameters.AddWithValue("@EMAIL", email);
                        cmd.Parameters.AddWithValue("@BAIRRO", bairro);
                        cmd.Parameters.AddWithValue("@CIDADE", cidade);
                        cmd.Parameters.AddWithValue("@ESTADO", estado);
                        cmd.Parameters.AddWithValue("@CEP", cep);

                        cmd.Connection = con;
                        cmd.CommandText = sql.ToString();
                        cmd.ExecuteNonQuery();

                    }
                }

            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder();

                        sql.Append("DELETE FROM TB_CLIENTE WHERE ID_CLIENTE = @ID");

                        cmd.Parameters.AddWithValue("@ID", id);

                        cmd.Connection = con;
                        cmd.CommandText = sql.ToString();
                        cmd.ExecuteNonQuery();

                    }

                }

            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
        public void Update(String nome, String telefone, String endereco, String email, String bairro, String cidade, String estado, String cep, int idCliente)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder();

                        sql.Append("UPDATE TB_CLIENTE SET ");
                        sql.Append("NOME=@NOME");
                        sql.Append(",TELEFONE=@TELEFONE");
                        sql.Append(",EMAIL=@EMAIL");
                        sql.Append(",ENDERECO=@ENDERECO");
                        sql.Append(",BAIRRO=@BAIRRO,");
                        sql.Append("CIDADE=@CIDADE,");
                        sql.Append("ESTADO=@ESTADO,");
                        sql.Append("CEP=@CEP ");
                        sql.Append("WHERE ID_CLIENTE=@ID");

                        cmd.Parameters.AddWithValue("@ID", idCliente);
                        cmd.Parameters.AddWithValue("@NOME", nome);
                        cmd.Parameters.AddWithValue("@TELEFONE", telefone);
                        cmd.Parameters.AddWithValue("@ENDERECO", endereco);
                        cmd.Parameters.AddWithValue("@EMAIL", email);
                        cmd.Parameters.AddWithValue("@BAIRRO", bairro);
                        cmd.Parameters.AddWithValue("@CIDADE", cidade);
                        cmd.Parameters.AddWithValue("@ESTADO", estado);
                        cmd.Parameters.AddWithValue("@CEP", cep);

                        cmd.Connection = con;
                        cmd.CommandText = sql.ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
        public DataTable SelectGrid(String sql)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    DataTable dtLista = new DataTable();
                    SqlDataAdapter adaptador = new SqlDataAdapter();

                    adaptador = new SqlDataAdapter(sql, con);

                    adaptador.Fill(dtLista);

                    return dtLista;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public int Select_Id_Faturamento()
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder();

                        sql.Append("INSERT INTO TB_FATURAMENTO (");
                        sql.Append("DT_CADASTRO");
                        sql.Append(",[STATUS]) ");

                        sql.Append("VALUES (");
                        sql.Append("FORMAT(GETDATE(),");
                        sql.Append("'dd/MM/yyyy'),");
                        sql.Append("'PENDENTE')");
                        sql.Append(";SELECT SCOPE_IDENTITY ()");

                        cmd.Connection = con;
                        cmd.CommandText = sql.ToString();
                        int id_Fat = Convert.ToInt32(cmd.ExecuteScalar());

                        return id_Fat;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
        public void InsertOs(String descricao, int quantidade, decimal preco, decimal total, int idCliente, int id_Fat)
        {
            try
            {
                using (con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        StringBuilder sql = new StringBuilder();

                        sql.Append("INSERT INTO TB_OS(");
                        sql.Append("ID_CLIENTE");
                        sql.Append(",DT_CADASTRO");
                        sql.Append(",DESCRICAO");
                        sql.Append(",QUANTIDADE");
                        sql.Append(",PRECO");
                        sql.Append(",TOTAL");
                        sql.Append(",ID_FAT) ");

                        sql.Append("VALUES (");
                        sql.Append("@ID_CLIENTE");
                        sql.Append(",FORMAT(GETDATE(),'dd/MM/yyyy')");
                        sql.Append(",@DESCRICAO");
                        sql.Append(",@QUANTIDADE");
                        sql.Append(",@PRECO");
                        sql.Append(",@TOTAL");
                        sql.Append(",@ID_FAT)");

                        cmd.Parameters.AddWithValue("@ID_CLIENTE", idCliente);
                        cmd.Parameters.AddWithValue("@DESCRICAO", descricao);
                        cmd.Parameters.AddWithValue("@QUANTIDADE", quantidade);
                        cmd.Parameters.AddWithValue("@PRECO", preco);
                        cmd.Parameters.AddWithValue("@TOTAL", total);
                        cmd.Parameters.AddWithValue("@ID_FAT", id_Fat);

                        cmd.Connection = con;
                        cmd.CommandText = sql.ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
    }
}
