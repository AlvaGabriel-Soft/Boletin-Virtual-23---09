using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace Boletin_Virtual_2025.moderador
{
    public partial class AgregarProfesor : System.Web.UI.Page
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["Boletin"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
           

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (ExisteUsuario(dni.Text))
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
                        commandUsuario.Parameters.AddWithValue("@rol", 2);  //profesor.
                        commandUsuario.Parameters.AddWithValue("@fecha_registro", DateTime.Now);

                        int idUsuario = Convert.ToInt32(commandUsuario.ExecuteScalar());

                        // 2. Insertar en Profesor usando el id_usuario recién generado
                        string queryProfesor = @"INSERT INTO dbo.Profesor (id_usuario, titulo, fecha_ingreso, especialidad) OUTPUT INSERTED.id_profesor
                                       VALUES (@id_usuario, @titulo, @fecha_ingreso, @especialidad)";

                        SqlCommand commandProfesor = new SqlCommand(queryProfesor, conexion, transaction);
                        commandProfesor.Parameters.AddWithValue("@id_usuario", idUsuario);
                        commandProfesor.Parameters.AddWithValue("@titulo", titulo.Text);
                        commandProfesor.Parameters.AddWithValue("@especialidad", especialidad.Text);
                        commandProfesor.Parameters.AddWithValue("@fecha_ingreso", DateTime.Now);

                        int idProfesor = Convert.ToInt32(commandProfesor.ExecuteScalar());

                        transaction.Commit();

                        Response.Write("<script>alert('Usuario y profesor creados correctamente');</script>");

                        primernombre.Text = idProfesor.ToString();
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







        public bool ExisteUsuario(string dni)
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

        }



}
      
       