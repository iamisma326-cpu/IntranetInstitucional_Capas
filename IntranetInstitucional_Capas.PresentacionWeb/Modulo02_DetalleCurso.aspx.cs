using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Modulo02_DetalleCurso : System.Web.UI.Page
    {
        private readonly AsistenciaLinqBLL _asistencia = new AsistenciaLinqBLL();

        private int _unidadId;
        private int _estudianteId = 1;
        private bool _justificacionEnviada;

        protected string Motivo
        {
            get { return ViewState["Motivo"] as string ?? "Salud_Medica"; }
            set { ViewState["Motivo"] = value; }
        }

        protected string Descripcion
        {
            get { return ViewState["Descripcion"] as string ?? string.Empty; }
            set { ViewState["Descripcion"] = value; }
        }

        protected string Documento
        {
            get { return ViewState["Documento"] as string ?? string.Empty; }
            set { ViewState["Documento"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            int valor;
            if (int.TryParse(Request.QueryString["unidadId"], out valor)) _unidadId = valor;

            if (int.TryParse(Request.QueryString["estudianteId"], out valor) && valor > 0)
            {
                _estudianteId = valor;
            }
            else if (SesionPagina.Usuario.EstudianteId.HasValue)
            {
                _estudianteId = SesionPagina.Usuario.EstudianteId.Value;
            }

            ConfigurarWizard();

            if (!IsPostBack) CargarHistorial();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (IsPostBack && PostbackDeWizard() && !_justificacionEnviada)
            {
                ClientScript.RegisterStartupScript(
                    GetType(),
                    "ReabrirModalJustificar",
                    "document.getElementById('modal_justificar').showModal();",
                    true);
            }
        }

        private void CargarHistorial()
        {
            List<AsistenciaHistorialItemDto> historial =
                _asistencia.ObtenerHistorial(_estudianteId, _unidadId);

            gvHistorial.DataSource = historial;
            gvHistorial.DataBind();
        }

        private void ConfigurarWizard()
        {
            if (ddlMotivo != null && ddlMotivo.Items.Count == 0)
            {
                ddlMotivo.Items.Add(new ListItem("Salud / Descanso M\u00e9dico (MINSA / EsSalud)", "Salud_Medica"));
                ddlMotivo.Items.Add(new ListItem("Laboral / Declaraci\u00f3n Jurada de Empleador", "Laboral"));
                ddlMotivo.Items.Add(new ListItem("Tr\u00e1mite Oficial / Citaci\u00f3n Judicial", "Tramite_Oficial"));
                ddlMotivo.Items.Add(new ListItem("Fuerza Mayor o Emergencia Familiar", "Fuerza_Mayor"));
            }

            if (wizJustificar != null)
            {
                wizJustificar.StartNextButtonText = "Siguiente";
                wizJustificar.StepNextButtonText = "Siguiente";
                wizJustificar.StepPreviousButtonText = "Atr\u00e1s";
                wizJustificar.FinishCompleteButtonText = "Enviar Justificaci\u00f3n";
            }
        }

        protected void wizJustificar_ActiveStepChanged(object sender, EventArgs e)
        {
            if (!IsPostBack) return;

            if (wizJustificar.ActiveStepIndex == 0 && ddlMotivo != null)
            {
                ddlMotivo.Value = Motivo;
            }
            else if (wizJustificar.ActiveStepIndex == 1 && txtDescripcion != null
                     && string.IsNullOrEmpty(txtDescripcion.Value))
            {
                txtDescripcion.Value = Descripcion;
            }
            else if (wizJustificar.ActiveStepIndex == 2 && txtDocumento != null
                     && string.IsNullOrEmpty(txtDocumento.Value))
            {
                txtDocumento.Value = Documento;
            }
        }

        protected void wizJustificar_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.CurrentStepIndex == 0)
            {
                string motivo = FormValor("ddlMotivo");
                if (string.IsNullOrEmpty(motivo) && ddlMotivo != null) motivo = ddlMotivo.Value;
                if (!string.IsNullOrEmpty(motivo)) Motivo = motivo;
            }
            else if (e.CurrentStepIndex == 1)
            {
                string descripcion = FormValor("txtDescripcion");
                if (descripcion == null && txtDescripcion != null) descripcion = txtDescripcion.Value;
                if (descripcion != null) Descripcion = descripcion;
            }
        }

        protected void wizJustificar_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            string documento = FormValor("txtDocumento");
            if (documento == null && txtDocumento != null) documento = txtDocumento.Value;
            if (documento != null) Documento = documento;

            int asistenciaId = 0;
            if (inputAsistenciaId != null) int.TryParse(inputAsistenciaId.Value, out asistenciaId);

            if (asistenciaId <= 0)
            {
                SesionPagina.Error("Debe indicar la asistencia que desea justificar.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                SesionPagina.Error("Describa el motivo de la inasistencia.");
                return;
            }

            CrearJustificacionDto dto = new CrearJustificacionDto
            {
                AsistenciaId = asistenciaId,
                Motivo = Motivo,
                Descripcion = Descripcion,
                DocumentoSustentoUrl = Documento
            };

            RespuestaTransaccion respuesta = _asistencia.SolicitarJustificacion(dto, _estudianteId);

            if (respuesta.EsExitosa)
            {
                SesionPagina.Exito(respuesta.Mensaje);
                _justificacionEnviada = true;

                Motivo = "Salud_Medica";
                Descripcion = string.Empty;
                Documento = string.Empty;
                if (inputAsistenciaId != null) inputAsistenciaId.Value = string.Empty;
                wizJustificar.ActiveStepIndex = 0;

                CargarHistorial();
            }
            else
            {
                SesionPagina.Error(respuesta.Mensaje);
            }
        }

        private string FormValor(string control)
        {
            foreach (string clave in Request.Form.AllKeys)
            {
                if (clave != null && clave.EndsWith(control, StringComparison.Ordinal))
                {
                    return Request.Form[clave];
                }
            }
            return null;
        }

        private bool PostbackDeWizard()
        {
            foreach (string clave in Request.Form.AllKeys)
            {
                if (clave != null && clave.IndexOf("wizJustificar", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected void gvHistorial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            AsistenciaHistorialItemDto item = e.Row.DataItem as AsistenciaHistorialItemDto;
            if (item == null) return;

            Panel pnlNota = e.Row.FindControl("pnlNota") as Panel;
            if (pnlNota != null) pnlNota.Visible = !string.IsNullOrEmpty(item.Observacion);

            Panel pnlSinTramite = e.Row.FindControl("pnlSinTramite") as Panel;
            Panel pnlFalta = e.Row.FindControl("pnlFalta") as Panel;
            Panel pnlJustificada = e.Row.FindControl("pnlJustificada") as Panel;
            if (pnlFalta == null || pnlJustificada == null) return;

            bool esFalta = string.Equals(item.Estado, "FALTA_INJUSTIFICADA", StringComparison.OrdinalIgnoreCase);
            bool esJustificada = string.Equals(item.Estado, "FALTA_JUSTIFICADA", StringComparison.OrdinalIgnoreCase);

            pnlJustificada.Visible = esJustificada;
            pnlFalta.Visible = esFalta;
            if (pnlSinTramite != null) pnlSinTramite.Visible = !esFalta && !esJustificada;

            if (!esFalta) return;

            Panel pnlPendiente = pnlFalta.FindControl("pnlTramitePendiente") as Panel;
            Panel pnlBoton72 = pnlFalta.FindControl("pnlBoton72") as Panel;
            Panel pnlPlazo = pnlFalta.FindControl("pnlPlazoVencido") as Panel;

            if (item.TieneJustificacion)
            {
                if (pnlPendiente != null) pnlPendiente.Visible = true;
            }
            else if (DentroDeVentana72(item))
            {
                if (pnlBoton72 != null)
                {
                    pnlBoton72.Visible = true;

                    Button btnJustificar = pnlBoton72.FindControl("btnJustificar") as Button;
                    if (btnJustificar != null)
                    {
                        btnJustificar.Text = "\ud83d\udcce Justificar Falta";
                        btnJustificar.OnClientClick =
                            "abrirModalJustificar(" + item.AsistenciaId + ", '" +
                            FechaTexto(item.FechaClase) + "', " + item.NumeroSemana +
                            "); return false;";
                    }
                }
            }
            else if (pnlPlazo != null)
            {
                pnlPlazo.Visible = true;
            }
        }

        private static bool DentroDeVentana72(AsistenciaHistorialItemDto item)
        {
            return (DateTime.Now - item.FechaClase).TotalHours <= 72.0;
        }

        protected string FechaTexto(object fecha)
        {
            DateTime f;
            try { f = Convert.ToDateTime(fecha); }
            catch { f = DateTime.Today; }
            return f.ToString("dd/MM/yyyy");
        }

        protected string HorasTexto(object horas)
        {
            int h;
            try { h = Convert.ToInt32(horas); }
            catch { h = 0; }
            return h + " hrs lectivas";
        }

        protected string TemaTexto(object tema)
        {
            string valor = Convert.ToString(tema);
            return string.IsNullOrEmpty(valor) ? "Sesion de Aprendizaje" : valor;
        }

        protected string ClaseSemana(object semana)
        {
            int n = ConvertirEntero(semana);
            if (n == 17) return "text-amber-600 bg-amber-50/50";
            if (n == 18) return "text-purple-600 bg-purple-50/50";
            return "text-indigo-600 bg-indigo-50/50";
        }

        protected string EtiquetaSemana(object semana)
        {
            int n = ConvertirEntero(semana);
            if (n == 17) return "17-R";
            if (n == 18) return "18-C";
            return n.ToString();
        }

        protected string ClaseEstado(object estado)
        {
            string valor = Convert.ToString(estado);
            if (valor == "PRESENTE") return "bg-emerald-500";
            if (valor == "TARDANZA") return "bg-amber-500";
            if (valor == "FALTA_JUSTIFICADA") return "bg-blue-500";
            return "bg-rose-500";
        }

        protected string PuntoEstado(object estado)
        {
            string valor = Convert.ToString(estado);
            if (valor == "PRESENTE") return "&#128994;";
            if (valor == "TARDANZA") return "&#128992;";
            if (valor == "FALTA_JUSTIFICADA") return "&#128309;";
            return "&#128308;";
        }

        protected string EtiquetaEstado(object estado, object minutos)
        {
            string valor = Convert.ToString(estado);
            if (valor == "PRESENTE") return "Presente";
            if (valor == "TARDANZA") return "Tardanza (" + ConvertirEntero(minutos) + " min)";
            if (valor == "FALTA_JUSTIFICADA") return "Justificada";
            return "Falta Injustificada";
        }

        private static int ConvertirEntero(object valor)
        {
            try { return Convert.ToInt32(valor); }
            catch { return 0; }
        }
    }
}
