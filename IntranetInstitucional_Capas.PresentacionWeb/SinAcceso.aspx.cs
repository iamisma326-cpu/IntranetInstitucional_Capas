using System;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class SinAcceso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            if (!IsPostBack)
            {
            }
        }
    }
}
