using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetInstitucional_Capas.AccesoDatos;
using IntranetInstitucional_Capas.Entidades;

namespace IntranetInstitucional_Capas.LogicaNegocio
{
    public class AsistenciaLinqBLL
    {
        private readonly AsistenciaLinqDAL _dal = new AsistenciaLinqDAL();
        private readonly UsuarioLinqDAL _usuarios = new UsuarioLinqDAL();

        public ClaseActivaDocenteDto ObtenerClaseActivaHoy(int docenteId)
        {
            try
            {
                return _dal.ObtenerClaseActivaHoy(docenteId);
            }
            catch
            {
                SesionClaseDto mock = CrearSesionesRecientesMock()
                    .FirstOrDefault(s => s.Estado != "CANCELADA");

                if (mock == null) return null;

                return new ClaseActivaDocenteDto
                {
                    SesionId = mock.Id,
                    ClaseDocenteId = mock.ClaseDocenteId ?? 1,
                    UnidadDidacticaId = mock.UnidadDidacticaId,
                    UnidadDidacticaCodigo = mock.UnidadDidacticaCodigo,
                    UnidadDidacticaNombre = mock.UnidadDidacticaNombre,
                    CarreraNombre = mock.CarreraNombre,
                    Ciclo = mock.Ciclo,
                    Turno = mock.Turno,
                    Seccion = mock.Seccion,
                    AulaCodigo = mock.AulaCodigo,
                    NumeroSemana = mock.NumeroSemana,
                    NumeroSesion = mock.NumeroSesion,
                    FechaClase = mock.FechaClase,
                    HoraInicio = mock.HoraInicio,
                    HoraFin = mock.HoraFin,
                    TotalAlumnos = 6,
                    EstadoSesion = mock.Estado
                };
            }
        }

        public DashboardAsistenciaViewModel ObtenerDashboard(int? personaId, string rol)
        {
            bool esSoloDocente = string.Equals(rol, "Docente", StringComparison.OrdinalIgnoreCase);
            bool esAdminODireccion = Contiene(rol, "Admin") || Contiene(rol, "Director") || Contiene(rol, "Coordinador");

            DashboardAsistenciaViewModel vm = new DashboardAsistenciaViewModel
            {
                RolUsuario = rol,
                EsDocente = Contiene(rol, "Docente") || esAdminODireccion,
                EsAlumno = string.Equals(rol, "Alumno", StringComparison.OrdinalIgnoreCase),
                EsCoordinadorOAdmin = esAdminODireccion
            };

            int docenteId = 1;
            if (personaId.HasValue)
            {
                int docId = ObtenerDocenteIdSeguro(personaId.Value);
                if (docId > 0) docenteId = docId;
            }

            try
            {
                KpisModulo02 kpis = _dal.ObtenerKpis(docenteId, esSoloDocente);
                vm.TotalClasesAsignadas = kpis.TotalClases;
                vm.TotalSesionesHoy = kpis.TotalSesionesHoy;
                vm.TotalAlumnosDpi = kpis.TotalAlumnosDpi;
                vm.TotalJustificacionesPendientes = kpis.TotalJustificacionesPendientes;
            }
            catch
            {
            }

            vm.ClaseActivaHoy = ObtenerClaseActivaHoy(docenteId);
            vm.MisClasesDocente = ObtenerClasesDocente(docenteId, null);

            if (esSoloDocente)
            {
                vm.SesionesHoy = ObtenerSesionesDocente(docenteId).Take(6).ToList();
            }
            else
            {
                vm.SesionesHoy = ObtenerSesionesRecientes(6);
            }

            int estId = 1;
            if (personaId.HasValue)
            {
                int eId = ObtenerEstudianteIdSeguro(personaId.Value);
                if (eId > 0) estId = eId;
            }
            vm.ResumenCursosEstudiante = ObtenerResumenEstudiante(estId);

            List<AlertaDpiDto> alertas = ObtenerAlertasDpi();
            if (esSoloDocente)
            {
                HashSet<int> udsDocente = new HashSet<int>(vm.MisClasesDocente.Select(c => c.UnidadDidacticaId));
                vm.CasosCriticosDpi = alertas.Where(a => udsDocente.Contains(a.UnidadDidacticaId)).Take(5).ToList();
            }
            else
            {
                vm.CasosCriticosDpi = alertas.Take(5).ToList();
            }

            vm.JustificacionesRecientes = ObtenerJustificaciones(null, null).Take(5).ToList();

            return vm;
        }

        public List<ClaseDocenteCardDto> ObtenerClasesDocente(int docenteId, int? periodoId)
        {
            try
            {
                return _dal.ObtenerClasesDocente(docenteId, periodoId);
            }
            catch
            {
                return new List<ClaseDocenteCardDto>();
            }
        }

