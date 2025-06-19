using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class ServPacoteRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public int Cadastrar(Servico_Pacote servPacote)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertServPacote(@servico, @pacote);", conexao);
                cmd.Parameters.Add("@servico", MySqlDbType.VarChar).Value = servPacote.NomeServico;
                cmd.Parameters.Add("@pacote", MySqlDbType.VarChar).Value = servPacote.NomePacote;

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conexao.Close();
                return linhasAfetadas;
            }
        }

        public bool Atualizar(Servico_Pacote servPacote)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Servico_Pacote set IdServico=@servico, IdPacote=@pacote where IdServicoPacote=@idServPacote", conexao);
                    cmd.Parameters.Add("@idServPacote", MySqlDbType.Int32).Value = servPacote.IdServicoPacote;
                    cmd.Parameters.Add("@servico", MySqlDbType.Int32).Value = servPacote.NomeServico;
                    cmd.Parameters.Add("@pacote", MySqlDbType.Int32).Value = servPacote.NomePacote;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao alterar serviço-pacote: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Servico_Pacote> TodosServPacote()
        {
            List<Servico_Pacote> servico_Pacotes = new List<Servico_Pacote>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select tb_Servico_Pacote.IdServicoPacote, tb_Servico.Nome as Nome_Serviço, tb_Pacote.Nome as Nome_Pacote, tb_Servico_Pacote.DataAdicao, tb_Servico_Pacote.DataAtualizacao  \r\n" +
                    "from tb_Pacote, tb_Servico, tb_Servico_Pacote\r\n" +
                    "where tb_Servico_Pacote.IdPacote = tb_Pacote.IdPacote and tb_Servico_Pacote.IdServico = tb_Servico.IdServico;", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    servico_Pacotes.Add(
                        new Servico_Pacote
                        {
                            IdServicoPacote = Convert.ToInt32(dr["IdServicoPacote"]),
                            NomeServico = ((string)dr["Nome_Serviço"]),
                            NomePacote = ((string)dr["Nome_Pacote"]),
                            DataAdicao = Convert.ToDateTime(dr["DataAdicao"]),
                            DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"]),
                        });
                }
                return servico_Pacotes;
            }
        }

        public Servico_Pacote ObterServPacote(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Servico_Pacote where IdServicoPacote = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Servico_Pacote servicoPacote = new Servico_Pacote();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    servicoPacote.IdServicoPacote = Convert.ToInt32(dr["IdServicoPacote"]);
                    servicoPacote.IdPacote = Convert.ToInt32(dr["IdPacote"]);
                    servicoPacote.IdServico = Convert.ToInt32(dr["IdServico"]);
                }
                return servicoPacote;
            }
        }

        public void Excluir(int IdServPacote)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tb_Servico_Pacote where IdServicoPacote = @idServPacote", conexao);
                cmd.Parameters.AddWithValue("@IdServPacote", IdServPacote);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}
