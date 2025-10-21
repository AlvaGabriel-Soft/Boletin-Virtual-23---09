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
    public partial class AgregarAlumno : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["boletin"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCarreras(); // Solo se carga la primera vez, no en postback
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (ExiteUsuario(dni.Text))
            {
                Response.Write("<script>alert('Usuario repetido');</script>");
            }
            else
            {
                using (SqlConnection conexion = new SqlConnection(Cadena))
                {
                    conexion.Open();
                    SqlTransaction transaction = conexion.BeginTransaction();

                    try
                    {
                        // 1. Insertar en Usuarios y obtener id generado
                        string queryUsuario = @"INSERT INTO dbo.Usuarios 
                                        (nombre, apellido, email, password, fecha_creacion, id_rol, dni) 
                                        OUTPUT INSERTED.id_usuario
                                        VALUES (@primernombre, @apellido, @email, @password, @fecha_registro, @rol, @dni);";

                        SqlCommand commandUsuario = new SqlCommand(queryUsuario, conexion, transaction);
                        commandUsuario.Parameters.AddWithValue("@primernombre", primernombre.Text);
                        commandUsuario.Parameters.AddWithValue("@apellido", apellido.Text);
                        commandUsuario.Parameters.AddWithValue("@email", email.Text);
                        commandUsuario.Parameters.AddWithValue("@dni", dni.Text);
                        commandUsuario.Parameters.AddWithValue("@password", dni.Text); 
                        commandUsuario.Parameters.AddWithValue("@rol", 3);
                        commandUsuario.Parameters.AddWithValue("@fecha_registro", DateTime.Now);

                        int idUsuario = Convert.ToInt32(commandUsuario.ExecuteScalar());

                        // 2. Insertar en Alumno usando el id_usuario recién generado
                        string queryAlumno = @"INSERT INTO dbo.Alumno (id_usuario, id_carrera, legajo, fecha_ingreso) OUTPUT INSERTED.id_alumno
                                       VALUES (@id_usuario, @id_carrera, @legajo, @fecha_ingreso)";

                        SqlCommand commandAlumno = new SqlCommand(queryAlumno, conexion, transaction);
                        commandAlumno.Parameters.AddWithValue("@id_usuario", idUsuario);
                        commandAlumno.Parameters.AddWithValue("@id_carrera", carreras.SelectedValue);
                        commandAlumno.Parameters.AddWithValue("@legajo", legajo.Text);
                        commandAlumno.Parameters.AddWithValue("@fecha_ingreso", DateTime.Now);

                        int idAlumno = Convert.ToInt32(commandAlumno.ExecuteScalar());


                        // 3. asignar Alumno a Materias dependiendo de la carrera

                        string queryAlumnoMateria = @"
    INSERT INTO dbo.Alumno_Materia (id_alumno, id_materia, fecha_inscripcion)
    SELECT @id_alumno, m.id_materia, @fecha_inscripcion
    FROM dbo.Materia m
    INNER JOIN dbo.Materia_Prerequisito mp ON m.id_materia = mp.id_materia
    WHERE m.id_carrera = @id_carrera AND mp.id_prerequisito = 0;";


                        SqlCommand commandAlumnoMateria = new SqlCommand(queryAlumnoMateria, conexion, transaction);

                        commandAlumnoMateria.Parameters.AddWithValue("@id_alumno", idAlumno);
                        commandAlumnoMateria.Parameters.AddWithValue("@id_carrera", carreras.SelectedValue);
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
                        commandCursada.Parameters.AddWithValue("@id_carrera", carreras.SelectedValue);
                        commandCursada.Parameters.AddWithValue("@fecha_inicio", DateTime.Now);
                        commandCursada.Parameters.AddWithValue("@estado", "Cursando" );
                        commandCursada.ExecuteNonQuery();

                       //
                        transaction.Commit();

                        Response.Write("<script>alert('Usuario y alumno creados correctamente');</script>");

                        primernombre.Text = "";
                        apellido.Text = "";
                        dni.Text = "";
                        email.Text = "";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                    }
                }
            }
        }

        public bool ExiteUsuario(string dni)
        {
            
            using (SqlConnection conexion = new SqlConnection(Cadena))
            {
               
                string query = "SELECT COUNT(*) FROM dbo.Usuarios WHERE dni = @dni";

                SqlCommand command = new SqlCommand(query, conexion);
                command.Parameters.AddWithValue("@dni", dni);

                try
                {
                    conexion.Open();
                   
                    int count = (int)command.ExecuteScalar();

                    
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
                   
                    return false;
                }
            }
        }

       
        private void CargarCarreras()
        {
            // tu cadena de conexión en Web.config con nombre "MiConexion"
            string connectionString = ConfigurationManager.ConnectionStrings["boletin"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT id_carrera, nombre_carrera FROM Carrera"; // ajustá a tu tabla real
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                carreras.DataSource = reader;
                carreras.DataTextField = "nombre_carrera";     
                carreras.DataValueField = "id_carrera"; 
                carreras.DataBind();

                reader.Close();
            }

            // opcional: agregar un ítem inicial
            carreras.Items.Insert(0, new ListItem("-- Seleccione una carrera --", "0"));
        }
    }





}