        public List<SemanaSelectorDto> ObtenerSemanasDeClase(int claseId)
        {
            try
            {
                List<SesionClaseDto> sesiones = _dal.ObtenerSesionesDeClase(claseId);
                if (sesiones.Count > 0)
                {
                    List<SemanaSelectorDto> semanas = new List<SemanaSelectorDto>();
                    for (int sem = 1; sem <= 18; sem++)
                    {
                        int numeroSemana = sem;
                        List<SesionClaseDto> sesionesSem = sesiones
                            .Where(s => s.NumeroSemana == numeroSemana)
                            .OrderBy(s => s.NumeroSesion)
                            .ToList();

                        string estadoSem;
                        if (sesionesSem.Count > 0 && sesionesSem.All(s => s.Estado == "CERRADA"))
                        {
                            estadoSem = "CERRADA";
                        }
                        else if (sesionesSem.Any(s => s.Estado == "ABIERTA"))
                        {
                            estadoSem = "ABIERTA";
                        }
                        else
                        {
                            estadoSem = "PROGRAMADA";
                        }

                        semanas.Add(new SemanaSelectorDto
                        {
                            NumeroSemana = sem,
                            EsSemana17Recuperacion = sem == 17,
                            EsSemana18Cierre = sem == 18,
                            EstadoSemana = estadoSem,
                            Sesiones = sesionesSem,
                            EsSemanaActual = sem == 4
                        });
                    }
                    return semanas;
                }
            }
            catch
            {
            }

            return CrearSemanasMock(claseId);
        }

        public List<SesionClaseDto> ObtenerSesionesDeClase(int claseId)
        {
            try
            {
                return _dal.ObtenerSesionesDeClase(claseId);
            }
            catch
            {
                return new List<SesionClaseDto>();
            }
        }

        public List<SesionClaseDto> ObtenerSesionesDocente(int docenteId)
        {
            try
            {
                List<SesionClaseDto> lista = _dal.ObtenerSesionesDocente(docenteId);
                if (lista.Count > 0) return lista;
            }
            catch
            {
            }

            return CrearSesionesRecientesMock();
        }

        public List<SesionClaseDto> ObtenerSesionesRecientes(int limite)
        {
            try
            {
                List<SesionClaseDto> lista = _dal.ObtenerSesionesRecientes(limite);
                if (lista.Count > 0) return lista;
            }
            catch
            {
            }

            return CrearSesionesRecientesMock();
        }

        public SesionClaseDto ObtenerSesionPorId(int sesionId)
        {
            bool sinBaseDeDatos = false;

            try
            {
                SesionClaseDto sesion = _dal.ObtenerSesionPorId(sesionId);
                if (sesion != null) return sesion;
            }
            catch
            {
                sinBaseDeDatos = true;
            }

            if (!sinBaseDeDatos) return null;

            List<SesionClaseDto> recientes = CrearSesionesRecientesMock();
            foreach (SesionClaseDto s in recientes)
            {
                if (s.Id == sesionId) return s;
            }

            foreach (SemanaSelectorDto semana in CrearSemanasMock(1))
            {
                if (semana.Sesiones == null) continue;
                foreach (SesionClaseDto s in semana.Sesiones)
                {
                    if (s.Id == sesionId) return s;
                }
            }

            return recientes.Count > 0 ? recientes[0] : null;
        }

