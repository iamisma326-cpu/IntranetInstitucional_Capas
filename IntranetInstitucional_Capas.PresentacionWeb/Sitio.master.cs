using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using IntranetInstitucional_Capas.Entidades;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Sitio : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            PintarSesion();
            PintarAlerta();

            if (!IsPostBack)
            {
                CargarSelectorDeRoles(SesionPagina.Usuario);
            }

            MarcarPaginaActiva();
        }

        private void PintarSesion()
        {
            SesionUsuario sesion = SesionPagina.Usuario;
            if (sesion == null) return;

            string nombre = sesion.NombreCompleto ?? string.Empty;
            string[] partes = nombre.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string iniciales = "US";
            if (partes.Length >= 2)
            {
                iniciales = partes[0].Substring(0, 1) + partes[1].Substring(0, 1);
            }
            else if (partes.Length == 1 && partes[0].Length >= 2)
            {
                iniciales = partes[0].Substring(0, 2);
            }

            spanIniciales.InnerText = iniciales.ToUpperInvariant();
            spanNombre.InnerText = nombre;
            spanRolActivo.InnerText = sesion.RolActivo;
        }

        private void CargarSelectorDeRoles(SesionUsuario sesion)
        {
            ddlRolActivo.Items.Clear();
            if (sesion == null) return;

            foreach (string rolPreferido in Rol.Orden)
            {
                if (sesion.TieneRol(rolPreferido))
                {
                    ddlRolActivo.Items.Add(new ListItem(EmojiDeRol(rolPreferido) + " Modo " + rolPreferido, rolPreferido));
                }
            }

            foreach (string rol in sesion.Roles)
            {
                bool yaAgregado = false;
                foreach (ListItem item in ddlRolActivo.Items)
                {
                    if (string.Equals(item.Value, rol, StringComparison.OrdinalIgnoreCase))
                    {
                        yaAgregado = true;
                        break;
                    }
                }

                if (!yaAgregado)
                {
                    ddlRolActivo.Items.Add(new ListItem(EmojiDeRol(rol) + " Modo " + rol, rol));
                }
            }

            if (ddlRolActivo.Items.Count > 0)
            {
                ListItem actual = ddlRolActivo.Items.FindByValue(sesion.RolActivo);
                if (actual != null) ddlRolActivo.SelectedValue = sesion.RolActivo;
            }

            itemSelectorRol.Visible = ddlRolActivo.Items.Count > 1;
        }

        private static string EmojiDeRol(string rol)
        {
            if (string.Equals(rol, Rol.Director, StringComparison.OrdinalIgnoreCase)) return "\U0001F3DB";
            if (string.Equals(rol, Rol.Docente, StringComparison.OrdinalIgnoreCase)) return "\U0001F468\u200D\U0001F3EB";
            if (string.Equals(rol, Rol.Alumno, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(rol, Rol.Estudiante, StringComparison.OrdinalIgnoreCase)) return "\U0001F393";
            if (string.Equals(rol, Rol.Coordinador, StringComparison.OrdinalIgnoreCase)) return "\U0001F4CB";
            if (string.Equals(rol, Rol.Secretaria, StringComparison.OrdinalIgnoreCase)) return "\U0001F4C2";
            if (string.Equals(rol, Rol.Tesoreria, StringComparison.OrdinalIgnoreCase)) return "\U0001F4B3";
            return "\U0001F3AD";
        }

        private void PintarAlerta()
        {
            string alerta = SesionPagina.TomarAlerta();
            if (!string.IsNullOrEmpty(alerta))
            {
                Literal literal = new Literal();
                literal.Text = alerta;
                phAlerta.Controls.Add(literal);
            }
        }

        protected void ddlRolActivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SesionPagina.CambiarRol(ddlRolActivo.SelectedValue))
            {
                SesionPagina.Exito("Ahora estas en modo " + ddlRolActivo.SelectedValue + ".");
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                ddlRolActivo.SelectedValue = SesionPagina.Usuario.RolActivo;
                SesionPagina.Error("No tienes asignado ese rol.");
            }
        }

        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            SesionPagina.CerrarSesion();
            SesionPagina.RedirigirALogin();
        }

        private void MarcarPaginaActiva()
        {
            string paginaActual = Path.GetFileName(Request.Path);

            Marcar(lnkPanelPrincipal, "Default.aspx", paginaActual);
            Marcar(lnkModulo02, "Modulo02.aspx", paginaActual);
        }

        private static void Marcar(HtmlAnchor enlace, string destino, string paginaActual)
        {
            if (enlace == null) return;

            bool activo = string.Equals(destino, paginaActual, StringComparison.OrdinalIgnoreCase)
                          || (string.Equals(destino, "Modulo02.aspx", StringComparison.OrdinalIgnoreCase)
                              && paginaActual.StartsWith("Modulo02", StringComparison.OrdinalIgnoreCase));

            if (!activo) return;

            if (string.Equals(destino, "Modulo02.aspx", StringComparison.OrdinalIgnoreCase))
            {
                enlace.Attributes["class"] = "flex items-center justify-between px-3 py-2 rounded-xl text-indigo-700 dark:text-indigo-300 bg-indigo-100/80 dark:bg-indigo-900/50 font-bold transition";
            }
            else
            {
                enlace.Attributes["class"] = "flex items-center gap-2.5 px-3 py-2 rounded-xl text-slate-900 dark:text-white bg-slate-100 dark:bg-slate-800 font-bold transition";
            }
        }
    }
}
