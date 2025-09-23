function mostrarPopupRegistroProfesor(nombreId, apellidoId, dniId, emailId,) {
    try {
        var nombre = document.getElementById(nombreId).value;
        var apellido = document.getElementById(apellidoId).value;
        var dni = document.getElementById(dniId).value;
        var email = document.getElementById(emailId).value;
        // Rol fijo
        document.getElementById("popupRol").innerText = "Profesor";



        // Mostrar valores
        document.getElementById("popupNombre").innerText = nombre;
        document.getElementById("popupApellido").innerText = apellido;
        document.getElementById("popupDni").innerText = dni;
        document.getElementById("popupEmail").innerText = email;
       
        // Mostrar popup
        document.getElementById("popup").style.display = "flex";
    } catch (err) {
        alert("Error al abrir popup: " + err);
    }
}

function cerrarPopup() {
    var popup = document.getElementById("popup");
    if (popup) {
        popup.style.display = "none";
    }
}