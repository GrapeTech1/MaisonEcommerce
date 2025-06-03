using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class AgendamentoRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public int Cadastrar(Agendamento agendamento) // dps testar importar a model do serviço e cliente
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertAgendamento(@idCli, @idServ, @dataHora)", conexao);
                cmd.Parameters.Add("@idCli", MySqlDbType.Int32).Value = agendamento.IdCliente_Agen;
                cmd.Parameters.Add("@idServ", MySqlDbType.Int32).Value = agendamento.IdServico_Agen;
                cmd.Parameters.Add("@data", MySqlDbType.Date).Value = agendamento.DataHora;

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();

                return linhasAfetadas;
            }
        }

        public bool Atualizar(Agendamento agendamento)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Agendamento set IdCliente_Agen=@idCli, IdServico_Agen=@idServ, DataHora=@dataHora where IdAgendamento=@idAgendamento", conexao);
                    cmd.Parameters.Add("@idAgendamento", MySqlDbType.Int32).Value = agendamento.IdAgendamento;
                    cmd.Parameters.Add("@idCli", MySqlDbType.Int32).Value = agendamento.IdCliente_Agen;
                    cmd.Parameters.Add("@idServ", MySqlDbType.Int32).Value = agendamento.IdServico_Agen;
                    cmd.Parameters.Add("@dataHora", MySqlDbType.DateTime).Value = agendamento.DataHora;


                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar agendamento: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Agendamento> TodosAgendamentos()
        {
            List <Agendamento> agendamentos = new List <Agendamento>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Agendamento", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    agendamentos.Add(
                        new Agendamento
                        {
                            IdAgendamento = Convert.ToInt32(dr["IdAgendamento"]),
                            IdCliente_Agen = Convert.ToInt32(dr["IdCliente_Agen"]),
                            IdServico_Agen = Convert.ToInt32(dr["IdServico_Agen"]),
                            DataHora = Convert.ToDateTime(dr["DataHora"]),
                            DataCadastro = Convert.ToDateTime(dr["DataCadastro"]),
                            DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"]),
                        });
                }
                return agendamentos;
            }
        }

        public Agendamento ObterAgendamento(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Agendamento where IdAgendamento = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Agendamento agendamento = new Agendamento();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    agendamento.IdAgendamento = Convert.ToInt32(dr["IdAgendamento"]);
                    agendamento.IdCliente_Agen = Convert.ToInt32(dr["IdCliente_Agen"]);
                    agendamento.IdServico_Agen = Convert.ToInt32(dr["IdServico_Agen"]);
                    agendamento.DataHora = Convert.ToDateTime(dr["DataHora"]);
                }
                return agendamento;
            }
        }

        public void Excluir(int IdAgendamento)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tb_Agendamento where IdAgendamento = @IdAgendamento", conexao);
                cmd.Parameters.AddWithValue("@IdAgendamento", IdAgendamento);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    } 
    
        
    
}
