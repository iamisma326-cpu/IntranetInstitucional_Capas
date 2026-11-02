using System;
using IntranetInstitucional_Capas.Entidades;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected string NombreUsuario = string.Empty;

        protected string RolActivo = string.Empty;

        protected bool EsDocenteModo;
        protected bool EsDirectorModo;
        protected bool EsCoordinadorModo;
        protected bool EsSecretariaModo;
        protected bool EsTesoreriaModo;

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            CargarContextoDeRol();

            if (!IsPostBack)
            {
            }
        }

        private void CargarContextoDeRol()
        {
            SesionUsuario sesion = SesionPagina.Usuario;
            if (sesion == null) return;

            NombreUsuario = string.IsNullOrEmpty(sesion.NombreCompleto) ? string.Empty : sesion.NombreCompleto;
            RolActivo = string.IsNullOrEmpty(sesion.RolActivo) ? Rol.Alumno : sesion.RolActivo;

            EsDocenteModo = string.Equals(RolActivo, Rol.Docente, StringComparison.OrdinalIgnoreCase);
            EsDirectorModo = string.Equals(RolActivo, Rol.Director, StringComparison.OrdinalIgnoreCase)
                          || string.Equals(RolActivo, Rol.Admin, StringComparison.OrdinalIgnoreCase);
            EsCoordinadorModo = string.Equals(RolActivo, Rol.Coordinador, StringComparison.OrdinalIgnoreCase);
            EsSecretariaModo = string.Equals(RolActivo, Rol.Secretaria, StringComparison.OrdinalIgnoreCase);
            EsTesoreriaModo = string.Equals(RolActivo, Rol.Tesoreria, StringComparison.OrdinalIgnoreCase);
        }
    }
}
