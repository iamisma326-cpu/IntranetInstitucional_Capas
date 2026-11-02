using System;
using System.Collections.Generic;
using System.Linq;
using IntranetInstitucional_Capas.Entidades;

namespace IntranetInstitucional_Capas.AccesoDatos
{
    public class KpisModulo02
    {
        public int TotalClases { get; set; }
        public int TotalSesionesHoy { get; set; }
        public int TotalAlumnosDpi { get; set; }
        public int TotalJustificacionesPendientes { get; set; }
    }

    public class AsistenciaLinqDAL
    {

        public ClaseActivaDocenteDto ObtenerClaseActivaHoy(int docenteId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from s in db.SesionesClase
                                 join cd in db.ClasesDocente on s.ClaseDocenteId equals (int?)cd.Id
                                 join ud in db.UnidadesDidacticas on cd.UnidadDidacticaId equals ud.Id
                                 join c in db.Carreras on cd.CarreraId equals c.Id
                                 join a in db.Aulas on s.AulaId equals (int?)a.Id into aulas
                                 from a in aulas.DefaultIfEmpty()
                                 where s.DocenteId == docenteId
                                       && (s.Estado == "ABIERTA" || s.Estado == "PROGRAMADA")
                                 select new
                                 {
                                     SesionId = s.Id,
                                     ClaseDocenteId = cd.Id,
                                     cd.UnidadDidacticaId,
                                     UnidadDidacticaCodigo = ud.Codigo,
                                     UnidadDidacticaNombre = ud.Nombre,
                                     CarreraNombre = c.Nombre,
                                     cd.Ciclo,
                                     cd.Turno,
                                     cd.Seccion,
                                     AulaCodigo = a.Codigo,
                                     NumeroSemana = s.NumeroSemana ?? 0,
                                     NumeroSesion = s.NumeroSesion ?? 0,
                                     s.FechaClase,
                                     s.HoraInicio,
                                     s.HoraFin,
                                     EstadoSesion = s.Estado,
                                     TotalAlumnos = db.Estudiantes.Count(e =>
                                         e.CarreraId == cd.CarreraId &&
                                         e.CicloActual == cd.Ciclo &&
                                         e.Turno == cd.Turno &&
                                         e.Seccion == cd.Seccion)
                                 }).ToList();

                    var fila = filas
                        .Where(x => x.FechaClase == DateTime.Today)
                        .OrderBy(x => x.HoraInicio)
                        .FirstOrDefault();

                    if (fila == null)
                    {
                        fila = filas
                            .OrderByDescending(x => x.Ciclo)
                            .ThenByDescending(x => x.TotalAlumnos)
                            .ThenBy(x => x.NumeroSemana)
                            .ThenBy(x => x.NumeroSesion)
                            .FirstOrDefault();
                    }

                    if (fila == null) return null;

