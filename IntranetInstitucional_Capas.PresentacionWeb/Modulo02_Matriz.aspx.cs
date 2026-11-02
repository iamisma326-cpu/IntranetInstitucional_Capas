using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using IntranetInstitucional_Capas.Entidades;
using IntranetInstitucional_Capas.LogicaNegocio;

namespace IntranetInstitucional_Capas.PresentacionWeb
{
    public partial class Modulo02_Matriz : System.Web.UI.Page
    {
        protected int claseId;
        protected MatrizAsistenciaViewModel matriz;
        protected List<ColumnaMatrizVm> columnas = new List<ColumnaMatrizVm>();
        protected List<FilaMatrizVm> filas = new List<FilaMatrizVm>();

        protected void Page_Load(object sender, EventArgs e)
        {
            SesionPagina.ExigirSesion();
            if (Response.IsRequestBeingRedirected) return;

            SesionPagina.ExigirRol("Docente", "Coordinador", "Director", "Admin");
            if (Response.IsRequestBeingRedirected) return;

            int.TryParse(Request.QueryString["claseId"], out claseId);
            if (claseId <= 0)
            {
                RedirigirAClasePorDefecto();
                return;
            }

            CargarMatriz();

            if (!IsPostBack)
            {
                repColumnas.DataSource = columnas;
                repColumnas.DataBind();

                repFilas.DataSource = filas;
                repFilas.DataBind();
            }
        }

        private void CargarMatriz()
        {
            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();
            matriz = bll.ObtenerMatriz(claseId);

            if (matriz == null)
            {
                SesionPagina.Error("La clase seleccionada no existe o no tiene alumnos matriculados.");
                Response.Redirect("~/Modulo02.aspx");
                return;
            }

            if (matriz.ColumnasSesiones == null) matriz.ColumnasSesiones = new List<ColumnaSesionMatrizDto>();
            if (matriz.FilasAlumnos == null) matriz.FilasAlumnos = new List<FilaAlumnoMatrizDto>();

            columnas = ConstruirColumnas(matriz.ColumnasSesiones);
            filas = ConstruirFilas(matriz.FilasAlumnos, matriz.ColumnasSesiones);
        }

        private void RedirigirAClasePorDefecto()
        {
            SesionUsuario sesion = SesionPagina.Usuario;
            int docenteId = 1;
            if (sesion != null && sesion.DocenteId.HasValue && sesion.DocenteId.Value > 0)
            {
                docenteId = sesion.DocenteId.Value;
            }

            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();
            List<ClaseDocenteCardDto> clases = bll.ObtenerClasesDocente(docenteId, null);

            if (clases != null && clases.Count > 0)
            {
                claseId = clases[0].ClaseId;
                Response.Redirect("~/Modulo02_Matriz.aspx?claseId=" + claseId.ToString(CultureInfo.InvariantCulture));
                return;
            }

            SesionPagina.Info("No tienes asignaturas activas asignadas actualmente.");
            Response.Redirect("~/Modulo02.aspx");
        }

        private List<ColumnaMatrizVm> ConstruirColumnas(List<ColumnaSesionMatrizDto> origen)
        {
            List<ColumnaMatrizVm> lista = new List<ColumnaMatrizVm>();

            foreach (ColumnaSesionMatrizDto col in origen)
            {
                ColumnaMatrizVm vm = new ColumnaMatrizVm();
                vm.NumeroSemana = col.NumeroSemana;
                vm.TieneSesion = col.TieneSesion;
                vm.SesionId = col.SesionId;
                vm.SesionIdsCsv = (col.SesionIds != null && col.SesionIds.Count > 0)
                    ? string.Join(",", col.SesionIds)
                    : string.Empty;
                vm.FechaTexto = col.FechaClase.ToString("dd/MM", CultureInfo.InvariantCulture);
                vm.FechaLarga = col.FechaClase.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (col.NumeroSemana == 17)
                {
                    vm.Etiqueta = "S17-R";
                    vm.TituloSemana = "Semana 17: Evaluaci&oacute;n de Recuperaci&oacute;n Ordinaria";
                    vm.ClaseColumna = "bg-amber-100 dark:bg-amber-950 text-amber-900 dark:text-amber-200 border-x border-amber-300";
                }
                else if (col.NumeroSemana == 18)
                {
                    vm.Etiqueta = "S18-C";
                    vm.TituloSemana = "Semana 18: Subsanaci&oacute;n y Cierre REGISTRA";
                    vm.ClaseColumna = "bg-purple-100 dark:bg-purple-950 text-purple-900 dark:text-purple-200 border-x border-purple-300";
                }
                else
                {
                    vm.Etiqueta = "S" + col.NumeroSemana.ToString(CultureInfo.InvariantCulture);
                    vm.TituloSemana = "Semana " + col.NumeroSemana.ToString(CultureInfo.InvariantCulture);
                    vm.ClaseColumna = "bg-slate-100 dark:bg-slate-800";
                }

                vm.UrlTomar = (col.TieneSesion && col.SesionId > 0)
                    ? ResolveUrl("~/Modulo02_TomarAsistencia.aspx?sesionId="
                        + col.SesionId.ToString(CultureInfo.InvariantCulture)
                        + "&claseId=" + claseId.ToString(CultureInfo.InvariantCulture))
                    : string.Empty;

                lista.Add(vm);
            }

            return lista;
        }

