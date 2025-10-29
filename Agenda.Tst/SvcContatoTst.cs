using Agenda.Mdl;
using Agenda.Svc;
using Agenda.Data;
using Oracle.ManagedDataAccess.Client;

namespace Agenda.Tst
{
    [TestClass]
    public class SvcContatoTst
    {
        

        [TestInitialize]
        public void Setup()
        {
            SvcContato.LimparContatosDeTeste();
        }



        [TestMethod]
        public void TesteListar()
        {                       
            var lista = SvcContato.ListarContato();
            
            Assert.IsTrue(lista.Count > 0);
        }

        [TestMethod]
        public void TesteAddContato()
        {
            var contato = new Contato();
            contato.Nome = "Teste";
            contato.Email = "teste%@teste.com";
            contato.Telefone = "1521-3094";

            SvcContato.AddContato(contato);

            Assert.IsTrue(contato.Id > 0, "O contato não foi inserido");
        }

        [TestMethod]
        [ExpectedException(typeof(OracleException))]
        public void TesteAddContatoInvalido()
        {
            var contato = new Contato();
            contato.Nome = null;
            contato.Email = "teste%@teste.com";
            contato.Telefone = "1521-3094";

            SvcContato.AddContato(contato);
        }

        [TestMethod]
        public void TesteEditContato()
        {
            var contato = new Contato();
            contato.Nome = "Teste";
            contato.Email = "teste%@teste.com";
            contato.Telefone = "9999-9999";

            SvcContato.AddContato(contato);

            contato.Nome = "Teste Atualizado";
            contato.Email = "testeup@testeup.com";
            contato.Telefone = "0000-0000";

            SvcContato.EditContato(contato);

            var contatoEditado = SvcContato.BuscarContatoPorId(contato.Id);

            Assert.IsNotNull(contatoEditado, "Contato não encontrado");
            Assert.AreEqual("Teste Atualizado", contatoEditado.Nome);
            Assert.AreEqual("testeup@testeup.com", contatoEditado.Email);
            Assert.AreEqual("0000-0000", contatoEditado.Telefone);
        }
    }
}
