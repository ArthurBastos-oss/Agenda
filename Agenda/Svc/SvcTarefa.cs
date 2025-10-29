using Agenda.Data;
using Agenda.Mdl;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Agenda.Svc
{
    internal class SvcTarefa
    {
        public static bool VerificaTarefa(DateTime dataInicio, DateTime dataFim, Recorrencia recorrencia)
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                if (recorrencia == Recorrencia.Nenhuma)
                { 
                    using (OracleCommand cmd = new OracleCommand("PKG_Tarefa.VerificaTarefa", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        OracleParameter retorno = new OracleParameter("RETURN_VALOR", OracleDbType.Int32);
                        retorno.Direction = System.Data.ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(retorno);

                        cmd.Parameters.Add("pDataInicio", OracleDbType.Date).Value = dataInicio;
                        cmd.Parameters.Add("pDataFim", OracleDbType.Date).Value = dataFim;

                        cmd.ExecuteNonQuery();

                        int count = ((Oracle.ManagedDataAccess.Types.OracleDecimal)retorno.Value).ToInt32();

                        return count > 0;
                    }
                }
                else
                {
                    DateTime limite = dataInicio.AddYears(5);
                    DateTime dataAtual = dataInicio;
                    List<string> datas = new List<string>();

                    while (dataAtual <= limite)
                    {
                        datas.Add(dataAtual.ToString("yyyy-MM-dd"));

                        switch (recorrencia)
                        {
                            case Recorrencia.Diaria: dataAtual = dataAtual.AddDays(1); break;
                            case Recorrencia.Semanal: dataAtual = dataAtual.AddDays(7); break;
                            case Recorrencia.Mensal: dataAtual = dataAtual.AddMonths(1); break;
                            case Recorrencia.Anual: dataAtual = dataAtual.AddYears(1); break;
                        }
                    }

                    string csvDatas = string.Join(",", datas);

                    using (OracleCommand cmd = new OracleCommand("PKG_TAREFA.VerificaTarefaRecorrente", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        OracleParameter retorno = new OracleParameter("RETURN_VALOR", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        cmd.Parameters.Add(retorno);

                        cmd.Parameters.Add("pDatasCsv", OracleDbType.Varchar2).Value = csvDatas;

                        cmd.ExecuteNonQuery();

                        int count = ((Oracle.ManagedDataAccess.Types.OracleDecimal)retorno.Value).ToInt32();
                        return count > 0;
                    }
                }
            }
        }


        //public static int AddTarefa(DateTime dataInicio, DateTime dataFim, string descricao, Recorrencia recorrencia, List<int> contatos = null)
        public static int AddTarefa(List<Tarefa> pTarefas)
        {
            List<string> erros = new List<string>();
            
            foreach (var tarefa in pTarefas)
            {
                if (VerificaTarefa(tarefa.DataInicio, tarefa.DataFim, tarefa.Recorrencia))
                    erros.Add($"Já existe uma tarefa no periodo: {tarefa.DataInicio} a {tarefa.DataFim}");
                    //throw new Exception("Já existe uma tarefa nesse período. Escolha outro intervalo de tempo.");

                if (tarefa.DataFim < tarefa.DataInicio)
                    erros.Add($"A data de termino da tarefa {tarefa.DataFim} deve vir apos a data de inicio da tarefa {tarefa.DataInicio}");
                    //throw new Exception("Data de término deve vir após a data de início.");
            }
            if (erros.Count > 0)
            {
                throw new Exception(string.Join(", ", erros));
            }

            int tarefasCriadas = 0;
                      

            using (OracleConnection conn = new Conexao().AbrirConexao())           

            using (var ts = conn.BeginTransaction())
            {
                try
                {
                    foreach (var tarefa in pTarefas)
                    {
                        string contatoCsv = CsvListaContatos(tarefa.Contatos);

                        using (OracleCommand cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = ts;
                            cmd.BindByName = true;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = "BEGIN :pResult := PKG_TAREFA.CriarTarefaCompleta(:pDataInicio, :pDataFim, :pDescricao, :pRecorrencia, :pContatosCsv); END;";

                            cmd.Parameters.Add("pDataInicio", OracleDbType.Date).Value = tarefa.DataInicio;
                            cmd.Parameters.Add("pDataFim", OracleDbType.Date).Value = tarefa.DataFim;
                            cmd.Parameters.Add("pDescricao", OracleDbType.Varchar2, 4000).Value = tarefa.Descricao ?? (object)DBNull.Value;
                            cmd.Parameters.Add("pRecorrencia", OracleDbType.Int32).Value = (int)tarefa.Recorrencia;
                            cmd.Parameters.Add("pContatosCsv", OracleDbType.Varchar2, 4000).Value = contatoCsv ?? (object)DBNull.Value;


                            var pResult = cmd.Parameters.Add("pResult", OracleDbType.Decimal);
                            pResult.Direction = System.Data.ParameterDirection.ReturnValue;

                            cmd.ExecuteNonQuery();

                            tarefasCriadas = ((Oracle.ManagedDataAccess.Types.OracleDecimal)pResult.Value).ToInt32();
                        }
                        
                    }
                    ts.Commit();
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw new Exception("Erro ao adicionar tarefa: " + ex.Message, ex);
                }
            }
            return tarefasCriadas;
            
        }

        public static string CsvListaContatos(List<TarefaContato> listaContatos)
        {
            string contatosCsv = (listaContatos != null && listaContatos.Count > 0)
                ? string.Join(",", listaContatos.Select(c => c.IdContato))
                : null;
            return contatosCsv;
        }

        public static void LimparTarefasDeTeste()
        {
            using (OracleConnection conn = new Conexao().AbrirConexao())
            {
                using (var cmd = new OracleCommand(
                    @"DELETE FROM TarefaContato 
                    WHERE IdTarefa IN (SELECT Id FROM Tarefa WHERE Descricao LIKE 'Teste%')", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new OracleCommand(
                    @"DELETE FROM Tarefa WHERE Descricao LIKE 'Teste%'", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
