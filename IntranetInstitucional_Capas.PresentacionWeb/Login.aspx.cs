using System;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SesionPagina.Autenticado)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string returnUrl = Request.QueryString["ReturnUrl"];
                hdnReturnUrl.Value = string.IsNullOrEmpty(returnUrl) ? string.Empty : returnUrl;
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string identificador = (txtIdentificador.Text ?? string.Empty).Trim();
            string password = (txtPassword.Text ?? string.Empty).Trim();

            if (identificador.Length == 0 || password.Length == 0)
            {
                MostrarError("Por favor ingrese su DNI / Codigo y contrasena.");
                return;
            }

            UsuarioLinqBLL bll = new UsuarioLinqBLL();
            ResultadoLogin resultado = bll.ValidarCredenciales(identificador, password);

            if (!resultado.Ok)
            {
                MostrarError(string.IsNullOrEmpty(resultado.Mensaje)
                    ? "Credenciales incorrectas. Verifique su DNI o contrasena."
                    : resultado.Mensaje);
                return;
            }

            SesionPagina.GuardarSesion(resultado.CrearSesion());

            string destino = hdnReturnUrl.Value;
            if (!string.IsNullOrEmpty(destino) && destino.StartsWith("/") && !destino.StartsWith("//"))
            {
                Response.Redirect(destino);
                return;
            }

            Response.Redirect("~/Default.aspx");
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            pnlError.Visible = true;
        }
    }
}
