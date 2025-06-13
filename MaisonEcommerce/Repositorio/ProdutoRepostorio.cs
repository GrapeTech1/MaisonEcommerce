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

                using (var cmd = new MySqlCommand("insertProduto", conexao))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("vFoto", produto.Foto);
                    cmd.Parameters.AddWithValue("vTipoFoto", produto.TipoFoto);
                    cmd.Parameters.AddWithValue("vNome", produto.Nome);
                    cmd.Parameters.AddWithValue("vDesc", produto.Descricao);
                    cmd.Parameters.AddWithValue("vQuant", produto.Quantidade);
                    cmd.Parameters.AddWithValue("vPreco", produto.Preco);

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

                using (var cmd = new MySqlCommand("select * from tb_Produto", conexao))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && reader.GetName(0) == "Erro")
                        {
                            await reader.ReadAsync();
                            Console.WriteLine($"Erro na stored procedure do produto:{reader.GetString("Erro")}");
                            return produtos;
                        }

                        while (await reader.ReadAsync())
                        {
                            produtos.Add(new Produto
                            {
                                IdProduto = reader.GetInt32("IdProduto"),
                                Foto = reader.IsDBNull(reader.GetOrdinal("Foto")) ? null : (byte[])reader["Foto"],
                                TipoFoto = reader.IsDBNull(reader.GetOrdinal("TipoFoto")) ? null : reader.GetString("TipoFoto"),
                                Nome = reader.GetString("Nome"),
                                Descricao = reader.GetString("Descricao"),
                                Quantidade = reader.GetInt32("Quantidade"),
                                Preco = reader.GetDecimal("Preco"),
                                DataCadastro = reader.GetDateTime("DataCadastro"),
                                DataAtualizacao = reader.GetDateTime("DataAtualizacao")
                            });
                        }
                    }
                }
                    
               
            }
            return produtos;
        }

        public async Task<Produto> ObterProduto(int codigo)
        {
            Produto produto = null;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                await conexao.OpenAsync();

                using (var cmd = new MySqlCommand("select * from tb_Produto where IdProduto = @codigo", conexao))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            produto = new Produto
                            {
                                IdProduto = reader.GetInt32(reader.GetOrdinal("IdProduto")),
                                Foto = reader.IsDBNull(reader.GetOrdinal("Foto")) ? null : (byte[])reader["Foto"],
                                TipoFoto = reader.IsDBNull(reader.GetOrdinal("TipoFoto")) ? null : reader.GetString(reader.GetOrdinal("TipoFoto")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                                Quantidade = reader.GetInt32(reader.GetOrdinal("Quantidade")),
                                Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
                                DataCadastro = reader.GetDateTime(reader.GetOrdinal("DataCadastro")),
                                DataAtualizacao = reader.GetDateTime(reader.GetOrdinal("DataAtualizacao"))
                            };
                        }
                    }
                }
                    
            }
            return produto;
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
