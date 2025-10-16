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
    public partial class weContato : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarContato();
            }
        }

        private void CarregarContato()
        {
            List<Contato> lista = SvcContato.ListarContato();
            GridViewContato.DataSource = lista;
            GridViewContato.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Contato c = new Contato 
            {
                Nome = NomeTextBox.Text,
                Email = EmailTextBox.Text,
                Telefone = TelefoneTextBox.Text
            };
            SvcContato.AddContato(c);

            CarregarContato();
        }

        protected void GridViewContato_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contato c = new Contato
            {
                Nome = NomeTextBox.Text,
                Email = EmailTextBox.Text,
                Telefone = TelefoneTextBox.Text
            };
            SvcContato.EditContato(c);

            CarregarContato();
        }

        protected void GridViewContato_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewContato.EditIndex = e.NewEditIndex;
            CarregarContato(); // Recarrega os dados para exibir a linha em modo de edição
        }

        protected void GridViewContato_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewContato.EditIndex = -1;
            CarregarContato(); // Sai do modo de edição
        }

        protected void GridViewContato_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridViewContato.DataKeys[e.RowIndex].Value);

            GridViewRow row = GridViewContato.Rows[e.RowIndex];
            string nome = ((TextBox)row.Cells[2].Controls[0]).Text;
            string email = ((TextBox)row.Cells[3].Controls[0]).Text;
            string telefone = ((TextBox)row.Cells[4].Controls[0]).Text;

            Contato c = new Contato
            {
                Id = id,
                Nome = nome,
                Email = email,
                Telefone = telefone
            };

            SvcContato.EditContato(c);

            GridViewContato.EditIndex = -1;
            CarregarContato();
        }

    }
}