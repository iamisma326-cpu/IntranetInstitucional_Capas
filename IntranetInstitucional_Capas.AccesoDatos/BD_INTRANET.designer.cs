#pragma warning disable 1591

namespace IntranetInstitucional_Capas.AccesoDatos
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    public class BD_INTRANETDataContext : DataContext
    {
        private static readonly MappingSource mappingSource = new AttributeMappingSource();

        public BD_INTRANETDataContext() :
            base(ConfigurationManager.ConnectionStrings["BD_INTRANETConnectionString"].ConnectionString, mappingSource)
        {
        }

        public BD_INTRANETDataContext(string connection) :
            base(connection, mappingSource)
        {
        }

        public BD_INTRANETDataContext(IDbConnection connection) :
            base(connection, mappingSource)
        {
        }

        public Table<Personas> Personas
        {
            get { return this.GetTable<Personas>(); }
        }

        public Table<Usuarios> Usuarios
        {
            get { return this.GetTable<Usuarios>(); }
        }

        public Table<Roles> Roles
        {
            get { return this.GetTable<Roles>(); }
        }

        public Table<UsuarioRoles> UsuarioRoles
        {
            get { return this.GetTable<UsuarioRoles>(); }
        }

        public Table<Docentes> Docentes
        {
            get { return this.GetTable<Docentes>(); }
        }

        public Table<Estudiantes> Estudiantes
        {
            get { return this.GetTable<Estudiantes>(); }
        }

        public Table<UnidadesDidacticas> UnidadesDidacticas
        {
            get { return this.GetTable<UnidadesDidacticas>(); }
        }

        public Table<Carreras> Carreras
        {
            get { return this.GetTable<Carreras>(); }
        }

        public Table<Aulas> Aulas
        {
            get { return this.GetTable<Aulas>(); }
        }

        public Table<PeriodosAcademicos> PeriodosAcademicos
        {
            get { return this.GetTable<PeriodosAcademicos>(); }
        }

        public Table<ClasesDocente> ClasesDocente
        {
            get { return this.GetTable<ClasesDocente>(); }
        }

        public Table<SesionesClase> SesionesClase
        {
            get { return this.GetTable<SesionesClase>(); }
        }

        public Table<Asistencias> Asistencias
        {
            get { return this.GetTable<Asistencias>(); }
        }

        public Table<Justificaciones> Justificaciones
        {
            get { return this.GetTable<Justificaciones>(); }
        }

        public Table<VResumenAsistenciaEstudiante> VResumenAsistenciaEstudiante
        {
            get { return this.GetTable<VResumenAsistenciaEstudiante>(); }
        }
    }

    [Table(Name = "core.personas")]
    public partial class Personas
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "dni", DbType = "NVarChar(15) NOT NULL", CanBeNull = false)]
        public string Dni { get; set; }

        [Column(Name = "nombres", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string Nombres { get; set; }

        [Column(Name = "apellidos", DbType = "NVarChar(100) NOT NULL", CanBeNull = false)]
        public string Apellidos { get; set; }

        [Column(Name = "fecha_nacimiento", DbType = "Date", CanBeNull = true)]
        public DateTime? FechaNacimiento { get; set; }

        [Column(Name = "sexo", DbType = "NVarChar(10)", CanBeNull = true)]
        public string Sexo { get; set; }

        [Column(Name = "email_personal", DbType = "NVarChar(150)", CanBeNull = true)]
        public string EmailPersonal { get; set; }

        [Column(Name = "telefono", DbType = "NVarChar(20)", CanBeNull = true)]
        public string Telefono { get; set; }

        [Column(Name = "direccion", DbType = "NVarChar(255)", CanBeNull = true)]
        public string Direccion { get; set; }

        [Column(Name = "foto_url", DbType = "NVarChar(255)", CanBeNull = true)]
        public string FotoUrl { get; set; }

        [Column(Name = "creado_en", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? CreadoEn { get; set; }
    }

    [Table(Name = "core.usuarios")]
    public partial class Usuarios
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "persona_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int PersonaId { get; set; }

        [Column(Name = "codigo_institucional", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string CodigoInstitucional { get; set; }

        [Column(Name = "email", DbType = "NVarChar(150) NOT NULL", CanBeNull = false)]
        public string Email { get; set; }

        [Column(Name = "password_hash", DbType = "NVarChar(255)", CanBeNull = true)]
        public string PasswordHash { get; set; }

        [Column(Name = "ultimo_acceso", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? UltimoAcceso { get; set; }

        [Column(Name = "estado", DbType = "Bit NOT NULL", CanBeNull = false)]
        public bool Estado { get; set; }

        [Column(Name = "creado_en", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? CreadoEn { get; set; }
    }

    [Table(Name = "core.roles")]
    public partial class Roles
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "nombre", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string Nombre { get; set; }

        [Column(Name = "descripcion", DbType = "NVarChar(200) NOT NULL", CanBeNull = false)]
        public string Descripcion { get; set; }
    }

    [Table(Name = "core.usuario_roles")]
    public partial class UsuarioRoles
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "usuario_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int UsuarioId { get; set; }

        [Column(Name = "rol_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int RolId { get; set; }

        [Column(Name = "asignado_en", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? AsignadoEn { get; set; }

        [Column(Name = "es_activo", DbType = "Bit", CanBeNull = true)]
        public bool? EsActivo { get; set; }
    }

    [Table(Name = "core.docentes")]
    public partial class Docentes
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "persona_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int PersonaId { get; set; }

        [Column(Name = "codigo_docente", DbType = "NVarChar(30) NOT NULL", CanBeNull = false)]
        public string CodigoDocente { get; set; }

        [Column(Name = "carrera_principal_id", DbType = "Int", CanBeNull = true)]
        public int? CarreraPrincipalId { get; set; }

        [Column(Name = "profesion", DbType = "NVarChar(150) NOT NULL", CanBeNull = false)]
        public string Profesion { get; set; }

        [Column(Name = "grado_academico", DbType = "NVarChar(100)", CanBeNull = true)]
        public string GradoAcademico { get; set; }

        [Column(Name = "condicion", DbType = "NVarChar(15)", CanBeNull = true)]
        public string Condicion { get; set; }
    }

    [Table(Name = "core.estudiantes")]
    public partial class Estudiantes
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "persona_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int PersonaId { get; set; }

        [Column(Name = "codigo_estudiante", DbType = "NVarChar(30) NOT NULL", CanBeNull = false)]
        public string CodigoEstudiante { get; set; }

        [Column(Name = "carrera_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int CarreraId { get; set; }

        [Column(Name = "periodo_ingreso_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int PeriodoIngresoId { get; set; }

        [Column(Name = "ciclo_actual", DbType = "NVarChar(5)", CanBeNull = true)]
        public string CicloActual { get; set; }

        [Column(Name = "turno", DbType = "NVarChar(10)", CanBeNull = true)]
        public string Turno { get; set; }

        [Column(Name = "condicion", DbType = "NVarChar(15)", CanBeNull = true)]
        public string Condicion { get; set; }

        [Column(Name = "seccion", DbType = "NVarChar(5)", CanBeNull = true)]
        public string Seccion { get; set; }
    }

    [Table(Name = "core.unidades_didacticas")]
    public partial class UnidadesDidacticas
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "carrera_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int CarreraId { get; set; }

        [Column(Name = "ciclo", DbType = "NVarChar(5) NOT NULL", CanBeNull = false)]
        public string Ciclo { get; set; }

        [Column(Name = "codigo", DbType = "NVarChar(20) NOT NULL", CanBeNull = false)]
        public string Codigo { get; set; }

        [Column(Name = "nombre", DbType = "NVarChar(150) NOT NULL", CanBeNull = false)]
        public string Nombre { get; set; }

        [Column(Name = "creditos", DbType = "Int", CanBeNull = true)]
        public int? Creditos { get; set; }

        [Column(Name = "horas_semanales", DbType = "Int", CanBeNull = true)]
        public int? HorasSemanales { get; set; }

        [Column(Name = "tipo", DbType = "NVarChar(30)", CanBeNull = true)]
        public string Tipo { get; set; }
    }

    [Table(Name = "core.carreras")]
    public partial class Carreras
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "codigo", DbType = "NVarChar(15) NOT NULL", CanBeNull = false)]
        public string Codigo { get; set; }

        [Column(Name = "nombre", DbType = "NVarChar(150) NOT NULL", CanBeNull = false)]
        public string Nombre { get; set; }

        [Column(Name = "total_semestres", DbType = "Int", CanBeNull = true)]
        public int? TotalSemestres { get; set; }

        [Column(Name = "modalidad", DbType = "NVarChar(20)", CanBeNull = true)]
        public string Modalidad { get; set; }

        [Column(Name = "estado", DbType = "Bit", CanBeNull = true)]
        public bool? Estado { get; set; }
    }

    [Table(Name = "core.aulas")]
    public partial class Aulas
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "codigo", DbType = "NVarChar(20) NOT NULL", CanBeNull = false)]
        public string Codigo { get; set; }

        [Column(Name = "pabellon", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string Pabellon { get; set; }

        [Column(Name = "aforo", DbType = "Int", CanBeNull = true)]
        public int? Aforo { get; set; }

        [Column(Name = "tipo", DbType = "NVarChar(30)", CanBeNull = true)]
        public string Tipo { get; set; }
    }

    [Table(Name = "core.periodos_academicos")]
    public partial class PeriodosAcademicos
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "codigo", DbType = "NVarChar(20) NOT NULL", CanBeNull = false)]
        public string Codigo { get; set; }

        [Column(Name = "fecha_inicio", DbType = "Date NOT NULL", CanBeNull = false)]
        public DateTime FechaInicio { get; set; }

        [Column(Name = "fecha_fin", DbType = "Date NOT NULL", CanBeNull = false)]
        public DateTime FechaFin { get; set; }

        [Column(Name = "es_activo", DbType = "Bit", CanBeNull = true)]
        public bool? EsActivo { get; set; }

        [Column(Name = "permite_matricula", DbType = "Bit", CanBeNull = true)]
        public bool? PermiteMatricula { get; set; }
    }

    [Table(Name = "mod02.clases_docente")]
    public partial class ClasesDocente
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "unidad_didactica_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int UnidadDidacticaId { get; set; }

        [Column(Name = "docente_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int DocenteId { get; set; }

        [Column(Name = "periodo_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int PeriodoId { get; set; }

        [Column(Name = "carrera_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int CarreraId { get; set; }

        [Column(Name = "ciclo", DbType = "NVarChar(5) NOT NULL", CanBeNull = false)]
        public string Ciclo { get; set; }

        [Column(Name = "turno", DbType = "NVarChar(10)", CanBeNull = true)]
        public string Turno { get; set; }

        [Column(Name = "seccion", DbType = "NVarChar(5)", CanBeNull = true)]
        public string Seccion { get; set; }

        [Column(Name = "aula_id", DbType = "Int", CanBeNull = true)]
        public int? AulaId { get; set; }

        [Column(Name = "total_sesiones_semanales", DbType = "Int", CanBeNull = true)]
        public int? TotalSesionesSemanales { get; set; }

        [Column(Name = "estado", DbType = "Bit", CanBeNull = true)]
        public bool? Estado { get; set; }

        [Column(Name = "creada_en", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? CreadaEn { get; set; }
    }

    [Table(Name = "mod02.sesiones_clase")]
    public partial class SesionesClase
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "clase_docente_id", DbType = "Int", CanBeNull = true)]
        public int? ClaseDocenteId { get; set; }

        [Column(Name = "unidad_didactica_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int UnidadDidacticaId { get; set; }

        [Column(Name = "docente_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int DocenteId { get; set; }

        [Column(Name = "periodo_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int PeriodoId { get; set; }

        [Column(Name = "aula_id", DbType = "Int", CanBeNull = true)]
        public int? AulaId { get; set; }

        [Column(Name = "fecha_clase", DbType = "Date NOT NULL", CanBeNull = false)]
        public DateTime FechaClase { get; set; }

        [Column(Name = "hora_inicio", DbType = "Time NOT NULL", CanBeNull = false)]
        public TimeSpan HoraInicio { get; set; }

        [Column(Name = "hora_fin", DbType = "Time NOT NULL", CanBeNull = false)]
        public TimeSpan HoraFin { get; set; }

        [Column(Name = "horas_pedagogicas", DbType = "Int", CanBeNull = true)]
        public int? HorasPedagogicas { get; set; }

        [Column(Name = "numero_semana", DbType = "Int", CanBeNull = true)]
        public int? NumeroSemana { get; set; }

        [Column(Name = "numero_sesion", DbType = "Int", CanBeNull = true)]
        public int? NumeroSesion { get; set; }

        [Column(Name = "es_semana_recuperacion", DbType = "Bit", CanBeNull = true)]
        public bool? EsSemanaRecuperacion { get; set; }

        [Column(Name = "tema_desarrollado", DbType = "NVarChar(255)", CanBeNull = true)]
        public string TemaDesarrollado { get; set; }

        [Column(Name = "observaciones_docente", DbType = "NVarChar(MAX)", CanBeNull = true)]
        public string ObservacionesDocente { get; set; }

        [Column(Name = "estado", DbType = "NVarChar(20)", CanBeNull = true)]
        public string Estado { get; set; }

        [Column(Name = "cerrada_en", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? CerradaEn { get; set; }

        [Column(Name = "creada_en", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? CreadaEn { get; set; }
    }

    [Table(Name = "mod02.asistencias")]
    public partial class Asistencias
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "sesion_clase_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int SesionClaseId { get; set; }

        [Column(Name = "estudiante_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int EstudianteId { get; set; }

        [Column(Name = "estado", DbType = "NVarChar(25)", CanBeNull = true)]
        public string Estado { get; set; }

        [Column(Name = "minutos_tardanza", DbType = "Int", CanBeNull = true)]
        public int? MinutosTardanza { get; set; }

        [Column(Name = "observacion", DbType = "NVarChar(255)", CanBeNull = true)]
        public string Observacion { get; set; }

        [Column(Name = "registrado_por_usuario_id", DbType = "Int", CanBeNull = true)]
        public int? RegistradoPorUsuarioId { get; set; }

        [Column(Name = "registrado_en", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? RegistradoEn { get; set; }

        [Column(Name = "modificado_en", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? ModificadoEn { get; set; }
    }

    [Table(Name = "mod02.justificaciones")]
    public partial class Justificaciones
    {
        [Column(Name = "id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int Id { get; set; }

        [Column(Name = "asistencia_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int AsistenciaId { get; set; }

        [Column(Name = "estudiante_id", DbType = "Int NOT NULL", CanBeNull = false)]
        public int EstudianteId { get; set; }

        [Column(Name = "motivo", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string Motivo { get; set; }

        [Column(Name = "descripcion", DbType = "NVarChar(MAX) NOT NULL", CanBeNull = false)]
        public string Descripcion { get; set; }

        [Column(Name = "documento_sustento_url", DbType = "NVarChar(255)", CanBeNull = true)]
        public string DocumentoSustentoUrl { get; set; }

        [Column(Name = "estado", DbType = "NVarChar(20)", CanBeNull = true)]
        public string Estado { get; set; }

        [Column(Name = "revisado_por_docente_id", DbType = "Int", CanBeNull = true)]
        public int? RevisadoPorDocenteId { get; set; }

        [Column(Name = "respuesta_observacion", DbType = "NVarChar(MAX)", CanBeNull = true)]
        public string RespuestaObservacion { get; set; }

        [Column(Name = "fecha_solicitud", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? FechaSolicitud { get; set; }

        [Column(Name = "fecha_resolucion", DbType = "DateTime2", CanBeNull = true)]
        public DateTime? FechaResolucion { get; set; }
    }

    [Table(Name = "mod02.v_resumen_asistencia_estudiante")]
    public partial class VResumenAsistenciaEstudiante
    {
        [Column(Name = "estudiante_id", DbType = "Int NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public int EstudianteId { get; set; }

        [Column(Name = "unidad_didactica_id", DbType = "Int NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public int UnidadDidacticaId { get; set; }

        [Column(Name = "periodo_id", DbType = "Int NOT NULL", IsPrimaryKey = true, CanBeNull = false)]
        public int PeriodoId { get; set; }

        [Column(Name = "unidad_didactica_nombre", DbType = "NVarChar(150) NOT NULL", CanBeNull = false)]
        public string UnidadDidacticaNombre { get; set; }

        [Column(Name = "unidad_didactica_codigo", DbType = "NVarChar(20) NOT NULL", CanBeNull = false)]
        public string UnidadDidacticaCodigo { get; set; }

        [Column(Name = "carrera_nombre", DbType = "NVarChar(150) NOT NULL", CanBeNull = false)]
        public string CarreraNombre { get; set; }

        [Column(Name = "ciclo", DbType = "NVarChar(5)", CanBeNull = true)]
        public string Ciclo { get; set; }

        [Column(Name = "turno", DbType = "NVarChar(10)", CanBeNull = true)]
        public string Turno { get; set; }

        [Column(Name = "total_horas_semestre", DbType = "Int", CanBeNull = true)]
        public int? TotalHorasSemestre { get; set; }

        [Column(Name = "cant_presentes", DbType = "Int", CanBeNull = true)]
        public int? CantPresentes { get; set; }

        [Column(Name = "cant_tardanzas", DbType = "Int", CanBeNull = true)]
        public int? CantTardanzas { get; set; }

        [Column(Name = "cant_faltas_justificadas", DbType = "Int", CanBeNull = true)]
        public int? CantFaltasJustificadas { get; set; }

        [Column(Name = "cant_faltas_injustificadas", DbType = "Int", CanBeNull = true)]
        public int? CantFaltasInjustificadas { get; set; }

        [Column(Name = "horas_falta_injustificada", DbType = "Int", CanBeNull = true)]
        public int? HorasFaltaInjustificada { get; set; }

        [Column(Name = "horas_asistidas", DbType = "Int", CanBeNull = true)]
        public int? HorasAsistidas { get; set; }

        [Column(Name = "total_sesiones_registradas", DbType = "Int", CanBeNull = true)]
        public int? TotalSesionesRegistradas { get; set; }

        [Column(Name = "porcentaje_inasistencia", DbType = "Decimal(10,2)", CanBeNull = true)]
        public decimal? PorcentajeInasistencia { get; set; }

        [Column(Name = "semaforo_estado", DbType = "VarChar(20) NOT NULL", CanBeNull = false)]
        public string SemaforoEstado { get; set; }

        [Column(Name = "horas_falta_disponibles", DbType = "Decimal(10,2)", CanBeNull = true)]
        public decimal? HorasFaltaDisponibles { get; set; }
    }
}
