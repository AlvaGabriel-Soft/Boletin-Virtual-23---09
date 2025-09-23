using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Boletin_Virtual_2025
{
    public partial class Login : System.Web.UI.MasterPage
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["boletin"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            string query = "SELECT id_usuario, id_rol FROM dbo.Usuarios WHERE email = @Email AND password = @Password";

            using (SqlConnection connection = new SqlConnection(Cadena))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Usuario encontrado
                        {
                            int idUsuario = Convert.ToInt32(reader["id_usuario"]); 
                            int rol = Convert.ToInt32(reader["id_rol"]);

                            // Guardamos sesión
                            Session["UsuarioID"] = idUsuario;   // lo guardamos para tener el id(Pero tambien podemos usar el email...)
                            Session["UserEmail"] = email;
                            Session["UserRol"] = rol;

                            // Redirigimos según rol
                            switch (rol)
                            {
                                case 1: // Admin
                                    Response.Redirect("Moderador/AgregarAlumno.aspx");
                                    break;
                                case 2: // Profesor
                                    Response.Redirect("Profesor/MisDatos.aspx");
                                    break;
                                case 3: // alumno
                                    Response.Redirect("Alumno/MisDatosAlumno.aspx");
                                    break;
                                default:
                                    Label1.Visible = true;
                                    Label1.Text = "Rol desconocido.";
                                    break;
                            }
                        }
                        else
                        {
                            Label1.Visible = true;
                            Label1.Text = "Email o contraseña incorrectos.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Label1.Visible = true;
                    Label1.Text = "Error de conexión: " + ex.Message;
                }
            }
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            // Verifica si el usuario ya existe primero
            if (ExiteUsuario(txtSignupEmail.Text))
            {
                Label2.Visible = true;
                Label2.Text = "Usuario existente, no se puede crear";
            }
            else
            {
               
                string query = "INSERT INTO dbo.Login (email, contraseña) VALUES (@email, @password)";


                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    SqlCommand command = new SqlCommand(query, conexion);

                    // Añade los parámetros y sus valores.
                    command.Parameters.AddWithValue("@email", txtSignupEmail.Text);
                    command.Parameters.AddWithValue("@password", txtSignupPassword.Text);

                    try
                    {
                        conexion.Open();
                        int filas = command.ExecuteNonQuery();

                        if (filas > 0)
                        {
                            Label2.Visible = true;
                            Label2.Text = "Usuario generado correctamente";
                        }

                        else
                        {
                            Label2.Visible = true;
                            Label2.Text = "Usuario no se pudo generar";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Es buena práctica manejar las excepciones
                        Label2.Text = "Ocurrió un error: " + ex.Message;
                    }
                }
            }
        }

        //SELECCION DE DATOS DE BASE DE DATOS
        //using (SqlConnection conexion = new SqlConnection(Cadena))
        //{
        //    string script = "SELECT * FROM USUARIO";

        //    conexion.Open();

        //    SqlCommand command = new SqlCommand(script, conexion);

        //    SqlDataReader reader = command.ExecuteReader();

        //    string id = String.Empty;
        //    string usuario = String.Empty;

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            id = reader.GetInt32(0).ToString();
        //            usuario = reader.GetString(1);
        //        }
        //    }

        //    lblMensaje.Text = "ID de usuario: " + id.ToString() + " - Usuario: " + usuario.Trim();

        //    reader.Close();
        //    conexion.Close();
        //}

        public bool ExiteUsuario(string email)
        {
            // Usa un bloque using para gestionar la conexión de forma segura.
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                // Usa un query con parámetros para evitar inyección SQL.
                string query = "SELECT COUNT(*) FROM dbo.Login WHERE email = @email";

                SqlCommand command = new SqlCommand(query, conexion);
                command.Parameters.AddWithValue("@email", email);

                try
                {
                    conexion.Open();
                    // ExecuteScalar devuelve el primer valor de la primera fila.
                    // Es ideal para consultas que devuelven un solo valor, como un conteo.
                    int count = (int)command.ExecuteScalar();

                    // Si el conteo es mayor a 0, significa que el usuario existe.
                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // Maneja la excepción si algo sale mal con la conexión o la consulta.
                    // Para depuración, es útil ver el mensaje de error.
                    // Por ahora, devolvemos 'false' ya que no se pudo verificar.
                    // Considera registrar este error.
                    return false;
                }
            }
        }
    }
}