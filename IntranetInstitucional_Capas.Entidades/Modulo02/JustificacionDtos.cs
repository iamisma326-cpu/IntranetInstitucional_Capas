using System;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class JustificacionDto
    {
        public int Id { get; set; }
        public int AsistenciaId { get; set; }
        public int EstudianteId { get; set; }
        public string EstudianteNombre { get; set; }
        public string EstudianteCodigo { get; set; }
        public string UnidadDidacticaNombre { get; set; }
        public DateTime FechaClase { get; set; }
        public int NumeroSemana { get; set; }
        public string Motivo { get; set; }
        public string Descripcion { get; set; }
        public string DocumentoSustentoUrl { get; set; }
        public string Estado { get; set; }
        public string DocenteNombre { get; set; }
        public string RespuestaObservacion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaResolucion { get; set; }

        public JustificacionDto()
        {
            EstudianteNombre = string.Empty;
            EstudianteCodigo = string.Empty;
            UnidadDidacticaNombre = string.Empty;
            Motivo = "Salud_Medica";
            Descripcion = string.Empty;
            Estado = "PENDIENTE";
        }

        public bool EsDentroDe72Horas
        {
            get { return (DateTime.Now - FechaClase).TotalHours <= 72.0; }
        }
    }

    public class CrearJustificacionDto
    {
        public int AsistenciaId { get; set; }
        public string Motivo { get; set; }
        public string Descripcion { get; set; }
        public string DocumentoSustentoUrl { get; set; }

        public CrearJustificacionDto()
        {
            Motivo = "Salud_Medica";
            Descripcion = string.Empty;
        }
    }

    public class ResolverJustificacionDto
    {
        public int JustificacionId { get; set; }
        public bool Aprobada { get; set; }
        public string Observacion { get; set; }
    }

    [Serializable]
    public class AlertaDpiDto
    {
        public int EstudianteId { get; set; }
        public string EstudianteNombre { get; set; }
        public string EstudianteCodigo { get; set; }
        public string CarreraNombre { get; set; }
        public string Ciclo { get; set; }
        public string Turno { get; set; }
        public int UnidadDidacticaId { get; set; }
        public string UnidadDidacticaNombre { get; set; }
        public string DocenteNombre { get; set; }
        public int HorasFalta { get; set; }
        public int TotalHoras { get; set; }
        public decimal PorcentajeInasistencia { get; set; }
        public string SemaforoEstado { get; set; }

        public AlertaDpiDto()
        {
            EstudianteNombre = string.Empty;
            EstudianteCodigo = string.Empty;
            CarreraNombre = string.Empty;
            Ciclo = string.Empty;
            Turno = string.Empty;
            UnidadDidacticaNombre = string.Empty;
            DocenteNombre = string.Empty;
            SemaforoEstado = "ALERTA";
        }

        public bool InhabilitadoRecuperacionSemana17
        {
            get { return PorcentajeInasistencia >= 30.00m; }
        }
    }

    [Serializable]
    public class DashboardAsistenciaViewModel
    {
        public string RolUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public bool EsDocente { get; set; }
        public bool EsAlumno { get; set; }
        public bool EsCoordinadorOAdmin { get; set; }

        public ClaseActivaDocenteDto ClaseActivaHoy { get; set; }

        public int TotalClasesAsignadas { get; set; }
        public int TotalSesionesHoy { get; set; }
        public int TotalAlumnosDpi { get; set; }
        public int TotalJustificacionesPendientes { get; set; }

        public System.Collections.Generic.List<ClaseDocenteCardDto> MisClasesDocente { get; set; }
        public System.Collections.Generic.List<SesionClaseDto> SesionesHoy { get; set; }
        public System.Collections.Generic.List<ResumenAsistenciaEstudianteDto> ResumenCursosEstudiante { get; set; }
        public System.Collections.Generic.List<AlertaDpiDto> CasosCriticosDpi { get; set; }
        public System.Collections.Generic.List<JustificacionDto> JustificacionesRecientes { get; set; }

        public DashboardAsistenciaViewModel()
        {
            RolUsuario = "Docente";
            NombreUsuario = string.Empty;
            MisClasesDocente = new System.Collections.Generic.List<ClaseDocenteCardDto>();
            SesionesHoy = new System.Collections.Generic.List<SesionClaseDto>();
            ResumenCursosEstudiante = new System.Collections.Generic.List<ResumenAsistenciaEstudianteDto>();
            CasosCriticosDpi = new System.Collections.Generic.List<AlertaDpiDto>();
            JustificacionesRecientes = new System.Collections.Generic.List<JustificacionDto>();
        }
    }
}
