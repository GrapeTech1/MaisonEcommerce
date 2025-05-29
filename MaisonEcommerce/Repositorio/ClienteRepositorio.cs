using Google.Protobuf.WellKnownTypes;
using MaisonEcommerce.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MaisonEcommerce.Repositorio
{
    public class ClienteRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public int Cadastrar (Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("Call insertCliente(@cpf, @nome, @telefone, @idade, @sexo, @dataCa);", conexao);
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cliente.CPF;
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cliente.Nome;
                
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@idade", MySqlDbType.Int32).Value = cliente.Idade;
                cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = cliente.Sexo;
                cmd.Parameters.Add("@dataCa", MySqlDbType.DateTime).Value = DateTime.Today;


                if (cliente.Idade < 18)
                {
                    return -1;
                }

                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();

                return linhasAfetadas;
            }
        }

        public bool Atualizar(Cliente cliente)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand("Update tb_Cliente set CPF=@cpf, Nome=@nome, Telefone=@telefone, Idade=@idade, Sexo=@sexo where IdCliente=@idCliente", conexao);
                    cmd.Parameters.Add("@idCliente", MySqlDbType.Int32).Value = cliente.IdCliente;
                    cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cliente.CPF;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cliente.Nome;
                    cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                    cmd.Parameters.Add("@idade", MySqlDbType.Int32).Value = cliente.Idade;
                    cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = cliente.Sexo;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }

            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar cliente: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Cliente> TodosClientes()
        {
            List<Cliente> ClientesLista = new List<Cliente>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Cliente", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    ClientesLista.Add(
                        new Cliente
                        {
                            IdCliente = Convert.ToInt32(dr["IdCliente"]),
                            CPF = ((string)dr["CPF"]),
                            Nome = ((string)dr["Nome"]),
                            Telefone = ((string)dr["Telefone"]),
                            Idade = Convert.ToInt32(dr["Idade"]),
                            Sexo = ((string)dr["Sexo"]),
                        });
                }
                return ClientesLista;
            }
        }

        public Cliente ObterCliente(int codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tb_Cliente where IdCliente = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Cliente cliente = new Cliente();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    cliente.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                    cliente.CPF = (string)(dr["CPF"]);
                    cliente.Nome = (string)(dr["Nome"]);
                    cliente.Telefone = (string)(dr["Telefone"]);
                    cliente.Idade = Convert.ToInt32(dr["Idade"]);
                    cliente.Sexo = (string)(dr["Sexo"]);
                }
                return cliente;
            }
        }

        public void Excluir(int IdCliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tb_Cliente where IdCliente = @IdCliente", conexao);
                cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                int linhasAfetadas = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}
