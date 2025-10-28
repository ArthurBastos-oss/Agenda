using Agenda.Data;
using Agenda.Mdl;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Agenda.Svc
{
    internal class SvcUsuario
    {
        public static List<Usuario> ListarUsuario()
        {
            List<Usuario> lista = new List<Usuario>();

            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "SELECT * FROM Usuario";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                using (OracleDataReader da = cmd.ExecuteReader())
                {
                    while (da.Read())
                    {
                        Usuario usuario = new Usuario
                        {
                            Email = da.GetString(da.GetOrdinal("Email")),
                            Nome = da.GetString(da.GetOrdinal("Nome")),
                            Senha = da.GetString(da.GetOrdinal("Senha"))
                        };
                        lista.Add(usuario);
                    }
                }
            }
            return lista;
        }

        public static void AddUsuario(Usuario usuario)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = @"INSERT INTO Usuario (Email, Nome, Senha)
                       VALUES (:Email, :Nome, :Senha)";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("Email", usuario.Email));
                    cmd.Parameters.Add(new OracleParameter("Nome", usuario.Nome));
                    cmd.Parameters.Add(new OracleParameter("Senha", usuario.Senha));
                    cmd.ExecuteNonQuery();
                    
                }
            }
        }

        public static bool LogarUsuario(string email, string senha)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "SELECT * FROM Usuario WHERE Email = :Email AND Senha = :Senha";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("Email", email));
                    cmd.Parameters.Add(new OracleParameter("Senha", senha));

                    using (OracleDataReader da = cmd.ExecuteReader())
                    {
                        return da.HasRows;
                    }
                }
            }
        }


        public static void EditUsuario(Usuario usuario)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "UPDATE Usuario SET Nome = :Nome, Senha = :Senha WHERE Email = :Email";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":Nome", usuario.Nome);
                    cmd.Parameters.Add(":Senha", usuario.Senha);
                    cmd.Parameters.Add(":Email", usuario.Email);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool DeleteUsuario(string email)
        {
            bool deletado = false;

            using(OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "DELETE FROM Usuario WHERE Email = :Email";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("Email", email));

                    int linhaDeletada = cmd.ExecuteNonQuery();
                    if (linhaDeletada > 0)
                    {
                        deletado = true;
                    }
                }
            }

            return deletado;
        }

        public static void LimparUsuariosDeTeste(OracleConnection conn)
        {
            string sql = "DELETE FROM Usuario WHERE Email LIKE 'teste%@teste.com'";

            using (OracleCommand cmd = new OracleCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }


        public static Usuario BuscarUsuarioPorEmail(string email)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "SELECT Nome, Senha FROM Usuario WHERE Email = :Email";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("Email", email));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Nome = reader.GetString(0),
                                Senha = reader.GetString(1)
                            };
                        }
                    }
                }
                return null;
            }                
        }

        public static int BuscarUsuario(string email)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "SELECT COUNT(*) FROM Usuario WHERE Email = :Email";

                using (OracleCommand cmd = new OracleCommand( sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("Email", email));

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
        }
    }
}
