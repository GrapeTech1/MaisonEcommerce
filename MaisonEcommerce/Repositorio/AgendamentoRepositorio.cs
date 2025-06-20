using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class AgendamentoRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public int Cadastrar(Agendamento agendamento)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertAgen(@cliente, @servico, @dataHora)", conexao);
                cmd.Parameters.Add("@cliente", MySqlDbType.VarChar).Value = agendamento.CPF;
                cmd.Parameters.Add("@servico", MySqlDbType.VarChar).Value = agendamento.NomeServico;
                cmd.Parameters.Add("@dataHora", MySqlDbType.DateTime).Value = agendamento.DataHora;

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();

                return linhasAfetadas;
            }
        }

        public bool Atualizar(Agendamento agendamento)
        {
            try
            {
                int agendamentoMarcado = TodosAgendamentos().Where(a => a.DataHora == agendamento.DataHora).Count();

                if (agendamentoMarcado > 0)
                {
                    return false;
                }

                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Agendamento set IdServico_Agen=@servico, DataHora = @dataHora where IdAgendamento=@idAgendamento", conexao);

                    cmd.Parameters.Add("@idAgendamento", MySqlDbType.Int32).Value = agendamento.IdAgendamento;
                    cmd.Parameters.Add("@servico", MySqlDbType.Int32).Value = agendamento.NomeServico;
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

                MySqlCommand cmd = new MySqlCommand("select tb_agendamento.IdAgendamento, tb_Cliente.CPF, tb_Cliente.Nome as Nome_Cliente, tb_Servico.Nome as Nome_Servico, tb_agendamento.DataHora, tb_agendamento.DataCadastro, tb_agendamento.DataAtualizacao " +
                    "from tb_Agendamento, tb_Cliente, tb_Servico " +
                    "where tb_Agendamento.IdCliente_Agen = tb_Cliente.IdCliente and tb_Agendamento.IdServico_Agen = tb_Servico.IdServico; ", conexao);

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
                            CPF = ((string)dr["CPF"]),
                            NomeCliente = ((string)dr["Nome_Cliente"]),
                            NomeServico = ((string)dr["Nome_Servico"]),
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

                MySqlCommand cmd = new MySqlCommand("select * \r\nfrom tb_Cliente, tb_Agendamento\r\nwhere tb_Agendamento.IdCliente_Agen = @codigo;", conexao);
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
                    agendamento.CPF = (string)(dr["CPF"]);
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
