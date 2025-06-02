using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class PlanoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public int Cadastrar(Plano plano)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertPlano(@nome, @descricao, @duracao, @preco)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = plano.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = plano.Descricao;
                cmd.Parameters.Add("@duracao", MySqlDbType.Int32).Value = plano.DuracaoPlano;
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = plano.Preco;

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
                return linhasAfetadas;
            }
        }

        public bool Atualizar(Plano plano)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Plano set Nome=@nome, Descricao=@descricao, DuracaoPlano=@duracao, Preco=@preco where IdPlano=@idPlano", conexao);
                    cmd.Parameters.Add("@idPlano", MySqlDbType.Int32).Value = plano.IdPlano;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = plano.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = plano.Descricao;
                    cmd.Parameters.Add("@duracao", MySqlDbType.Int32).Value = plano.DuracaoPlano;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = plano.Preco;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar plano: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Plano> TodosPlano()
        { 
            List<Plano> planos = new List<Plano>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))

                    conexao.Open();

            MySqlCommand cmd = new MySqlCommand("select * from tb_Plano", conexao);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            conexao.Close();    

            foreach (DataRow dr in dt.Rows)
            {
                Plano.Add(
                    new Plano
                    {
                        IdPlano = Convert.ToInt32(dr["IdPlano"]),
                        Nome = dr["Nome"].ToString(),
                        Descricao = dr["Descricao"].ToString(),
                        DuracaoPlano = dr["DuracaoPlano"].ToString(),
                        Preco = Convert.ToDecimal(dr["Preco"]),
                        DataCadastro = Convert.ToDateTime(dr["DataCadastro"]),
                        DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"])
                    }

                );
            }
            return Plano;
        }
        public Servico ObterServico(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Plano where IdPlano = @idPlano", conexao);
                cmd.Parameters.AddWithValue("@idPlano", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Servico servico = new Servico();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    servico.IdServico = Convert.ToInt32(dr["IdPlano"]);
                    servico.Nome = (string)(dr["Nome"]);
                    servico.Descricao = (string)(dr["Descricao"]);
                    servico.Preco = Convert.ToDecimal(dr["Preco"]);
                }
                return servico;
            }
        }

        public void Excluir(int IdPlano)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from tb_Plano where IdServico = @idPlano", conexao);
                cmd.Parameters.AddWithValue("@idPlano", IdPlano);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}
