using Agenda.Data;
using Agenda.Mdl;
using Agenda.Svc;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Agenda.Web
{
    public partial class weUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarUsuarios();
            }
        }

        private void CarregarUsuarios()
        {
            List<Usuario> lista = SvcUsuario.ListarUsuario();
            ListViewUsuario.DataSource = lista;
            ListViewUsuario.DataBind();
        }

        protected void ListViewUsuario_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            string email = ((TextBox)e.Item.FindControl("emailTextBox")).Text;
            string nome = ((TextBox)e.Item.FindControl("nomeTextBox")).Text;
            string senha = ((TextBox)e.Item.FindControl("senhaTextBox")).Text;

            Usuario u = new Usuario { Email = email, Nome = nome, Senha = senha };
            SvcUsuario.AddUsuario(u);

            CarregarUsuarios();
        }

        protected void ListViewUsuario_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListViewUsuario.EditIndex = e.NewEditIndex;
            CarregarUsuarios();
        }

        protected void ListViewUsuario_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListViewUsuario.EditIndex = -1;
            CarregarUsuarios();
        }

        protected void ListViewUsuario_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            string emailOriginal = ListViewUsuario.DataKeys[e.ItemIndex].Value.ToString();

            ListViewDataItem item = ListViewUsuario.Items[e.ItemIndex];
            string nome = ((TextBox)item.FindControl("nomeTextBox")).Text;
            string senha = ((TextBox)item.FindControl("senhaTextBox")).Text;

            Usuario usuario = new Usuario
            {
                Email = emailOriginal,
                Nome = nome,
                Senha = senha
            };

            SvcUsuario.EditUsuario(usuario);

            ListViewUsuario.EditIndex = -1;
            CarregarUsuarios();
        }


        protected void ListViewUsuario_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            string email = ((Label)ListViewUsuario.Items[e.ItemIndex].FindControl("emailLabel")).Text;
            SvcUsuario.DeleteUsuario(email);
            CarregarUsuarios();
        }



    }
}