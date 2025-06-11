using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class ServicoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public int Cadastrar(Servico servico)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertServico(@nome, @descricao, @preco);", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = servico.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = servico.Descricao;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = servico.Preco;

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
                return linhasAfetadas;
            }
        }

        public bool Atualizar(Servico servico)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Servico set Nome=@nome, Descricao=@descricao, Preco=@preco where IdServico=@idServico", conexao);
                    cmd.Parameters.Add("@idServico", MySqlDbType.Int32).Value = servico.IdServico;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = servico.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = servico.Descricao;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = servico.Preco;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar serviço: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Servico> TodosServicos()
        {
            List<Servico> Servicos = new List<Servico>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Servico", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Servicos.Add(
                        new Servico
                        {
                            IdServico = Convert.ToInt32(dr["IdServico"]),
                            Nome = ((string)dr["Nome"]),
                            Descricao = ((string)dr["Descricao"]),
                            Preco = Convert.ToDecimal(dr["Preco"]),
                        }
                    );
                }
                return Servicos;
            }
        }

        public Servico ObterServico(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Servico where IdServico = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Servico servico = new Servico();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    servico.IdServico = Convert.ToInt32(dr["IdServico"]);
                    servico.Nome = (string)(dr["Nome"]);
                    servico.Descricao = (string)(dr["Descricao"]);
                    servico.Preco = Convert.ToDecimal(dr["Preco"]);
                }
                return servico;
            }
        }

        public void Excluir(int IdServico)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("set @Servico = @IdServico; delete from tb_Servico where IdServico = @IdServico", conexao);
                cmd.Parameters.AddWithValue("@IdServico", IdServico);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}
