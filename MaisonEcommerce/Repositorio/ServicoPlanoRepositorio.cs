using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;


namespace MaisonEcommerce.Repositorio
{

    public class ServicoPlanoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public int Cadastrar(Servico_Plano servicoPlano)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertServicoPlano(@servico, @plano);", conexao);
                cmd.Parameters.Add("@servico", MySqlDbType.VarChar).Value = servicoPlano.NomeServico;
                cmd.Parameters.Add("@plano", MySqlDbType.VarChar).Value = servicoPlano.NomePlano;

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conexao.Close();
                return linhasAfetadas;
            }
        }

        public bool Atualizar(Servico_Plano ServicoPlano)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Servico_Plano set IdServico=@servico, IdPlano=@plano where IdServicoPlano=@idServicoPlano", conexao);
                    cmd.Parameters.Add("@idServicoPlano", MySqlDbType.Int32).Value = ServicoPlano.IdServicoPlano;
                    cmd.Parameters.Add("@servico", MySqlDbType.Int32).Value = ServicoPlano.NomeServico;
                    cmd.Parameters.Add("@plano", MySqlDbType.Int32).Value = ServicoPlano.NomePlano;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao alterar serviço-plano: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Servico_Plano> TodosServicoPlano()
        {
            List<Servico_Plano> servico_Planos = new List<Servico_Plano>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select tb_Servico_Plano.IdServicoPlano, tb_Servico.Nome as Nome_Servico, tb_Plano.Nome as Nome_Plano, tb_Servico_Plano.DataAdicao, tb_Servico_Plano.DataAtualizacao \r\n" +
                    "from tb_Plano, tb_Servico, tb_Servico_Plano\r\n" +
                    "where tb_Servico_Plano.IdPlano = tb_Plano.IdPlano and tb_Servico_Plano.IdServico = tb_Servico.IdServico;", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    servico_Planos.Add(
                        new Servico_Plano
                        {
                            IdServicoPlano = Convert.ToInt32(dr["IdServicoPlano"]),
                            NomeServico = ((string)dr["Nome_Servico"]),
                            NomePlano = ((string)dr["Nome_Plano"]),
                            DataAdicao = Convert.ToDateTime(dr["DataAdicao"]),
                            DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"]),
                        });
                }
                return servico_Planos;
            }
        }

        public Servico_Plano ObterServicoPlano(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Servico_Plano where IdServicoPlano = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Servico_Plano servicoPlano = new Servico_Plano();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    servicoPlano.IdServicoPlano = Convert.ToInt32(dr["IdServicoPlano"]);
                    servicoPlano.IdPlano = Convert.ToInt32(dr["IdPlano"]);
                    servicoPlano.IdServico = Convert.ToInt32(dr["IdServico"]);
                }
                return servicoPlano;
            }
        }

        public void Excluir(int IdServicoPlano)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tb_Servico_Plano where IdServicoPlano = @idServicoPlano", conexao);
                cmd.Parameters.AddWithValue("@IdServicoPlano", IdServicoPlano);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao .Close();
            }

        }
    }

}
