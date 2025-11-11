using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace Boletin_Virtual_2025.Profesor
{
    public partial class MisDatos : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Boletin"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)


               {
            if (Session["UserRol"] == null)
            {
                // No hay sesión activa → redirigir al login
                Response.Redirect("../Login2.aspx");

            }
            else
            {
                int rol = Convert.ToInt32(Session["UserRol"]);

                // Validar acceso según el rol
                if (rol != 2) // Solo los alumnos (rol = 2)
                {
                    Session.Clear();     // Borra todas las variables de sesión
                    Session.Abandon();   // Marca la sesión como terminada
                    Response.Redirect("../Login2.aspx");
                }
                else
                {
                    // Si el rol es correcto, se ejecuta el contenido de la página
                    if (!IsPostBack)
                    {
                        TraerDatos();
            LlenarGridViewMateriasProfesor(); // Solo se carga la primera vez
                    }
                }
            }
        }


        private void TraerDatos()
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioID"] != null)
                {
                    int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        string query = "SELECT u.nombre, u.apellido, u.dni, p.titulo, u.email, p.especialidad FROM dbo.Usuarios u INNER JOIN dbo.Profesor p ON u.id_Usuario = p.id_usuario WHERE u.id_usuario = @id";
                        SqlCommand cmd = new SqlCommand(query, conexion);
                        cmd.Parameters.AddWithValue("@id", idUsuario);
                        conexion.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblPrimerNombre.Text = "Nombre: " + reader["nombre"].ToString();
                            lblApellido.Text = "Apellido: " + reader["apellido"].ToString();
                            lblDni.Text = "Dni: " + reader["dni"].ToString();

                            lblEmail.Text = "email: " + reader["email"].ToString();
                            lblEspecialidad.Text = "especialidad: " + reader["especialidad"].ToString();
                            lblTitulo.Text = "Titulo: " + reader["titulo"].ToString();
                        }
                    }
                }
                else
                {
                    Response.Redirect("../Login.aspx"); // si no hay sesión, volver al login
                }
            }
        }


        private void LlenarGridViewMateriasProfesor()
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioID"] != null)
                {
                    int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        string query = "SELECT u.nombre + ' ' + u.apellido AS NombreProfesor, m.nombre_materia AS NombreMateria FROM Profesor_Materia pm INNER JOIN Profesor p ON pm.id_profesor = p.id_profesor INNER JOIN Usuarios u ON p.id_usuario = u.id_usuario INNER JOIN Materia m ON pm.id_materia = m.id_materia WHERE p.id_usuario = @id";

                        using (SqlCommand cmd = new SqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@id", idUsuario);

                            conexion.Open();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            GridViewMaterias.DataSource = dt;
                            GridViewMaterias.DataBind();
                        }
                    }
                }
            }
        }

        }
    }