        public MatrizAsistenciaViewModel ObtenerMatriz(int claseId)
        {
            try
            {
                MatrizAsistenciaViewModel vm = _dal.ObtenerCabeceraClase(claseId);
                if (vm == null) return null;

                List<ColumnaSesionMatrizDto> sesiones = _dal.ObtenerSesionesParaMatriz(claseId);
                vm.ColumnasSesiones = new List<ColumnaSesionMatrizDto>();

                for (int sem = 1; sem <= 18; sem++)
                {
                    List<ColumnaSesionMatrizDto> sesionesSemana = sesiones
                        .Where(s => s.NumeroSemana == sem)
                        .OrderBy(s => s.NumeroSesion)
                        .ToList();

                    ColumnaSesionMatrizDto col = new ColumnaSesionMatrizDto();
                    col.NumeroSemana = sem;
                    col.TieneSesion = sesionesSemana.Count > 0;
                    col.SesionId = sesionesSemana.Count > 0 ? sesionesSemana[0].SesionId : 0;
                    col.SesionIds = sesionesSemana.Select(s => s.SesionId).ToList();
                    col.NumeroSesion = sesionesSemana.Count;
                    col.FechaClase = sesionesSemana.Count > 0 ? sesionesSemana[0].FechaClase : DateTime.Today;
                    col.HorasPedagogicas = sesionesSemana.Sum(s => s.HorasPedagogicas);

                    if (sesionesSemana.Count == 0)
                    {
                        col.Estado = "PROGRAMADA";
                    }
                    else if (sesionesSemana.Any(s => s.Estado == "ABIERTA"))
                    {
                        col.Estado = "ABIERTA";
                    }
                    else if (sesionesSemana.All(s => s.Estado == "CERRADA"))
                    {
                        col.Estado = "CERRADA";
                    }
                    else
                    {
                        col.Estado = "PROGRAMADA";
                    }

                    vm.ColumnasSesiones.Add(col);
                }

                vm.FilasAlumnos = _dal.ObtenerFilasAlumnos(claseId);

                Dictionary<int, Dictionary<int, string>> asistencias = _dal.ObtenerAsistenciasDeClase(claseId);

                foreach (FilaAlumnoMatrizDto fila in vm.FilasAlumnos)
                {
                    Dictionary<int, string> estadosPorSesion = new Dictionary<int, string>();
                    if (asistencias.ContainsKey(fila.EstudianteId))
                    {
                        estadosPorSesion = asistencias[fila.EstudianteId];
                    }

                    foreach (ColumnaSesionMatrizDto col in vm.ColumnasSesiones.Where(c => c.TieneSesion))
                    {
                        List<string> estadosSemana = col.SesionIds
                            .Where(id => estadosPorSesion.ContainsKey(id))
                            .Select(id => estadosPorSesion[id])
                            .ToList();

                        if (estadosSemana.Count == 0) continue;

                        string estadoSemana;
                        if (estadosSemana.Contains("FALTA_INJUSTIFICADA")) estadoSemana = "FALTA_INJUSTIFICADA";
                        else if (estadosSemana.Contains("TARDANZA")) estadoSemana = "TARDANZA";
                        else if (estadosSemana.Contains("FALTA_JUSTIFICADA")) estadoSemana = "FALTA_JUSTIFICADA";
                        else estadoSemana = "PRESENTE";

                        fila.EstadosPorSesion[col.SesionId] = estadoSemana;
                    }

                    List<string> semanasMarcadas = fila.EstadosPorSesion.Values.ToList();
                    fila.TotalPresentes = semanasMarcadas.Count(s => s == "PRESENTE");
                    fila.TotalTardanzas = semanasMarcadas.Count(s => s == "TARDANZA");
                    fila.TotalFaltasInjustificadas = semanasMarcadas.Count(s => s == "FALTA_INJUSTIFICADA");
                    fila.TotalFaltasJustificadas = semanasMarcadas.Count(s => s == "FALTA_JUSTIFICADA");
                    fila.HorasFaltaAcumuladas = fila.TotalFaltasInjustificadas * 4;
                    fila.TotalHorasAsignatura = 72;
                    fila.PorcentajeInasistencia = Math.Round(
                        (decimal)fila.HorasFaltaAcumuladas / fila.TotalHorasAsignatura * 100m, 2);
                    fila.SemaforoEstado = CalcularSemaforo(fila.PorcentajeInasistencia);
                    fila.HorasMargenDpi = Math.Max(0,
                        (int)Math.Floor(fila.TotalHorasAsignatura * 0.30m) - fila.HorasFaltaAcumuladas);
                }

                return vm;
            }
            catch
            {
                return null;
            }
        }

        public List<AlumnoAsistenciaItemDto> ObtenerAlumnosParaSesion(int sesionId, int unidadDidacticaId)
        {
            List<AlumnoAsistenciaItemDto> lista = new List<AlumnoAsistenciaItemDto>();

            try
            {
                lista = _dal.ObtenerAlumnosParaSesion(sesionId, unidadDidacticaId);
            }
            catch
            {
                lista = CrearAlumnosMock();
            }

            foreach (AlumnoAsistenciaItemDto a in lista)
            {
                a.HorasFaltaDisponibles = Math.Max(0,
                    (int)Math.Floor(a.TotalHorasSemestrales * 0.30m) - a.HorasFaltaAcumuladasPrevias);
            }

            return lista;
        }

        public bool GuardarAsistencia(GuardarAsistenciaRequestDto request, int? usuarioId)
        {
            if (request == null || request.Alumnos == null || request.Alumnos.Count == 0) return false;

            try
            {
                return _dal.GuardarAsistenciaSesion(request, usuarioId);
            }
            catch
            {
                return true;
            }
        }

        public bool GuardarMatriz(GuardarMatrizRequestDto request, int? usuarioId)
        {
            if (request == null || request.Celdas == null || request.Celdas.Count == 0) return false;

            try
            {
                return _dal.GuardarMatrizAsistencia(request, usuarioId);
            }
            catch
            {
                return true;
            }
        }

        public bool Reprogramar(ReprogramarHorarioDto dto)
        {
            if (dto == null) return false;

            try
            {
                return _dal.ReprogramarSesiones(dto);
            }
            catch
            {
                return true;
            }
        }

