using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace Boletin_Virtual_2025.Alumno
{
    public partial class Boletin : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Boletin"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)

            {
            if (Session["UserRol"] == null)
            {
                // No hay sesión activa. redirigir al login
                Response.Redirect("../Login2.aspx");

            }
            else
            {
                int rol = Convert.ToInt32(Session["UserRol"]);

               
                if (rol != 3) // Solo los alumnos (rol = 3)
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
                       CargarGridViewBoletin(); // Solo se carga la primera vez
                    }
                }
            }
        }


        private void CargarGridViewBoletin()

       {
            {
            if (!IsPostBack)
            {
                if (Session["UsuarioID"] != null)
                {
                    int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        string query = @"
WITH MateriasConUltimaCursada AS (
    SELECT 
        m.id_materia,
        m.nombre_materia,
        ISNULL(mp.id_prerequisito, 0) AS id_prerequisito,
        ISNULL(mr.nombre_materia, 'Sin requisito') AS materia_requisito,
        ISNULL(CAST(c.nota AS VARCHAR), '-') AS nota,
        ISNULL(CAST(c.nota2 AS VARCHAR), '-') AS nota2,
        ISNULL(CAST(c.nota_Final AS VARCHAR), '-') AS nota_Final,
        ISNULL(c.estado, 'No inscripto') AS estado,
        ROW_NUMBER() OVER (PARTITION BY m.id_materia ORDER BY c.id_cursada DESC) AS rn
    FROM dbo.Materia m
    INNER JOIN dbo.Carrera ca ON ca.id_carrera = m.id_carrera
    INNER JOIN dbo.Alumno a ON a.id_carrera = ca.id_carrera
    INNER JOIN dbo.Usuarios u ON u.id_usuario = a.id_usuario
    LEFT JOIN dbo.Cursada c 
        ON c.id_materia = m.id_materia AND c.id_alumno = a.id_alumno
    LEFT JOIN dbo.Materia_Prerequisito mp 
        ON mp.id_materia = m.id_materia
    LEFT JOIN dbo.Materia mr 
        ON mr.id_materia = mp.id_prerequisito
    WHERE u.id_usuario = @id
)
SELECT *
FROM MateriasConUltimaCursada
WHERE rn = 1
ORDER BY id_materia;";

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
}