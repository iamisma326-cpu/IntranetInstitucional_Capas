using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public class RadarIndicador
    {
        public string Etiqueta { get; set; }
        public string Valor { get; set; }
        public string ClaseValor { get; set; }
        public string Nota { get; set; }
        public string ClaseNota { get; set; }
    }

    public partial class Modulo02_ReportesDpi : System.Web.UI.Page
    {
        private readonly AsistenciaLinqBLL _asistencia = new AsistenciaLinqBLL();

        private string _tabActiva = "reporte";

        private List<AlertaDpiDto> _alertas = new List<AlertaDpiDto>();

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            if (SesionPagina.Usuario.ModoAlumno)
            {
                SesionPagina.Error("Acceso denegado: El radar de deserci\u00f3n y reportes DPI es exclusivo para la Coordinaci\u00f3n y Direcci\u00f3n Acad\u00e9mica.");
                Response.Redirect("~/Modulo02_MiAsistencia.aspx");
                return;
            }

            _alertas = _asistencia.ObtenerAlertasDpi();

            if (!IsPostBack)
            {
                dlIndicadores.DataSource = CrearIndicadores(_alertas);
                dlIndicadores.DataBind();

                tablaRadarDpi.DataSource = _alertas;
                tablaRadarDpi.DataBind();
            }
        }

        private static List<RadarIndicador> CrearIndicadores(List<AlertaDpiDto> casos)
        {
            int total = casos.Count;
            int dpi = 0;

            foreach (AlertaDpiDto c in casos)
            {
                if (string.Equals(c.SemaforoEstado, "DPI", StringComparison.OrdinalIgnoreCase)) dpi++;
            }

            int alerta = total - dpi;

            return new List<RadarIndicador>
            {
                new RadarIndicador
                {
                    Etiqueta = "Total Alumnos en Riesgo / DPI",
                    Valor = total.ToString(),
                    ClaseValor = "text-rose-600 dark:text-rose-400",
                    Nota = "Estudiantes detectados por el algoritmo",
                    ClaseNota = "text-slate-400"
                },
                new RadarIndicador
                {
                    Etiqueta = "Condici\u00f3n DPI Oficial (\u226530%)",
                    Valor = dpi.ToString(),
                    ClaseValor = "text-rose-700 dark:text-rose-500",
                    Nota = "Inhabilitados para Semana 17 (Nota 00)",
                    ClaseNota = "text-rose-600"
                },
                new RadarIndicador
                {
                    Etiqueta = "Zona de Alerta Temprana (10% \u2013 29%)",
                    Valor = alerta.ToString(),
                    ClaseValor = "text-amber-600 dark:text-amber-400",
                    Nota = "Derivables a tutor\u00eda y prevenci\u00f3n",
                    ClaseNota = "text-amber-600"
                }
            };
        }

        protected string RutaAsistencia
        {
            get { return ResolveUrl("~/Modulo02_Matriz.aspx"); }
        }

        protected string RutaDetalle
        {
            get { return ResolveUrl("~/Modulo02_Clase.aspx"); }
        }

        protected string RutaJustificaciones
        {
            get { return ResolveUrl("~/Modulo02_Justificaciones.aspx"); }
        }

        protected string RutaReporte
        {
            get { return ResolveUrl("~/Modulo02_ReportesDpi.aspx"); }
        }

        protected string TabAsistencia
        {
            get { return ClaseTab("asistencia"); }
        }

        protected string TabDetalle
        {
            get { return ClaseTab("detalle"); }
        }

        protected string TabJustificaciones
        {
            get { return ClaseTab("justificaciones"); }
        }

        protected string TabReporte
        {
            get { return ClaseTab("reporte"); }
        }

        private string ClaseTab(string activa)
        {
            bool actual = string.Equals(_tabActiva, activa, StringComparison.OrdinalIgnoreCase);

            return actual
                ? "rounded-lg border border-sky-200 dark:border-sky-800 bg-sky-50 dark:bg-sky-950/60 px-3 py-1.5 text-sky-700 dark:text-sky-300"
                : "rounded-lg border border-transparent px-3 py-1.5 text-slate-500 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800";
        }

        protected void tablaRadarDpi_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            AlertaDpiDto item = e.Row.DataItem as AlertaDpiDto;
            if (item == null) return;

            e.Row.Attributes["data-estudiante"] = Minusculas(item.EstudianteNombre);
            e.Row.Attributes["data-codigo"] = item.EstudianteCodigo ?? string.Empty;
            e.Row.Attributes["data-carrera"] = Minusculas(item.CarreraNombre);
            e.Row.Attributes["data-curso"] = Minusculas(item.UnidadDidacticaNombre);
        }

        protected void tablaRadarDpi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "VerDetalle", StringComparison.Ordinal)) return;

            Control origen = e.CommandSource as Control;
            GridViewRow fila = origen != null ? origen.NamingContainer as GridViewRow : null;
            if (fila == null) return;

            int indice = fila.RowIndex;
            if (indice < 0 || indice >= _alertas.Count) return;

            AlertaDpiDto alerta = _alertas[indice];
            Response.Redirect("~/Modulo02_DetalleCurso.aspx?unidadId=" + alerta.UnidadDidacticaId +
                              "&estudianteId=" + alerta.EstudianteId);
        }

        private static string Minusculas(string texto)
        {
            return string.IsNullOrEmpty(texto) ? string.Empty : texto.ToLowerInvariant();
        }

        protected string ClasePorcentaje(object porcentaje)
        {
            decimal p = ConvertirDecimal(porcentaje);
            return p >= 30.00m
                ? "text-rose-600 dark:text-rose-400"
                : "text-amber-600 dark:text-amber-400";
        }

        protected string PorcentajeTexto(object porcentaje)
        {
            return ConvertirDecimal(porcentaje).ToString("0.##") + "%";
        }

        protected string ClaseSemaforo(object estado)
        {
            string valor = Convert.ToString(estado);
            if (valor == "DPI")
            {
                return "bg-rose-600 text-white font-black border-0 badge";
            }
            if (valor == "RIESGO_ALTO")
            {
                return "bg-amber-500 text-white font-bold border-0 badge";
            }
            return "bg-amber-100 dark:bg-amber-950 text-amber-800 dark:text-amber-300 border border-amber-200 dark:border-amber-800 font-semibold";
        }

        protected string EmojiSemaforo(object estado)
        {
            string valor = Convert.ToString(estado);
            if (valor == "DPI") return "&#128680;";
            if (valor == "RIESGO_ALTO") return "&#9888;&#65039;";
            return string.Empty;
        }

        protected string EtiquetaSemaforo(object estado)
        {
            string valor = Convert.ToString(estado);
            if (valor == "DPI") return "DPI (Nota 00 - Sin Recup)";
            if (valor == "RIESGO_ALTO") return "Riesgo Alto (&ge;20%)";
            return "Alerta Preventiva";
        }

        private static decimal ConvertirDecimal(object valor)
        {
            if (valor == null) return 0m;
            if (valor is decimal) return (decimal)valor;

            decimal resultado;
            if (decimal.TryParse(Convert.ToString(valor), out resultado)) return resultado;
            return 0m;
        }
    }
}
