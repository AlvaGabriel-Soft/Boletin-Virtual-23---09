using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Boletin_Virtual_2025
{
    public partial class MenuModerador : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

       protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Limpia la sesión
            Session.Clear();
            Session.Abandon();
            // Redirige al login
            Response.Redirect("~/Login2.aspx");
        }
    }
}