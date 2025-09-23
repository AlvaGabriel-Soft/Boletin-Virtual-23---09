<%@ Page Title="" Language="C#" MasterPageFile="~/moderador/MenuModerador.Master" AutoEventWireup="true" CodeBehind="AgregarProfesor.aspx.cs" Inherits="Boletin_Virtual_2025.moderador.AgregarProfesor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="formbold-main-wrapper">
        <div class="formbold-form-wrapper">
            
            
            <div class="formbold-form-title">
                <h2 class="">Registro de profesores</h2>

                <p>Ingresar datos de profesor</p>
            </div>

       
    


       <asp:DropDownList ID="materias" runat="server" CssClass="formbold-form-input" Visible="true" AutoPostBack="true">
</asp:DropDownList>

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
                <label for="titulo" class="formbold-form-label">titulo</label>
                <asp:TextBox ID="titulo" runat="server" CssClass="formbold-form-input"></asp:TextBox>
            </div>
             <div class="formbold-mb-3">
                <label for="especialidad" class="formbold-form-label">especialidad</label>
                <asp:TextBox ID="especialidad" runat="server" CssClass="formbold-form-input"></asp:TextBox>
            </div>
 

              <!-- 🔹 BOTÓN PARA MOSTRAR POPUP -->
            <div class="formbold-checkbox-wrapper">
               <button type="button"
        class="formbold-btn"
         onclick="mostrarPopupRegistroProfesor(
                '<%= primernombre.ClientID %>',
    '<%= apellido.ClientID %>',
    '<%= dni.ClientID %>',
    '<%= email.ClientID %>',
  
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
                <p><strong>Rol:</strong> <span id="popupRol"></span></p>
                          
                <p><strong>Nombre:</strong> <span id="popupNombre"></span></p>
                <p><strong>Apellido:</strong> <span id="popupApellido"></span></p>
                <p><strong>DNI:</strong> <span id="popupDni"></span></p>
                <p><strong>Email:</strong> <span id="popupEmail"></span></p>
                


                <asp:Label ID="lblConfirmacion" runat="server" 
                           Text="¿Estás seguro de que tus datos son correctos?"></asp:Label>
                <br /><br />
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar"  CssClass="formbold-btn" OnClick="btnRegister_Click" />
                <button type="button" class="close-btn" onclick="cerrarPopup()">Cancelar</button>
            </div>
        </div>
    </div>


</asp:Content>
