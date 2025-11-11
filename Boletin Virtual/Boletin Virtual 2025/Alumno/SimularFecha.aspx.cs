using System;
using System.Web;
using System.Web.UI;

namespace Boletin_Virtual_2025.Alumno   // 🔹 CAMBIADO: antes era Boletin_Virtual_2025
{
    public partial class SimularFecha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["FechaSimulada"] == null)
                    Session["FechaSimulada"] = DateTime.Now;

                lblFechaActual.Text = "Fecha actual simulada: " +
                    ((DateTime)Session["FechaSimulada"]).ToString("dd/MM/yyyy");
            }
        }

        protected void btnCambiarFecha_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime nuevaFecha = Convert.ToDateTime(txtFechaSimulada.Text);
                Session["FechaSimulada"] = nuevaFecha;

                lblFechaActual.Text = "Fecha actual simulada: " +
                    nuevaFecha.ToString("dd/MM/yyyy");
            }
            catch
            {
                lblFechaActual.Text = "⚠️ Fecha inválida. Use formato dd/mm/aaaa";
            }
        }

        public static DateTime ObtenerFechaActual()
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["FechaSimulada"] != null)
                return (DateTime)HttpContext.Current.Session["FechaSimulada"];
            else
                return DateTime.Now;
        }
    }
}
