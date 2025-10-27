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

        public static bool AddContato(Contato pContato) 
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

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    OracleDecimal oracleDecimal = (OracleDecimal)pId.Value;
                    pContato.Id = oracleDecimal.ToInt32();

                    return linhasAfetadas > 0;

                } 
            } 
        }

        public static bool EditContato(Contato contato)
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
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
