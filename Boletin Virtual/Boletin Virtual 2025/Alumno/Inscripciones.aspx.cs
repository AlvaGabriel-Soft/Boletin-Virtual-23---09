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
    public partial class Inscripciones : System.Web.UI.Page
    {

        private static string Cadena = ConfigurationManager.ConnectionStrings["Boletin"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
       {
            if (!IsPostBack){
        { 
           
            
                // Obtener la fecha actual (usa tu simulador si existe)
                DateTime fechaActual = SimularFecha.ObtenerFechaActual();

                // Definir el período de inscripción
                DateTime fechaInicio = new DateTime(2025, 11, 10);
                DateTime fechaFin = new DateTime(2025, 11, 20);

                // Reiniciar visibilidad
                pnlContenidoEspecial.Visible = false;
                pnlContenidoEspecialCerrado.Visible = false;
                MostrarBoletin.Visible = false;

                // Evaluar la fecha actual
                if (fechaActual < fechaInicio) 
                {
                    // Todavía no comenzó el período
                    pnlContenidoEspecialCerrado.Visible = true;
                }
                else if (fechaActual >= fechaInicio && fechaActual <= fechaFin)
                { 
                    // Período abierto: mostrar inscripción y boletín
                    pnlContenidoEspecial.Visible = true;
                    MostrarBoletin.Visible = true;
                    CargarGridViewBoletin();
                }
                else
                { 
                    // Ya terminó el período
                    pnlContenidoEspecialCerrado.Visible = true;

                }
             }
            }
       } 

        private void CargarGridViewBoletin()
        {
            
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
        


         private void MostrarPagina()
        {
            if (!IsPostBack)
            {
                DateTime fechaActual = SimularFecha.ObtenerFechaActual();

                DateTime fechaInicio = new DateTime(2025, 11, 10);
                DateTime fechaFin = new DateTime(2025, 11, 20);

                if (fechaActual >= fechaInicio && fechaActual <= fechaFin)
                {
                    pnlContenidoEspecial.Visible = true;  // Mostrar contenido
                }
                else
                {
                    pnlContenidoEspecial.Visible = false; // Ocultar contenido
                }
            }
        }


         protected void GridViewMateriasAlumno_RowCommand(object sender, GridViewCommandEventArgs e)
         {
             if (e.CommandName == "Inscribirse")
             {
                 int idMateria = Convert.ToInt32(e.CommandArgument);
                 int idAlumno = Convert.ToInt32(Session["UsuarioID"]);
                 InscribirAlumnoEnMateria(idAlumno, idMateria);
             }
         }


         private void InscribirAlumnoEnMateria(int idAlumno, int idMateria)
         {
             string connectionString = ConfigurationManager.ConnectionStrings["Boletin"].ConnectionString;

             try
             {
                 using (SqlConnection conn = new SqlConnection(connectionString))
                 {
                     conn.Open();
                     using (SqlTransaction transaction = conn.BeginTransaction())
                     {
                         try
                         {
                             // 1️⃣ Verificar prerequisitos
                             string sqlCheckPrereq = @"
                        SELECT COUNT(*) 
                        FROM Materia_Prerequisito mp
                        LEFT JOIN Cursada c 
                            ON c.id_materia = mp.id_prerequisito 
                           AND c.id_alumno = @idAlumno
                        WHERE mp.id_materia = @idMateria
                          AND (c.estado IS NULL OR c.estado != 'Aprobado')";

                             using (SqlCommand cmdCheck = new SqlCommand(sqlCheckPrereq, conn, transaction))
                             {
                                 cmdCheck.Parameters.AddWithValue("@idAlumno", idAlumno);
                                 cmdCheck.Parameters.AddWithValue("@idMateria", idMateria);
                                 int prereqPendientes = (int)cmdCheck.ExecuteScalar();

                                 if (prereqPendientes > 0)
                                 {
                                     lblMensaje.Visible = true;
                                     lblMensaje.Text = "❌ No puedes inscribirte: faltan prerequisitos.";
                                     transaction.Rollback();
                                     return;
                                 }
                             }

                             // 2️⃣ Insertar en Cursada (si no existe ya)
                             string sqlInsertCursada = @"
                        INSERT INTO Cursada (id_alumno, id_materia, estado, id_profesor)
                        SELECT @idAlumno, @idMateria, 'Cursando', MPF.id_profesor
                        FROM Materia m
                        LEFT JOIN Profesor_Materia MPF ON MPF.id_materia = m.id_materia
                        WHERE m.id_materia = @idMateria
                          AND NOT EXISTS (
                              SELECT 1 FROM Cursada c
                              WHERE c.id_alumno = @idAlumno
                                AND c.id_materia = @idMateria
                          );";

                             using (SqlCommand cmdInsert = new SqlCommand(sqlInsertCursada, conn, transaction))
                             {
                                 cmdInsert.Parameters.AddWithValue("@idAlumno", idAlumno);
                                 cmdInsert.Parameters.AddWithValue("@idMateria", idMateria);
                                 cmdInsert.ExecuteNonQuery();
                             }

                             // 3️⃣ Insertar en Alumno_materia (si no existe)
                             string sqlInsertAlumnoMateria = @"
                        INSERT INTO Alumno_materia (id_alumno, id_materia, fecha_inscripcion)
                        SELECT @idAlumno, @idMateria, GETDATE()
                        WHERE NOT EXISTS (
                            SELECT 1 FROM Alumno_materia am
                            WHERE am.id_alumno = @idAlumno
                              AND am.id_materia = @idMateria
                        );";

                             using (SqlCommand cmdAlumnoMateria = new SqlCommand(sqlInsertAlumnoMateria, conn, transaction))
                             {
                                 cmdAlumnoMateria.Parameters.AddWithValue("@idAlumno", idAlumno);
                                 cmdAlumnoMateria.Parameters.AddWithValue("@idMateria", idMateria);
                                 cmdAlumnoMateria.ExecuteNonQuery();
                             }

                             // 4️⃣ Reinscribir materias desaprobadas (si aplica)
                             string sqlReinscribirDesaprobado = @"
                        INSERT INTO Cursada (id_alumno, id_materia, estado, id_profesor)
                        SELECT @idAlumno, @idMateria, 'Cursando', MPF.id_profesor
                        FROM Materia m
                        LEFT JOIN Profesor_Materia MPF ON MPF.id_materia = m.id_materia
                        WHERE m.id_materia = @idMateria
                          AND EXISTS (
                              SELECT 1 FROM Cursada c
                              WHERE c.id_alumno = @idAlumno
                                AND c.id_materia = @idMateria
                                AND c.estado = 'Desaprobado'
                          )
                          AND NOT EXISTS (
                              SELECT 1 FROM Cursada c2
                              WHERE c2.id_alumno = @idAlumno
                                AND c2.id_materia = @idMateria
                                AND c2.estado = 'Cursando'
                          );";

                             using (SqlCommand cmdDesaprobado = new SqlCommand(sqlReinscribirDesaprobado, conn, transaction))
                             {
                                 cmdDesaprobado.Parameters.AddWithValue("@idAlumno", idAlumno);
                                 cmdDesaprobado.Parameters.AddWithValue("@idMateria", idMateria);
                                 cmdDesaprobado.ExecuteNonQuery();
                             }

                             transaction.Commit();
                             lblMensaje.Visible = true;
                             lblMensaje.Text = "✅ Te inscribiste correctamente en la materia.";
                         }
                         catch (Exception exTrans)
                         {
                             transaction.Rollback();
                             lblMensaje.Visible = true;
                             lblMensaje.Text = "❌ Error al inscribirte: " + exTrans.Message;
                         }
                     }
                 }
             }
             catch (Exception ex)
             {
                 lblMensaje.Visible = true;
                 lblMensaje.Text = "❌ Error de conexión: " + ex.Message;
             }
         }
                       

}





    }
