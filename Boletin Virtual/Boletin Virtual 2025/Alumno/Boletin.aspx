<%@ Page Title="" Language="C#" MasterPageFile="~/Alumno/MenuAlumnos.Master" AutoEventWireup="true" CodeBehind="Boletin.aspx.cs" Inherits="Boletin_Virtual_2025.Alumno.Boletin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="formbold-main-wrapper">
        <div class="formbold-form-wrapper-boletin">
            
 <div class="formbold-form-title"> 
    <p>Mis materias</p>
</div> 
<asp:GridView ID="GridViewMateriasAlumno" runat="server" AutoGenerateColumns="False"
    CssClass="tabla-materias"
    GridLines="None">

    <Columns>
        <asp:BoundField DataField="nombre_materia" HeaderText="Materia" />
        <asp:BoundField DataField="materia_requisito" HeaderText="Requisito" />
        <asp:BoundField DataField="nota" HeaderText="Nota 1" />
        <asp:BoundField DataField="nota2" HeaderText="Nota 2" />
        <asp:BoundField DataField="nota_Final" HeaderText="Nota Final" />
        <asp:BoundField DataField="estado" HeaderText="Estado" />
    </Columns>
</asp:GridView>



         

            <asp:Label ID="lblMensaje" runat="server" Text="Label"></asp:Label>



</asp:Content>