using Agenda.Mdl;
using Agenda.Svc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Tst
{
    [TestClass]
    public class SvcContatoTst
    {
        [TestMethod]
        public void TesteListar()
        {
            List<Contato> lista = new List<Contato>();

            Contato contato = new Contato
            {
                Id = 1,
                Email = "arthur@gmail",
                Nome = "Arthur",
                Telefone = "4445-6254"
            };            

            lista.Add(contato);
            
            var resultado = SvcContato.ListarContato();
            
            Assert.IsTrue(resultado.Count > 0);

            Assert.AreEqual(lista[0].Id, resultado[0].Id);
            Assert.AreEqual(lista[0].Email, resultado[0].Email);
            Assert.AreEqual(lista[0].Nome, resultado[0].Nome);
            Assert.AreEqual(lista[0].Telefone, resultado[0].Telefone);

        }
    }
}
