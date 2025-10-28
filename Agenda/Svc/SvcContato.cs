using Agenda.Data;
using Agenda.Mdl;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;

namespace Agenda.Svc
{
    internal class SvcContato
    {
        public static List<Contato> ListarContato()
        {
            List<Contato> lista = new List<Contato>();

            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "SELECT * FROM Contato";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                using (OracleDataReader da = cmd.ExecuteReader())
                {
                    while (da.Read())
                    {
                        Contato contato = new Contato
                        {
                            Id = da.GetInt32(da.GetOrdinal("Id")),
                            Email = da.GetString(da.GetOrdinal("Email")),
                            Nome = da.GetString(da.GetOrdinal("Nome")),
                            Telefone = da.GetString(da.GetOrdinal("Telefone"))
                        };
                        lista.Add(contato);
                    }
                }
            }
            return lista;
        }

        public static void AddContato(Contato pContato) 
        { 
            using (OracleConnection conn = new Conexao().AbrirConexao()) 
            {
                string sql = @"INSERT INTO Contato (Id, Email, Nome, Telefone)
                               VALUES (SQ_Contato_Id.NEXTVAL, :pEmail, :pNome, :pTelefone)
                               RETURNING Id INTO :pId";

                using (OracleCommand cmd = new OracleCommand(sql, conn)) 
                { 
                    cmd.Parameters.Add(new OracleParameter("pEmail", pContato.Email)); 
                    cmd.Parameters.Add(new OracleParameter("pNome", pContato.Nome)); 
                    cmd.Parameters.Add(new OracleParameter("pTelefone", pContato.Telefone));

                    OracleParameter pId = new OracleParameter("pId", OracleDbType.Int32);
                    pId.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(pId);

                    cmd.ExecuteNonQuery();

                    OracleDecimal oracleDecimal = (OracleDecimal)pId.Value;
                    pContato.Id = oracleDecimal.ToInt32();
                } 
            } 
        }

        public static void EditContato(Contato contato)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "UPDATE Contato SET Nome = :Nome, Email = :Email, Telefone = :Telefone WHERE Id = :Id";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add("Nome", contato.Nome);
                    cmd.Parameters.Add("Email", contato.Email);
                    cmd.Parameters.Add("Telefone", contato.Telefone);
                    cmd.Parameters.Add("Id", contato.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void LimparContatosDeTeste(OracleConnection conn)
        {
            using (var cmd = new OracleCommand(
                @"DELETE FROM TarefaContato 
          WHERE IdContato IN (SELECT Id FROM Contato WHERE Email LIKE 'teste%@teste.com')", conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new OracleCommand(
                @"DELETE FROM Contato WHERE Email LIKE 'teste%@teste.com'", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }


        public static Contato BuscarContatoPorId(int id)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                string sql = "SELECT Id, Nome, Email, Telefone FROM Contato WHERE Id = :Id";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("Id", id));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Contato
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Email = reader.GetString(2),
                                Telefone = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return null;
        }


    }
}
