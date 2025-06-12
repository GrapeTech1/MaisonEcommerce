using MySql.Data.MySqlClient;
using MaisonEcommerce.Models;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class LoginRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public Usuario ObterUsuario(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new("select * from tb_Usuario where Email = @email", conexao);
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    Usuario usuario = null;

                    if (dr.Read())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            Email = dr["Email"].ToString(),
                            Senha = dr["Senha"].ToString()
                        };
                    }

                    return usuario;
                }

            }
        }

        public int Cadastrar(Usuario usuario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertUsuario(@nome, @email, @senha)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = usuario.Nome;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = usuario.Email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = usuario.Senha;

                int linhasAfetadas = cmd.ExecuteNonQuery();
                conexao.Close();
                return linhasAfetadas;
            }
        }

        public bool Alterar(Usuario usuario)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Usuario set Senha=@senha where IdUsuario=@IdUsuario", conexao);
                    cmd.Parameters.Add("@IdUsuario", MySqlDbType.Int32).Value = usuario.IdUsuario;
                    cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = usuario.Senha;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao redefiinir a sua senha: {ex.Message}");
                return false;
            }
        }
    }
}
