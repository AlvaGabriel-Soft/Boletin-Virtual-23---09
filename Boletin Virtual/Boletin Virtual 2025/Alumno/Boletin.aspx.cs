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
            
            CargarGridViewBoletin();
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
                        string query = @"SELECT 
    m.nombre_materia,
    ISNULL(mp.id_prerequisito, 0) AS id_prerequisito,
    ISNULL(mr.nombre_materia, 'Sin requisito') AS materia_requisito,
    ISNULL(CAST(c.nota AS VARCHAR), '-') AS nota,
    ISNULL(CAST(c.nota2 AS VARCHAR), '-') AS nota2,
    ISNULL(CAST(c.nota_Final AS VARCHAR), '-') AS nota_Final,
    ISNULL(c.estado, 'No inscripto') AS estado
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
WHERE u.id_usuario = @id;";

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



        private void CargarGridView()
    
{
    string query = @"
    SELECT 
        m.nombre_materia, 
        U.nombre AS profesor_nombre, 
        U.apellido AS profesor_apellido, 
        c.nota, 
        c.nota2, 
        c.nota_Final, 
        c.Estado
    FROM dbo.Cursada c
    INNER JOIN dbo.Materia m ON m.id_materia = c.id_materia
    LEFT JOIN dbo.Profesor p ON p.id_profesor = c.id_profesor
    LEFT JOIN dbo.Usuarios U ON U.id_usuario = p.id_usuario
    WHERE c.id_alumno = (
        SELECT a.id_alumno
        FROM dbo.Alumno a
        WHERE a.id_usuario = 2023
    )";

    string conexionString = ConfigurationManager.ConnectionStrings["boletin"].ConnectionString;

    using (SqlConnection con = new SqlConnection(conexionString))
    {
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
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