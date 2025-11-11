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
                       CargarMaterias(); // Solo se carga la primera vez
                       CargarAnios();
                    }
                }
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

            // ← Aquí va el bloque que calcula notaFinalDecimal y estado
            decimal? notaFinalDecimal = ConvertirDecimal(notaFinal);
            string estado;
            if (!notaFinalDecimal.HasValue)
            {
                estado = "Cursando";
            }
            else if (notaFinalDecimal.Value > 4)
            {
                estado = "Aprobado";
            }
            else
            {
                estado = "Desaprobado";
            }


            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string update = @"UPDATE Cursada 
                          SET nota = @nota, 
                              nota2 = @nota2, 
                              nota_Final = @notaFinal,
                              estado = @estado
                          WHERE id_alumno = @idAlumno AND id_materia = @idMateria";

                using (SqlCommand cmd = new SqlCommand(update, conexion))
                {
                    // ← Usamos la notaFinalDecimal y estado calculado
                    cmd.Parameters.AddWithValue("@nota", ConvertirDecimal(nota) ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nota2", ConvertirDecimal(nota2) ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@notaFinal", notaFinalDecimal ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@estado", estado);
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

        protected string FormatearNota(object valor)
        {
            if (valor == DBNull.Value || valor == null || string.IsNullOrWhiteSpace(valor.ToString()))
                return string.Empty;

            decimal nota;
            if (decimal.TryParse(valor.ToString(), out nota))
            {
                if (nota > 4)
                    return "<span style='color:green;'>" + nota.ToString() + " (A)</span>";  // aprobado
                else
                    return "<span style='color:red;'>" + nota.ToString() + " (D)</span>";    // desaprobado
            }

            return valor.ToString();
        }

        private void CargarGridViewBoletin()
        {
            if (Session["UsuarioID"] != null)
            {
                int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
                string valor = ddlMaterias.SelectedValue;
                string anioSeleccionado = ddlAnios.SelectedItem.Text;

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


                if (ddlAnios.SelectedIndex <= 0)
                {
                    lblMensaje.Text = "Por favor, seleccione un año.";
                    return;
                }

                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    string query = @" SELECT 
                    a.id_alumno,
                    ua.nombre + ' ' + ua.apellido AS alumno,
                    m.nombre_materia,
                    c.nota,
                    c.nota2,
                    c.nota_Final,
                    c.estado,
                    c.fecha
                FROM dbo.Cursada c
                INNER JOIN dbo.Materia m ON m.id_materia = c.id_materia
                INNER JOIN dbo.Alumno a ON a.id_alumno = c.id_alumno
                INNER JOIN dbo.Usuarios ua ON ua.id_usuario = a.id_usuario
                INNER JOIN dbo.Profesor p ON p.id_profesor = c.id_profesor
                INNER JOIN dbo.Usuarios up ON up.id_usuario = p.id_usuario
                WHERE up.id_usuario = @idUsuario
                  AND m.id_materia = @idMateria
                  AND YEAR(c.fecha) = @anio;";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@idMateria", valor);
                        cmd.Parameters.AddWithValue("@anio", Convert.ToInt32(anioSeleccionado));

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


        private decimal? ConvertirDecimal(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            valor = valor.Replace(",", "."); // reemplaza coma decimal por punto
            decimal resultado;
            if (decimal.TryParse(valor, System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.InvariantCulture, out resultado))
                return resultado;

            return null; // si no es un número válido
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


       
        private void CargarAnios()
        {
            if (!IsPostBack)
                {
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string query = "SELECT DISTINCT YEAR(fecha) AS Anio FROM Cursada ORDER BY Anio DESC";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    ddlAnios.Items.Clear();
                    
                    while (dr.Read())
                    {
                        int anio = Convert.ToInt32(dr["Anio"]);
                        ddlAnios.Items.Add(new ListItem(anio.ToString(), anio.ToString()));
                    }

                    dr.Close();
                }

                ddlAnios.Items.Insert(0, new ListItem("-- Seleccione un año --", "0"));
            }
        }
        }



     }
}