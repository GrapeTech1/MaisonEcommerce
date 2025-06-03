using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class FuncionarioRepositorio(IConfiguration configuration)
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
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar funcionário: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Funcionario> TodosFuncionarios()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();

            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Funcionario", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    funcionarios.Add(
                        new Funcionario
                        {
                            IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]),
                            CPF = ((string)dr["CPF"]),
                            Nome = ((string)dr["Nome"]),
                            Idade = Convert.ToInt32(dr["Idade"]),
                            Sexo = ((string)dr["Sexo"]),
                            Cargo = ((string)dr["Cargo"]),
                            DataCadastro = Convert.ToDateTime(dr["DataCadastro"]),
                            DataAtualizacao = Convert.ToDateTime(dr["DataAtualizacao"])
                        });
                }
                return funcionarios;
            }
        }

        public Funcionario ObterFuncionario(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Funcionario where IdFuncionario = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Funcionario funcionario = new Funcionario();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    funcionario.IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]);
                    funcionario.CPF = (string)(dr["CPF"]);
                    funcionario.Nome = (string)(dr["Nome"]);
                    funcionario.Idade = Convert.ToInt32(dr["Idade"]);
                    funcionario.Sexo = (string)(dr["Sexo"]);
                    funcionario.Cargo = (string)(dr["Cargo"]);
                    
                }
                return funcionario;
            }
        }

        public void Excluir(int IdFuncionario)
        {
            using (var conexao = new MySqlConnection(_conexaMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tb_Funcionario where IdFuncionario = @IdFuncionario", conexao);
                cmd.Parameters.AddWithValue("@IdFuncionario", IdFuncionario);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
    
}

