using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Modulo02_TomarAsistencia : Page
    {
        private static readonly string[] EstadosValidos = new string[]
        {
            "PRESENTE", "TARDANZA", "FALTA_INJUSTIFICADA", "FALTA_JUSTIFICADA"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected)
            {
                return;
            }

            if (SesionPagina.Usuario != null && SesionPagina.Usuario.ModoAlumno)
            {
                SesionPagina.Error("Acceso denegado: esta pagina es solo para docentes y personal administrativo.");
                Response.Redirect("~/Modulo02_MiAsistencia.aspx");
                return;
            }

            SesionPagina.ExigirRol(
                Rol.Docente,
                Rol.Coordinador,
                Rol.Director,
                Rol.Admin,
                Rol.Secretaria,
                Rol.Tesoreria);
            if (Response.IsRequestBeingRedirected)
            {
                return;
            }

            if (!IsPostBack)
            {
                ResolverParametros();
            }

            CargarPagina();
        }

        private void ResolverParametros()
        {
            int sesionId = 0;
            int claseId = 0;

            int.TryParse(Request.QueryString["sesionId"], out sesionId);
            int.TryParse(Request.QueryString["claseId"], out claseId);

            var bll = new AsistenciaLinqBLL();

            if (sesionId <= 0 && claseId > 0)
            {
                List<SemanaSelectorDto> semanas = bll.ObtenerSemanasDeClase(claseId);
                if (semanas != null && semanas.Count > 0 && semanas[0].Sesiones != null && semanas[0].Sesiones.Count > 0)
                {
                    sesionId = semanas[0].Sesiones[0].Id;
                }
            }

            if (sesionId <= 0)
            {
                int docenteId = 1;
                if (SesionPagina.Usuario != null && SesionPagina.Usuario.DocenteId.HasValue)
                {
                    docenteId = SesionPagina.Usuario.DocenteId.Value;
                }

                ClaseActivaDocenteDto activa = bll.ObtenerClaseActivaHoy(docenteId);
                if (activa != null)
                {
                    sesionId = activa.SesionId;
                    claseId = activa.ClaseDocenteId;
                }
            }

            ViewState["SesionId"] = sesionId;
            ViewState["ClaseId"] = claseId;
        }

        private int SesionIdCargada
        {
            get { return ViewState["SesionId"] == null ? 0 : (int)ViewState["SesionId"]; }
        }

        private void CargarPagina()
        {
            int sesionId = SesionIdCargada;
            var bll = new AsistenciaLinqBLL();
            SesionClaseDto sesion = sesionId > 0 ? bll.ObtenerSesionPorId(sesionId) : null;

            if (sesion == null)
            {
                MostrarVacio("No hay una sesion de clase disponible para hoy. Vuelve al panel del modulo 02 para elegir una semana.");
                return;
            }

            List<AlumnoAsistenciaItemDto> alumnos = bll.ObtenerAlumnosParaSesion(sesion.Id, sesion.UnidadDidacticaId);
            if (alumnos == null || alumnos.Count == 0)
            {
                MostrarVacio("Esta sesion aun no tiene alumnos matriculados.");
                return;
            }

            int presentes = Contar(alumnos, "PRESENTE");
            int tardanzas = Contar(alumnos, "TARDANZA");
            int faltas = Contar(alumnos, "FALTA_INJUSTIFICADA");

            pnlTabla.Visible = true;
            pnlVacio.Visible = false;
            btnGuardar.Enabled = true;
            btnOficializar.Enabled = true;

            litCodigoCurso.Text = Html(sesion.UnidadDidacticaCodigo);
            litNombreCurso.Text = Html(sesion.UnidadDidacticaNombre);
            litDocenteAula.Text = Html(sesion.DocenteNombreCompleto) + " &bull; Aula " + Html(sesion.AulaCodigo);
            litCursoChip.Text = Html(sesion.UnidadDidacticaCodigo);
            litSemanaChip.Text = sesion.NumeroSemana.ToString();
            litPresentesChip.Text = presentes.ToString();

            litSemanaTitulo.Text = sesion.NumeroSemana.ToString();
            litPresentesResumen.Text = presentes.ToString();
            litFaltasResumen.Text = faltas.ToString();
            litTardanzasResumen.Text = tardanzas.ToString();
            litAula.Text = Html(sesion.AulaCodigo);

            dlAlumnos.DataSource = alumnos;
            dlAlumnos.DataBind();
        }

        private void MostrarVacio(string mensaje)
        {
            pnlTabla.Visible = false;
            pnlVacio.Visible = true;
            litVacio.Text = mensaje;
            btnGuardar.Enabled = false;
            btnOficializar.Enabled = false;
        }

        private static int Contar(List<AlumnoAsistenciaItemDto> alumnos, string estado)
        {
            int total = 0;
            foreach (AlumnoAsistenciaItemDto a in alumnos)
            {
                if (string.Equals(a.EstadoAsistencia, estado, StringComparison.OrdinalIgnoreCase))
                {
                    total++;
                }
            }
            return total;
        }

        protected void dlAlumnos_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            AlumnoAsistenciaItemDto dto = e.Item.DataItem as AlumnoAsistenciaItemDto;
            if (dto == null)
            {
                return;
            }

            string estado = string.IsNullOrEmpty(dto.EstadoAsistencia) ? "PRESENTE" : dto.EstadoAsistencia;

            var fila = e.Item.FindControl("filaAlumno") as System.Web.UI.HtmlControls.HtmlTableRow;
            if (fila != null)
            {
                fila.Attributes["data-estudiante-id"] = dto.EstudianteId.ToString();
                fila.Attributes["data-estado"] = estado;
            }

            PonerLiteral(e.Item, "litNumero", (e.Item.ItemIndex + 1).ToString());
            PonerLiteral(e.Item, "litIniciales", Iniciales(dto));
            PonerLiteral(e.Item, "litNombre", Html(dto.NombreCompleto));
            PonerLiteral(e.Item, "litDni", Html(dto.Dni));

            var litDpi = e.Item.FindControl("litDpi") as Literal;
            if (litDpi != null)
            {
                string porcentaje = dto.PorcentajeInasistenciaPrevio.ToString("0.##");
                if (dto.EsDpi)
                {
                    litDpi.Text = "<span class=\"rounded-full bg-rose-100 px-2.5 py-1 text-[10px] font-black text-rose-700\">DPI "
                        + porcentaje + "%</span>";
                }
                else if (dto.EnRiesgoDpi)
                {
                    litDpi.Text = "<span class=\"rounded-full bg-amber-100 px-2.5 py-1 text-[10px] font-bold text-amber-700\">Riesgo "
                        + porcentaje + "%</span>";
                }
                else
                {
                    litDpi.Text = "<span class=\"rounded-full bg-emerald-100 px-2.5 py-1 text-[10px] font-bold text-emerald-700\">"
                        + porcentaje + "%</span>";
                }
            }

            var hdn = e.Item.FindControl("hdnEstado") as HiddenField;
            if (hdn != null)
            {
                hdn.Value = estado;
            }

            var txtMinutos = e.Item.FindControl("txtMinutos") as TextBox;
            if (txtMinutos != null)
            {
                txtMinutos.Text = dto.MinutosTardanza.ToString();
            }

            var txtObservacion = e.Item.FindControl("txtObservacion") as TextBox;
            if (txtObservacion != null)
            {
                txtObservacion.Text = dto.Observacion;
            }
        }

        private static string Iniciales(AlumnoAsistenciaItemDto dto)
        {
            string nombres = string.IsNullOrEmpty(dto.Nombres) ? "A" : dto.Nombres;
            string apellidos = string.IsNullOrEmpty(dto.Apellidos) ? "A" : dto.Apellidos;
            return (nombres.Substring(0, 1) + apellidos.Substring(0, 1)).ToUpperInvariant();
        }

        private static void PonerLiteral(DataListItem item, string id, string valor)
        {
            var lit = item.FindControl(id) as Literal;
            if (lit != null)
            {
                lit.Text = valor;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarAsistencia(false);
        }

        protected void btnOficializar_Click(object sender, EventArgs e)
        {
            GuardarAsistencia(true);
        }

        private void GuardarAsistencia(bool cerrar)
        {
            int sesionId = SesionIdCargada;
            var bll = new AsistenciaLinqBLL();
            SesionClaseDto sesion = sesionId > 0 ? bll.ObtenerSesionPorId(sesionId) : null;

            if (sesion == null)
            {
                SesionPagina.Error("No se pudo identificar la sesion de clase.");
                Response.Redirect(Request.RawUrl);
                return;
            }

            var dto = new GuardarAsistenciaRequestDto
            {
                SesionClaseId = sesionId,
                TemaDesarrollado = sesion.TemaDesarrollado,
                ObservacionesDocente = string.Empty,
                CerrarSesion = cerrar
            };

            foreach (DataListItem item in dlAlumnos.Items)
            {
                if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem)
                {
                    continue;
                }

                var fila = item.FindControl("filaAlumno") as System.Web.UI.HtmlControls.HtmlTableRow;
                var hdn = item.FindControl("hdnEstado") as HiddenField;
                var txtMinutos = item.FindControl("txtMinutos") as TextBox;
                var txtObservacion = item.FindControl("txtObservacion") as TextBox;

                if (fila == null || hdn == null)
                {
                    continue;
                }

                int estudianteId;
                if (!int.TryParse(fila.Attributes["data-estudiante-id"], out estudianteId) || estudianteId <= 0)
                {
                    continue;
                }

                string estado = (hdn.Value ?? string.Empty).Trim().ToUpperInvariant();
                if (Array.IndexOf(EstadosValidos, estado) < 0)
                {
                    estado = "PRESENTE";
                }

                int minutos = 0;
                if (txtMinutos != null)
                {
                    int.TryParse(txtMinutos.Text, out minutos);
                }
                if (minutos < 0)
                {
                    minutos = 0;
                }
                if (minutos > 180)
                {
                    minutos = 180;
                }

                dto.Alumnos.Add(new GuardarItemAsistenciaDto
                {
                    EstudianteId = estudianteId,
                    Estado = estado,
                    MinutosTardanza = estado == "TARDANZA" ? minutos : 0,
                    Observacion = txtObservacion != null ? txtObservacion.Text : string.Empty
                });
            }

            if (dto.Alumnos.Count == 0)
            {
                SesionPagina.Error("No hay alumnos para guardar en esta sesion.");
                Response.Redirect(Request.RawUrl);
                return;
            }

            int usuarioId = 1;
            if (SesionPagina.Usuario != null && SesionPagina.Usuario.UsuarioId > 0)
            {
                usuarioId = SesionPagina.Usuario.UsuarioId;
            }

            bool guardado = bll.GuardarAsistencia(dto, usuarioId);
            if (guardado)
            {
                SesionPagina.Exito(cerrar
                    ? "Asistencia oficializada y sesion cerrada correctamente."
                    : "Asistencia guardada correctamente.");

                if (cerrar)
                {
                    if (sesion.ClaseDocenteId.HasValue)
                    {
                        Response.Redirect("~/Modulo02_Clase.aspx?claseId=" + sesion.ClaseDocenteId.Value);
                    }
                    else
                    {
                        Response.Redirect("~/Modulo02.aspx");
                    }
                    return;
                }
            }
            else
            {
                SesionPagina.Error("No se pudo guardar la asistencia.");
            }

            Response.Redirect(Request.RawUrl);
        }

        private static string Html(string valor)
        {
            return string.IsNullOrEmpty(valor)
                ? string.Empty
                : System.Web.HttpUtility.HtmlEncode(valor);
        }
    }
}