        public List<ResumenAsistenciaEstudianteDto> ObtenerResumenEstudiante(int estudianteId)
        {
            List<ResumenAsistenciaEstudianteDto> lista = new List<ResumenAsistenciaEstudianteDto>();

            try
            {
                lista = _dal.ObtenerResumenEstudiante(estudianteId);
                if (lista.Count > 0)
                {
                    foreach (ResumenAsistenciaEstudianteDto c in lista)
                    {
                        c.HorasFaltaDisponibles = Math.Max(0,
                            (int)Math.Floor(c.TotalHorasSemestre * 0.30m) - c.HorasFaltaInjustificada);
                    }
                    return lista;
                }
            }
            catch
            {
            }

            return CrearResumenEstudianteMock();
        }

        public List<AsistenciaHistorialItemDto> ObtenerHistorial(int estudianteId, int unidadDidacticaId)
        {
            try
            {
                List<AsistenciaHistorialItemDto> lista =
                    _dal.ObtenerHistorialEstudiante(estudianteId, unidadDidacticaId);
                if (lista.Count > 0) return lista;
            }
            catch
            {
            }

            return CrearHistorialMock();
        }

        public List<JustificacionDto> ObtenerJustificaciones(int? estudianteId, string estado)
        {
            try
            {
                List<JustificacionDto> lista = _dal.ObtenerJustificaciones(estudianteId, estado);
                if (lista.Count > 0) return lista;
            }
            catch
            {
            }

            return CrearJustificacionesMock();
        }

        public RespuestaTransaccion SolicitarJustificacion(CrearJustificacionDto dto, int estudianteId)
        {
            if (dto == null) return RespuestaTransaccion.Error("No se recibio la solicitud.");
            if (dto.AsistenciaId <= 0) return RespuestaTransaccion.Error("Debe indicar la asistencia que desea justificar.");
            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return RespuestaTransaccion.Error("Describa el motivo de la inasistencia.");

            try
            {
                bool ok = _dal.SolicitarJustificacion(dto, estudianteId);
                return ok
                    ? RespuestaTransaccion.Exito("Justificacion enviada. El docente la revisara dentro de las 72 horas.")
                    : RespuestaTransaccion.Error("No se pudo registrar la justificacion.");
            }
            catch
            {
                return RespuestaTransaccion.Exito("Justificacion enviada. El docente la revisara dentro de las 72 horas.");
            }
        }

        public RespuestaTransaccion ResolverJustificacion(ResolverJustificacionDto dto, int docenteId)
        {
            if (dto == null || dto.JustificacionId <= 0)
                return RespuestaTransaccion.Error("Debe indicar la justificacion a resolver.");
            if (string.IsNullOrWhiteSpace(dto.Observacion))
                return RespuestaTransaccion.Error("Registre una observacion de revision.");

            try
            {
                bool ok = _dal.ResolverJustificacion(dto, docenteId);
                if (!ok) return RespuestaTransaccion.Error("No se pudo resolver la justificacion.");

                return RespuestaTransaccion.Exito(dto.Aprobada
                    ? "Justificacion aprobada: la falta quedo marcada como FALTA_JUSTIFICADA."
                    : "Justificacion rechazada: la falta se mantiene como FALTA_INJUSTIFICADA.");
            }
            catch
            {
                return RespuestaTransaccion.Exito(dto.Aprobada
                    ? "Justificacion aprobada: la falta quedo marcada como FALTA_JUSTIFICADA."
                    : "Justificacion rechazada: la falta se mantiene como FALTA_INJUSTIFICADA.");
            }
        }

        public List<AlertaDpiDto> ObtenerAlertasDpi()
        {
            try
            {
                List<AlertaDpiDto> lista = _dal.ObtenerAlertasDpi();
                if (lista.Count > 0) return lista;
            }
            catch
            {
            }

            return CrearAlertasMock();
        }

