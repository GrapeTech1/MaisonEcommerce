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

                MySqlCommand cmd = new MySqlCommand("Call insertPlano(@nome, @descricao, @duracao, @preco);", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = plano.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = plano.Descricao;
                cmd.Parameters.Add("@duracao", MySqlDbType.Int32).Value = plano.Duracao;
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

                    MySqlCommand cmd = new MySqlCommand("Update tb_Plano set Nome=@nome, Descricao=@descricao, Duracao=@duracao, Preco=@preco where IdPlano=@idPlano", conexao);
                    cmd.Parameters.Add("@idPlano", MySqlDbType.Int32).Value = plano.IdPlano;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = plano.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = plano.Descricao;
                    cmd.Parameters.Add("@duracao", MySqlDbType.Int32).Value = plano.Duracao;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = plano.Preco;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao editar plano: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Plano> TodosPlanos()
        {
            List<Plano> planos = new List<Plano>();

            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from tb_Plano", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    planos.Add(
                        new Plano
                        {
                            IdPlano = Convert.ToInt32(dr["IdPlano"]),
                            Nome = ((string)dr["Nome"]),
                            Descricao = ((string)dr["Descricao"]),
                            Duracao = Convert.ToInt32(dr["Duracao"]),
                            Preco = Convert.ToDecimal(dr["Preco"]),
                        }
                    );
                }
                return planos;
            }
        }

        public Plano ObterPlano(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Select * from tb_Plano where IdPlano = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Plano plano = new Plano();


                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    plano.IdPlano = Convert.ToInt32(dr["IdPlano"]);
                    plano.Nome = (string)(dr["Nome"]);
                    plano.Descricao = (string)(dr["Descricao"]);
                    plano.Duracao = Convert.ToInt32(dr["Duracao"]);
                    plano.Preco = Convert.ToDecimal(dr["Preco"]);
                }
                return plano;
            }
        }

        public void Excluir(int IdPlano)
        {

            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Delete from tb_Plano where IdPlano = @IdPlano", conexao);
                cmd.Parameters.AddWithValue("@IdPlano", IdPlano);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}
