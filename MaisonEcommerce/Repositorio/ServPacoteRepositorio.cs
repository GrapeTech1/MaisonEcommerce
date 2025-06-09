using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class ServPacoteRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public IEnumerable<Servico_Pacote> TodosServPacote()
        {
            List<Servico_Pacote> servico_Pacotes = new List<Servico_Pacote>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Servico_Pacote", conexao);
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
                            IdPacote = Convert.ToInt32(dr["IdPacote"]),
                            IdServico = Convert.ToInt32(dr["IdServico"]),
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

                MySqlCommand cmd = new MySqlCommand("", conexao);
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
                    //servicoPacote.NomeServico = (string)(dr["Nome_Serviço"]);
                    //servicoPacote.NomePacote = (string)(dr["Nome_Pacote"]);
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
