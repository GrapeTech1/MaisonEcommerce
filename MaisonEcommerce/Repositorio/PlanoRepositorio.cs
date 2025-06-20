using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
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

                MySqlCommand cmd = new MySqlCommand("Call insertPlano(@nome, @descricao, @duracao);", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = plano.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = plano.Descricao;
                cmd.Parameters.Add("@duracao", MySqlDbType.VarChar).Value = plano.Duracao;

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
                return linhasAfetadas;
            }
        }

        public bool Atualizar(Plano plano)
        {
            try
            {
                int planoDuplicado = TodosPlanos().Where(a => a.Nome == plano.Nome).Count();

                if (planoDuplicado > 0)
                {
                    return false;
                }

                using (var conexao = new MySqlConnection(_conexaMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Plano set Descricao=@descricao, Duracao=@duracao where IdPlano=@idPlano", conexao);
                    cmd.Parameters.Add("@idPlano", MySqlDbType.Int32).Value = plano.IdPlano;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = plano.Descricao;
                    cmd.Parameters.Add("@duracao", MySqlDbType.VarChar).Value = plano.Duracao;

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
                            Duracao = ((string)dr["Duracao"]),
                            Preco = dr["Preco"] is DBNull ? 0 : Convert.ToDecimal(dr["Preco"]),
                            DataCadastro = Convert.ToDateTime(dr["DataCadastro"]),
                            DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"]),
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
                    plano.Duracao = (string)(dr["Duracao"]);
                    plano.Preco = dr["Preco"] is DBNull ? 0 : Convert.ToDecimal(dr["Preco"]);
                }
                return plano;
            }
        }

        public void Excluir(int IdPlano)
        {

            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tb_Servico_Plano where IdPlano = @IdPlano; Delete from tb_Plano where IdPlano = @IdPlano", conexao);
                cmd.Parameters.AddWithValue("@IdPlano", IdPlano);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}
