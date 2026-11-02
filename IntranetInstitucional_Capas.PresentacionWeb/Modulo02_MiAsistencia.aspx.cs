using System;
using System.Collections.Generic;
using System.Globalization;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Modulo02_MiAsistencia : System.Web.UI.Page
    {
        protected int _estudianteId = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            SesionUsuario sesion = SesionPagina.Usuario;

            _estudianteId = (sesion != null && sesion.EstudianteId.HasValue)
                ? sesion.EstudianteId.Value
                : 1;

            CargarCursos();

            if (!IsPostBack)
            {
            }
        }

        private void CargarCursos()
        {
            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();
            List<ResumenAsistenciaEstudianteDto> cursos = bll.ObtenerResumenEstudiante(_estudianteId);

            repCursos.DataSource = cursos;
            repCursos.DataBind();
            pnlCursosVacio.Visible = (cursos == null || cursos.Count == 0);
        }

        protected string Pct(object valor)
        {
            return Convert.ToDecimal(valor).ToString("0.##", CultureInfo.InvariantCulture);
        }

        protected string AnchoDpi(object valor)
        {
            decimal porcentaje = Convert.ToDecimal(valor);
            decimal ancho = Math.Min(100m, (porcentaje / 0.30m));
            return ancho.ToString("0.##", CultureInfo.InvariantCulture);
        }

        protected string BadgeSemaforo(object valor)
        {
            string estado = Convert.ToString(valor);

            if (estado == "DPI") return "badge-error text-white";
            if (estado == "RIESGO_ALTO") return "badge-warning text-white";
            if (estado == "ALERTA") return "bg-amber-100 dark:bg-amber-950 text-amber-800 dark:text-amber-300 border-amber-200";
            return "bg-emerald-50 dark:bg-emerald-950 text-emerald-700 dark:text-emerald-300 border-emerald-200";
        }

        protected string BarraSemaforo(object valor)
        {
            string estado = Convert.ToString(valor);

            if (estado == "DPI") return "bg-rose-500";
            if (estado == "RIESGO_ALTO") return "bg-amber-500";
            if (estado == "ALERTA") return "bg-amber-400";
            return "bg-emerald-500";
        }
    }
}
