using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Modulo02_Clase : System.Web.UI.Page
    {
        private int _claseId;
        private string _nombreCurso = "Asignatura";
        private string _codigoCurso = "UD";
        private string _ciclo = "I";
        private string _turno = "Noche";

        private string _tabActiva = "detalle";

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            SesionPagina.ExigirRol(Rol.Director, Rol.Admin, Rol.Coordinador, Rol.Docente,
                                   Rol.Secretaria, Rol.Tesoreria);
            if (Response.IsRequestBeingRedirected) return;

            if (!IsPostBack)
            {
                CargarSelectorDeSemanas();
            }

            CargarClase();
        }

        private void CargarClase()
        {
            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();

            int claseId;
            int.TryParse(Request.QueryString["claseId"], out claseId);

            if (claseId <= 0)
            {
                SesionUsuario usuario = SesionPagina.Usuario;
                int docenteId = (usuario != null && usuario.DocenteId.HasValue)
                    ? usuario.DocenteId.Value
                    : 1;

                List<ClaseDocenteCardDto> clases = bll.ObtenerClasesDocente(docenteId, null);
                if (clases.Count == 0)
                {
                    SesionPagina.Info("No tienes asignaturas activas asignadas actualmente.");
                    Response.Redirect("~/Modulo02.aspx");
                    return;
                }

                claseId = clases[0].ClaseId;
            }

            _claseId = claseId;

            List<SemanaSelectorDto> semanas = bll.ObtenerSemanasDeClase(_claseId);
            SesionClaseDto sesionEjemplo = null;
            foreach (SemanaSelectorDto sem in semanas)
            {
                if (sem.Sesiones != null && sem.Sesiones.Count > 0)
                {
                    sesionEjemplo = sem.Sesiones[0];
                    break;
                }
            }

            if (sesionEjemplo != null)
            {
                _nombreCurso = sesionEjemplo.UnidadDidacticaNombre ?? "Asignatura";
                _codigoCurso = sesionEjemplo.UnidadDidacticaCodigo ?? "UD";
                _ciclo = sesionEjemplo.Ciclo ?? "I";
                _turno = sesionEjemplo.Turno ?? "Noche";
            }

            repSemanas.DataSource = CrearVistaSemanas(semanas, _claseId);
            repSemanas.DataBind();
        }

        private void CargarSelectorDeSemanas()
        {
            ddlDesdeSemana.Items.Clear();
            for (int w = 1; w <= 18; w++)
            {
                string texto = "A partir de la Semana " + w;
                if (w == 4)
                {
                    texto += " (Semana Actual)";
                }
                ddlDesdeSemana.Items.Add(new ListItem(texto, w.ToString()));
            }
        }

        protected void btnReprogramar_Click(object sender, EventArgs e)
        {
            ReprogramarHorarioDto dto = new ReprogramarHorarioDto();
            dto.ClaseDocenteId = _claseId;

            int desdeSemana;
            dto.DesdeSemana = int.TryParse(ddlDesdeSemana.SelectedValue, out desdeSemana) ? desdeSemana : 0;

            int diaSemana;
            dto.NuevoDiaSemana = int.TryParse(ddlNuevoDia.SelectedValue, out diaSemana) ? diaSemana : 1;

            TimeSpan horaInicio;
            dto.NuevaHoraInicio = TimeSpan.TryParse(txtHoraInicio.Text, out horaInicio)
                ? horaInicio
                : new TimeSpan(18, 35, 0);

            TimeSpan horaFin;
            dto.NuevaHoraFin = TimeSpan.TryParse(txtHoraFin.Text, out horaFin)
                ? horaFin
                : new TimeSpan(21, 45, 0);

            dto.MotivoReprogramacion = txtMotivo.Text ?? string.Empty;

            if (dto.ClaseDocenteId <= 0 || dto.DesdeSemana < 1)
            {
                SesionPagina.Error("Parametros de reprogramacion invalidos.");
                Response.Redirect("~/Modulo02.aspx");
                return;
            }

            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();
            bool ok = bll.Reprogramar(dto);

            if (ok)
            {
                SesionPagina.Exito("Sesiones futuras a partir de la Semana " + dto.DesdeSemana
                    + " reprogramadas con exito sin alterar el historial.");
            }
            else
            {
                SesionPagina.Error("No se pudo completar la reprogramacion de horarios.");
            }

            Response.Redirect("~/Modulo02_Clase.aspx?claseId=" + dto.ClaseDocenteId);
        }

        protected string NombreCurso
        {
            get { return _nombreCurso; }
        }

        protected string CodigoCurso
        {
            get { return _codigoCurso; }
        }

        protected string Ciclo
        {
            get { return _ciclo; }
        }

        protected string Turno
        {
            get { return _turno; }
        }

        protected string RutaSabana
        {
            get
            {
                return _claseId > 0
                    ? ResolveUrl("~/Modulo02_Matriz.aspx?claseId=" + _claseId)
                    : ResolveUrl("~/Modulo02_Matriz.aspx");
            }
        }

        protected string RutaVolver
        {
            get { return ResolveUrl("~/Modulo02.aspx"); }
        }

        protected string RutaAsistencia
        {
            get { return RutaSabana; }
        }

        protected string RutaDetalle
        {
            get
            {
                return _claseId > 0
                    ? ResolveUrl("~/Modulo02_Clase.aspx?claseId=" + _claseId)
                    : ResolveUrl("~/Modulo02_Clase.aspx");
            }
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

        private List<SemanaFichaVista> CrearVistaSemanas(List<SemanaSelectorDto> semanas, int claseId)
        {
            List<SemanaFichaVista> lista = new List<SemanaFichaVista>();
            if (semanas == null) return lista;

            foreach (SemanaSelectorDto sem in semanas)
            {
                bool esSem17 = sem.NumeroSemana == 17;
                bool esSem18 = sem.NumeroSemana == 18;

                SemanaFichaVista vista = new SemanaFichaVista();
                vista.NumeroSemana = sem.NumeroSemana;
                vista.EstadoSemana = sem.EstadoSemana;

                if (sem.EstadoSemana == "CERRADA")
                {
                    vista.TarjetaBorder = "border-emerald-200/80 dark:border-emerald-900 bg-white dark:bg-[#111827] hover:border-emerald-300";
                    vista.BadgeSemana = "badge-success text-white font-bold";
                }
                else if (sem.EstadoSemana == "ABIERTA")
                {
                    vista.TarjetaBorder = "border-sky-400 bg-sky-50/30 dark:bg-sky-950/20 shadow-md";
                    vista.BadgeSemana = "badge-warning text-white font-bold animate-pulse";
                }
                else
                {
                    vista.TarjetaBorder = esSem17
                        ? "border-amber-200 dark:border-amber-900 bg-amber-50/10"
                        : (esSem18
                            ? "border-purple-200 dark:border-purple-900 bg-purple-50/10"
                            : "border-slate-200 dark:border-slate-800 bg-white dark:bg-[#111827]");
                    vista.BadgeSemana = esSem17
                        ? "bg-amber-100 text-amber-900 font-bold border-amber-300"
                        : (esSem18
                            ? "bg-purple-100 text-purple-900 font-bold border-purple-300"
                            : "badge-ghost text-slate-500");
                }

                vista.ClaseIcono = esSem17
                    ? "bg-amber-100 text-amber-800"
                    : (esSem18
                        ? "bg-purple-100 text-purple-800"
                        : (sem.EstadoSemana == "CERRADA"
                            ? "bg-emerald-100 text-emerald-700"
                            : "bg-sky-100 text-sky-700"));

                vista.TituloSemanaHtml = esSem17
                    ? "Semana 17: Recuperaci&oacute;n"
                    : (esSem18
                        ? "Semana 18: Cierre REGISTRA"
                        : "Semana " + sem.NumeroSemana);

                int totalSesiones = (sem.Sesiones != null) ? sem.Sesiones.Count : 0;
                vista.SubtituloSemanaHtml = esSem17
                    ? "Evaluaci&oacute;n Ordinaria (Notas 10-12)"
                    : (esSem18
                        ? "Subsanaci&oacute;n y Actas Oficiales"
                        : totalSesiones + " sesi&oacute;n lectiva");

                List<SesionFichaVista> sesiones = new List<SesionFichaVista>();
                if (sem.Sesiones != null)
                {
                    foreach (SesionClaseDto s in sem.Sesiones)
                    {
                        sesiones.Add(CrearVistaSesion(s, claseId));
                    }
                }
                vista.Sesiones = sesiones;

                lista.Add(vista);
            }

            return lista;
        }

        private SesionFichaVista CrearVistaSesion(SesionClaseDto s, int claseId)
        {
            SesionFichaVista v = new SesionFichaVista();
            v.Id = s.Id;
            v.FechaTexto = s.FechaClase.ToString("dd/MM/yyyy");
            v.HoraInicioTexto = s.HoraInicio.ToString(@"hh\:mm");
            v.HoraFinTexto = s.HoraFin.ToString(@"hh\:mm");
            v.HorasPedagogicas = s.HorasPedagogicas;

            v.TotalAlumnos = s.TotalAlumnos;
            v.TotalPresentes = s.TotalPresentes;
            v.TotalTardanzas = s.TotalTardanzas;
            v.TotalFaltas = s.TotalFaltas;
            v.TotalJustificadas = s.TotalJustificadas;
            v.MostrarMetricas = s.TotalAlumnos > 0;

            v.TemaHtml = string.IsNullOrEmpty(s.TemaDesarrollado)
                ? string.Empty
                : "<p class=\"text-[11px] text-slate-600 dark:text-slate-400 line-clamp-1 italic\">&#128221; "
                  + HttpUtility.HtmlEncode(s.TemaDesarrollado) + "</p>";

            bool cerrada = string.Equals(s.Estado, "CERRADA", StringComparison.OrdinalIgnoreCase);

            v.EnCursoHtml = cerrada
                ? string.Empty
                : "<span class=\"text-slate-400 font-normal\">(sesi&oacute;n en curso)</span>";

            v.ClaseBoton = cerrada
                ? "btn-ghost text-sky-700 dark:text-sky-300 hover:bg-sky-50"
                : "bg-sky-600 hover:bg-sky-700 text-white";

            v.TextoBotonHtml = cerrada
                ? "Ver Asistencia"
                : "&#9889; Tomar Lista Ahora";

            v.UrlAsistencia = ResolveUrl("~/Modulo02_TomarAsistencia.aspx?sesionId=" + s.Id
                + "&amp;claseId=" + claseId);

            return v;
        }
    }

    public class SemanaFichaVista
    {
        public SemanaFichaVista()
        {
            EstadoSemana = "PROGRAMADA";
            Sesiones = new List<SesionFichaVista>();
        }

        public int NumeroSemana { get; set; }
        public string EstadoSemana { get; set; }
        public string TarjetaBorder { get; set; }
        public string BadgeSemana { get; set; }
        public string ClaseIcono { get; set; }
        public string TituloSemanaHtml { get; set; }
        public string SubtituloSemanaHtml { get; set; }
        public List<SesionFichaVista> Sesiones { get; set; }
    }

    public class SesionFichaVista
    {
        public SesionFichaVista()
        {
            TemaHtml = string.Empty;
            EnCursoHtml = string.Empty;
        }

        public int Id { get; set; }
        public string FechaTexto { get; set; }
        public string HoraInicioTexto { get; set; }
        public string HoraFinTexto { get; set; }
        public int HorasPedagogicas { get; set; }
        public int TotalAlumnos { get; set; }
        public int TotalPresentes { get; set; }
        public int TotalTardanzas { get; set; }
        public int TotalFaltas { get; set; }
        public int TotalJustificadas { get; set; }
        public bool MostrarMetricas { get; set; }
        public string TemaHtml { get; set; }
        public string EnCursoHtml { get; set; }
        public string ClaseBoton { get; set; }
        public string TextoBotonHtml { get; set; }
        public string UrlAsistencia { get; set; }
    }
}
