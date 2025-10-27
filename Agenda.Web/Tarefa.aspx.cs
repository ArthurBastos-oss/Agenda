using Agenda.Mdl;
using Agenda.Svc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Agenda.Web
{
    public partial class Tarefa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                List<Contato> listaContato = SvcContato.ListarContato();
                ViewState["Contatos"] = listaContato;
                ViewState["QtDropDowns"] = 1;
                PrimeiroDropDown(listaContato);
            }
            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (ViewState["Contatos"] == null)
                ViewState["Contatos"] = SvcContato.ListarContato();

            if (ViewState["Contatos"] != null)
                RecriarDropDown();
        }

        private void PrimeiroDropDown(List<Contato> listaContato)
        {
            LstContatos.Controls.Clear();

            Label lbl = new Label { Text = "Contatos Relacionados:" };
            LstContatos.Controls.Add(lbl);
            LstContatos.Controls.Add(new LiteralControl("<br />"));

            DropDownList ddl = CriarDropDown("ddContatos_0", listaContato);
            LstContatos.Controls.Add(ddl);
            LstContatos.Controls.Add(new LiteralControl("<br/>"));

            Button rem = new Button
            {
                ID = "remContato",
                Text = "Remover Contato"
            };
        }

        private DropDownList CriarDropDown(string id, List<Contato> listaContato)
        {
            DropDownList ddl = new DropDownList();
            ddl.ID = id;
            ddl.DataSource = listaContato;
            ddl.DataTextField = "Nome";
            ddl.DataValueField = "Id";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Selecione", ""));
            return ddl;
        }

        private void RecriarDropDown()
        {
            List<Contato> listaContato = (List<Contato>)ViewState["Contatos"];
            int count = ViewState["QtDropDowns"] != null ? (int)ViewState["QtDropDowns"] : 1;

            Dictionary<string, string> valoresSelecionados = new Dictionary<string, string>();
            foreach (Control ctrl in LstContatos.Controls)
            {
                if (ctrl is DropDownList ddl)
                    valoresSelecionados[ddl.ID] = ddl.SelectedValue;
            }

            LstContatos.Controls.Clear();
            LstContatos.Controls.Add(new Label { Text = "Contatos relacionados:" });
            LstContatos.Controls.Add(new LiteralControl("<br/>"));

            for (int i = 0; i < count; i++)
            {
                string ddlId = $"ddContatos_{i}";
                DropDownList ddl = CriarDropDown(ddlId, listaContato);

                if (valoresSelecionados.ContainsKey(ddlId))
                    ddl.SelectedValue = valoresSelecionados[ddlId];

                LstContatos.Controls.Add(ddl);
                LstContatos.Controls.Add(new LiteralControl("<br/>"));
            }

            Button rem = new Button();
            rem.ID = "remContato";
            rem.Text = "Remover Contato";
            rem.Click += BtRemover_Click;
            LstContatos.Controls.Add(rem);

        }

        protected void BtAdicionar_Click(object sender, EventArgs e)
        {
            int count = ViewState["QtDropDowns"] != null ? (int)ViewState["QtDropDowns"] : 1;
            count++;
            ViewState["QtDropDowns"] = count;
            RecriarDropDown();
        }

        protected void BtRemover_Click(object sender, EventArgs e)
        {
            if (ViewState["QtDropDowns"] == null)
                return;

            int count = (int)ViewState["QtDropDowns"];
            if (count > 1)
                count--;

            ViewState["QtDropDowns"] = count;
            RecriarDropDown();
        }

        protected void BtSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var tarefa = new Mdl.Tarefa();
                tarefa.Descricao = TxtDesc.Text.Trim();
                tarefa.DataInicio = DateTime.Parse(DataIniBox.Text);
                tarefa.DataFim = DateTime.Parse(DataEndBox.Text);
                tarefa.Recorrencia = (Recorrencia)Enum.Parse(typeof(Recorrencia), ddlRecorrencia.SelectedValue);


                List<int> contatosSelecionados = new List<int>();
                foreach (Control ctrl in LstContatos.Controls)
                {
                    if (ctrl is DropDownList ddl && !string.IsNullOrEmpty(ddl.SelectedValue))
                        contatosSelecionados.Add(int.Parse(ddl.SelectedValue));
                }

                int idTarefa = SvcTarefa.AddTarefa(tarefa.DataInicio, tarefa.DataFim, tarefa.Descricao, tarefa.Recorrencia, contatosSelecionados);
                
                

                if (tarefa.Recorrencia != Recorrencia.Nenhuma)
                {
                    DateTime dataAtualInicio = tarefa.DataInicio;
                    DateTime dataAtualFim = tarefa.DataFim;

                    DateTime limite = tarefa.DataInicio.AddYears(5);

                    while (true)
                    {
                        switch (tarefa.Recorrencia)
                        {
                            case Recorrencia.Diaria:
                                dataAtualInicio = dataAtualInicio.AddDays(1);
                                dataAtualFim = dataAtualFim.AddDays(1);
                                break;

                            case Recorrencia.Semanal:
                                dataAtualInicio = dataAtualInicio.AddDays(7);
                                dataAtualFim = dataAtualFim.AddDays(7);
                                break;

                            case Recorrencia.Mensal:
                                dataAtualInicio = dataAtualInicio.AddMonths(1);
                                dataAtualFim = dataAtualFim.AddMonths(1);
                                break;

                            case Recorrencia.Anual:
                                dataAtualInicio = dataAtualInicio.AddYears(1);
                                dataAtualFim = dataAtualFim.AddYears(1);
                                break;
                        }

                        if (dataAtualInicio > limite)
                            break;

                        int novoIdTarefa = SvcTarefa.AddTarefa(dataAtualInicio, dataAtualFim, tarefa.Descricao, tarefa.Recorrencia, contatosSelecionados);

                        
                    }

                    Lmsg.Text = $"Tarefa cadastrada com recorrência ({tarefa.Recorrencia}).";
                }
                else
                {
                    Lmsg.Text = contatosSelecionados.Count > 0
                        ? "Tarefa cadastrada com contatos relacionados."
                        : "Tarefa cadastrada sem contatos relacionados.";
                }
            }
            catch (Exception ex)
            {
                Lmsg.Text = "Erro ao cadastrar tarefa: " + ex.Message;
            }
        }
    }
}