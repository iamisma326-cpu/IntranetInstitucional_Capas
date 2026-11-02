using System;
using System.Collections.Generic;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class ClaseActivaDocenteDto
    {
        public int SesionId { get; set; }
        public int ClaseDocenteId { get; set; }
        public int UnidadDidacticaId { get; set; }
        public string UnidadDidacticaCodigo { get; set; }
        public string UnidadDidacticaNombre { get; set; }
        public string CarreraNombre { get; set; }
        public string Ciclo { get; set; }
        public string Turno { get; set; }
        public string Seccion { get; set; }
        public string AulaCodigo { get; set; }
        public int NumeroSemana { get; set; }
        public int NumeroSesion { get; set; }
        public bool EsSemana17Recuperacion { get; set; }
        public bool EsSemana18Cierre { get; set; }
        public DateTime FechaClase { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int TotalAlumnos { get; set; }
        public string EstadoSesion { get; set; }

        public ClaseActivaDocenteDto()
        {
            UnidadDidacticaCodigo = string.Empty;
            UnidadDidacticaNombre = string.Empty;
            CarreraNombre = string.Empty;
            Ciclo = "I";
            Turno = "Noche";
            Seccion = "A";
            AulaCodigo = "LAB-302";
            EstadoSesion = "ABIERTA";
        }
    }

    [Serializable]
    public class ClaseDocenteCardDto
    {
        public int ClaseId { get; set; }
        public int UnidadDidacticaId { get; set; }
        public string UnidadDidacticaCodigo { get; set; }
        public string UnidadDidacticaNombre { get; set; }
        public int CarreraId { get; set; }
        public string CarreraNombre { get; set; }
        public string Ciclo { get; set; }
        public string Turno { get; set; }
        public string Seccion { get; set; }
        public int? AulaId { get; set; }
        public string AulaCodigo { get; set; }
        public int TotalSesionesSemanales { get; set; }
        public int HorasSemanales { get; set; }
        public int TotalAlumnosMatriculados { get; set; }
        public int SemanasRegistradas { get; set; }
        public int TotalSemanasRegulares { get; set; }
        public int UltimaSemanaRegistrada { get; set; }
        public int? ProximaSesionId { get; set; }

        public ClaseDocenteCardDto()
        {
            UnidadDidacticaCodigo = string.Empty;
            UnidadDidacticaNombre = string.Empty;
            CarreraNombre = string.Empty;
            Ciclo = "I";
            Turno = "Noche";
            Seccion = "A";
            AulaCodigo = string.Empty;
            TotalSesionesSemanales = 1;
            HorasSemanales = 4;
            TotalSemanasRegulares = 18;
        }
    }

    [Serializable]
    public class SemanaSelectorDto
    {
        public int NumeroSemana { get; set; }
        public bool EsSemana17Recuperacion { get; set; }
        public bool EsSemana18Cierre { get; set; }
        public string EstadoSemana { get; set; }
        public List<SesionClaseDto> Sesiones { get; set; }
        public bool EsSemanaActual { get; set; }

        public SemanaSelectorDto()
        {
            EstadoSemana = "PROGRAMADA";
            Sesiones = new List<SesionClaseDto>();
        }

        public string TituloSemana
        {
            get
            {
                if (EsSemana17Recuperacion) return "Semana 17 (Evaluacion de Recuperacion Ordinaria)";
                if (EsSemana18Cierre) return "Semana 18 (Subsanacion y Cierre REGISTRA)";
                return "Semana " + NumeroSemana;
            }
        }
    }

    [Serializable]
    public class SesionClaseDto
    {
        public int Id { get; set; }
        public int? ClaseDocenteId { get; set; }
        public int UnidadDidacticaId { get; set; }
        public string UnidadDidacticaCodigo { get; set; }
        public string UnidadDidacticaNombre { get; set; }
        public string CarreraNombre { get; set; }
        public string Ciclo { get; set; }
        public string Turno { get; set; }
        public string Seccion { get; set; }
        public int DocenteId { get; set; }
        public string DocenteNombreCompleto { get; set; }
        public int PeriodoId { get; set; }
        public string PeriodoCodigo { get; set; }
        public int? AulaId { get; set; }
        public string AulaCodigo { get; set; }
        public DateTime FechaClase { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int HorasPedagogicas { get; set; }
        public int NumeroSemana { get; set; }
        public int NumeroSesion { get; set; }
        public string TemaDesarrollado { get; set; }
        public string ObservacionesDocente { get; set; }
        public string Estado { get; set; }
        public DateTime? CerradaEn { get; set; }
        public int TotalAlumnos { get; set; }
        public int TotalPresentes { get; set; }
        public int TotalTardanzas { get; set; }
        public int TotalFaltas { get; set; }
        public int TotalJustificadas { get; set; }

        public SesionClaseDto()
        {
            UnidadDidacticaCodigo = string.Empty;
            UnidadDidacticaNombre = string.Empty;
            CarreraNombre = string.Empty;
            Ciclo = string.Empty;
            Turno = string.Empty;
            Seccion = string.Empty;
            DocenteNombreCompleto = string.Empty;
            PeriodoCodigo = string.Empty;
            AulaCodigo = string.Empty;
            TemaDesarrollado = null;
            ObservacionesDocente = null;
            Estado = "ABIERTA";
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
    }

    [Serializable]
    public class TabsFichaViewModel
    {
        public int ClaseId { get; set; }

        public string Activa { get; set; }

        public TabsFichaViewModel()
        {
            Activa = "asistencia";
        }
    }
}
