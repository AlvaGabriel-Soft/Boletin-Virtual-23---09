using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace Boletin_Virtual_2025.moderador
{
    public partial class SubirArchivos : System.Web.UI.Page
    {
        private string connStr = @"Data Source=DESKTOP-N05KOOR\SQLEXPRESS01;Integrated Security=True;Initial Catalog=Boletin;";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // 🟡 BOTÓN PREVISUALIZAR
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (!fuCsv.HasFile)
            {
                lblMsg.Text = "⚠️ Selecciona un archivo CSV para previsualizar.";
                return;
            }

            try
            {
                DataTable dt = GetDataTableFromCsv(fuCsv.FileContent);
                gvPreview.DataSource = dt;
                gvPreview.DataBind();
                lblMsg.Text = "✅ Archivo previsualizado correctamente. Registros encontrados: " + dt.Rows.Count;
                ViewState["CSV_DATA"] = dt;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "❌ Error al previsualizar: " + ex.Message;
            }
        }

        // 🟢 BOTÓN CARGAR A BASE DE DATOS
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (ViewState["CSV_DATA"] == null)
            {
                lblMsg.Text = "⚠️ Primero previsualiza el archivo antes de cargarlo.";
                return;
            }

            DataTable dt = (DataTable)ViewState["CSV_DATA"];
            int cargados = 0;
            int fallidos = 0;
            string logErrores = "";

            foreach (DataRow fila in dt.Rows)
            {
                string nombre = fila["Nombre"].ToString().Trim();
                string apellido = fila["Apellido"].ToString().Trim();
                string dni = fila["DNI"].ToString().Trim();
                string email = fila["Email"].ToString().Trim();
                string carreraNombre = fila["Carrera"].ToString().Trim();
                string legajo = fila["Legajo"].ToString().Trim();

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(carreraNombre))
                {
                    fallidos++;
                    logErrores += "Fila incompleta (DNI: " + dni + ").<br/>";
                    continue;
                }

                try
                {
                    using (SqlConnection conexion = new SqlConnection(connStr))
                    {
                        conexion.Open();
                        SqlTransaction transaction = conexion.BeginTransaction();

                        try
                        {
                            // 🔍 Buscar id_carrera por nombre
                            int idCarrera = GetCarreraId(conexion, transaction, carreraNombre);

                            if (idCarrera == -1)
                            {
                                throw new Exception("Carrera '" + carreraNombre + "' no encontrada en la base de datos.");
                            }

                            // 1️⃣ Insertar en Usuarios
                            string queryUsuario = @"INSERT INTO dbo.Usuarios 
                                (nombre, apellido, email, password, fecha_creacion, id_rol, dni) 
                                OUTPUT INSERTED.id_usuario
                                VALUES (@nombre, @apellido, @correo, @password, @fecha, @rol, @dni);";

                            SqlCommand cmdUsuario = new SqlCommand(queryUsuario, conexion, transaction);
                            cmdUsuario.Parameters.AddWithValue("@nombre", nombre);
                            cmdUsuario.Parameters.AddWithValue("@apellido", apellido);
                            cmdUsuario.Parameters.AddWithValue("@correo", email);
                            cmdUsuario.Parameters.AddWithValue("@dni", dni);
                            cmdUsuario.Parameters.AddWithValue("@password", dni);
                            cmdUsuario.Parameters.AddWithValue("@rol", 3);
                            cmdUsuario.Parameters.AddWithValue("@fecha", DateTime.Now);

                            int idUsuario = Convert.ToInt32(cmdUsuario.ExecuteScalar());

                            // 2️⃣ Insertar en Alumno
                            string queryAlumno = @"INSERT INTO dbo.Alumno (id_usuario, id_carrera, legajo, fecha_ingreso)
                                OUTPUT INSERTED.id_alumno
                                VALUES (@id_usuario, @id_carrera, @legajo, @fecha_ingreso)";
                            SqlCommand cmdAlumno = new SqlCommand(queryAlumno, conexion, transaction);
                            cmdAlumno.Parameters.AddWithValue("@id_usuario", idUsuario);
                            cmdAlumno.Parameters.AddWithValue("@id_carrera", idCarrera);
                            cmdAlumno.Parameters.AddWithValue("@legajo", legajo);
                            cmdAlumno.Parameters.AddWithValue("@fecha_ingreso", DateTime.Now);

                            int idAlumno = Convert.ToInt32(cmdAlumno.ExecuteScalar());

                            // 3️⃣ Insertar en Alumno_Materia
                            string queryAlumnoMateria = @"
    INSERT INTO dbo.Alumno_Materia (id_alumno, id_materia, fecha_inscripcion)
    SELECT @id_alumno, m.id_materia, @fecha_inscripcion
    FROM dbo.Materia m
    INNER JOIN dbo.Materia_Prerequisito mp ON m.id_materia = mp.id_materia
    WHERE m.id_carrera = @id_carrera AND mp.id_prerequisito = 0;";


                            SqlCommand commandAlumnoMateria = new SqlCommand(queryAlumnoMateria, conexion, transaction);

                            commandAlumnoMateria.Parameters.AddWithValue("@id_alumno", idAlumno);
                            commandAlumnoMateria.Parameters.AddWithValue("@id_carrera", idCarrera);
                            commandAlumnoMateria.Parameters.AddWithValue("@fecha_inscripcion", DateTime.Now);
                            commandAlumnoMateria.ExecuteNonQuery();

                            // 4. Asignar alumno y profesor de la materia al Curso. 

                            string queryCursada = @"INSERT INTO dbo.Cursada (id_alumno, id_materia, id_profesor, fecha, estado) 
SELECT 
    @id_alumno, 
    m.id_materia, 
    pm.id_profesor,   
    @fecha_inicio, 
    @estado 
FROM dbo.Materia m
LEFT JOIN dbo.Materia_Prerequisito mp 
    ON m.id_materia = mp.id_materia
LEFT JOIN dbo.Profesor_Materia pm 
    ON pm.id_materia = m.id_materia
WHERE m.id_carrera = @id_carrera 
  AND ISNULL(mp.id_prerequisito, 0) = 0;";

                            SqlCommand commandCursada = new SqlCommand(queryCursada, conexion, transaction);

                            commandCursada.Parameters.AddWithValue("@id_alumno", idAlumno);
                            commandCursada.Parameters.AddWithValue("@id_carrera", idCarrera);
                            commandCursada.Parameters.AddWithValue("@fecha_inicio", DateTime.Now);
                            commandCursada.Parameters.AddWithValue("@estado", "Cursando");
                            commandCursada.ExecuteNonQuery();


                            transaction.Commit();
                            cargados++;
                        }
                        catch (Exception exFila)
                        {
                            transaction.Rollback();
                            fallidos++;
                            logErrores += "❌ Error con " + nombre + " " + apellido + " (DNI: " + dni + "): " + exFila.Message + "<br/>";
                        }
                    }
                }
                catch (Exception exGeneral)
                {
                    fallidos++;
                    logErrores += "⚠️ Error general con " + nombre + " " + apellido + " (DNI: " + dni + "): " + exGeneral.Message + "<br/>";
                }
            }

            lblMsg.Text = "✅ Carga finalizada.<br/>" +
                          "✔️ Cargados correctamente: " + cargados + "<br/>" +
                          "❌ Fallidos: " + fallidos + "<br/><br/>" +
                          logErrores;
        }

        // 🧩 Buscar el ID de carrera
        private int GetCarreraId(SqlConnection conexion, SqlTransaction trans, string nombreCarrera)
        {
            string query = "SELECT id_carrera FROM dbo.Carrera WHERE nombre_carrera = @nombre";
            SqlCommand cmd = new SqlCommand(query, conexion, trans);
            cmd.Parameters.AddWithValue("@nombre", nombreCarrera);

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                int id;
                if (int.TryParse(result.ToString(), out id))
                    return id;
            }

            return -1; // si no encontró o no pudo convertir
        }

        // 🧾 Leer CSV y convertirlo a DataTable
        private DataTable GetDataTableFromCsv(Stream csvStream)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Apellido");
            dt.Columns.Add("DNI");
            dt.Columns.Add("Email");
            dt.Columns.Add("Carrera");
            dt.Columns.Add("Legajo");

            using (StreamReader sr = new StreamReader(csvStream))
            {
                string header = sr.ReadLine(); // omitir encabezado
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length < 6) continue;

                    DataRow r = dt.NewRow();
                    r["Nombre"] = parts[0].Trim();
                    r["Apellido"] = parts[1].Trim();
                    r["DNI"] = parts[2].Trim();
                    r["Email"] = parts[3].Trim();
                    r["Carrera"] = parts[4].Trim();
                    r["Legajo"] = parts[5].Trim();
                    dt.Rows.Add(r);
                }
            }
            return dt;
        }
    }
}