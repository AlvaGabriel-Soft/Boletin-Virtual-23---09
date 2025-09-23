<%@ Page Title="" Language="C#" MasterPageFile="~/moderador/MenuModerador.Master" AutoEventWireup="true" CodeBehind="AgregarAlumno.aspx.cs" Inherits="Boletin_Virtual_2025.AgregarAlumno" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="formbold-main-wrapper">
        <div class="formbold-form-wrapper">
            
            
            <div class="formbold-form-title">
                <h2 class="">Registro de Alumnos</h2>

                <p>Ingresar datos del Alumno</p>
            </div> 

             <div class="formbold-input-flex">
                <div>
                    <label for="firstname" class="formbold-form-label">Primer nombre</label>
                    <asp:TextBox ID="primernombre" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
                <div >
                    <label for="lastname" class="formbold-form-label">Apellido</label>
                    <asp:TextBox ID="apellido" runat="server" CssClass="formbold-form-input"></asp:TextBox>
                </div>
            </div>

            <div class="formbold-mb-3">
                <label for="address" class="formbold-form-label">DNI</label>
                <asp:TextBox ID="dni" runat="server" CssClass="formbold-form-input"></asp:TextBox>
            </div>

            <div class="formbold-mb-3">
                <label for="address2" class="formbold-form-label">Email</label>
                <asp:TextBox ID="email" runat="server" CssClass="formbold-form-input"></asp:TextBox>
            </div>
             <div class="formbold-mb-3">
                <label  class="formbold-form-label">Seleccionar la carrera</label>
          <asp:DropDownList ID="carreras" runat="server" CssClass="formbold-form-input" Visible="true"  >
</asp:DropDownList>
            </div>
                 <div class="formbold-mb-3">
                <label  class="formbold-form-label">Legajo</label>
                <asp:TextBox ID="legajo" runat="server" CssClass="formbold-form-input"></asp:TextBox>
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

              <!-- 🔹 BOTÓN PARA MOSTRAR POPUP -->
            <div class="formbold-checkbox-wrapper">
               <button type="button"
        class="formbold-btn"
        onclick="mostrarPopupRegistro(
            '<%= primernombre.ClientID %>',
            '<%= apellido.ClientID %>',
            '<%= dni.ClientID %>',
            '<%= email.ClientID %>',
            '<%= carreras.ClientID %>',
            '<%= legajo.ClientID %>'
        )">
    Revisar datos
</button>
            </div>
            <asp:Label ID="Label1" runat="server" EnableViewState="false" />
        </div>

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
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" OnClick="btnRegister_Click" CssClass="formbold-btn" />
                <button type="button" class="close-btn" onclick="cerrarPopup()">Cancelar</button>
            </div>
        </div>
    </div>
</asp:Content>