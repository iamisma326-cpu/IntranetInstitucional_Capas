using System;
using System.Web;
using IntranetInstitucional_Capas.Entidades;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public static class SesionPagina
    {
        private const string CLAVE_USUARIO = "SesionUsuario";
        private const string CLAVE_ALERTA = "AlertaPagina";

        public static SesionUsuario Usuario
        {
            get { return HttpContext.Current.Session[CLAVE_USUARIO] as SesionUsuario; }
        }

        public static bool Autenticado
        {
            get { return Usuario != null; }
        }

        public static void GuardarSesion(SesionUsuario sesion)
        {
            HttpContext.Current.Session[CLAVE_USUARIO] = sesion;
        }

        public static void CerrarSesion()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public static void RedirigirALogin()
        {
            HttpContext.Current.Response.Redirect("~/Login.aspx");
        }

        public static void ExigirSesion()
        {
            if (!Autenticado)
            {
                RedirigirALogin();
            }
        }

        public static void ExigirRol(params string[] rolesPermitidos)
        {
            SesionUsuario sesion = Usuario;
            if (sesion == null)
            {
                RedirigirALogin();
                return;
            }

            foreach (string rol in rolesPermitidos)
            {
                if (string.Equals(sesion.RolActivo, rol, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            HttpContext.Current.Response.Redirect("~/SinAcceso.aspx");
        }

        public static bool RolEs(string rol)
        {
            SesionUsuario sesion = Usuario;
            return sesion != null && string.Equals(sesion.RolActivo, rol, StringComparison.OrdinalIgnoreCase);
        }

        public static bool RolActivoEs(params string[] roles)
        {
            SesionUsuario sesion = Usuario;
            if (sesion == null) return false;

            foreach (string rol in roles)
            {
                if (string.Equals(sesion.RolActivo, rol, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                foreach (string propio in sesion.Roles)
                {
                    if (string.Equals(propio, rol, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CambiarRol(string rol)
        {
            SesionUsuario sesion = Usuario;
            if (sesion == null) return false;

            IntranetInstitucional_Capas.LogicaNegocio.UsuarioLinqBLL bll =
                new IntranetInstitucional_Capas.LogicaNegocio.UsuarioLinqBLL();
            bool cambiado = bll.CambiarRolActivo(sesion, rol);
            if (cambiado)
            {
                GuardarSesion(sesion);
            }
            return cambiado;
        }

        public static void Exito(string mensaje)
        {
            HttpContext.Current.Session[CLAVE_ALERTA] = "success|" + mensaje;
        }

        public static void Error(string mensaje)
        {
            HttpContext.Current.Session[CLAVE_ALERTA] = "danger|" + mensaje;
        }

        public static void Info(string mensaje)
        {
            HttpContext.Current.Session[CLAVE_ALERTA] = "info|" + mensaje;
        }

        public static string TomarAlerta()
        {
            object valor = HttpContext.Current.Session[CLAVE_ALERTA];
            if (valor == null) return null;

            HttpContext.Current.Session[CLAVE_ALERTA] = null;

            string texto = Convert.ToString(valor);
            int separador = texto.IndexOf('|');
            if (separador < 0) return texto;

            string tipo = texto.Substring(0, separador);
            string mensaje = texto.Substring(separador + 1);

            string clases;
            if (tipo == "success")
            {
                clases = "alert-success bg-emerald-50 dark:bg-emerald-950/40 text-emerald-800 dark:text-emerald-300 border-emerald-200 dark:border-emerald-800";
            }
            else if (tipo == "danger")
            {
                clases = "alert-error bg-rose-50 dark:bg-rose-950/40 text-rose-800 dark:text-rose-300 border-rose-200 dark:border-rose-800";
            }
            else
            {
                clases = "alert-info bg-blue-50 dark:bg-blue-950/40 text-blue-800 dark:text-blue-300 border-blue-200 dark:border-blue-800";
            }

            return "<div class=\"alert " + clases + " shadow-sm rounded-2xl mb-6 flex items-center justify-between text-xs py-3 px-4\" role=\"alert\">"
                 + "<span>" + System.Web.HttpUtility.HtmlEncode(mensaje) + "</span>"
                 + "<button type=\"button\" class=\"btn btn-ghost btn-xs\" onclick=\"this.parentElement.remove()\">✕</button>"
                 + "</div>";
        }
    }
}
