using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Crypto.Prng;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class PacoteRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaMySQL = configuration.GetConnectionString("ConexaoMySQL");
        public int Cadastrar(Pacote pacote)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("Call insertPacote(@nome, @descricao, @preco)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = pacote.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = pacote.Descricao;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = pacote.Preco;
                int linhasAfetadas = cmd.ExecuteNonQuery();
                conexao.Close();
                return linhasAfetadas;
            }
        }
        public bool Atualizar(Pacote pacote)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Update tb_Pacote set Nome=@nome, Descricao=@descricao, Preco=@preco where IdPacote=@idPacote", conexao);
                    cmd.Parameters.Add("@IdPacote", MySqlDbType.Int32).Value = pacote.IdPacote;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = pacote.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = pacote.Descricao;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = pacote.Preco;
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar pacote: {ex.Message}");
                return false;
            }   
        }

        public IEnumerable<Pacote> TodosPacotes()
        {
            List<Pacote> pacotes = new List<Pacote>();

            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Pacote", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    pacotes.Add(
                        new Pacote
                        {
                            IdPacote = Convert.ToInt32(dr["IdPacote"]),
                            Nome = ((string)dr["Nome"]),
                            Descricao = ((string)dr["Descricao"]),
                            Preco = Convert.ToDecimal(dr["Preco"]),
                            DataCadastro = Convert.ToDateTime(dr["DataCadastro"]),
                            DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"]),
                        });
                }
                return pacotes;
            }
        }

        public Pacote ObterPacote(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Pacote where IdPacote=@codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Pacote pacote = new Pacote();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    pacote.IdPacote = Convert.ToInt32(dr["IdPacote"]);
                    pacote.Nome = (string)(dr["Nome"]);
                    pacote.Descricao = (string)(dr["Descricao"]);
                    pacote.Preco = Convert.ToDecimal(dr["Preco"]);
                }
                return pacote;
            }
        }

        public void Excluir(int IdPacote)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tb_Pacote where IdPacote = @IdPacote", conexao);
                cmd.Parameters.AddWithValue("@IdPacote", IdPacote);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
    
    
}
