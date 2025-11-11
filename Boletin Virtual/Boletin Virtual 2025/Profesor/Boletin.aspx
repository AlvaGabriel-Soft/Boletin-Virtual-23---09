<%@ Page Title="" Language="C#" MasterPageFile="~/Profesor/MenuProfesor.Master" AutoEventWireup="true" CodeBehind="Boletin.aspx.cs" Inherits="Boletin_Virtual_2025.Profesor.Boletin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="formbold-main-wrapper">
        <div class="formbold-form-wrapper-profesorNotas">
            
            
            <div class="formbold-form-title"> 
    <h2 class="">Mis Datos</h2>
</div> 

<div class="formbold-mb-3">
        <asp:label  runat="server" for="lastname" class="formbold-form-label">Buscar Materia</asp:label> 


        <asp:DropDownList ID="ddlMaterias" runat="server" class="formbold-form-label" 
            >
        </asp:DropDownList>

        <asp:DropDownList ID="ddlAnios" runat="server" AutoPostBack="true" 
    >
</asp:DropDownList>

    <asp:Button ID="BtnBuscar" runat="server" Text="Button" OnClick="BtnBuscar_Click" />



     <asp:label ID="lblMensaje"  runat="server" for="lastname" class="formbold-form-label"></asp:label> 

   <asp:GridView ID="GridViewBoletin" runat="server" AutoGenerateColumns="False"
    AutoGenerateEditButton="True" 
    OnRowEditing="GridViewBoletin_RowEditing"
    OnRowCancelingEdit="GridViewBoletin_RowCancelingEdit"
    OnRowUpdating="GridViewBoletin_RowUpdating"
    DataKeyNames="id_alumno"
        CssClass="tabla-materias">


    <Columns>   

        <asp:BoundField DataField="id_alumno" HeaderText="ID Alumno" ReadOnly="True" />
        <asp:BoundField DataField="alumno" HeaderText="Alumno" ReadOnly="True" />
        <asp:BoundField DataField="nombre_materia" HeaderText="Materia" ReadOnly="True" />

       <asp:TemplateField HeaderText="Nota">
    <ItemTemplate>
        <asp:Literal ID="litNota" runat="server" Text='<%# FormatearNota(Eval("nota")) %>'></asp:Literal>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtNota" runat="server" Text='<%# Bind("nota") %>' />
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Nota 2">
    <ItemTemplate>
        <asp:Literal ID="litNota2" runat="server" Text='<%# FormatearNota(Eval("nota2")) %>'></asp:Literal>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtNota2" runat="server" Text='<%# Bind("nota2") %>' />
    </EditItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Nota Final">
    <ItemTemplate>
        <asp:Literal ID="litNotaFinal" runat="server" Text='<%# FormatearNota(Eval("nota_Final")) %>'></asp:Literal>
    </ItemTemplate>
    <EditItemTemplate>
        <asp:TextBox ID="txtNotaFinal" runat="server" Text='<%# Bind("nota_Final") %>' />
    </EditItemTemplate>
</asp:TemplateField>

        <asp:BoundField DataField="estado" HeaderText="Estado" ReadOnly="True" />

    </Columns>
</asp:GridView>




    <Columns>
        <asp:BoundField DataField="alumno" HeaderText="Alumno" />        
        <asp:BoundField DataField="nota" HeaderText="Nota 1" />
        <asp:BoundField DataField="nota2" HeaderText="Nota 2" />
        <asp:BoundField DataField="nota_Final" HeaderText="Nota Final" />
        <asp:BoundField DataField="Estado" HeaderText="Estado" />
    </Columns>
</asp:GridView>



 </div>




</div>
</asp:Content>