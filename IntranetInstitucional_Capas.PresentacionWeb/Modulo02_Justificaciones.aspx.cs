using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Modulo02_Justificaciones : System.Web.UI.Page
    {
        private readonly AsistenciaLinqBLL _asistencia = new AsistenciaLinqBLL();

        protected List<JustificacionDto> _lista = new List<JustificacionDto>();

        protected bool _esSoloAlumno;

        protected bool _abrirModal;

        private string _estadoActual = "TODAS";

        private const string TAB_ACTIVA = "justificaciones";

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            SesionUsuario sesion = SesionPagina.Usuario;
            _esSoloAlumno = sesion.ModoAlumno;
            _estadoActual = ValidarEstado(Request.QueryString["estado"]);

            pnlSolicitud.Visible = sesion.PuedeSolicitarJustificacion;

            txtDescripcion.Attributes["placeholder"] = "Describe los motivos especificos de tu inasistencia...";
            txtDocumentoUrl.Attributes["placeholder"] = "https://drive.google.com/... o enlace de certificado";
            txtObservacion.Attributes["placeholder"] = "Ingresa el motivo de aprobacion o rechazo...";

            if (ddlMotivo.Items.Count == 0)
            {
                ddlMotivo.Items.Add(new ListItem("Salud M\u00e9dica", "Salud_Medica"));
                ddlMotivo.Items.Add(new ListItem("Laboral", "Laboral"));
                ddlMotivo.Items.Add(new ListItem("Tr\u00e1mite Oficial", "Tramite_Oficial"));
                ddlMotivo.Items.Add(new ListItem("Fuerza Mayor", "Fuerza_Mayor"));
            }

            if (ddlResolucion.Items.Count == 0)
            {
                ddlResolucion.Items.Add(new ListItem("\u2705 Aprobar Justificaci\u00f3n (Reclasificar a Falta Justificada)", "APROBADA"));
                ddlResolucion.Items.Add(new ListItem("\u274c Rechazar Justificaci\u00f3n (Mantener Falta Injustificada)", "RECHAZADA"));
            }

            int? estudianteFiltro = _esSoloAlumno ? (sesion.EstudianteId ?? 1) : (int?)null;
            string estadoFiltro = _estadoActual == "TODAS" ? null : _estadoActual;
            _lista = _asistencia.ObtenerJustificaciones(estudianteFiltro, estadoFiltro);

            gvJustificaciones.Visible = _lista.Count > 0;
            pnlVacio.Visible = _lista.Count == 0;

            if (!IsPostBack)
            {
                gvJustificaciones.DataSource = _lista;
                gvJustificaciones.DataBind();
            }
        }

        private static string ValidarEstado(string estado)
        {
            if (string.Equals(estado, "PENDIENTE", StringComparison.OrdinalIgnoreCase)) return "PENDIENTE";
            if (string.Equals(estado, "APROBADA", StringComparison.OrdinalIgnoreCase)) return "APROBADA";
            if (string.Equals(estado, "RECHAZADA", StringComparison.OrdinalIgnoreCase)) return "RECHAZADA";
            return "TODAS";
        }

        protected string Subtitulo
        {
            get
            {
                return _esSoloAlumno
                    ? "Seguimiento de tus solicitudes de justificaci\u00f3n m\u00e9dica y laboral ingresadas."
                    : "Revisi\u00f3n y dictamen docente de descansos m\u00e9dicos y constancias laborales.";
            }
        }

        protected string RutaVuelta
        {
            get
            {
                return _esSoloAlumno
                    ? ResolveUrl("~/Modulo02_MiAsistencia.aspx")
                    : ResolveUrl("~/Modulo02.aspx");
            }
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
            bool actual = string.Equals(activa, TAB_ACTIVA, StringComparison.OrdinalIgnoreCase);

            return actual
                ? "rounded-lg border border-sky-200 dark:border-sky-800 bg-sky-50 dark:bg-sky-950/60 px-3 py-1.5 text-sky-700 dark:text-sky-300"
                : "rounded-lg border border-transparent px-3 py-1.5 text-slate-500 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800";
        }

        protected string ClaseTodas
        {
            get { return ClaseFiltro("TODAS"); }
        }

        protected string ClasePendientes
        {
            get { return ClaseFiltro("PENDIENTE"); }
        }

        protected string ClaseAprobadas
        {
            get { return ClaseFiltro("APROBADA"); }
        }

        protected string ClaseRechazadas
        {
            get { return ClaseFiltro("RECHAZADA"); }
        }

        private string ClaseFiltro(string estado)
        {
            bool activo = string.Equals(estado, _estadoActual, StringComparison.OrdinalIgnoreCase);

            return activo
                ? "bg-indigo-600 text-white border-0"
                : "bg-white dark:bg-slate-800 text-slate-600 dark:text-slate-300 border border-slate-200 dark:border-slate-700";
        }

        protected int TotalTodas
        {
            get { return _lista.Count; }
        }

        protected int TotalPendientes
        {
            get { return _lista.Count(x => x.Estado == "PENDIENTE"); }
        }

        protected int TotalAprobadas
        {
            get { return _lista.Count(x => x.Estado == "APROBADA"); }
        }

        protected int TotalRechazadas
        {
            get { return _lista.Count(x => x.Estado == "RECHAZADA"); }
        }

        protected string ResumenMotivo
        {
            get
            {
                if (ddlMotivo == null || ddlMotivo.SelectedItem == null) return "-";
                return Server.HtmlEncode(ddlMotivo.SelectedItem.Text);
            }
        }

        protected string ResumenDescripcion
        {
            get
            {
                string texto = txtDescripcion == null ? null : txtDescripcion.Text;
                return string.IsNullOrEmpty(texto) ? "-" : Server.HtmlEncode(texto);
            }
        }

        protected string ResumenAsistencia
        {
            get
            {
                string texto = txtAsistenciaId == null ? null : txtAsistenciaId.Text;
                return string.IsNullOrEmpty(texto) ? "-" : Server.HtmlEncode(texto);
            }
        }

        protected string ResumenDocumento
        {
            get
            {
                string texto = txtDocumentoUrl == null ? null : txtDocumentoUrl.Value;
                return string.IsNullOrEmpty(texto) ? "-" : Server.HtmlEncode(texto);
            }
        }

        protected void gvJustificaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.Equals(e.CommandName, "Evaluar", StringComparison.Ordinal)) return;

            SesionUsuario sesion = SesionPagina.Usuario;
            if (sesion.ModoAlumno)
            {
                SesionPagina.Error("Acceso denegado: solo el docente puede emitir el dictamen de una justificaci\u00f3n.");
                Response.Redirect("~/SinAcceso.aspx");
                return;
            }

            int id = 0;
            int.TryParse(Convert.ToString(e.CommandArgument), out id);

            JustificacionSelId = id;
            _abrirModal = true;
        }

        protected int JustificacionSelId
        {
            get
            {
                object valor = ViewState["_justificacionSelId"];
                return valor == null ? 0 : (int)valor;
            }
            set { ViewState["_justificacionSelId"] = value; }
        }

        protected string TextoModalEstudiante
        {
            get
            {
                int id = JustificacionSelId;
                if (id <= 0) return "Estudiante...";

                foreach (JustificacionDto j in _lista)
                {
                    if (j.Id == id)
                    {
                        return Server.HtmlEncode(j.EstudianteNombre) + " (" + Server.HtmlEncode(j.Motivo) + ")";
                    }
                }
                return "Estudiante...";
            }
        }

        protected void btnEmitirDictamen_Click(object sender, EventArgs e)
        {
            SesionUsuario sesion = SesionPagina.Usuario;
            if (sesion.ModoAlumno)
            {
                SesionPagina.Error("Acceso denegado: solo el docente puede emitir el dictamen de una justificaci\u00f3n.");
                Response.Redirect("~/SinAcceso.aspx");
                return;
            }

            int justificacionId = JustificacionSelId;
            if (justificacionId <= 0)
            {
                SesionPagina.Error("Debe indicar la justificacion a resolver.");
                return;
            }

            string observacion = txtObservacion == null ? null : txtObservacion.Text;
            if (string.IsNullOrWhiteSpace(observacion))
            {
                SesionPagina.Error("Registre una observacion de revision.");
                _abrirModal = true;
                return;
            }

            ResolverJustificacionDto dto = new ResolverJustificacionDto
            {
                JustificacionId = justificacionId,
                Aprobada = string.Equals(ddlResolucion.SelectedValue, "APROBADA", StringComparison.Ordinal),
                Observacion = observacion
            };

            int docenteId = sesion.DocenteId ?? 1;
            RespuestaTransaccion respuesta = _asistencia.ResolverJustificacion(dto, docenteId);

            if (respuesta.EsExitosa) SesionPagina.Exito(respuesta.Mensaje);
            else SesionPagina.Error(respuesta.Mensaje);

            Response.Redirect(RutaDeVuelta());
        }

        protected void wizJustificacion_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            SesionUsuario sesion = SesionPagina.Usuario;

            int asistenciaId = 0;
            int.TryParse(txtAsistenciaId == null ? null : txtAsistenciaId.Text, out asistenciaId);

            CrearJustificacionDto dto = new CrearJustificacionDto
            {
                AsistenciaId = asistenciaId,
                Motivo = ddlMotivo.SelectedValue,
                Descripcion = txtDescripcion.Text,
                DocumentoSustentoUrl = txtDocumentoUrl == null ? null : txtDocumentoUrl.Value
            };

            int estudianteId = sesion.EstudianteId ?? 1;
            RespuestaTransaccion respuesta = _asistencia.SolicitarJustificacion(dto, estudianteId);

            if (respuesta.EsExitosa) SesionPagina.Exito(respuesta.Mensaje);
            else SesionPagina.Error(respuesta.Mensaje);

            wizJustificacion.ActiveStepIndex = 0;
            Response.Redirect(RutaDeVuelta());
        }

        protected string RutaDeVuelta()
        {
            string estado = Request.QueryString["estado"];
            if (string.IsNullOrEmpty(estado)) return ResolveUrl("~/Modulo02_Justificaciones.aspx");
            return ResolveUrl("~/Modulo02_Justificaciones.aspx?estado=" + Server.UrlEncode(estado));
        }

        protected string UrlAdjunto(object documento)
        {
            string url = Convert.ToString(documento);
            return string.IsNullOrEmpty(url) ? "#" : url;
        }

        protected string EstiloAdjunto(object documento)
        {
            return string.IsNullOrEmpty(Convert.ToString(documento)) ? "display:none;" : string.Empty;
        }

        protected string EstiloSinAdjunto(object documento)
        {
            return string.IsNullOrEmpty(Convert.ToString(documento)) ? string.Empty : "display:none;";
        }

        protected bool EsEstado(object estado, string esperado)
        {
            return string.Equals(Convert.ToString(estado), esperado, StringComparison.OrdinalIgnoreCase);
        }

        protected string EstiloEstado(object estado, string valor)
        {
            return EsEstado(estado, valor) ? string.Empty : "display:none;";
        }

        protected string TextoAccionAlumno(object estado)
        {
            if (EsEstado(estado, "PENDIENTE")) return "&#9203; En evaluaci&oacute;n";
            if (EsEstado(estado, "APROBADA")) return "&#9989; Reclasificada";
            return "&#10060; No procede";
        }

        protected string FechaTexto(object fecha)
        {
            if (fecha == null) return string.Empty;
            if (fecha is DateTime) return ((DateTime)fecha).ToString("dd/MM/yyyy");

            DateTime valor;
            if (DateTime.TryParse(Convert.ToString(fecha), out valor)) return valor.ToString("dd/MM/yyyy");
            return string.Empty;
        }
    }
}
