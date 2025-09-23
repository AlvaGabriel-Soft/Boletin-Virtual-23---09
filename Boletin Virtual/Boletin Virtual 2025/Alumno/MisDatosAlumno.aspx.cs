using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace Boletin_Virtual_2025
{
    public partial class MenuDeAlumnos : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Boletin"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            TraerDatos();
            LlenarGridViewMateriasAlumno();
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
                        string query = "SELECT u.nombre, u.apellido, u.dni, a.legajo, u.email, c.nombre_carrera FROM dbo.Usuarios u INNER JOIN dbo.Alumno a ON u.id_usuario = a.id_usuario INNER JOIN dbo.carrera c ON a.id_carrera = c.id_carrera WHERE u.id_usuario = @id";
                        SqlCommand cmd = new SqlCommand(query, conexion);
                        cmd.Parameters.AddWithValue("@id", idUsuario);
                        conexion.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblPrimerNombre.Text = "nombre: " + reader["nombre"].ToString();
                            lblApellido.Text = reader["apellido"].ToString();
                            lblDni.Text = reader["dni"].ToString();

                            lblEmail.Text = reader["email"].ToString();
                            lblLegajo.Text = reader["legajo"].ToString();
                            lblCarrera.Text = reader["nombre_carrera"].ToString();
                        }
                    }
                }
                else
                {
                    Response.Redirect("../Login.aspx"); // si no hay sesión, volver al login
                }
            }
        }



        private void LlenarGridViewMateriasAlumno()
        {
            if (!IsPostBack)
            {
                if (Session["UsuarioID"] != null)
                {
                    int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        string query = "SELECT m.nombre_materia AS NombreMateria FROM Materia m INNER JOIN dbo.Alumno_Materia am ON m.id_materia = am.id_materia INNER JOIN dbo.Alumno a ON a.id_alumno = am.id_alumno WHERE id_usuario = @id  ";

                        using (SqlCommand cmd = new SqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@id", idUsuario);

                            conexion.Open();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            GridViewMateriasAlumno.DataSource = dt;
                            GridViewMateriasAlumno.DataBind();
                        }
                    }
                }
            }
        }


       

    }
}



