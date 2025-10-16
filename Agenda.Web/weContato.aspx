<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePrincipal.Master" AutoEventWireup="true" CodeBehind="weContato.aspx.cs" Inherits="Agenda.Web.weContato" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:Label ID="Label2" runat="server" Text="Label">Inserir novo Contato</asp:Label>
    <br />
    <asp:Label ID="NomeLabel" runat="server" Text="Label">Nome</asp:Label>
    <br />
    <asp:TextBox ID="NomeTextBox" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="EmailLabel" runat="server" Text="Label">Email</asp:Label>
    <br />
    <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="TelefoneLabel" runat="server" Text="Label">Telefone</asp:Label>
    <br />
    <asp:TextBox ID="TelefoneTextBox" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="Button1" runat="server" Text="Salvar" CommandName="Salvar" OnClick="Button1_Click" />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Label">Lista de Contato</asp:Label>
    <asp:GridView ID="GridViewContato" runat="server"
    DataKeyNames="Id"
    AutoGenerateColumns="False"
    OnRowEditing="GridViewContato_RowEditing"
    OnRowCancelingEdit="GridViewContato_RowCancelingEdit"
    OnRowUpdating="GridViewContato_RowUpdating"
    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
    CellPadding="3" ForeColor="Black" GridLines="Vertical">
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:CommandField ShowDeleteButton="true" ShowEditButton="true" ShowSelectButton="true" />
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="true" />
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="" />
            <asp:BoundField DataField="Telefone" HeaderText="Telefone" SortExpression="" />
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#808080" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#383838" />
    </asp:GridView>
</asp:Content>