        private List<FilaMatrizVm> ConstruirFilas(List<FilaAlumnoMatrizDto> origen, List<ColumnaSesionMatrizDto> columnasOrigen)
        {
            List<FilaMatrizVm> lista = new List<FilaMatrizVm>();
            int numeroFila = 1;

            foreach (FilaAlumnoMatrizDto fila in origen)
            {
                FilaMatrizVm vm = new FilaMatrizVm();
                vm.NumeroFila = numeroFila;
                numeroFila++;

                vm.EstudianteId = fila.EstudianteId;
                vm.Dni = fila.Dni;
                vm.CodigoEstudiante = fila.CodigoEstudiante;
                vm.NombreCompleto = fila.NombreCompleto;
                vm.NombreBusqueda = (fila.NombreCompleto ?? string.Empty).ToLowerInvariant();
                vm.EsRiesgoTexto = fila.PorcentajeInasistencia >= 20 ? "true" : "false";

                vm.TotalPresentes = fila.TotalPresentes;
                vm.TotalTardanzas = fila.TotalTardanzas;
                vm.TotalFaltasInjustificadas = fila.TotalFaltasInjustificadas;
                vm.TotalFaltasJustificadas = fila.TotalFaltasJustificadas;

                vm.PorcentajeTexto = fila.PorcentajeInasistencia.ToString("0.00", CultureInfo.InvariantCulture);
                vm.ClasePorcentaje = fila.PorcentajeInasistencia >= 30
                    ? "text-rose-600 bg-rose-50 dark:bg-rose-950"
                    : "text-slate-800 dark:text-slate-200";

                vm.EsDpi = fila.SemaforoEstado == "DPI";
                vm.EsRiesgoAlto = fila.SemaforoEstado == "RIESGO_ALTO";

                vm.Celdas = new List<CeldaMatrizVm>();
                foreach (ColumnaSesionMatrizDto col in columnasOrigen)
                {
                    vm.Celdas.Add(CrearCelda(fila, col));
                }

                lista.Add(vm);
            }

            return lista;
        }

        private static CeldaMatrizVm CrearCelda(FilaAlumnoMatrizDto fila, ColumnaSesionMatrizDto col)
        {
            CeldaMatrizVm celda = new CeldaMatrizVm();
            celda.TieneSesion = col.TieneSesion;
            celda.SesionId = col.SesionId;
            celda.SesionIdsCsv = (col.SesionIds != null && col.SesionIds.Count > 0)
                ? string.Join(",", col.SesionIds)
                : string.Empty;

            string estado = "SIN_REGISTRO";
            if (col.TieneSesion && fila.EstadosPorSesion != null)
            {
                string encontrado;
                if (fila.EstadosPorSesion.TryGetValue(col.SesionId, out encontrado)
                    && !string.IsNullOrEmpty(encontrado))
                {
                    estado = encontrado;
                }
            }

            celda.Estado = estado;
            celda.BadgeChar = BadgeDeEstado(estado);
            celda.BadgeClase = ClaseBadgeDeEstado(estado);
            return celda;
        }

        private static string BadgeDeEstado(string estado)
        {
            if (estado == "PRESENTE") return "P";
            if (estado == "TARDANZA") return "T";
            if (estado == "FALTA_INJUSTIFICADA") return "F";
            if (estado == "FALTA_JUSTIFICADA") return "J";
            return "-";
        }

        private static string ClaseBadgeDeEstado(string estado)
        {
            if (estado == "PRESENTE") return "bg-emerald-500 text-white";
            if (estado == "TARDANZA") return "bg-amber-500 text-white";
            if (estado == "FALTA_INJUSTIFICADA") return "bg-rose-500 text-white font-black";
            if (estado == "FALTA_JUSTIFICADA") return "bg-blue-500 text-white";
            return "bg-slate-100 dark:bg-slate-800 text-slate-400";
        }

        protected void repFilas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            FilaMatrizVm fila = (FilaMatrizVm)e.Item.DataItem;
            Repeater repCeldas = (Repeater)e.Item.FindControl("repCeldas");
            if (repCeldas == null) return;

            repCeldas.DataSource = fila.Celdas;
            repCeldas.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarMatrizRequestDto solicitud = new GuardarMatrizRequestDto();
            solicitud.ClaseId = claseId;

