<%@ Page Title="" Language="C#" MasterPageFile="~/Alumno/MenuAlumnos.Master" AutoEventWireup="true" CodeBehind="Inscripciones.aspx.cs" Inherits="Boletin_Virtual_2025.Alumno.Inscripciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">




<asp:Panel ID="pnlContenidoEspecialCerrado" runat="server" Visible="false">
    <div style="padding: 15px; background-color: #e9f5ff; border-radius: 8px;">
        <h3>🎓 ¡Período de Inscripción Cerrado!</h3>
        <p>Del 10 al 20 de noviembre podrás inscribirte a tus materias.</p>
    </div>
</asp:Panel>


<asp:Panel ID="pnlContenidoEspecial" runat="server" Visible="false">
    <div style="padding: 15px; background-color: #e9f5ff; border-radius: 8px;">
        <h3>🎓 ¡Período de Inscripción Abierto!</h3>
        <p>Del 10 al 20 de noviembre podrás inscribirte a tus materias.</p>
    </div>
</asp:Panel>

<asp:Label ID="lblMensaje" runat="server" 
                   CssClass="mensaje-estado"
                  >mensaje</asp:Label>


<asp:Panel ID="MostrarBoletin" runat="server" Visible="false">
    <div class="formbold-main-wrapper">
        <div class="formbold-form-wrapper-boletin">
            
 <div class="formbold-form-title"> 
    <p>Mis materias</p>
</div> 



<asp:GridView ID="GridViewMateriasAlumno" runat="server" AutoGenerateColumns="False"
    CssClass="tabla-materias"
    GridLines="None"
    DataKeyNames="id_materia"
     OnRowCommand="GridViewMateriasAlumno_RowCommand">

      <Columns>
       <asp:BoundField DataField="id_materia" HeaderText="ID" Visible="False" />
        <asp:BoundField DataField="nombre_materia" HeaderText="Materia" />
        <asp:BoundField DataField="estado" HeaderText="Estado" />
        <asp:TemplateField HeaderText="Acción">
    <ItemTemplate>
        <asp:Button ID="btnInscribirse" runat="server"
            Text='<%# Eval("estado").ToString() == "Cursando" ? "Inscripto" : "Inscribirse" %>'
            CommandName="Inscribirse"
            CommandArgument='<%# Eval("id_materia") %>'
           Enabled='<%# 
                        Eval("estado").ToString() != "Cursando" && 
                        Eval("estado").ToString() != "Aprobado" %>' />
    </ItemTemplate>
</asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Panel>

</asp:Content>
