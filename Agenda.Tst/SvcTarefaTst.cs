using Agenda.Data;
using Agenda.Mdl;
using Agenda.Svc;

namespace Agenda.Tst
{
    [TestClass]
    public class SvcTarefaTst
    {
        [TestInitialize]
        public void Setup()
        {
            SvcTarefa.LimparTarefasDeTeste();
        }

        [TestMethod]
        public void TesteVerificaTarefa_TarefaExistente()
        {
            var tarefa = new Tarefa(); 
            tarefa.DataInicio = new DateTime(2025, 10, 27, 10, 0, 0);
            tarefa.DataFim = new DateTime(2025, 10, 27, 12, 0, 0);
            tarefa.Recorrencia = Recorrencia.Nenhuma;

            bool verificar = SvcTarefa.VerificaTarefa(tarefa.DataInicio, tarefa.DataFim, tarefa.Recorrencia);
            Assert.IsTrue(verificar, "O método retornou falso mesmo sem tarefas existentes.");
        }

        [TestMethod]
        public void TesteVerificaTarefa_TarefaNaoExistente()
        {
            var tarefa = new Tarefa();
            tarefa.DataInicio = new DateTime(2027, 04, 03, 10, 0, 0);
            tarefa.DataFim = new DateTime(2027, 04, 03, 12, 0, 0);
            tarefa.Recorrencia = Recorrencia.Nenhuma;

            bool verificar = SvcTarefa.VerificaTarefa(tarefa.DataInicio, tarefa.DataFim, tarefa.Recorrencia);
            Assert.IsFalse(verificar, "O método retornou verdairo mesmo sem tarefas existentes.");
        }

        [TestMethod]
        public void TesteAddTarefa()
        {
            List<Tarefa> tarefas = new List<Tarefa>();

            tarefas.Add(new Tarefa
            {
                DataInicio = new DateTime(2027, 04, 03, 10, 0, 0),
                DataFim = new DateTime(2027, 04, 03, 12, 0, 0),
                Descricao = "Teste",
            });
            

            int idTarefa = SvcTarefa.AddTarefa(tarefas);

            Assert.IsTrue(idTarefa > 0, "O id não foi retornado");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TesteAddTarefaExistente()
        {
            List<Tarefa> tarefas = new List<Tarefa>();

            tarefas.Add(new Tarefa
            {
                DataInicio = new DateTime(2027, 04, 03, 10, 0, 0),
                DataFim = new DateTime(2027, 04, 03, 12, 0, 0),
                Descricao = "Teste",
            });

            

            SvcTarefa.AddTarefa(tarefas);

            SvcTarefa.AddTarefa(tarefas);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TesteAddTarefa_DataInvetida()
        {
            List<Tarefa> tarefas = new List<Tarefa>();

            tarefas.Add(new Tarefa
            {
                DataInicio = new DateTime(2027, 04, 03, 12, 0, 0),
                DataFim = new DateTime(2027, 04, 03, 10, 0, 0),
                Descricao = "Teste",
            });


            SvcTarefa.AddTarefa(tarefas);
        }

        [TestMethod]
        public void TesteAddTarefaSemContatos()
        {
            List<Tarefa> tarefas = new List<Tarefa>();

            tarefas.Add(new Tarefa
            {
                DataInicio = new DateTime(2027, 04, 03, 10, 0, 0),
                DataFim = new DateTime(2027, 04, 03, 12, 0, 0),
                Descricao = "Teste",
                Contatos = new List<TarefaContato>()
            });

            int idTarefa = SvcTarefa.AddTarefa(tarefas);

            Assert.IsTrue(idTarefa > 0, "O id não foi retornado");
        }

        [TestMethod]
        public void TesteAddTarefaComContato()
        {
            var contato1 = new Contato();
            contato1.Nome = "Teste1";
            contato1.Email = "teste1%@teste.com";
            contato1.Telefone = "1111-1111";
            SvcContato.AddContato(contato1);

            var contato2 = new Contato();
            contato2.Nome = "Teste2";
            contato2.Email = "teste2%@teste.com";
            contato2.Telefone = "2222-2222";
            SvcContato.AddContato(contato2);

            List<Tarefa> tarefas = new List<Tarefa>();

            tarefas.Add(new Tarefa
            {
                DataInicio = new DateTime(2027, 04, 03, 10, 0, 0),
                DataFim = new DateTime(2027, 04, 03, 12, 0, 0),
                Descricao = "Teste",
                Contatos = new List<TarefaContato>()
                { 
                    new TarefaContato {IdContato = contato1.Id},
                    new TarefaContato {IdContato = contato2.Id},
                }
            });
            

            int id = SvcTarefa.AddTarefa(tarefas);

            Assert.IsTrue(id > 0, "O id não foi retornado");
        }

    }
}