            foreach (RepeaterItem filaItem in repFilas.Items)
            {
                HiddenField hidEstudiante = (HiddenField)filaItem.FindControl("hidEstudiante");
                int estudianteId;
                if (hidEstudiante == null || !int.TryParse(hidEstudiante.Value, out estudianteId))
                {
                    continue;
                }
                if (estudianteId <= 0) continue;

                Repeater repCeldas = (Repeater)filaItem.FindControl("repCeldas");
                if (repCeldas == null) continue;

                foreach (RepeaterItem celdaItem in repCeldas.Items)
                {
                    HiddenField hidEstado = (HiddenField)celdaItem.FindControl("hidEstado");
                    HiddenField hidSesiones = (HiddenField)celdaItem.FindControl("hidSesiones");

                    if (hidEstado == null || hidSesiones == null) continue;

                    string estado = hidEstado.Value;
                    if (string.IsNullOrEmpty(estado) || estado == "SIN_REGISTRO") continue;

                    foreach (string parte in hidSesiones.Value.Split(','))
                    {
                        int sesionId;
                        if (!int.TryParse(parte.Trim(), out sesionId)) continue;
                        if (sesionId <= 0) continue;

                        GuardarCeldaMatrizDto celda = new GuardarCeldaMatrizDto();
                        celda.EstudianteId = estudianteId;
                        celda.SesionId = sesionId;
                        celda.Estado = estado;
                        solicitud.Celdas.Add(celda);
                    }
                }
            }

            if (solicitud.ClaseId <= 0)
            {
                SesionPagina.Error("Datos de matriz invalidos.");
                Response.Redirect(Request.RawUrl);
                return;
            }

            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();
            SesionUsuario sesion = SesionPagina.Usuario;
            int usuarioId = sesion != null ? sesion.UsuarioId : 0;

            bool ok = bll.GuardarMatriz(solicitud, usuarioId);
            if (ok)
            {
                SesionPagina.Exito("\u2705 Sabana de asistencia sincronizada con exito ("
                    + solicitud.Celdas.Count.ToString(CultureInfo.InvariantCulture)
                    + " registros actualizados).");
                Response.Redirect(Request.RawUrl);
                return;
            }

            SesionPagina.Error("No se pudo sincronizar la matriz.");
            Response.Redirect(Request.RawUrl);
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();
            byte[] contenido = bll.ExportarSabanaCsv(claseId);
            Descargar(contenido, "text/csv",
                "Sabana_Asistencia_Clase_" + claseId.ToString(CultureInfo.InvariantCulture)
                + "_" + DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".csv");
        }

        protected void btnExportarRegistra_Click(object sender, EventArgs e)
        {
            AsistenciaLinqBLL bll = new AsistenciaLinqBLL();
            byte[] contenido = bll.GenerarActaRegistra(claseId);
            Descargar(contenido, "text/plain",
                "Acta_Registra_MINEDU_Clase_" + claseId.ToString(CultureInfo.InvariantCulture)
                + "_" + DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".txt");
        }

        private void Descargar(byte[] contenido, string contentType, string nombreArchivo)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + nombreArchivo + "\"");
            Response.BinaryWrite(contenido);
            Response.End();
        }
    }

    public class ColumnaMatrizVm
    {
        public int NumeroSemana { get; set; }
        public bool TieneSesion { get; set; }
        public int SesionId { get; set; }
        public string SesionIdsCsv { get; set; }
        public string FechaTexto { get; set; }
        public string FechaLarga { get; set; }
        public string Etiqueta { get; set; }
        public string TituloSemana { get; set; }
        public string ClaseColumna { get; set; }
        public string UrlTomar { get; set; }

        public ColumnaMatrizVm()
        {
            SesionIdsCsv = string.Empty;
            FechaTexto = "--/--";
            FechaLarga = "--/--";
            Etiqueta = string.Empty;
            TituloSemana = string.Empty;
            ClaseColumna = "bg-slate-100 dark:bg-slate-800";
            UrlTomar = string.Empty;
        }
    }

    public class CeldaMatrizVm
    {
        public bool TieneSesion { get; set; }
        public int SesionId { get; set; }
        public string SesionIdsCsv { get; set; }
        public string Estado { get; set; }
        public string BadgeChar { get; set; }
        public string BadgeClase { get; set; }

        public CeldaMatrizVm()
        {
            SesionIdsCsv = string.Empty;
            Estado = "SIN_REGISTRO";
            BadgeChar = "-";
            BadgeClase = "bg-slate-100 dark:bg-slate-800 text-slate-400";
        }
    }

    public class FilaMatrizVm
    {
        public int NumeroFila { get; set; }
        public int EstudianteId { get; set; }
        public string Dni { get; set; }
        public string CodigoEstudiante { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreBusqueda { get; set; }
        public string EsRiesgoTexto { get; set; }
        public int TotalPresentes { get; set; }
        public int TotalTardanzas { get; set; }
        public int TotalFaltasInjustificadas { get; set; }
        public int TotalFaltasJustificadas { get; set; }
        public string PorcentajeTexto { get; set; }
        public string ClasePorcentaje { get; set; }
        public bool EsDpi { get; set; }
        public bool EsRiesgoAlto { get; set; }
        public List<CeldaMatrizVm> Celdas { get; set; }

        public FilaMatrizVm()
        {
            Dni = string.Empty;
            CodigoEstudiante = string.Empty;
            NombreCompleto = string.Empty;
            NombreBusqueda = string.Empty;
            EsRiesgoTexto = "false";
            PorcentajeTexto = "0.00";
            ClasePorcentaje = "text-slate-800 dark:text-slate-200";
            Celdas = new List<CeldaMatrizVm>();
        }
    }
}