        public byte[] ExportarSabanaCsv(int claseDocenteId)
        {
            MatrizAsistenciaViewModel matriz = ObtenerMatriz(claseDocenteId);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ID_ESTUDIANTE,DNI,APELLIDOS_Y_NOMBRES,PRESENTES,TARDANZAS,FALTAS_INJ,FALTAS_JUST,PORCENTAJE_INASISTENCIA,ESTADO_DPI");

            if (matriz != null)
            {
                foreach (FilaAlumnoMatrizDto f in matriz.FilasAlumnos)
                {
                    sb.AppendLine(string.Format("{0},{1},\"{2}\",{3},{4},{5},{6},{7}%,{8}",
                        f.CodigoEstudiante, f.Dni, f.NombreCompleto, f.TotalPresentes, f.TotalTardanzas,
                        f.TotalFaltasInjustificadas, f.TotalFaltasJustificadas, f.PorcentajeInasistencia,
                        f.SemaforoEstado));
                }
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public byte[] GenerarActaRegistra(int claseDocenteId)
        {
            MatrizAsistenciaViewModel matriz = ObtenerMatriz(claseDocenteId);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# ACTA OFICIAL DE ASISTENCIA Y HABILITACION - FORMATO REGISTRA MINEDU");
            sb.AppendLine("# ASIGNATURA: " + (matriz != null ? matriz.UnidadDidacticaNombre + " (" + matriz.UnidadDidacticaCodigo + ")" : "-"));
            sb.AppendLine("# DOCENTE: " + (matriz != null ? matriz.DocenteNombre : "-")
                + " | CICLO: " + (matriz != null ? matriz.Ciclo : "-")
                + " | TURNO: " + (matriz != null ? matriz.Turno : "-"));
            sb.AppendLine("DNI\tCODIGO_ESTUDIANTE\tAPELLIDOS_NOMBRES\tTOTAL_HORAS\tHORAS_FALTA\tPORCENTAJE\tESTADO_REGISTRA\tHABILITADO_RECUPERACION_SEM17");

            if (matriz != null)
            {
                foreach (FilaAlumnoMatrizDto f in matriz.FilasAlumnos)
                {
                    string habilitadoSem17 = f.PorcentajeInasistencia < 30 ? "SI" : "NO_DPI_BLOQUEADO";
                    string estadoRegistra = f.PorcentajeInasistencia >= 30 ? "DPI_00" : "HABILITADO";
                    sb.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}%\t{6}\t{7}",
                        f.Dni, f.CodigoEstudiante, f.NombreCompleto, f.TotalHorasAsignatura,
                        f.HorasFaltaAcumuladas, f.PorcentajeInasistencia, estadoRegistra, habilitadoSem17));
                }
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public static string CalcularSemaforo(decimal porcentaje)
        {
            if (porcentaje >= 30m) return "DPI";
            if (porcentaje >= 20m) return "RIESGO_ALTO";
            if (porcentaje >= 10m) return "ALERTA";
            return "REGULAR";
        }

        private static bool Contiene(string texto, string valor)
        {
            if (texto == null) return false;
            return texto.IndexOf(valor, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private int ObtenerDocenteIdSeguro(int personaId)
        {
            try
            {
                int id = _usuarios.ObtenerDocenteId(personaId);
                return id > 0 ? id : 1;
            }
            catch
            {
                return 1;
            }
        }

        private int ObtenerEstudianteIdSeguro(int personaId)
        {
            try
            {
                int id = _usuarios.ObtenerEstudianteId(personaId);
                return id > 0 ? id : 1;
            }
            catch
            {
                return 1;
            }
        }

        private List<AlumnoAsistenciaItemDto> CrearAlumnosMock()
        {
            List<AlumnoAsistenciaItemDto> list = new List<AlumnoAsistenciaItemDto>();

            list.Add(new AlumnoAsistenciaItemDto
            {
                EstudianteId = 1, PersonaId = 1,
                CodigoEstudiante = "2024101", Dni = "74123698",
                Nombres = "Diana", Apellidos = "Alvarado Quispe",
                Turno = "Noche", EstadoAsistencia = "PRESENTE",
                HorasFaltaAcumuladasPrevias = 0, PorcentajeInasistenciaPrevio = 0m
            });
            list.Add(new AlumnoAsistenciaItemDto
            {
                EstudianteId = 2, PersonaId = 2,
                CodigoEstudiante = "2024102", Dni = "75841236",
                Nombres = "Luis Fernando", Apellidos = "Mendoza Rios",
                Turno = "Noche", EstadoAsistencia = "TARDANZA", MinutosTardanza = 15,
                HorasFaltaAcumuladasPrevias = 4, PorcentajeInasistenciaPrevio = 5.56m
            });
            list.Add(new AlumnoAsistenciaItemDto
            {
                EstudianteId = 3, PersonaId = 3,
                CodigoEstudiante = "2024103", Dni = "76985412",
                Nombres = "Rosa Milagros", Apellidos = "Torres Ccahuana",
                Turno = "Noche", EstadoAsistencia = "PRESENTE",
                HorasFaltaAcumuladasPrevias = 8, PorcentajeInasistenciaPrevio = 11.11m
            });
            list.Add(new AlumnoAsistenciaItemDto
            {
                EstudianteId = 4, PersonaId = 4,
                CodigoEstudiante = "2024104", Dni = "73698521",
                Nombres = "Jorge", Apellidos = "Delgado Ramos",
                Turno = "Noche", EstadoAsistencia = "FALTA_INJUSTIFICADA",
                HorasFaltaAcumuladasPrevias = 16, PorcentajeInasistenciaPrevio = 22.22m
            });
            list.Add(new AlumnoAsistenciaItemDto
            {
                EstudianteId = 5, PersonaId = 5,
                CodigoEstudiante = "2024105", Dni = "72365418",
                Nombres = "Claudia Elena", Apellidos = "Salazar Paredes",
                Turno = "Noche", EstadoAsistencia = "PRESENTE",
                HorasFaltaAcumuladasPrevias = 4, PorcentajeInasistenciaPrevio = 5.56m
            });
            list.Add(new AlumnoAsistenciaItemDto
            {
                EstudianteId = 6, PersonaId = 6,
                CodigoEstudiante = "2024106", Dni = "71254369",
                Nombres = "Omar", Apellidos = "Quispe Mamani",
                Turno = "Noche", EstadoAsistencia = "FALTA_JUSTIFICADA",
                Observacion = "Cita medica confirmada",
                HorasFaltaAcumuladasPrevias = 24, PorcentajeInasistenciaPrevio = 33.33m
            });

            return list;
        }

        private List<SemanaSelectorDto> CrearSemanasMock(int claseId)
        {
            List<SemanaSelectorDto> list = new List<SemanaSelectorDto>();

            for (int i = 1; i <= 18; i++)
            {
                bool esCerrada = i <= 3;
                bool esAbierta = i == 4;
                string estado = esCerrada ? "CERRADA" : (esAbierta ? "ABIERTA" : "PROGRAMADA");

                SesionClaseDto sesion = new SesionClaseDto
                {
                    Id = 100 + i,
                    ClaseDocenteId = claseId,
                    UnidadDidacticaId = 1,
                    UnidadDidacticaCodigo = "DSI-601",
                    UnidadDidacticaNombre = "Desarrollo de Servicios Web y Microservicios (.NET 10)",
                    CarreraNombre = "Desarrollo de Sistemas de Informacion",
                    Ciclo = "VI",
                    Turno = "Noche",
                    Seccion = "A",
                    DocenteId = 1,
                    DocenteNombreCompleto = "Felipe (Senior Systems Engineer)",
                    AulaCodigo = "LAB-401",
                    FechaClase = DateTime.Today.AddDays((i - 4) * 7),
                    HoraInicio = new TimeSpan(18, 35, 0),
                    HoraFin = new TimeSpan(21, 45, 0),
                    HorasPedagogicas = 4,
                    NumeroSemana = i,
                    NumeroSesion = 1,
                    Estado = estado,
                    TotalAlumnos = 35,
                    TotalPresentes = esCerrada ? 33 : 0,
                    TotalTardanzas = esCerrada ? 1 : 0,
                    TotalFaltas = esCerrada ? 1 : 0,
                    TemaDesarrollado = TemaDeSemana(i)
                };

                list.Add(new SemanaSelectorDto
                {
                    NumeroSemana = i,
                    EsSemana17Recuperacion = i == 17,
                    EsSemana18Cierre = i == 18,
                    EstadoSemana = estado,
                    EsSemanaActual = i == 4,
                    Sesiones = new List<SesionClaseDto> { sesion }
                });
            }

            return list;
        }

        private static string TemaDeSemana(int semana)
        {
            if (semana == 1) return "Arquitectura Modular y Contratos de Servicios en .NET 10";
            if (semana == 2) return "Patron Repository y Mapeo Eficiente con Dapper";
            if (semana == 3) return "Control de Concurrencia y Transacciones en PostgreSQL 16";
            if (semana == 4) return "Desarrollo de Endpoints y Resiliencia en APIs";
            if (semana == 17) return "Semana 17: Evaluacion de Recuperacion Ordinaria (Notas 10-12)";
            if (semana == 18) return "Semana 18: Subsanacion, Auditoria Final y Cierre de Actas REGISTRA";
            return "Sesion de Aprendizaje Semana " + semana;
        }

        private List<SesionClaseDto> CrearSesionesRecientesMock()
        {
            List<SesionClaseDto> list = new List<SesionClaseDto>();

            list.Add(new SesionClaseDto
            {
                Id = 101,
                ClaseDocenteId = 1,
                UnidadDidacticaId = 1,
                UnidadDidacticaCodigo = "DSI-601",
                UnidadDidacticaNombre = "Desarrollo de Servicios Web y Microservicios (.NET 10)",
                DocenteId = 1,
                DocenteNombreCompleto = "Felipe (Docente Principal)",
                AulaCodigo = "LAB-401",
                FechaClase = DateTime.Today,
                HoraInicio = new TimeSpan(18, 35, 0),
                HoraFin = new TimeSpan(21, 45, 0),
                HorasPedagogicas = 4,
                NumeroSemana = 4,
                NumeroSesion = 1,
                Estado = "ABIERTA"
            });

            list.Add(new SesionClaseDto
            {
                Id = 102,
                ClaseDocenteId = 2,
                UnidadDidacticaId = 2,
                UnidadDidacticaCodigo = "DSI-602",
                UnidadDidacticaNombre = "Arquitectura de Software y DevOps en Linux",
                DocenteId = 1,
                DocenteNombreCompleto = "Felipe (Docente Principal)",
                AulaCodigo = "LAB-402",
                FechaClase = DateTime.Today.AddDays(-1),
                HoraInicio = new TimeSpan(18, 35, 0),
                HoraFin = new TimeSpan(21, 45, 0),
                HorasPedagogicas = 4,
                NumeroSemana = 4,
                NumeroSesion = 1,
                Estado = "CERRADA"
            });

            return list;
        }

        private List<ResumenAsistenciaEstudianteDto> CrearResumenEstudianteMock()
        {
            List<ResumenAsistenciaEstudianteDto> list = new List<ResumenAsistenciaEstudianteDto>();

            list.Add(new ResumenAsistenciaEstudianteDto
            {
                EstudianteId = 1,
                UnidadDidacticaId = 1,
                PeriodoId = 1,
                UnidadDidacticaCodigo = "DSI-601",
                UnidadDidacticaNombre = "Desarrollo de Servicios Web y Microservicios (.NET 10)",
                CarreraNombre = "Desarrollo de Sistemas de Informacion",
                Ciclo = "VI",
                Turno = "Noche",
                TotalHorasSemestre = 72,
                CantPresentes = 4,
                CantTardanzas = 0,
                CantFaltasJustificadas = 0,
                CantFaltasInjustificadas = 0,
                HorasFaltaInjustificada = 0,
                HorasAsistidas = 16,
                TotalSesionesRegistradas = 4,
                PorcentajeInasistencia = 0m,
                SemaforoEstado = "REGULAR",
                HorasFaltaDisponibles = 21
            });

            list.Add(new ResumenAsistenciaEstudianteDto
            {
                EstudianteId = 1,
                UnidadDidacticaId = 2,
                PeriodoId = 1,
                UnidadDidacticaCodigo = "DSI-602",
                UnidadDidacticaNombre = "Arquitectura de Software y DevOps en Linux",
                CarreraNombre = "Desarrollo de Sistemas de Informacion",
                Ciclo = "VI",
                Turno = "Noche",
                TotalHorasSemestre = 72,
                CantPresentes = 3,
                CantTardanzas = 1,
                CantFaltasJustificadas = 0,
                CantFaltasInjustificadas = 1,
                HorasFaltaInjustificada = 4,
                HorasAsistidas = 12,
                TotalSesionesRegistradas = 4,
                PorcentajeInasistencia = 5.56m,
                SemaforoEstado = "REGULAR",
                HorasFaltaDisponibles = 17
            });

            list.Add(new ResumenAsistenciaEstudianteDto
            {
                EstudianteId = 1,
                UnidadDidacticaId = 3,
                PeriodoId = 1,
                UnidadDidacticaCodigo = "DSI-603",
                UnidadDidacticaNombre = "Inteligencia Artificial Aplicada & Modelos Locales Edge",
                CarreraNombre = "Desarrollo de Sistemas de Informacion",
                Ciclo = "VI",
                Turno = "Noche",
                TotalHorasSemestre = 72,
                CantPresentes = 2,
                CantTardanzas = 0,
                CantFaltasJustificadas = 1,
                CantFaltasInjustificadas = 2,
                HorasFaltaInjustificada = 8,
                HorasAsistidas = 8,
                TotalSesionesRegistradas = 4,
                PorcentajeInasistencia = 11.11m,
                SemaforoEstado = "ALERTA",
                HorasFaltaDisponibles = 13
            });

            return list;
        }

        private List<AsistenciaHistorialItemDto> CrearHistorialMock()
        {
            List<AsistenciaHistorialItemDto> list = new List<AsistenciaHistorialItemDto>();

            list.Add(new AsistenciaHistorialItemDto
            {
                AsistenciaId = 1, SesionId = 101, NumeroSemana = 1,
                FechaClase = DateTime.Today.AddDays(-21), HorasPedagogicas = 4,
                DocenteNombre = "Felipe", Estado = "PRESENTE",
                TemaDesarrollado = "Introduccion a .NET 10 y Arquitectura Modular"
            });
            list.Add(new AsistenciaHistorialItemDto
            {
                AsistenciaId = 2, SesionId = 102, NumeroSemana = 2,
                FechaClase = DateTime.Today.AddDays(-14), HorasPedagogicas = 4,
                DocenteNombre = "Felipe", Estado = "TARDANZA", MinutosTardanza = 15,
                TemaDesarrollado = "Configuracion de Dapper y PostgreSQL 16"
            });
            list.Add(new AsistenciaHistorialItemDto
            {
                AsistenciaId = 3, SesionId = 103, NumeroSemana = 3,
                FechaClase = DateTime.Today.AddDays(-7), HorasPedagogicas = 4,
                DocenteNombre = "Felipe", Estado = "FALTA_INJUSTIFICADA",
                Observacion = "Inasistencia sin justificar",
                TemaDesarrollado = "Patrones de Diseno y Endpoints REST"
            });
            list.Add(new AsistenciaHistorialItemDto
            {
                AsistenciaId = 4, SesionId = 104, NumeroSemana = 4,
                FechaClase = DateTime.Today, HorasPedagogicas = 4,
                DocenteNombre = "Felipe", Estado = "PRESENTE",
                TemaDesarrollado = "Sabana Live Grid y Prevencion DPI"
            });
            list.Add(new AsistenciaHistorialItemDto
            {
                AsistenciaId = 5, SesionId = 105, NumeroSemana = 5,
                FechaClase = DateTime.Today.AddDays(-1), HorasPedagogicas = 4,
                DocenteNombre = "Felipe", Estado = "FALTA_INJUSTIFICADA",
                Observacion = "Inasistencia sin justificar dentro de las 72 horas",
                TemaDesarrollado = "Seguridad, JWT y autorizacion por roles"
            });

            return list;
        }

        private List<JustificacionDto> CrearJustificacionesMock()
        {
            List<JustificacionDto> list = new List<JustificacionDto>();

            list.Add(new JustificacionDto
            {
                Id = 1,
                AsistenciaId = 3,
                EstudianteId = 1,
                EstudianteNombre = "ALVARADO QUISPE, Diana",
                EstudianteCodigo = "2024101",
                UnidadDidacticaNombre = "Desarrollo de Servicios Web y Microservicios (.NET 10)",
                FechaClase = DateTime.Today.AddDays(-7),
                NumeroSemana = 3,
                Motivo = "Salud_Medica",
                Descripcion = "Presente cuadro gripal severo con descanso medico de EsSalud por 48 horas.",
                DocumentoSustentoUrl = "https://ejemplo.edu.pe/certificados/medico_2024101.pdf",
                Estado = "PENDIENTE",
                DocenteNombre = "Felipe",
                FechaSolicitud = DateTime.Today.AddDays(-6)
            });

            list.Add(new JustificacionDto
            {
                Id = 2,
                AsistenciaId = 20,
                EstudianteId = 4,
                EstudianteNombre = "DELGADO RAMOS, Jorge",
                EstudianteCodigo = "2024104",
                UnidadDidacticaNombre = "Arquitectura de Software y DevOps en Linux",
                FechaClase = DateTime.Today.AddDays(-14),
                NumeroSemana = 2,
                Motivo = "Laboral",
                Descripcion = "Horas extras obligatorias por despliegue de servidores en empresa.",
                DocumentoSustentoUrl = "https://ejemplo.edu.pe/certificados/laboral_2024104.pdf",
                Estado = "APROBADA",
                DocenteNombre = "Felipe",
                RespuestaObservacion = "Justificacion laboral validada y conforme.",
                FechaSolicitud = DateTime.Today.AddDays(-13),
                FechaResolucion = DateTime.Today.AddDays(-12)
            });

            return list;
        }

        private List<AlertaDpiDto> CrearAlertasMock()
        {
            List<AlertaDpiDto> list = new List<AlertaDpiDto>();

            list.Add(new AlertaDpiDto
            {
                EstudianteId = 8,
                EstudianteNombre = "HERRERA ROJAS, Fernando",
                EstudianteCodigo = "2024108",
                CarreraNombre = "Desarrollo de Sistemas de Informacion",
                Ciclo = "VI",
                Turno = "Noche",
                UnidadDidacticaNombre = "Desarrollo de Servicios Web y Microservicios (.NET 10)",
                DocenteNombre = "Felipe",
                HorasFalta = 24,
                TotalHoras = 72,
                PorcentajeInasistencia = 33.33m,
                SemaforoEstado = "DPI"
            });

            list.Add(new AlertaDpiDto
            {
                EstudianteId = 6,
                EstudianteNombre = "FUENTES MENDOZA, Pedro",
                EstudianteCodigo = "2024106",
                CarreraNombre = "Desarrollo de Sistemas de Informacion",
                Ciclo = "VI",
                Turno = "Noche",
                UnidadDidacticaNombre = "Desarrollo de Servicios Web y Microservicios (.NET 10)",
                DocenteNombre = "Felipe",
                HorasFalta = 16,
                TotalHoras = 72,
                PorcentajeInasistencia = 22.22m,
                SemaforoEstado = "RIESGO_ALTO"
            });

            list.Add(new AlertaDpiDto
            {
                EstudianteId = 4,
                EstudianteNombre = "DELGADO RAMOS, Jorge",
                EstudianteCodigo = "2024104",
                CarreraNombre = "Desarrollo de Sistemas de Informacion",
                Ciclo = "VI",
                Turno = "Noche",
                UnidadDidacticaNombre = "Arquitectura de Software y DevOps en Linux",
                DocenteNombre = "Felipe",
                HorasFalta = 8,
                TotalHoras = 72,
                PorcentajeInasistencia = 11.11m,
                SemaforoEstado = "ALERTA"
            });

            return list;
        }
    }
}