                    return new ClaseActivaDocenteDto
                    {
                        SesionId = fila.SesionId,
                        ClaseDocenteId = fila.ClaseDocenteId,
                        UnidadDidacticaId = fila.UnidadDidacticaId,
                        UnidadDidacticaCodigo = fila.UnidadDidacticaCodigo,
                        UnidadDidacticaNombre = fila.UnidadDidacticaNombre,
                        CarreraNombre = fila.CarreraNombre,
                        Ciclo = fila.Ciclo,
                        Turno = fila.Turno,
                        Seccion = fila.Seccion,
                        AulaCodigo = fila.AulaCodigo ?? "AULA-401",
                        NumeroSemana = fila.NumeroSemana,
                        NumeroSesion = fila.NumeroSesion,
                        EsSemana17Recuperacion = fila.NumeroSemana == 17,
                        EsSemana18Cierre = fila.NumeroSemana == 18,
                        FechaClase = fila.FechaClase,
                        HoraInicio = fila.HoraInicio,
                        HoraFin = fila.HoraFin,
                        TotalAlumnos = fila.TotalAlumnos,
                        EstadoSesion = fila.EstadoSesion
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener la clase activa: " + ex.Message);
                }
            }
        }

        public KpisModulo02 ObtenerKpis(int docenteId, bool soloDocente)
        {
            KpisModulo02 kpis = new KpisModulo02();

            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    IQueryable<ClasesDocente> clases = db.ClasesDocente.Where(cd => cd.Estado == true);
                    if (soloDocente) clases = clases.Where(cd => cd.DocenteId == docenteId);
                    kpis.TotalClases = clases.Count();

                    IQueryable<SesionesClase> sesiones = db.SesionesClase;
                    if (soloDocente) sesiones = sesiones.Where(s => s.DocenteId == docenteId);
                    kpis.TotalSesionesHoy = sesiones.Count(s => s.FechaClase == DateTime.Today);

                    IQueryable<VResumenAsistenciaEstudiante> resumen =
                        db.VResumenAsistenciaEstudiante.Where(r => r.SemaforoEstado == "DPI");

                    if (soloDocente)
                    {
                        kpis.TotalAlumnosDpi = (from r in resumen
                                                join cd in db.ClasesDocente.Where(c => c.DocenteId == docenteId)
                                                     on r.UnidadDidacticaId equals cd.UnidadDidacticaId
                                                select r.EstudianteId).Distinct().Count();
                    }
                    else
                    {
                        kpis.TotalAlumnosDpi = (from r in resumen
                                                join cd in db.ClasesDocente
                                                     on r.UnidadDidacticaId equals cd.UnidadDidacticaId
                                                select r.EstudianteId).Distinct().Count();
                    }

                    var justificaciones = from j in db.Justificaciones
                                          join a in db.Asistencias on j.AsistenciaId equals a.Id
                                          join s in db.SesionesClase on a.SesionClaseId equals s.Id
                                          where j.Estado == "PENDIENTE"
                                          select new { j.Id, s.DocenteId };

                    if (soloDocente) justificaciones = justificaciones.Where(x => x.DocenteId == docenteId);
                    kpis.TotalJustificacionesPendientes = justificaciones.Count();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener los KPIs: " + ex.Message);
                }
            }

            return kpis;
        }

        public List<ClaseDocenteCardDto> ObtenerClasesDocente(int docenteId, int? periodoId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var consulta = from cd in db.ClasesDocente
                                   join ud in db.UnidadesDidacticas on cd.UnidadDidacticaId equals ud.Id
                                   join c in db.Carreras on cd.CarreraId equals c.Id
                                   join a in db.Aulas on cd.AulaId equals (int?)a.Id into aulas
                                   from a in aulas.DefaultIfEmpty()
                                   where cd.DocenteId == docenteId && cd.Estado == true
                                   select new
                                   {
                                       ClaseId = cd.Id,
                                       cd.UnidadDidacticaId,
                                       UnidadDidacticaCodigo = ud.Codigo,
                                       UnidadDidacticaNombre = ud.Nombre,
                                       cd.CarreraId,
                                       CarreraNombre = c.Nombre,
                                       cd.Ciclo,
                                       cd.Turno,
                                       cd.Seccion,
                                       cd.AulaId,
                                       AulaCodigo = a.Codigo,
                                       TotalSesionesSemanales = cd.TotalSesionesSemanales ?? 0,
                                       HorasSemanales = ud.HorasSemanales ?? 0,
                                       cd.PeriodoId,
                                       TotalAlumnosMatriculados = db.Estudiantes.Count(e =>
                                           e.CarreraId == cd.CarreraId &&
                                           e.CicloActual == cd.Ciclo &&
                                           e.Turno == cd.Turno &&
                                           e.Seccion == cd.Seccion),
                                       SemanasRegistradas = db.SesionesClase
                                           .Where(s => s.ClaseDocenteId == cd.Id && s.Estado == "CERRADA")
                                           .Select(s => s.NumeroSemana)
                                           .Distinct()
                                           .Count(),
                                       UltimaSemanaRegistrada = (db.SesionesClase
                                           .Where(s => s.ClaseDocenteId == cd.Id && s.Estado == "CERRADA")
                                           .Max(s => s.NumeroSemana) ?? 0),
                                       ProximaSesionId = db.SesionesClase
                                           .Where(s => s.ClaseDocenteId == cd.Id && s.Estado == "ABIERTA")
                                           .OrderBy(s => s.FechaClase)
                                           .ThenBy(s => s.HoraInicio)
                                           .Select(s => (int?)s.Id)
                                           .FirstOrDefault()
                                   };

                    if (periodoId.HasValue)
                    {
                        consulta = consulta.Where(x => x.PeriodoId == periodoId.Value);
                    }

                    List<ClaseDocenteCardDto> lista = consulta
                        .ToList()
                        .OrderByDescending(x => x.Ciclo)
                        .ThenByDescending(x => x.TotalAlumnosMatriculados)
                        .ThenBy(x => x.UnidadDidacticaNombre)
                        .Select(x => new ClaseDocenteCardDto
                        {
                            ClaseId = x.ClaseId,
                            UnidadDidacticaId = x.UnidadDidacticaId,
                            UnidadDidacticaCodigo = x.UnidadDidacticaCodigo,
                            UnidadDidacticaNombre = x.UnidadDidacticaNombre,
                            CarreraId = x.CarreraId,
                            CarreraNombre = x.CarreraNombre,
                            Ciclo = x.Ciclo,
                            Turno = x.Turno,
                            Seccion = x.Seccion,
                            AulaId = x.AulaId,
                            AulaCodigo = x.AulaCodigo,
                            TotalSesionesSemanales = x.TotalSesionesSemanales,
                            HorasSemanales = x.HorasSemanales,
                            TotalAlumnosMatriculados = x.TotalAlumnosMatriculados,
                            SemanasRegistradas = x.SemanasRegistradas,
                            UltimaSemanaRegistrada = x.UltimaSemanaRegistrada,
                            ProximaSesionId = x.ProximaSesionId
                        })
                        .ToList();

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener las clases del docente: " + ex.Message);
                }
            }
        }

        public List<SesionClaseDto> ObtenerSesionesDeClase(int claseId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from s in db.SesionesClase
                                 join ud in db.UnidadesDidacticas on s.UnidadDidacticaId equals ud.Id
                                 join d in db.Docentes on s.DocenteId equals d.Id
                                 join p in db.Personas on d.PersonaId equals p.Id
                                 where s.ClaseDocenteId == claseId
                                 orderby s.NumeroSemana, s.NumeroSesion
                                 select new
                                 {
                                     s.Id,
                                     s.ClaseDocenteId,
                                     s.UnidadDidacticaId,
                                     UnidadDidacticaCodigo = ud.Codigo,
                                     UnidadDidacticaNombre = ud.Nombre,
                                     DocenteNombreCompleto = p.Apellidos + ", " + p.Nombres,
                                     s.FechaClase,
                                     s.HoraInicio,
                                     s.HoraFin,
                                     HorasPedagogicas = s.HorasPedagogicas ?? 0,
                                     NumeroSemana = s.NumeroSemana ?? 0,
                                     NumeroSesion = s.NumeroSesion ?? 0,
                                     s.TemaDesarrollado,
                                     s.ObservacionesDocente,
                                     Estado = s.Estado ?? "PROGRAMADA",
                                     s.CerradaEn,
                                     TotalAlumnos = db.Asistencias.Count(a => a.SesionClaseId == s.Id),
                                     TotalPresentes = db.Asistencias.Count(a => a.SesionClaseId == s.Id && a.Estado == "PRESENTE"),
                                     TotalTardanzas = db.Asistencias.Count(a => a.SesionClaseId == s.Id && a.Estado == "TARDANZA"),
                                     TotalFaltas = db.Asistencias.Count(a => a.SesionClaseId == s.Id && a.Estado == "FALTA_INJUSTIFICADA"),
                                     TotalJustificadas = db.Asistencias.Count(a => a.SesionClaseId == s.Id && a.Estado == "FALTA_JUSTIFICADA")
                                 }).ToList();

                    return filas.Select(x => new SesionClaseDto
                    {
                        Id = x.Id,
                        ClaseDocenteId = x.ClaseDocenteId,
                        UnidadDidacticaId = x.UnidadDidacticaId,
                        UnidadDidacticaCodigo = x.UnidadDidacticaCodigo,
                        UnidadDidacticaNombre = x.UnidadDidacticaNombre,
                        DocenteNombreCompleto = x.DocenteNombreCompleto,
                        FechaClase = x.FechaClase,
                        HoraInicio = x.HoraInicio,
                        HoraFin = x.HoraFin,
                        HorasPedagogicas = x.HorasPedagogicas,
                        NumeroSemana = x.NumeroSemana,
                        NumeroSesion = x.NumeroSesion,
                        TemaDesarrollado = x.TemaDesarrollado,
                        ObservacionesDocente = x.ObservacionesDocente,
                        Estado = x.Estado,
                        CerradaEn = x.CerradaEn,
                        TotalAlumnos = x.TotalAlumnos,
                        TotalPresentes = x.TotalPresentes,
                        TotalTardanzas = x.TotalTardanzas,
                        TotalFaltas = x.TotalFaltas,
                        TotalJustificadas = x.TotalJustificadas
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener las sesiones de la clase: " + ex.Message);
                }
            }
        }

        public List<SesionClaseDto> ObtenerSesionesDocente(int docenteId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from s in db.SesionesClase
                                 join ud in db.UnidadesDidacticas on s.UnidadDidacticaId equals ud.Id
                                 join d in db.Docentes on s.DocenteId equals d.Id
                                 join p in db.Personas on d.PersonaId equals p.Id
                                 where s.DocenteId == docenteId
                                 orderby s.FechaClase descending, s.HoraInicio
                                 select new
                                 {
                                     s.Id,
                                     s.ClaseDocenteId,
                                     s.UnidadDidacticaId,
                                     UnidadDidacticaCodigo = ud.Codigo,
                                     UnidadDidacticaNombre = ud.Nombre,
                                     DocenteNombreCompleto = p.Apellidos + ", " + p.Nombres,
                                     s.FechaClase,
                                     s.HoraInicio,
                                     s.HoraFin,
                                     HorasPedagogicas = s.HorasPedagogicas ?? 0,
                                     NumeroSemana = s.NumeroSemana ?? 0,
                                     NumeroSesion = s.NumeroSesion ?? 0,
                                     Estado = s.Estado ?? "PROGRAMADA"
                                 }).ToList();

                    return filas.Select(x => new SesionClaseDto
                    {
                        Id = x.Id,
                        ClaseDocenteId = x.ClaseDocenteId,
                        UnidadDidacticaId = x.UnidadDidacticaId,
                        UnidadDidacticaCodigo = x.UnidadDidacticaCodigo,
                        UnidadDidacticaNombre = x.UnidadDidacticaNombre,
                        DocenteNombreCompleto = x.DocenteNombreCompleto,
                        FechaClase = x.FechaClase,
                        HoraInicio = x.HoraInicio,
                        HoraFin = x.HoraFin,
                        HorasPedagogicas = x.HorasPedagogicas,
                        NumeroSemana = x.NumeroSemana,
                        NumeroSesion = x.NumeroSesion,
                        Estado = x.Estado
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener las sesiones del docente: " + ex.Message);
                }
            }
        }

        public List<SesionClaseDto> ObtenerSesionesRecientes(int limite)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from s in db.SesionesClase
                                 join ud in db.UnidadesDidacticas on s.UnidadDidacticaId equals ud.Id
                                 join d in db.Docentes on s.DocenteId equals d.Id
                                 join p in db.Personas on d.PersonaId equals p.Id
                                 join a in db.Aulas on s.AulaId equals (int?)a.Id into aulas
                                 from a in aulas.DefaultIfEmpty()
                                 orderby s.FechaClase descending, s.HoraInicio
                                 select new
                                 {
                                     s.Id,
                                     s.ClaseDocenteId,
                                     s.UnidadDidacticaId,
                                     UnidadDidacticaCodigo = ud.Codigo,
                                     UnidadDidacticaNombre = ud.Nombre,
                                     DocenteNombreCompleto = p.Apellidos + ", " + p.Nombres,
                                     s.FechaClase,
                                     s.HoraInicio,
                                     s.HoraFin,
                                     HorasPedagogicas = s.HorasPedagogicas ?? 0,
                                     NumeroSemana = s.NumeroSemana ?? 0,
                                     NumeroSesion = s.NumeroSesion ?? 0,
                                     Estado = s.Estado ?? "PROGRAMADA",
                                     AulaCodigo = a.Codigo
                                 }).Take(limite).ToList();

                    return filas.Select(x => new SesionClaseDto
                    {
                        Id = x.Id,
                        ClaseDocenteId = x.ClaseDocenteId,
                        UnidadDidacticaId = x.UnidadDidacticaId,
                        UnidadDidacticaCodigo = x.UnidadDidacticaCodigo,
                        UnidadDidacticaNombre = x.UnidadDidacticaNombre,
                        DocenteNombreCompleto = x.DocenteNombreCompleto,
                        FechaClase = x.FechaClase,
                        HoraInicio = x.HoraInicio,
                        HoraFin = x.HoraFin,
                        HorasPedagogicas = x.HorasPedagogicas,
                        NumeroSemana = x.NumeroSemana,
                        NumeroSesion = x.NumeroSesion,
                        Estado = x.Estado,
                        AulaCodigo = x.AulaCodigo
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener las sesiones recientes: " + ex.Message);
                }
            }
        }

        public SesionClaseDto ObtenerSesionPorId(int sesionId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var fila = (from s in db.SesionesClase
                                join cd in db.ClasesDocente on s.ClaseDocenteId equals (int?)cd.Id
                                join ud in db.UnidadesDidacticas on s.UnidadDidacticaId equals ud.Id
                                join c in db.Carreras on cd.CarreraId equals c.Id
                                join d in db.Docentes on s.DocenteId equals d.Id
                                join p in db.Personas on d.PersonaId equals p.Id
                                join a in db.Aulas on s.AulaId equals (int?)a.Id into aulas
                                from a in aulas.DefaultIfEmpty()
                                where s.Id == sesionId
                                select new
                                {
                                    s.Id,
                                    s.ClaseDocenteId,
                                    s.UnidadDidacticaId,
                                    UnidadDidacticaCodigo = ud.Codigo,
                                    UnidadDidacticaNombre = ud.Nombre,
                                    CarreraNombre = c.Nombre,
                                    cd.Ciclo,
                                    cd.Turno,
                                    cd.Seccion,
                                    s.DocenteId,
                                    DocenteNombreCompleto = p.Apellidos + ", " + p.Nombres,
                                    cd.PeriodoId,
                                    s.AulaId,
                                    AulaCodigo = a.Codigo,
                                    s.FechaClase,
                                    s.HoraInicio,
                                    s.HoraFin,
                                    HorasPedagogicas = s.HorasPedagogicas ?? 0,
                                    NumeroSemana = s.NumeroSemana ?? 0,
                                    NumeroSesion = s.NumeroSesion ?? 0,
                                    s.TemaDesarrollado,
                                    s.ObservacionesDocente,
                                    Estado = s.Estado ?? "PROGRAMADA",
                                    s.CerradaEn
                                }).FirstOrDefault();

                    if (fila == null) return null;

                    return new SesionClaseDto
                    {
                        Id = fila.Id,
                        ClaseDocenteId = fila.ClaseDocenteId,
                        UnidadDidacticaId = fila.UnidadDidacticaId,
                        UnidadDidacticaCodigo = fila.UnidadDidacticaCodigo,
                        UnidadDidacticaNombre = fila.UnidadDidacticaNombre,
                        CarreraNombre = fila.CarreraNombre,
                        Ciclo = fila.Ciclo,
                        Turno = fila.Turno,
                        Seccion = fila.Seccion,
                        DocenteId = fila.DocenteId,
                        DocenteNombreCompleto = fila.DocenteNombreCompleto,
                        PeriodoId = fila.PeriodoId,
                        AulaId = fila.AulaId,
                        AulaCodigo = fila.AulaCodigo,
                        FechaClase = fila.FechaClase,
                        HoraInicio = fila.HoraInicio,
                        HoraFin = fila.HoraFin,
                        HorasPedagogicas = fila.HorasPedagogicas,
                        NumeroSemana = fila.NumeroSemana,
                        NumeroSesion = fila.NumeroSesion,
                        TemaDesarrollado = fila.TemaDesarrollado,
                        ObservacionesDocente = fila.ObservacionesDocente,
                        Estado = fila.Estado,
                        CerradaEn = fila.CerradaEn
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener la sesion: " + ex.Message);
                }
            }
        }

        public MatrizAsistenciaViewModel ObtenerCabeceraClase(int claseId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var fila = (from cd in db.ClasesDocente
                                join ud in db.UnidadesDidacticas on cd.UnidadDidacticaId equals ud.Id
                                join c in db.Carreras on cd.CarreraId equals c.Id
                                join d in db.Docentes on cd.DocenteId equals d.Id
                                join p in db.Personas on d.PersonaId equals p.Id
                                where cd.Id == claseId
                                select new
                                {
                                    ClaseId = cd.Id,
                                    cd.UnidadDidacticaId,
                                    UnidadDidacticaCodigo = ud.Codigo,
                                    UnidadDidacticaNombre = ud.Nombre,
                                    CarreraNombre = c.Nombre,
                                    cd.Ciclo,
                                    cd.Turno,
                                    cd.Seccion,
                                    DocenteNombre = p.Apellidos + ", " + p.Nombres,
                                    cd.PeriodoId
                                }).FirstOrDefault();

                    if (fila == null) return null;

                    return new MatrizAsistenciaViewModel
                    {
                        ClaseId = fila.ClaseId,
                        UnidadDidacticaId = fila.UnidadDidacticaId,
                        UnidadDidacticaCodigo = fila.UnidadDidacticaCodigo,
                        UnidadDidacticaNombre = fila.UnidadDidacticaNombre,
                        CarreraNombre = fila.CarreraNombre,
                        Ciclo = fila.Ciclo,
                        Turno = fila.Turno,
                        Seccion = fila.Seccion,
                        DocenteNombre = fila.DocenteNombre,
                        PeriodoId = fila.PeriodoId
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener la cabecera de la clase: " + ex.Message);
                }
            }
        }

        public List<ColumnaSesionMatrizDto> ObtenerSesionesParaMatriz(int claseId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from s in db.SesionesClase
                                 where s.ClaseDocenteId == claseId
                                 orderby s.NumeroSemana, s.NumeroSesion
                                 select new
                                 {
                                     SesionId = s.Id,
                                     NumeroSemana = s.NumeroSemana ?? 0,
                                     NumeroSesion = s.NumeroSesion ?? 0,
                                     s.FechaClase,
                                     HorasPedagogicas = s.HorasPedagogicas ?? 0,
                                     Estado = s.Estado ?? "PROGRAMADA"
                                 }).ToList();

                    return filas.Select(x =>
                    {
                        ColumnaSesionMatrizDto col = new ColumnaSesionMatrizDto();
                        col.SesionId = x.SesionId;
                        col.SesionIds.Add(col.SesionId);
                        col.TieneSesion = true;
                        col.NumeroSemana = x.NumeroSemana;
                        col.NumeroSesion = x.NumeroSesion;
                        col.FechaClase = x.FechaClase;
                        col.HorasPedagogicas = x.HorasPedagogicas;
                        col.Estado = x.Estado;
                        return col;
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener las sesiones de la sabana: " + ex.Message);
                }
            }
        }

        public List<FilaAlumnoMatrizDto> ObtenerFilasAlumnos(int claseId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var clase = (from cd in db.ClasesDocente
                                 where cd.Id == claseId
                                 select new { cd.CarreraId, cd.Ciclo, cd.Turno, cd.Seccion }).FirstOrDefault();

                    if (clase == null) return new List<FilaAlumnoMatrizDto>();

                    var filas = (from e in db.Estudiantes
                                 join p in db.Personas on e.PersonaId equals p.Id
                                 where e.CarreraId == clase.CarreraId
                                       && e.CicloActual == clase.Ciclo
                                       && e.Turno == clase.Turno
                                       && e.Seccion == clase.Seccion
                                 orderby p.Apellidos, p.Nombres
                                 select new
                                 {
                                     EstudianteId = e.Id,
                                     e.CodigoEstudiante,
                                     p.Dni,
                                     NombreCompleto = p.Apellidos + ", " + p.Nombres
                                 }).ToList();

                    return filas.Select(x => new FilaAlumnoMatrizDto
                    {
                        EstudianteId = x.EstudianteId,
                        CodigoEstudiante = x.CodigoEstudiante,
                        Dni = x.Dni,
                        NombreCompleto = x.NombreCompleto
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener los alumnos de la sabana: " + ex.Message);
                }
            }
        }

        public Dictionary<int, Dictionary<int, string>> ObtenerAsistenciasDeClase(int claseId)
        {
            Dictionary<int, Dictionary<int, string>> mapa =
                new Dictionary<int, Dictionary<int, string>>();

            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from a in db.Asistencias
                                 join s in db.SesionesClase on a.SesionClaseId equals s.Id
                                 where s.ClaseDocenteId == claseId
                                 select new { a.EstudianteId, a.SesionClaseId, a.Estado }).ToList();

                    foreach (var fila in filas)
                    {
                        if (!mapa.ContainsKey(fila.EstudianteId))
                        {
                            mapa[fila.EstudianteId] = new Dictionary<int, string>();
                        }
                        mapa[fila.EstudianteId][fila.SesionClaseId] = fila.Estado;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener las asistencias: " + ex.Message);
                }
            }

            return mapa;
        }

        public List<AlumnoAsistenciaItemDto> ObtenerAlumnosParaSesion(int sesionId, int unidadDidacticaId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var clase = (from sc in db.SesionesClase
                                 join cd in db.ClasesDocente on sc.ClaseDocenteId equals (int?)cd.Id
                                 where sc.Id == sesionId
                                 select new { cd.CarreraId, cd.Ciclo, cd.Turno, cd.Seccion }).FirstOrDefault();

                    if (clase == null) return new List<AlumnoAsistenciaItemDto>();

                    var filas = (from e in db.Estudiantes
                                 join p in db.Personas on e.PersonaId equals p.Id
                                 where e.CarreraId == clase.CarreraId
                                       && e.CicloActual == clase.Ciclo
                                       && e.Turno == clase.Turno
                                       && e.Seccion == clase.Seccion
                                 join a in db.Asistencias on new { Sesion = sesionId, Estudiante = e.Id }
                                      equals new { Sesion = a.SesionClaseId, Estudiante = a.EstudianteId } into asistencias
                                 from a in asistencias.DefaultIfEmpty()
                                 join res in db.VResumenAsistenciaEstudiante on new { Estudiante = e.Id, Unidad = unidadDidacticaId }
                                      equals new { Estudiante = res.EstudianteId, Unidad = res.UnidadDidacticaId } into resumenes
                                 from res in resumenes.DefaultIfEmpty()
                                 orderby p.Apellidos, p.Nombres
                                 select new
                                 {
                                     EstudianteId = e.Id,
                                     e.PersonaId,
                                     e.CodigoEstudiante,
                                     p.Dni,
                                     p.Nombres,
                                     p.Apellidos,
                                     e.Turno,
                                     AsistenciaId = (int?)a.Id,
                                     EstadoAsistencia = a.Estado ?? "PRESENTE",
                                     MinutosTardanza = a.MinutosTardanza ?? 0,
                                     a.Observacion,
                                     HorasFaltaAcumuladasPrevias = res.HorasFaltaInjustificada ?? 0,
                                     TotalHorasSemestrales = res.TotalHorasSemestre ?? 72,
                                     PorcentajeInasistenciaPrevio = res.PorcentajeInasistencia ?? 0m,
                                     HorasFaltaDisponibles = res.HorasFaltaDisponibles ?? 0m
                                 }).ToList();

                    return filas.Select(x => new AlumnoAsistenciaItemDto
                    {
                        EstudianteId = x.EstudianteId,
                        PersonaId = x.PersonaId,
                        CodigoEstudiante = x.CodigoEstudiante,
                        Dni = x.Dni,
                        Nombres = x.Nombres,
                        Apellidos = x.Apellidos,
                        Turno = x.Turno,
                        AsistenciaId = x.AsistenciaId,
                        EstadoAsistencia = x.EstadoAsistencia,
                        MinutosTardanza = x.MinutosTardanza,
                        Observacion = x.Observacion,
                        HorasFaltaAcumuladasPrevias = x.HorasFaltaAcumuladasPrevias,
                        TotalHorasSemestrales = x.TotalHorasSemestrales,
                        PorcentajeInasistenciaPrevio = x.PorcentajeInasistenciaPrevio,
                        HorasFaltaDisponibles = Convert.ToInt32(x.HorasFaltaDisponibles)
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener los alumnos de la sesion: " + ex.Message);
                }
            }
        }

        public bool GuardarAsistenciaSesion(GuardarAsistenciaRequestDto request, int? usuarioId)
        {
            if (request == null) return false;

            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    SesionesClase sesion = db.SesionesClase.FirstOrDefault(s => s.Id == request.SesionClaseId);
                    if (sesion == null) return false;

                    sesion.TemaDesarrollado = request.TemaDesarrollado;
                    sesion.ObservacionesDocente = request.ObservacionesDocente;
                    sesion.Estado = request.CerrarSesion ? "CERRADA" : "ABIERTA";
                    sesion.CerradaEn = request.CerrarSesion ? (DateTime?)DateTime.Now : null;

                    if (request.Alumnos != null)
                    {
                        foreach (GuardarItemAsistenciaDto item in request.Alumnos)
                        {
                            Asistencias asistencia = db.Asistencias.FirstOrDefault(a =>
                                a.SesionClaseId == request.SesionClaseId && a.EstudianteId == item.EstudianteId);

                            if (asistencia == null)
                            {
                                asistencia = new Asistencias
                                {
                                    SesionClaseId = request.SesionClaseId,
                                    EstudianteId = item.EstudianteId,
                                    Estado = item.Estado,
                                    MinutosTardanza = item.MinutosTardanza,
                                    Observacion = item.Observacion,
                                    RegistradoPorUsuarioId = usuarioId,
                                    RegistradoEn = DateTime.Now,
                                    ModificadoEn = DateTime.Now
                                };
                                db.Asistencias.InsertOnSubmit(asistencia);
                            }
                            else
                            {
                                asistencia.Estado = item.Estado;
                                asistencia.MinutosTardanza = item.MinutosTardanza;
                                asistencia.Observacion = item.Observacion;
                                asistencia.ModificadoEn = DateTime.Now;
                            }
                        }
                    }

                    db.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("GuardarAsistenciaSesion (LINQ to SQL): " + ex.Message);
                    return false;
                }
            }
        }

        public bool GuardarMatrizAsistencia(GuardarMatrizRequestDto request, int? usuarioId)
        {
            if (request == null || request.Celdas == null) return false;

            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    foreach (GuardarCeldaMatrizDto celda in request.Celdas)
                    {
                        Asistencias asistencia = db.Asistencias.FirstOrDefault(a =>
                            a.SesionClaseId == celda.SesionId && a.EstudianteId == celda.EstudianteId);

                        if (asistencia == null)
                        {
                            asistencia = new Asistencias
                            {
                                SesionClaseId = celda.SesionId,
                                EstudianteId = celda.EstudianteId,
                                Estado = celda.Estado,
                                MinutosTardanza = 0,
                                RegistradoPorUsuarioId = usuarioId,
                                RegistradoEn = DateTime.Now,
                                ModificadoEn = DateTime.Now
                            };
                            db.Asistencias.InsertOnSubmit(asistencia);
                        }
                        else
                        {
                            asistencia.Estado = celda.Estado;
                            asistencia.ModificadoEn = DateTime.Now;
                        }
                    }

                    db.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("GuardarMatrizAsistencia (LINQ to SQL): " + ex.Message);
                    return false;
                }
            }
        }

        public bool ReprogramarSesiones(ReprogramarHorarioDto dto)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    List<SesionesClase> sesiones = db.SesionesClase
                        .Where(s => s.ClaseDocenteId == dto.ClaseDocenteId
                                    && (s.NumeroSemana ?? 0) >= dto.DesdeSemana
                                    && s.Estado == "PROGRAMADA")
                        .ToList();

                    foreach (SesionesClase sesion in sesiones)
                    {
                        sesion.HoraInicio = dto.NuevaHoraInicio;
                        sesion.HoraFin = dto.NuevaHoraFin;
                        if (dto.NuevoAulaId.HasValue) sesion.AulaId = dto.NuevoAulaId.Value;
                        sesion.ObservacionesDocente = "Reprogramado: " + (dto.MotivoReprogramacion ?? string.Empty);
                    }

                    db.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al reprogramar las sesiones: " + ex.Message);
                }
            }
        }

        public List<ResumenAsistenciaEstudianteDto> ObtenerResumenEstudiante(int estudianteId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from r in db.VResumenAsistenciaEstudiante
                                 where r.EstudianteId == estudianteId
                                 orderby r.UnidadDidacticaNombre
                                 select new
                                 {
                                     r.EstudianteId,
                                     r.UnidadDidacticaId,
                                     r.PeriodoId,
                                     r.UnidadDidacticaNombre,
                                     r.UnidadDidacticaCodigo,
                                     r.CarreraNombre,
                                     r.Ciclo,
                                     r.Turno,
                                     TotalHorasSemestre = r.TotalHorasSemestre ?? 0,
                                     CantPresentes = r.CantPresentes ?? 0,
                                     CantTardanzas = r.CantTardanzas ?? 0,
                                     CantFaltasJustificadas = r.CantFaltasJustificadas ?? 0,
                                     CantFaltasInjustificadas = r.CantFaltasInjustificadas ?? 0,
                                     HorasFaltaInjustificada = r.HorasFaltaInjustificada ?? 0,
                                     HorasAsistidas = r.HorasAsistidas ?? 0,
                                     TotalSesionesRegistradas = r.TotalSesionesRegistradas ?? 0,
                                     PorcentajeInasistencia = r.PorcentajeInasistencia ?? 0m,
                                     r.SemaforoEstado,
                                     HorasFaltaDisponibles = r.HorasFaltaDisponibles ?? 0m
                                 }).ToList();

                    return filas.Select(r => new ResumenAsistenciaEstudianteDto
                    {
                        EstudianteId = r.EstudianteId,
                        UnidadDidacticaId = r.UnidadDidacticaId,
                        PeriodoId = r.PeriodoId,
                        UnidadDidacticaNombre = r.UnidadDidacticaNombre,
                        UnidadDidacticaCodigo = r.UnidadDidacticaCodigo,
                        CarreraNombre = r.CarreraNombre,
                        Ciclo = r.Ciclo,
                        Turno = r.Turno,
                        TotalHorasSemestre = r.TotalHorasSemestre,
                        CantPresentes = r.CantPresentes,
                        CantTardanzas = r.CantTardanzas,
                        CantFaltasJustificadas = r.CantFaltasJustificadas,
                        CantFaltasInjustificadas = r.CantFaltasInjustificadas,
                        HorasFaltaInjustificada = r.HorasFaltaInjustificada,
                        HorasAsistidas = r.HorasAsistidas,
                        TotalSesionesRegistradas = r.TotalSesionesRegistradas,
                        PorcentajeInasistencia = r.PorcentajeInasistencia,
                        SemaforoEstado = r.SemaforoEstado,
                        HorasFaltaDisponibles = Convert.ToInt32(r.HorasFaltaDisponibles)
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener el resumen del estudiante: " + ex.Message);
                }
            }
        }

        public List<AsistenciaHistorialItemDto> ObtenerHistorialEstudiante(int estudianteId, int unidadDidacticaId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from a in db.Asistencias
                                 join s in db.SesionesClase on a.SesionClaseId equals s.Id
                                 join d in db.Docentes on s.DocenteId equals d.Id
                                 join p in db.Personas on d.PersonaId equals p.Id
                                 join j in db.Justificaciones on a.Id equals j.AsistenciaId into justificaciones
                                 from j in justificaciones.DefaultIfEmpty()
                                 where a.EstudianteId == estudianteId && s.UnidadDidacticaId == unidadDidacticaId
                                 orderby s.NumeroSemana, s.NumeroSesion
                                 select new
                                 {
                                     AsistenciaId = a.Id,
                                     SesionId = s.Id,
                                     s.FechaClase,
                                     NumeroSemana = s.NumeroSemana ?? 0,
                                     NumeroSesion = s.NumeroSesion ?? 0,
                                     HorasPedagogicas = s.HorasPedagogicas ?? 0,
                                     s.TemaDesarrollado,
                                     DocenteNombre = p.Apellidos + ", " + p.Nombres,
                                     Estado = a.Estado ?? "PRESENTE",
                                     MinutosTardanza = a.MinutosTardanza ?? 0,
                                     a.Observacion,
                                     TieneJustificacion = j != null,
                                     EstadoJustificacion = j.Estado
                                 }).ToList();

                    return filas.Select(x => new AsistenciaHistorialItemDto
                    {
                        AsistenciaId = x.AsistenciaId,
                        SesionId = x.SesionId,
                        FechaClase = x.FechaClase,
                        NumeroSemana = x.NumeroSemana,
                        NumeroSesion = x.NumeroSesion,
                        HorasPedagogicas = x.HorasPedagogicas,
                        TemaDesarrollado = x.TemaDesarrollado,
                        DocenteNombre = x.DocenteNombre,
                        Estado = x.Estado,
                        MinutosTardanza = x.MinutosTardanza,
                        Observacion = x.Observacion,
                        TieneJustificacion = x.TieneJustificacion,
                        EstadoJustificacion = x.EstadoJustificacion
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener el historial: " + ex.Message);
                }
            }
        }

        public List<JustificacionDto> ObtenerJustificaciones(int? estudianteId, string estado)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var consulta = from j in db.Justificaciones
                                   join e in db.Estudiantes on j.EstudianteId equals e.Id
                                   join pe in db.Personas on e.PersonaId equals pe.Id
                                   join a in db.Asistencias on j.AsistenciaId equals a.Id
                                   join s in db.SesionesClase on a.SesionClaseId equals s.Id
                                   join ud in db.UnidadesDidacticas on s.UnidadDidacticaId equals ud.Id
                                   join d in db.Docentes on s.DocenteId equals d.Id
                                   join pd in db.Personas on d.PersonaId equals pd.Id
                                   select new
                                   {
                                       j.Id,
                                       j.AsistenciaId,
                                       j.EstudianteId,
                                       EstudianteNombre = pe.Apellidos + ", " + pe.Nombres,
                                       e.CodigoEstudiante,
                                       UnidadDidacticaNombre = ud.Nombre,
                                       s.FechaClase,
                                       NumeroSemana = s.NumeroSemana ?? 0,
                                       j.Motivo,
                                       j.Descripcion,
                                       j.DocumentoSustentoUrl,
                                       j.Estado,
                                       DocenteNombre = pd.Apellidos + ", " + pd.Nombres,
                                       j.RespuestaObservacion,
                                       j.FechaSolicitud,
                                       j.FechaResolucion
                                   };

                    if (estudianteId.HasValue)
                    {
                        consulta = consulta.Where(x => x.EstudianteId == estudianteId.Value);
                    }
                    if (!string.IsNullOrEmpty(estado))
                    {
                        consulta = consulta.Where(x => x.Estado == estado);
                    }

                    var filas = consulta.OrderByDescending(x => x.FechaSolicitud).ToList();

                    return filas.Select(x => new JustificacionDto
                    {
                        Id = x.Id,
                        AsistenciaId = x.AsistenciaId,
                        EstudianteId = x.EstudianteId,
                        EstudianteNombre = x.EstudianteNombre,
                        EstudianteCodigo = x.CodigoEstudiante,
                        UnidadDidacticaNombre = x.UnidadDidacticaNombre,
                        FechaClase = x.FechaClase,
                        NumeroSemana = x.NumeroSemana,
                        Motivo = x.Motivo,
                        Descripcion = x.Descripcion,
                        DocumentoSustentoUrl = x.DocumentoSustentoUrl,
                        Estado = x.Estado,
                        DocenteNombre = x.DocenteNombre,
                        RespuestaObservacion = x.RespuestaObservacion,
                        FechaSolicitud = x.FechaSolicitud ?? DateTime.MinValue,
                        FechaResolucion = x.FechaResolucion
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener las justificaciones: " + ex.Message);
                }
            }
        }

        public bool SolicitarJustificacion(CrearJustificacionDto dto, int estudianteId)
        {
            if (dto == null) return false;

            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    Justificaciones justificacion = new Justificaciones
                    {
                        AsistenciaId = dto.AsistenciaId,
                        EstudianteId = estudianteId,
                        Motivo = dto.Motivo,
                        Descripcion = dto.Descripcion,
                        DocumentoSustentoUrl = dto.DocumentoSustentoUrl,
                        Estado = "PENDIENTE",
                        FechaSolicitud = DateTime.Now
                    };

                    db.Justificaciones.InsertOnSubmit(justificacion);
                    db.SubmitChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al registrar la justificacion: " + ex.Message);
                }
            }
        }

        public bool ResolverJustificacion(ResolverJustificacionDto dto, int docenteId)
        {
            if (dto == null) return false;

            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    Justificaciones justificacion = db.Justificaciones
                        .FirstOrDefault(j => j.Id == dto.JustificacionId);

                    if (justificacion == null) return false;

                    justificacion.Estado = dto.Aprobada ? "APROBADA" : "RECHAZADA";
                    justificacion.RespuestaObservacion = dto.Observacion;
                    justificacion.RevisadoPorDocenteId = docenteId;
                    justificacion.FechaResolucion = DateTime.Now;

                    if (dto.Aprobada)
                    {
                        Asistencias asistencia = db.Asistencias
                            .FirstOrDefault(a => a.Id == justificacion.AsistenciaId);

                        if (asistencia != null)
                        {
                            asistencia.Estado = "FALTA_JUSTIFICADA";
                            asistencia.ModificadoEn = DateTime.Now;
                        }
                    }

                    db.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al resolver la justificacion: " + ex.Message);
                }
            }
        }

        public List<AlertaDpiDto> ObtenerAlertasDpi()
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var filas = (from r in db.VResumenAsistenciaEstudiante
                                 join e in db.Estudiantes on r.EstudianteId equals e.Id
                                 join p in db.Personas on e.PersonaId equals p.Id
                                 where r.SemaforoEstado == "DPI"
                                       || r.SemaforoEstado == "RIESGO_ALTO"
                                       || r.SemaforoEstado == "ALERTA"
                                 orderby r.PorcentajeInasistencia descending
                                 select new
                                 {
                                     EstudianteId = r.EstudianteId,
                                     EstudianteNombre = p.Apellidos + ", " + p.Nombres,
                                     e.CodigoEstudiante,
                                     r.CarreraNombre,
                                     r.Ciclo,
                                     r.Turno,
                                     r.UnidadDidacticaId,
                                     r.UnidadDidacticaNombre,
                                     HorasFalta = r.HorasFaltaInjustificada ?? 0,
                                     TotalHoras = r.TotalHorasSemestre ?? 0,
                                     PorcentajeInasistencia = r.PorcentajeInasistencia ?? 0m,
                                     r.SemaforoEstado
                                 }).ToList();

                    return filas.Select(x => new AlertaDpiDto
                    {
                        EstudianteId = x.EstudianteId,
                        EstudianteNombre = x.EstudianteNombre,
                        EstudianteCodigo = x.CodigoEstudiante,
                        CarreraNombre = x.CarreraNombre,
                        Ciclo = x.Ciclo,
                        Turno = x.Turno,
                        UnidadDidacticaId = x.UnidadDidacticaId,
                        UnidadDidacticaNombre = x.UnidadDidacticaNombre,
                        DocenteNombre = "Planta Docente",
                        HorasFalta = x.HorasFalta,
                        TotalHoras = x.TotalHoras,
                        PorcentajeInasistencia = x.PorcentajeInasistencia,
                        SemaforoEstado = x.SemaforoEstado
                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener las alertas DPI: " + ex.Message);
                }
            }
        }
    }
}
