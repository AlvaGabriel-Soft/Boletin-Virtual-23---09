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
    public partial class Boletin : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Boletin"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMaterias();
            }
        }

        protected void GridViewBoletin_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewBoletin.EditIndex = e.NewEditIndex;
            CargarGridViewBoletin(); // recarga con el row en modo edición
        }

        protected void GridViewBoletin_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewBoletin.EditIndex = -1;
            CargarGridViewBoletin(); // recarga normal
        }

        protected void GridViewBoletin_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idAlumno = Convert.ToInt32(GridViewBoletin.DataKeys[e.RowIndex].Value);
            string idMateria = ddlMaterias.SelectedValue;

            GridViewRow row = GridViewBoletin.Rows[e.RowIndex];

            string nota = ((TextBox)row.FindControl("txtNota")).Text;
            string nota2 = ((TextBox)row.FindControl("txtNota2")).Text;
            string notaFinal = ((TextBox)row.FindControl("txtNotaFinal")).Text;

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string update = @"UPDATE Cursada 
                          SET nota = @nota, nota2 = @nota2, nota_Final = @notaFinal
                          WHERE id_alumno = @idAlumno AND id_materia = @idMateria";

                using (SqlCommand cmd = new SqlCommand(update, conexion))
                {
                    cmd.Parameters.AddWithValue("@nota", string.IsNullOrEmpty(nota) ? (object)DBNull.Value : nota);
                    cmd.Parameters.AddWithValue("@nota2", string.IsNullOrEmpty(nota2) ? (object)DBNull.Value : nota2);
                    cmd.Parameters.AddWithValue("@notaFinal", string.IsNullOrEmpty(notaFinal) ? (object)DBNull.Value : notaFinal);
                    cmd.Parameters.AddWithValue("@idAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@idMateria", idMateria);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            GridViewBoletin.EditIndex = -1;
            CargarGridViewBoletin();
        }



        protected void BtnBuscar_Click(object sender, EventArgs e)
        {                   
            CargarGridViewBoletin();
        }

        private void CargarGridViewBoletin()
        {
            if (Session["UsuarioID"] != null)
            {
                int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
                string valor = ddlMaterias.SelectedValue;

                // Validar que se haya seleccionado una materia
                if (string.IsNullOrEmpty(valor) || valor == "0")
                {
                    // Limpiar GridView o mostrar mensaje
                    GridViewBoletin.DataSource = null;
                    GridViewBoletin.DataBind();
                    // opcional: mostrar mensaje al usuario
                    lblMensaje.Text = "Por favor seleccione una materia.";
                    return;
                }

                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    string query = @"SELECT 
                a.id_alumno,
                ua.nombre + ' ' + ua.apellido AS alumno,
                m.nombre_materia,
                c.nota,
                c.nota2,
                c.nota_Final,
                c.estado
            FROM dbo.Cursada c
            INNER JOIN dbo.Materia m ON m.id_materia = c.id_materia
            INNER JOIN dbo.Alumno a ON a.id_alumno = c.id_alumno
            INNER JOIN dbo.Usuarios ua ON ua.id_usuario = a.id_usuario
            INNER JOIN dbo.Profesor p ON p.id_profesor = c.id_profesor
            INNER JOIN dbo.Usuarios up ON up.id_usuario = p.id_usuario
            WHERE up.id_usuario = @idUsuario
              AND m.id_materia = @idMateria;";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@idMateria", valor);

                        conexion.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        GridViewBoletin.DataKeyNames = new string[] { "id_alumno" };  // ← clave primaria para identificar la fila
                        GridViewBoletin.DataSource = dt;
                        GridViewBoletin.DataBind();
                    }
                }
            }
        }


     private void CargarMaterias()
    {

         if (!IsPostBack)
            {
                if (Session["UsuarioID"] != null)
                {
                    int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

                    using (SqlConnection conexion = new SqlConnection(Cadena))
                    {
                        string query = "select m.id_materia, m.nombre_materia from dbo.Materia m inner join Profesor_Materia pm on pm.id_materia = m.id_materia inner join Profesor p on p.id_profesor = pm.id_profesor inner join Usuarios u on u.id_usuario = p.id_usuario where u.id_usuario = @id";

                        using (SqlCommand cmd = new SqlCommand(query, conexion))
                        {
                     
                            conexion.Open();

                           cmd.Parameters.AddWithValue("@id", idUsuario);
                           ddlMaterias.DataSource = cmd.ExecuteReader();
                           ddlMaterias.DataTextField = "nombre_materia";  // lo que se muestra en el combo
                           ddlMaterias.DataValueField = "id_materia";     // el valor interno
                           ddlMaterias.DataBind();
                        }
                    }
                    ddlMaterias.Items.Insert(0, new ListItem("-- Seleccione una materia --", "0"));

                  }


            }
     }
     }
}