using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;

namespace MaisonEcommerce.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public async Task<string> Cadastrar(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                using (var cmd = new MySqlCommand("Call insertProduto(@nome, @descricao, @quantidade, @preco);", conexao))
                {
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                    cmd.Parameters.AddWithValue("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return reader.GetString(reader.GetOrdinal("Mensagem"));
                            }

                            return "Erro ao cadastrar produto.";
                        }
                    }

                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Erro MySQL ao cadastrar produto: {ex.Message}");
                        return $"Erro de banco de dados ao cadastrar produto: {ex.Message}";
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro inesperado no repositório ao cadastrar produto: {ex.Message}");
                        return $"Erro inesperado ao cadastrar: {ex.Message}";
                    }
                }
            }
        }

        public bool Atualizar(Produto produto)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Produto set Nome=@nome, Descricao=@descricao, Quantidade=@quantidade, Preco=@preco where IdProduto = @idProduto", conexao);
                    cmd.Parameters.Add("@idProduto", MySqlDbType.Int32).Value = produto.IdProduto;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.Preco;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao editar produto: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Produto>> TodosProdutos()
        {
            var produtos = new List<Produto>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Produto", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    produtos.Add(
                        new Produto
                        {
                            IdProduto = Convert.ToInt32(dr["IdProduto"]),
                            Nome = ((string)dr["Nome"]),
                            Descricao = ((string)dr["Descricao"]),
                            Quantidade = Convert.ToInt32(dr["Quantidade"]),
                            Preco = Convert.ToDecimal(dr["Preco"]),
                            DataCadastro = Convert.ToDateTime(dr["DataCadastro"]),
                            DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"]),
                        }
                        );
                }
                return produtos;
            }
        }

        public Produto ObterProduto(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Produto where IdProduto = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Produto produto = new Produto();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    produto.IdProduto = Convert.ToInt32(dr["IdProduto"]);
                    produto.Nome = (string)(dr["Nome"]);
                    produto.Descricao = (string)(dr["Descricao"]);
                    produto.Quantidade = Convert.ToInt32(dr["Quantidade"]);
                    produto.Preco = Convert.ToDecimal(dr["Preco"]);
                }
                return produto;
            }
        }

        public void Excluir(int IdProduto)
        {
            using (var conexao  = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tb_Produto where IdProduto = @IdProduto", conexao);
                cmd.Parameters.AddWithValue("@IdProduto", IdProduto);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}
