using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Prng;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class FuncionarioRepositorio (IConfiguration configuration)
    {
        private readonly string _conexaMySQL = configuration.GetConnectionString("ConexaoMySQL");
        public int Cadastrar(Funcionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("Call insertFuncionario(@cpf, @nome, @idade, @sexo, @cargo)", conexao);
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = funcionario.CPF;
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                cmd.Parameters.Add("@idade", MySqlDbType.Int32).Value = funcionario.Idade;
                cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = funcionario.Sexo;
                cmd.Parameters.Add("@cargo", MySqlDbType.VarChar).Value = funcionario.Cargo;
                int linhasAfetadas = cmd.ExecuteNonQuery();
                conexao.Close();
                return linhasAfetadas;
            }
        }
        public bool Atualizar(Funcionario funcionario)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaMySQL))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Update tb_Funcionario set CPF=@cpf, Nome=@nome, Idade=@idade, Sexo=@sexo, Cargo=@cargo where IdFuncionario=@idFuncionario", conexao);
                    cmd.Parameters.Add("@IdFuncionario", MySqlDbType.Int32).Value = funcionario.IdFuncionario;
                    cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = funcionario.CPF;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                    cmd.Parameters.Add("@idade", MySqlDbType.Int32).Value = funcionario.Idade;
                    cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = funcionario.Sexo;
                    cmd.Parameters.Add("@cargo", MySqlDbType.VarChar).Value = funcionario.Cargo;
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
    {
    }
}
