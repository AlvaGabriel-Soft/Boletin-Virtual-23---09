using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Boletin_Virtual_2025.moderador
{
    public partial class AsignarProfesorMateria : System.Web.UI.Page
    {
        private static readonly string Cadena = ConfigurationManager.ConnectionStrings["Boletin"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LimpiarCampos();
            }
        }

        // Búsqueda por DNI
        protected void DniBusqueda_TextChanged(object sender, EventArgs e)
        {
            string busqueda = DniBusqueda.Text.Trim();

            if (string.IsNullOrEmpty(busqueda))
            {
                LimpiarCampos();
                return;
            }

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string query = @"SELECT TOP 1 p.id_profesor, u.nombre, u.apellido
                                 FROM Profesor p
                                 INNER JOIN Usuarios u ON p.id_usuario = u.id_usuario
                                 WHERE u.dni = @dni";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.Add("@dni", SqlDbType.VarChar, 50).Value = busqueda;
                    conexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Nombre.Text = dr["nombre"].ToString();
                            Apellido.Text = dr["apellido"].ToString();
                            hiddenIdProfesor.Value = dr["id_profesor"].ToString();

                            lblMensaje.Text = "Profesor encontrado.";
                            lblMensaje.ForeColor = System.Drawing.Color.Green;

                            CargarMaterias(int.Parse(hiddenIdProfesor.Value));
                            btnAsignarMaterias.Enabled = true;
                        }
                        else
                        {
                            LimpiarCampos();
                            lblMensaje.Text = "No se encontró el profesor.";
                            lblMensaje.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }

        // Carga las materias, marcando las ya asignadas
        private void CargarMaterias(int idProfesor)
        {
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                string query = @"
                    SELECT m.id_materia, m.nombre_materia, c.nombre_carrera,
                           CASE WHEN pm.id_profesor IS NOT NULL THEN 1 ELSE 0 END AS Asignado
                    FROM Materia m
                    INNER JOIN Carrera c ON m.id_carrera = c.id_carrera
                    LEFT JOIN Profesor_Materia pm ON pm.id_materia = m.id_materia AND pm.id_profesor = @idProfesor";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.Add("@idProfesor", SqlDbType.Int).Value = idProfesor;
                    conexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        gvMaterias.DataSource = dr;
                        gvMaterias.DataBind();
                    }
                }
            }
        }

        // Preselecciona checkboxes y colores en el GridView
        protected void gvMaterias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chkSeleccionar");
                bool asignado = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Asignado"));
                chk.Checked = asignado;

                if (asignado)
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        // Asigna materias (agrega nuevas y elimina desmarcadas)
        protected void btnAsignarMaterias_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hiddenIdProfesor.Value))
            {
                lblMensaje.Text = "Seleccione un profesor antes de asignar materias.";
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int idProfesor = int.Parse(hiddenIdProfesor.Value);

            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
                conexion.Open();
                SqlTransaction trans = conexion.BeginTransaction();

                try
                {
                    // Primero eliminamos las asignaciones que fueron desmarcadas
                    string deleteQuery = @"
                        DELETE FROM ProfesorMateria
                        WHERE id_profesor = @idProfesor
                        AND id_materia NOT IN (
                            SELECT id_materia FROM @tempTable
                        );";

                    // Creamos DataTable temporal para los checkboxes marcados
                    DataTable dt = new DataTable();
                    dt.Columns.Add("id_materia", typeof(int));

                    foreach (GridViewRow row in gvMaterias.Rows)
                    {
                        CheckBox chk = (CheckBox)row.FindControl("chkSeleccionar");
                        if (chk != null && chk.Checked)
                        {
                            int idMateria = Convert.ToInt32(gvMaterias.DataKeys[row.RowIndex].Value);
                            dt.Rows.Add(idMateria);
                        }
                    }

                    // Eliminar todas las que no están marcadas
                    foreach (GridViewRow row in gvMaterias.Rows)
                    {
                        CheckBox chk = (CheckBox)row.FindControl("chkSeleccionar");
                        int idMateria = Convert.ToInt32(gvMaterias.DataKeys[row.RowIndex].Value);

                        if (chk != null && chk.Checked)
                        {
                            // Insertar solo si no existe
                            string insertQuery = @"
                                IF NOT EXISTS (SELECT 1 FROM Profesor_Materia WHERE id_profesor = @idProfesor AND id_materia = @idMateria)
                                INSERT INTO Profesor_Materia (id_profesor, id_materia) VALUES (@idProfesor, @idMateria);";

                            using (SqlCommand cmd = new SqlCommand(insertQuery, conexion, trans))
                            {
                                cmd.Parameters.Add("@idProfesor", SqlDbType.Int).Value = idProfesor;
                                cmd.Parameters.Add("@idMateria", SqlDbType.Int).Value = idMateria;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Eliminar si está desmarcado
                            string deleteSingle = @"DELETE FROM Profesor_Materia WHERE id_profesor = @idProfesor AND id_materia = @idMateria";
                            using (SqlCommand cmd = new SqlCommand(deleteSingle, conexion, trans))
                            {
                                cmd.Parameters.Add("@idProfesor", SqlDbType.Int).Value = idProfesor;
                                cmd.Parameters.Add("@idMateria", SqlDbType.Int).Value = idMateria;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    trans.Commit();
                    lblMensaje.Text = "Asignaciones actualizadas correctamente.";
                    lblMensaje.ForeColor = System.Drawing.Color.Green;

                    // Recargar materias con los cambios
                    CargarMaterias(idProfesor);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMensaje.Text = "Error al asignar materias: " + ex.Message;
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void LimpiarCampos()
        {
            Nombre.Text = "";
            Apellido.Text = "";
            hiddenIdProfesor.Value = "";
            btnAsignarMaterias.Enabled = false;
            gvMaterias.DataSource = null;
            gvMaterias.DataBind();
        }
    }
}