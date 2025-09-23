<%@ Page Title="" Language="C#" MasterPageFile="~/Profesor/MenuProfesor.Master" AutoEventWireup="true" CodeBehind="MisDatos.aspx.cs" Inherits="Boletin_Virtual_2025.Profesor.MisDatos" %>
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
        <asp:label ID="lblApellido" runat="server" for="lastname" class="formbold-form-label">Apellido</asp:label> 
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
    <asp:label ID="lblEspecialidad" runat="server" for="Especialidad" class="formbold-form-label">Especialidad</asp:label>
</div>
<div class="formbold-mb-3">
    <asp:label ID="lblTitulo" runat="server" for="Titulo" class="formbold-form-label">Titulo</asp:label>
</div>

            <!--<div class="formbold-input-flex">
                <div>
                    <label for="state" class="formbold-form-label">State/Prvince</label>
                    <asp:TextBox ID="state" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
                <div>
                    <label for="country" class="formbold-form-label">Country</label>
                    <asp:TextBox ID="country" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
            </div>

            <div class="formbold-input-flex">
                <div>
                    <label for="post" class="formbold-form-label">Post/Zip code</label>
                    <asp:TextBox ID="post" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
                <div>
                    <label for="area" class="formbold-form-label">Area Code</label>
                    <asp:TextBox ID="area" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
            </div>     -->

            
            
            <asp:Label ID="Label1" runat="server" EnableViewState="false" />
        </div>



 <div class="formbold-form-title"> 
    <p>Mis materias</p>
</div> 

    <asp:GridView ID="GridViewMaterias" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="NombreProfesor" HeaderText="Profesor" />
        <asp:BoundField DataField="NombreMateria" HeaderText="Materia" />
    </Columns>
</asp:GridView>





        <!-- 🔹 POPUP -->
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