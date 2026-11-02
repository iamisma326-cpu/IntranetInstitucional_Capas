using System;
using System.Collections.Generic;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class AlumnoAsistenciaItemDto
    {
        public int EstudianteId { get; set; }
        public int PersonaId { get; set; }
        public string CodigoEstudiante { get; set; }
        public string Dni { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string FotoUrl { get; set; }
        public string Turno { get; set; }

        public int? AsistenciaId { get; set; }
        public string EstadoAsistencia { get; set; }
        public int MinutosTardanza { get; set; }
        public string Observacion { get; set; }

        public int HorasFaltaAcumuladasPrevias { get; set; }
        public int TotalHorasSemestrales { get; set; }
        public decimal PorcentajeInasistenciaPrevio { get; set; }
        public int HorasFaltaDisponibles { get; set; }

        public AlumnoAsistenciaItemDto()
        {
            CodigoEstudiante = string.Empty;
            Dni = string.Empty;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            FotoUrl = null;
            Turno = "Noche";
            EstadoAsistencia = "PRESENTE";
            TotalHorasSemestrales = 72;
        }

        public string NombreCompleto
        {
            get { return Apellidos + ", " + Nombres; }
        }

        public bool EnRiesgoDpi
        {
            get { return PorcentajeInasistenciaPrevio >= 20.00m; }
        }

        public bool EsDpi
        {
            get { return PorcentajeInasistenciaPrevio >= 30.00m; }
        }
    }

    [Serializable]
    public class GuardarAsistenciaRequestDto
    {
        public int SesionClaseId { get; set; }
        public string TemaDesarrollado { get; set; }
        public string ObservacionesDocente { get; set; }
        public bool CerrarSesion { get; set; }
        public List<GuardarItemAsistenciaDto> Alumnos { get; set; }

        public GuardarAsistenciaRequestDto()
        {
            CerrarSesion = false;
            Alumnos = new List<GuardarItemAsistenciaDto>();
        }
    }

    public class GuardarItemAsistenciaDto
    {
        public int EstudianteId { get; set; }
        public string Estado { get; set; }
        public int MinutosTardanza { get; set; }
        public string Observacion { get; set; }

        public GuardarItemAsistenciaDto()
        {
            Estado = "PRESENTE";
        }
    }

    [Serializable]
    public class ResumenAsistenciaEstudianteDto
    {
        public int EstudianteId { get; set; }
        public int UnidadDidacticaId { get; set; }
        public int PeriodoId { get; set; }
        public string UnidadDidacticaNombre { get; set; }
        public string UnidadDidacticaCodigo { get; set; }
        public string CarreraNombre { get; set; }
        public string Ciclo { get; set; }
        public string Turno { get; set; }
        public int TotalHorasSemestre { get; set; }
        public int CantPresentes { get; set; }
        public int CantTardanzas { get; set; }
        public int CantFaltasJustificadas { get; set; }
        public int CantFaltasInjustificadas { get; set; }
        public int HorasFaltaInjustificada { get; set; }
        public int HorasAsistidas { get; set; }
        public int TotalSesionesRegistradas { get; set; }
        public decimal PorcentajeInasistencia { get; set; }
        public string SemaforoEstado { get; set; }
        public int HorasFaltaDisponibles { get; set; }

        public ResumenAsistenciaEstudianteDto()
        {
            UnidadDidacticaNombre = string.Empty;
            UnidadDidacticaCodigo = string.Empty;
            CarreraNombre = string.Empty;
            Ciclo = string.Empty;
            Turno = string.Empty;
            SemaforoEstado = "REGULAR";
        }

        public bool InhabilitadoRecuperacionSemana17
        {
            get { return PorcentajeInasistencia >= 30.00m; }
        }
    }

    [Serializable]
    public class AsistenciaHistorialItemDto
    {
        public int AsistenciaId { get; set; }
        public int SesionId { get; set; }
        public DateTime FechaClase { get; set; }
        public int NumeroSemana { get; set; }
        public int NumeroSesion { get; set; }
        public int HorasPedagogicas { get; set; }
        public string TemaDesarrollado { get; set; }
        public string DocenteNombre { get; set; }
        public string Estado { get; set; }
        public int MinutosTardanza { get; set; }
        public string Observacion { get; set; }
        public bool TieneJustificacion { get; set; }
        public string EstadoJustificacion { get; set; }

        public AsistenciaHistorialItemDto()
        {
            TemaDesarrollado = null;
            DocenteNombre = string.Empty;
            Estado = "PRESENTE";
            NumeroSesion = 1;
        }

        public bool EsSemana17Recuperacion
        {
            get { return NumeroSemana == 17; }
        }

        public bool EsSemana18Cierre
        {
            get { return NumeroSemana == 18; }
        }

        public bool DentroDeVentana72Horas
        {
            get { return (DateTime.Now - FechaClase).TotalHours <= 72.0; }
        }
    }

    [Serializable]
    public class ReprogramarHorarioDto
    {
        public int ClaseDocenteId { get; set; }
        public int DesdeSemana { get; set; }
        public int NuevoDiaSemana { get; set; }
        public TimeSpan NuevaHoraInicio { get; set; }
        public TimeSpan NuevaHoraFin { get; set; }
        public int? NuevoAulaId { get; set; }
        public string MotivoReprogramacion { get; set; }

        public ReprogramarHorarioDto()
        {
            DesdeSemana = 4;
            NuevoDiaSemana = 1;
            NuevaHoraInicio = new TimeSpan(18, 30, 0);
            NuevaHoraFin = new TimeSpan(21, 30, 0);
            MotivoReprogramacion = string.Empty;
        }
    }
}
