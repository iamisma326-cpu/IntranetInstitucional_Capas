using System;
using System.Collections.Generic;
using System.Globalization;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Modulo02 : System.Web.UI.Page
    {
        protected DashboardAsistenciaViewModel _vm;
        protected ClaseActivaDocenteDto _claseActiva;

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            SesionUsuario sesion = SesionPagina.Usuario;

            if (sesion != null && sesion.ModoAlumno)
            {
                Response.Redirect("~/Modulo02_MiAsistencia.aspx");
                return;
            }

            CargarDashboard(sesion);

            if (!IsPostBack)
            {
            }
        }

        private void CargarDashboard(SesionUsuario sesion)
        {
            int? personaId = null;
            string rol = "Docente";

            if (sesion != null)
            {
                personaId = sesion.PersonaId;
                rol = sesion.RolActivo;
            }

            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();
            _vm = bll.ObtenerDashboard(personaId, rol);
            if (_vm == null)
            {
                _vm = new DashboardAsistenciaViewModel();
            }

            _claseActiva = _vm.ClaseActivaHoy;

            repClasesDocente.DataSource = _vm.MisClasesDocente;
            repClasesDocente.DataBind();

            repSesiones.DataSource = _vm.SesionesHoy;
            repSesiones.DataBind();
            pnlSesionesVacio.Visible = (_vm.SesionesHoy == null || _vm.SesionesHoy.Count == 0);

            repCasosDpi.DataSource = _vm.CasosCriticosDpi;
            repCasosDpi.DataBind();
            pnlCasosDpiVacio.Visible = (_vm.CasosCriticosDpi == null || _vm.CasosCriticosDpi.Count == 0);
        }

        protected string Hora(TimeSpan valor)
        {
            return valor.ToString(@"hh\:mm");
        }

        protected string HoraDe(object valor)
        {
            if (valor is TimeSpan)
            {
                return ((TimeSpan)valor).ToString(@"hh\:mm");
            }
            return string.Empty;
        }

        protected string AulaDe(object valor)
        {
            string aula = valor == null ? string.Empty : valor.ToString();
            return string.IsNullOrEmpty(aula) ? "LAB-401" : aula;
        }

        protected string Avance(object valor)
        {
            int semanas = valor == null ? 0 : Convert.ToInt32(valor);
            int porcentaje = Math.Min(100, (semanas * 100 / 18));
            return porcentaje.ToString(CultureInfo.InvariantCulture);
        }

        protected bool EsCerrada(object estado)
        {
            return string.Equals(Convert.ToString(estado), "CERRADA", StringComparison.OrdinalIgnoreCase);
        }

        protected bool EsDpi(object porcentaje)
        {
            return Convert.ToDecimal(porcentaje) >= 30m;
        }

        protected string Pct(object valor)
        {
            return Convert.ToDecimal(valor).ToString("0.##", CultureInfo.InvariantCulture);
        }
    }
}
