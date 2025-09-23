using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Boletin_Virtual_2025
{
    public partial class MenuAlumnos : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
     {
            {
                // ---Asegúrate de que esto solo se ejecute la primera vez que la página se carga
               // if (!IsPostBack)
                {
                    // ---Verifica si la sesión existe
               //     if (Session["UserEmail"] != null)
                    {
                        //--- Muestra un mensaje de bienvenida usando la información de la sesión
                      //  lblUserWelcome.Text = "Welcome, " + Session["UserEmail"].ToString();
                    }
                   // else
                    {
                        //--- Si no hay sesión, redirige al usuario a la página de login
                        // Response.Redirect("Login2.aspx");
                    }
                }
            }
 

        
        }
    }
}