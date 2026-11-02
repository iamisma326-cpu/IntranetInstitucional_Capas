using System;
using System.Collections.Generic;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class MatrizAsistenciaViewModel
    {
        public int ClaseId { get; set; }
        public int UnidadDidacticaId { get; set; }
        public string UnidadDidacticaCodigo { get; set; }
        public string UnidadDidacticaNombre { get; set; }
        public string CarreraNombre { get; set; }
        public string Ciclo { get; set; }
        public string Turno { get; set; }
        public string Seccion { get; set; }
        public string DocenteNombre { get; set; }
        public int PeriodoId { get; set; }
        public string PeriodoCodigo { get; set; }

        public List<ColumnaSesionMatrizDto> ColumnasSesiones { get; set; }

        public List<FilaAlumnoMatrizDto> FilasAlumnos { get; set; }

        public MatrizAsistenciaViewModel()
        {
            UnidadDidacticaCodigo = string.Empty;
            UnidadDidacticaNombre = string.Empty;
            CarreraNombre = string.Empty;
            Ciclo = string.Empty;
            Turno = string.Empty;
            Seccion = string.Empty;
            DocenteNombre = string.Empty;
            PeriodoCodigo = "2026-I";
            ColumnasSesiones = new List<ColumnaSesionMatrizDto>();
            FilasAlumnos = new List<FilaAlumnoMatrizDto>();
        }
    }

    [Serializable]
    public class ColumnaSesionMatrizDto
    {
        public int SesionId { get; set; }

        public List<int> SesionIds { get; set; }

        public bool TieneSesion { get; set; }

        public int NumeroSemana { get; set; }
        public int NumeroSesion { get; set; }
        public DateTime FechaClase { get; set; }
        public int HorasPedagogicas { get; set; }
        public string Estado { get; set; }

        public ColumnaSesionMatrizDto()
        {
            SesionIds = new List<int>();
            Estado = "CERRADA";
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
    public class FilaAlumnoMatrizDto
    {
        public int EstudianteId { get; set; }
        public string CodigoEstudiante { get; set; }
        public string Dni { get; set; }
        public string NombreCompleto { get; set; }

        public Dictionary<int, string> EstadosPorSesion { get; set; }

        public int TotalPresentes { get; set; }
        public int TotalTardanzas { get; set; }
        public int TotalFaltasInjustificadas { get; set; }
        public int TotalFaltasJustificadas { get; set; }
        public int HorasFaltaAcumuladas { get; set; }
        public int TotalHorasAsignatura { get; set; }
        public decimal PorcentajeInasistencia { get; set; }
        public string SemaforoEstado { get; set; }
        public int HorasMargenDpi { get; set; }

        public FilaAlumnoMatrizDto()
        {
            CodigoEstudiante = string.Empty;
            Dni = string.Empty;
            NombreCompleto = string.Empty;
            EstadosPorSesion = new Dictionary<int, string>();
            TotalHorasAsignatura = 72;
            SemaforoEstado = "REGULAR";
        }

        public bool EsDpiInhabilitado
        {
            get { return PorcentajeInasistencia >= 30.00m; }
        }
    }

    [Serializable]
    public class GuardarMatrizRequestDto
    {
        public int ClaseId { get; set; }
        public List<GuardarCeldaMatrizDto> Celdas { get; set; }

        public GuardarMatrizRequestDto()
        {
            Celdas = new List<GuardarCeldaMatrizDto>();
        }
    }

    public class GuardarCeldaMatrizDto
    {
        public int EstudianteId { get; set; }
        public int SesionId { get; set; }
        public string Estado { get; set; }

        public GuardarCeldaMatrizDto()
        {
            Estado = "PRESENTE";
        }
    }
}
