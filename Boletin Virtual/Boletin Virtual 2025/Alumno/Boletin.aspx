<%@ Page Title="" Language="C#" MasterPageFile="~/Alumno/MenuAlumnos.Master" AutoEventWireup="true" CodeBehind="Boletin.aspx.cs" Inherits="Boletin_Virtual_2025.Alumno.Boletin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">






</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="formbold-main-wrapper">
        <div class="formbold-form-wrapper">
            
            
            <div class="formbold-form-title"> 
    <h2 class="">Mis Datos</h2>
    <p>Personal</p>
</div> 
<div class="formbold-card">

<!-- Primer nombre + Apellido en la misma línea -->
<div class="formbold-input-flex" style="display: flex; gap: 20px;">
    <div class="formbold-mb-3">
       <asp:Label ID="lblPrimerNombre" runat="server" CssClass="formbold-form-label" >
    Primer nombre
</asp:Label>
        
    </div>
    <div class="formbold-mb-3">
        <asp:label ID="lblApellidoo" runat="server" for="lastname" class="formbold-form-label">Apellido</asp:label> 
    </div>
</div>

<!-- Los demás van uno abajo del otro -->
<div class="formbold-mb-3">
    <asp:label ID="lblDni" runat="server" for="dni" class="formbold-form-label">DNI</asp:label>
</div>

<div class="formbold-mb-3">
    <asp:label ID="lblEmail" runat="server" for="email" class="formbold-form-label">Email</asp:label>
   
</div>

<div class="formbold-mb-3">
    <asp:label ID="lblLegajo" runat="server" for="legajo" class="formbold-form-label">Legajo</asp:label>
   
</div>

<div class="formbold-mb-3">
    <asp:label ID="lblCarrera" runat="server" for="Titulo" class="formbold-form-label">Carrera</asp:label>
</div>

        
            
            <asp:Label ID="Label1" runat="server" EnableViewState="false" />
        </div>



 <div class="formbold-form-title"> 
    <p>Mis materias</p>
</div> 

<asp:GridView ID="GridViewMateriasAlumno" runat="server" AutoGenerateColumns="False"
    GridLines="None" AlternatingRowStyle-BackColor="#f2f2f2"
    HeaderStyle-BackColor="#4CAF50" HeaderStyle-ForeColor="White"
    RowStyle-BackColor="White" BorderStyle="Solid" BorderWidth="1px" BorderColor="#ddd">

    <Columns>
        <asp:BoundField DataField="nombre_materia" HeaderText="Materia" />
        <asp:BoundField DataField="materia_requisito" HeaderText="Requisito" />
        <asp:BoundField DataField="nota" HeaderText="Nota 1" />
        <asp:BoundField DataField="nota2" HeaderText="Nota 2" />
        <asp:BoundField DataField="nota_Final" HeaderText="Nota Final" />
        <asp:BoundField DataField="estado" HeaderText="Estado" />
    </Columns>
</asp:GridView>


        <!--  POPUP -->
        <div class="popup" id="popup">
            <div class="popup-content">
                <h3>Revisa tus datos</h3>
                <p><strong>Nombre:</strong> <span id="popupNombre"></span></p>
                <p><strong>Apellido:</strong> <span id="popupApellido"></span></p>
                <p><strong>DNI:</strong> <span id="popupDni"></span></p>
                <p><strong>Email:</strong> <span id="popupEmail"></span></p>
                <p id="popupCarreraContainer"><strong>Carrera:</strong> <span id="popupCarrera"></span></p>
                <p><strong>legajo:</strong> <span id="popupLegajo"></span></p>


                <asp:Label ID="lblConfirmacion" runat="server" 
                           Text="¿Estás seguro de que tus datos son correctos?"></asp:Label>
                <br /><br />
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar"  CssClass="formbold-btn" />
                <button type="button" class="close-btn" onclick="cerrarPopup()">Cancelar</button>
            </div>
        </div>
    </div>
</asp:Content>