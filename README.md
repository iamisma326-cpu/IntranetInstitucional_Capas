# Intranet Institucional — AQUILA A-ERP · IESTP "Argentina"

Sistema de intranet institucional desarrollado con **ASP.NET WebForms (.NET Framework 4.8)**, **arquitectura en capas**, **Microsoft SQL Server** como base de datos y **LINQ to SQL** (`System.Data.Linq`) como capa de acceso a datos.

El repositorio contiene la **base del proyecto** (login, sesión, dashboard y navegación por módulos) junto con el **Módulo 02 — Control de Asistencia & Prevención de Deserción (DPI)** completamente implementado sobre la base de datos institucional de 10 esquemas.

---

## Cómo ejecutar el proyecto (Windows · Visual Studio · SQL Server)

> **Todo con interfaz gráfica.** No hace falta abrir la línea de comandos ni escribir comandos:
> se usan **Visual Studio 2022** y **SQL Server Management Studio (SSMS)**.

### Requisitos

| Requisito | Detalle |
|---|---|
| **Visual Studio 2022** | Con la carga de trabajo **"Desarrollo de ASP.NET y web"**. Trae .NET Framework 4.8 y **IIS Express**, que es lo que ejecuta el sitio con `F5`. |
| **SQL Server 2016 o superior** | Instancia local: `localhost`, `.\SQLEXPRESS` o `(localdb)\MSSQLLocalDB`. |
| **SQL Server Management Studio (SSMS)** | Para crear la base ejecutando el script. |

Si al instalar Visual Studio solo se marcó "Desarrollo de escritorio de .NET", hay que volver al
**Visual Studio Installer → Modificar** y agregar la carga de trabajo **"Desarrollo de ASP.NET y web"**.

### Paso 1 — Descargar el proyecto

En la página del repositorio, botón verde **Code → Download ZIP**, y descomprimir la carpeta.
Si ya usan **GitHub Desktop**: **Code → Open with GitHub Desktop → Clone**.

### Paso 2 — Crear la base de datos (SSMS)

1. Abrir **SQL Server Management Studio** y conectarse a `localhost` (o `.\SQLEXPRESS`).
2. En el menú: **Archivo → Abrir → Archivo...** (File → Open → File...) y elegir
   **`db_intranet_capas.sql`** de la raíz del proyecto.
3. Presionar **Ejecutar (Execute)** — o directamente `F5`.
4. Esperar unos minutos (el script es grande). Al terminar, en el explorador de objetos,
   dentro de **Databases**, debe aparecer **`BD_INTRANET_2026`**.

El script crea la base, los 10 esquemas (`core`, `mod01` … `mod09`), las 102 tablas, las vistas,
los triggers de auditoría y los datos de demostración. No hay que crear nada a mano.

<details>
<summary><strong>¿La base ya existe y quieres recrearla desde cero?</strong></summary>

En SSMS, seleccionando la base `master`, ejecutar con `F5`:

```sql
IF DB_ID('BD_INTRANET_2026') IS NOT NULL
BEGIN
    ALTER DATABASE BD_INTRANET_2026 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BD_INTRANET_2026;
END;
```

Y después volver a ejecutar `db_intranet_capas.sql` completo.
</details>

### Paso 3 — Configurar las cadenas de conexión

Abrir el archivo **`IntranetInstitucional_Capas.PresentacionWeb/Web.config`** (doble clic desde el
Solution Explorer de Visual Studio). El proyecto declara **dos cadenas** que deben apuntar a la
misma base:

| Cadena | La usa |
|---|---|
| `BD_INTRANETConnectionString` | `BD_INTRANETDataContext` — el `DataContext` de LINQ to SQL (todas las consultas del DAL) |
| `CnxBD_Intranet` | `ConexionBD.ObtenerConexion()` — entrada ADO.NET con `System.Data.SqlClient.SqlConnection` |

Reemplazar el `Data Source` y el modo de autenticación por los de cada uno **en las dos**:

**Opción A — Autenticación de Windows** (lo más simple si SQL Server está en tu misma PC):

```xml
<connectionStrings>
  <add name="CnxBD_Intranet"
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=BD_INTRANET_2026;Integrated Security=True;Encrypt=False;"
       providerName="System.Data.SqlClient" />
  <add name="BD_INTRANETConnectionString"
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=BD_INTRANET_2026;Integrated Security=True;Encrypt=False;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Opción B — Usuario y contraseña de SQL Server:**

```xml
<connectionStrings>
  <add name="CnxBD_Intranet"
       connectionString="Data Source=localhost;Initial Catalog=BD_INTRANET_2026;User ID=sa;Password=TU_PASSWORD;TrustServerCertificate=True;"
       providerName="System.Data.SqlClient" />
  <add name="BD_INTRANETConnectionString"
       connectionString="Data Source=localhost;Initial Catalog=BD_INTRANET_2026;User ID=sa;Password=TU_PASSWORD;TrustServerCertificate=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

> El repositorio **no incluye credenciales**: cada integrante pone la suya en su copia local.
> Esa modificación **no se sube** al repositorio.

Si no sabes qué `Data Source` usar, en SSMS mira el nombre con el que te conectaste en el paso 2.
El `providerName` debe ser **`System.Data.SqlClient`** en ambas: el proyecto ya no usa Npgsql.

### Paso 4 — Abrir la solución y ejecutar

1. Doble clic en **`IntranetInstitucional_Capas.sln`** (abre Visual Studio 2022).
2. En el *Explorador de soluciones*, clic derecho sobre
   **`IntranetInstitucional_Capas.PresentacionWeb`** → **Establecer como proyecto de inicio**
   (*Set as Startup Project*).
3. Presionar **`F5`** (o el botón ▶ **Iniciar**).
4. Se abre el navegador en **`http://localhost:5189/`** con la pantalla de login.

### Paso 5 — Iniciar sesión

Usar cualquier cuenta de [Usuarios de prueba](#usuarios-de-prueba). Todas las cuentas de
demostración tienen la contraseña **`123456`**; por ejemplo `ADMIN-2026` / `123456`.

### Si algo no funciona

| Síntoma | Causa habitual |
|---|---|
| La pantalla de login se ve, pero al entrar dice *"El usuario no existe o esta inactivo"* | La cadena de conexión no apunta a la base correcta, o la base no se creó. Revisar el Paso 2 y el Paso 3. |
| Error *"No se puede conectar al servidor"* / *"network-related error"* | El servicio SQL Server no está corriendo, o el `Data Source` está mal escrito. |
| El login entra, pero el Módulo 02 muestra siempre datos de ejemplo | La cadena `BD_INTRANETConnectionString` falta o está mal: la BLL captura el error y cae al respaldo demo. Revisar el Paso 3. |
| `F5` no aparece o el proyecto web no carga | Falta la carga de trabajo **"Desarrollo de ASP.NET y web"** en Visual Studio. |
| Error *"The imported project ... Microsoft.WebApplication.targets was not found"* | Igual que el anterior: falta la carga de trabajo web. |

---

## Tabla de contenidos

1. [Cómo ejecutar el proyecto](#cómo-ejecutar-el-proyecto-windows--visual-studio--sql-server)
2. [Estado del proyecto](#estado-del-proyecto)
3. [Cambios de esta versión](#cambios-de-esta-versión)
4. [Tecnologías](#tecnologías)
5. [Arquitectura](#arquitectura)
6. [Base de datos](#base-de-datos)
7. [Módulo 02 — Asistencia & DPI](#módulo-02--asistencia--dpi)
8. [Usuarios de prueba](#usuarios-de-prueba)
9. [Estructura del repositorio](#estructura-del-repositorio)
10. [Anexo — Ejecutar en Linux con Mono](#anexo--ejecutar-en-linux-con-mono-opcional)

---

## Estado del proyecto

| Esquema | Módulo | Estado en la web |
|---|---|---|
| `core` | Seguridad Core & Auditoría (personas, usuarios, roles, carreras, periodos, aulas, unidades didácticas) | ✅ Implementado (login, sesión y control de acceso por rol) |
| `mod01` | Matrícula & Admisión | 🗄️ Solo base de datos |
| `mod02` | **Asistencia & Deserción (DPI)** | ✅ **Implementado completo** |
| `mod03` | Inventario & Equipos | 🗄️ Solo base de datos |
| `mod04` | Proyectos & Innovación (proyectos, categorías, etiquetas, evaluaciones docentes) | 🗄️ Solo base de datos |
| `mod05` | Incidencias & Requerimientos (mesa de ayuda TI) | 🗄️ Solo base de datos |
| `mod06` | Egresados & Titulación | 🗄️ Solo base de datos |
| `mod07` | Encuestas & Calidad Docente | 🗄️ Solo base de datos |
| `mod08` | Login, Seguridad & Roles (sesiones, permisos, bitácora) | 🗄️ Solo base de datos |
| `mod09` | Tesorería & Pagos TUPA | 🗄️ Solo base de datos |

Los esquemas existen con sus tablas, restricciones, vistas y triggers de auditoría. La capa web solo consume por ahora `core` y `mod02`; el resto queda listo para que cada responsable conecte su módulo.

---

## Cambios de esta versión

La versión actual deja atrás PostgreSQL y concentra el acceso a datos en **SQL Server + LINQ to SQL**:

| Cambio | Detalle |
|---|---|
| Motor de base de datos | PostgreSQL/Npgsql → **SQL Server** (`System.Data.SqlClient`) |
| Acceso a datos | SQL con `NpgsqlCommand` → **LINQ to SQL** (`System.Data.Linq`) |
| Modelo | Nuevo `BD_INTRANET.dbml` + `BD_INTRANET.designer.cs` con **15 objetos** (14 tablas + 1 vista) y `BD_INTRANETDataContext` |
| Relaciones | Sin asociaciones FK en el modelo: se resuelven con `join` explícitos en el DAL |
| DAL | `UsuarioDAL` → **`UsuarioLinqDAL`** · `Modulo02/AsistenciaDAL` → **`Modulo02/AsistenciaLinqDAL`** |
| BLL | `UsuarioBLL` → **`UsuarioLinqBLL`** · `Modulo02/AsistenciaBLL` → **`Modulo02/AsistenciaLinqBLL`** |
| Configuración | `Web.config` con dos cadenas (`CnxBD_Intranet` y `BD_INTRANETConnectionString`) y sin *binding redirects* de Npgsql |
| Paquetes | Eliminadas las referencias y el paquete `Npgsql`; el proyecto `AccesoDatos` referencia `System.Data.Linq` |
| Comentarios | El código C# no lleva comentarios: la documentación vive en este README |

Las páginas (`Modulo02*.aspx.cs`, `Login.aspx.cs`, `App_Start/SesionPagina.cs`) ya instancian
las clases `...LinqBLL`, y la solución compila completa con MSBuild / Visual Studio 2022.

---

## Tecnologías

### Aplicación

| Capa | Tecnología |
|---|---|
| Framework web | ASP.NET WebForms |
| Runtime | .NET Framework **4.8** |
| Lenguaje | C# |
| Acceso a datos | **LINQ to SQL** (`System.Data.Linq`) — modelo de objetos `BD_INTRANET.dbml` + `BD_INTRANET.designer.cs` y `DataContext` `BD_INTRANETDataContext` (sin SQL inline) |
| Consultas | **LINQ to SQL** en el DAL (`from … join … where … select`) y **LINQ to Objects** (`System.Linq`) en la BLL/DTOs |
| Escrituras | LINQ to SQL: `InsertOnSubmit` + `SubmitChanges()` (transacción implícita por llamada) |
| Vistas | ASP.NET Master Pages + controles WebForms (`GridView`, `Repeater`, `DataList`) |
| Estilos | Tailwind CSS (CDN) + daisyUI 4.7.2 + modo claro/oscuro |
| Sesión | `Session` en memoria (`InProc`), clase `SesionPagina` |
| Contratos de datos | DTOs propios (`*Dtos.cs`): la BLL y la UI conservan las mismas firmas; el DAL proyecta con LINQ to SQL hacia esos DTOs |

> **Nota de migración.** El proyecto se migró desde PostgreSQL/Npgsql a **SQL Server con
> LINQ to SQL**. La carpeta conserva el sufijo `_PostgreSQL` por historia del repositorio,
> pero ya no quedan dependencias de Npgsql: el único proveedor es `System.Data.SqlClient`
> y la referencia de datos es `System.Data.Linq`.

**¿Cómo se usa LINQ to SQL?**

El acceso a datos es **LINQ to SQL** (`System.Data.Linq`): el DAL trabaja contra el
`DataContext` generado desde `BD_INTRANET.dbml` (`BD_INTRANET.designer.cs`), con sus
**15 entidades mapeadas** (`Personas`, `Usuarios`, `Roles`, `UsuarioRoles`, `Docentes`,
`Estudiantes`, `Carreras`, `Aulas`, `UnidadesDidacticas`, `PeriodosAcademicos`,
`ClasesDocente`, `SesionesClase`, `Asistencias`, `Justificaciones`,
`VResumenAsistenciaEstudiante`). El modelo **no declara asociaciones FK**: las relaciones se
resuelven con `join` explícitos, igual que en el proyecto de referencia `SitioVeterinaria_Capas`.

- **Lecturas** — consultas LINQ que combinan tablas con `join`, proyectan a los DTOs
  existentes y usan `DefaultIfEmpty()` para los `LEFT JOIN`, además de `Count(predicado)`,
  `Distinct()`, `Max()` y subconsultas correlacionadas. Ejemplo
  (`UsuarioLinqDAL.BuscarPorIdentificador`):

  ```csharp
  using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
  {
      var fila = (from u in db.Usuarios
                  join p in db.Personas on u.PersonaId equals p.Id
                  where u.Estado
                        && (p.Dni.ToLower() == limpio
                            || u.CodigoInstitucional.ToLower() == limpio
                            || u.Email.ToLower() == limpio)
                  select new { Usuario = u, Persona = p }).FirstOrDefault();
  }
  ```

- **Escrituras** — con el `DataContext`: `InsertOnSubmit` y `SubmitChanges()`
  (un único `SubmitChanges()` se ejecuta como una transacción). Ejemplo
  (`AsistenciaLinqDAL.GuardarAsistenciaSesion`):

  ```csharp
  Asistencias asistencia = new Asistencias
  {
      SesionClaseId = request.SesionClaseId,
      EstudianteId = item.EstudianteId,
      Estado = item.Estado,
      MinutosTardanza = item.MinutosTardanza,
      RegistradoPorUsuarioId = usuarioId,
      RegistradoEn = DateTime.Now
  };

  db.Asistencias.InsertOnSubmit(asistencia);
  db.SubmitChanges();
  ```

- **Conexión** — el `DataContext` sin parámetros lee la cadena
  **`BD_INTRANETConnectionString`** del `Web.config` mediante `ConfigurationManager`.
  `ConexionBD.ObtenerConexion()` queda disponible como entrada ADO.NET
  (`SqlConnection`) sobre la cadena `CnxBD_Intranet`.

Se mapean **15 objetos**: 14 tablas de `core`/`mod02` + la vista
`mod02.v_resumen_asistencia_estudiante`. La **BLL** sigue usando **LINQ to Objects**
(`System.Linq`) para filtrar, ordenar y agrupar las listas de DTOs
(`AsistenciaLinqBLL`, `Modulo02_Justificaciones.aspx.cs`).

### Motor de base de datos

| Elemento | Detalle |
|---|---|
| Motor | **Microsoft SQL Server 2016 o superior** (usa `FOR JSON` / `JSON_VALUE`) |
| Base de datos | `BD_INTRANET_2026` |
| Esquemas | `core`, `mod01` … `mod09` |
| Objetos | 102 tablas · 14 vistas · 113 triggers · 1 procedimiento · 163 FK · 93 CHECK · 76 índices |

### Herramientas de desarrollo

| Uso | Herramienta |
|---|---|
| IDE | **Visual Studio 2022** (carga de trabajo *Desarrollo de ASP.NET y web*) |
| Servidor local | **IIS Express** (incluido en Visual Studio) |
| Cliente SQL | **SQL Server Management Studio (SSMS)** |
| Control de versiones | Git + GitHub (o GitHub Desktop) |
| Alternativa Linux (no requerida) | `xbuild` / **Mono `xsp4`** |

---

## Arquitectura

Arquitectura en **4 capas** con dependencias unidireccionales:

```
IntranetInstitucional_Capas.PresentacionWeb   (UI: .aspx + code-behind)
              │
              ▼
IntranetInstitucional_Capas.LogicaNegocio     (BLL: reglas, validaciones, semáforos)
              │
              ▼
IntranetInstitucional_Capas.AccesoDatos       (DAL: LINQ to SQL + BD_INTRANETDataContext)
              │
              ▼
IntranetInstitucional_Capas.Entidades         (DTOs y modelos compartidos)
```

- **PresentacionWeb**: páginas `.aspx`, `Sitio.master` (layout), `App_Start/SesionPagina.cs` (sesión, control de acceso por rol y alertas toast).
- **LogicaNegocio**: `UsuarioLinqBLL` (login y roles) y `Modulo02/AsistenciaLinqBLL` (reglas del módulo: KPIs, semáforo de asistencia, DPI, exportaciones).
- **AccesoDatos**: `ConexionBD` (entrada ADO.NET sobre `CnxBD_Intranet`), el modelo LINQ to SQL (`BD_INTRANET.dbml` + `BD_INTRANET.designer.cs` con `BD_INTRANETDataContext`), `UsuarioLinqDAL` y `Modulo02/AsistenciaLinqDAL` (consultas LINQ to SQL y escrituras con `SubmitChanges`).
- **Entidades**: `Usuario`, `Persona`, `Rol`, `SesionUsuario`, `ResultadoLogin`, `RespuestaTransaccion` y los DTOs de `Modulo02`.

---

## Base de datos

### Objetos por esquema

| Esquema | Tablas | Contenido principal |
|---|---:|---|
| `core` | 12 | `personas`, `usuarios`, `roles`, `usuario_roles`, `carreras`, `periodos_academicos`, `aulas`, `docentes`, `estudiantes`, `unidades_didacticas`, `administrativos`, `auditoria_logs` |
| `mod01` | 11 | `matriculas`, `detalles_matricula`, `carnes`, `historial_academico`, `oferta_ciclo`, `vacantes`, `tipos_matricula`, `ciclos`, `turnos` |
| `mod02` | 6 | `clases_docente`, `clase_horarios`, `sesiones_clase`, `asistencias`, `justificaciones`, `configuracion` |
| `mod03` | 10 | `bien`, `ambiente`, `almacen`, `asignacion`, `transferencia`, `inventario_anual`, `calificaciones`, `clasificaciones_bien`, `especialidad` |
| `mod04` | 9 | `proyectos`, `proyecto_estudiantes`, `proyecto_categorias`, `proyecto_etiquetas`, `categorias`, `etiquetas`, `evaluaciones_docente`, `historial_proyecto` |
| `mod05` | 9 | `incidencias`, `reportes_incidencia`, `requerimientos`, `atenciones_topico`, `atenciones_psicologia`, `citas_psicologia`, `botica_medicamentos` |
| `mod06` | 9 | `egresos`, `expedientes`, `expedientes_detalle`, `seguimiento_egresados`, `tramites_tupa`, `requisitos_catalogo` |
| `mod07` | 7 | `libros`, `preguntas`, `respuestas_encuesta`, `control_votos`, `asignacion_docente` |
| `mod08` | 13 | `sesiones_login`, `intentos_login`, `asignaciones_rol`, `permisos_modulos`, `politicas_acceso`, `bitacora`, `identidades_externas`, `ofertas_laborales` |
| `mod09` | 16 | `pagos`, `conceptos_pago`, `tipos_tramite`, `tramites`, `voucher_archivos`, `avisos`, `whatsapp_*`, `feriados` |

### Vistas

`v_resumen_asistencia_estudiante`, `v_usuarios_roles`, `v_cubrimiento_roles`, `v_estado_acceso`,
`v_inventario`, `v_inventario_anual`, `v_transferencias`, `v_incidencias_detalle`,
`v_reportes_incidencia_detalle`, `v_requerimientos_detalle`, `v_citas_psicologia`,
`v_telemetria_equipos`, `v_telemetria_tablas_detalle`, `v_conexiones_en_vivo`.

### Auditoría

Cada tabla (excepto `core.auditoria_logs`) tiene un trigger `AFTER INSERT, UPDATE, DELETE`
que registra el cambio en `core.auditoria_logs` con:

- usuario de base de datos, esquema, entidad y acción
- `registro_id` obtenido con `JSON_VALUE(... FOR JSON PATH)`
- estado anterior y nuevo en JSON
- IP de origen (`sys.dm_exec_connections`)

El procedimiento `core.sp_activar_auditoria_esquema` genera estos triggers dinámicamente
para un esquema completo, resolviendo la PK real de cada tabla.

Además de los **101 triggers de auditoría**, el script crea **12 triggers de negocio**
(validaciones previas, sincronización de estados y referencias) en `mod05`, `mod06` y `mod08`.

### Roles

`core.roles` siembra **10 roles**: `Admin`, `Director`, `Coordinador`, `Secretaria`,
`Tesoreria`, `Docente`, `Alumno`, `Jefe General`, `Jefe de Especialidad` y `OTIS`.

La capa web reconoce los siete primeros (más el alias `Estudiante` para `Alumno`) y ordena el
selector de roles como `Director → Docente → Alumno → Coordinador → Secretaría → Tesorería`
(`Rol.Orden`). `Jefe General`, `Jefe de Especialidad` y `OTIS` corresponden a los módulos de
inventario y mantenimiento.

### Datos de demostración incluidos

| Tabla | Filas |
|---|---:|
| `core.personas` | 77 |
| `core.usuarios` | 77 |
| `core.estudiantes` | 48 |
| `core.docentes` | 20 |
| `mod02.sesiones_clase` | 192 |
| `mod02.asistencias` | 1 063 |
| `mod02.justificaciones` | 2 |
| `core.auditoria_logs` | 1 873 |

Los datos son ficticios (nombres, DNI y correos de demostración).

---

## Módulo 02 — Asistencia & DPI

Control de las **18 semanas académicas** con toma de lista rápida, justificaciones en 72 horas
y cálculo automático del **DPI** (índice de deserción / riesgo de abandono).

### Páginas

| Página | Rol | Función |
|---|---|---|
| `Default.aspx` | Todos | Dashboard con saludo por rol, accesos frecuentes y mapa de los 10 módulos |
| `Modulo02.aspx` | Todos excepto Alumno (el Alumno se redirige a `Modulo02_MiAsistencia.aspx`) | Panel del módulo: clase activa de hoy, KPIs (cursos, sesiones, alumnos en DPI, justificaciones pendientes) y tarjetas de cursos |
| `Modulo02_Clase.aspx` | Docente, Coordinador, Director, Admin, Secretaría, Tesorería | Detalle de una clase: semanas, sesiones programadas y reprogramación |
| `Modulo02_Matriz.aspx` | Docente, Coordinador, Director, Admin | Sábana de 18 semanas (alumnos × sesiones) con edición y guardado masivo |
| `Modulo02_TomarAsistencia.aspx` | Docente, Coordinador, Director, Admin, Secretaría, Tesorería | Toma de asistencia "flash" de una sesión: marcar todos presentes, estado por alumno y copia del formato REGISTRA |
| `Modulo02_MiAsistencia.aspx` | Alumno | Resumen e historial de asistencia del estudiante |
| `Modulo02_DetalleCurso.aspx` | Alumno | Detalle de asistencia por curso |
| `Modulo02_Justificaciones.aspx` | Alumno (solicita) / Docente (dictamina) | Solicitud y resolución de justificaciones (ventana de 72 h) |
| `Modulo02_ReportesDpi.aspx` | Todos excepto Alumno | Radar DPI: alumnos con ≥ 30 % de faltas, inhabilitados para la recuperación de la Semana 17 |

Los permisos se validan en cada `Page_Load` con `SesionPagina.ExigirRol(...)` o con
`SesionPagina.Usuario.ModoAlumno`; todas las páginas exigen sesión iniciada.

### Lógica principal (`AsistenciaLinqBLL`)

- `ObtenerDashboard` / `ObtenerClaseActivaHoy` — panel y clase del día
- `ObtenerMatriz` / `GuardarMatriz` — sábana de 18 semanas
- `ObtenerAlumnosParaSesion` / `GuardarAsistencia` — toma rápida por sesión
- `ObtenerResumenEstudiante` / `ObtenerHistorial` — vista del alumno
- `SolicitarJustificacion` / `ResolverJustificacion` — flujo de justificaciones
- `ObtenerAlertasDpi` — cálculo de riesgo (≥ 30 % de inasistencias)
- `ExportarSabanaCsv` / `GenerarActaRegistra` — exportables para REGISTRA MINEDU
- `CalcularSemaforo` — semáforo de asistencia: `REGULAR` (< 10 %), `ALERTA` (≥ 10 %), `RIESGO_ALTO` (≥ 20 %) y `DPI` (≥ 30 %)

### Tablas que consume

`core.personas`, `core.docentes`, `core.estudiantes`, `core.carreras`, `core.aulas`,
`core.unidades_didacticas`, `mod02.clases_docente`, `mod02.sesiones_clase`,
`mod02.asistencias`, `mod02.justificaciones` y la vista
`mod02.v_resumen_asistencia_estudiante`.

El modelo LINQ to SQL mapea además `core.usuarios`, `core.roles`, `core.usuario_roles`
(login y selector de roles) y `core.periodos_academicos` (mapeada para el catálogo de periodos,
todavía sin consultas en el DAL).

---

## Usuarios de prueba

Todas las cuentas de demostración usan la contraseña **`123456`** (guardada tal cual en
`password_hash`). Además, `UsuarioLinqBLL` acepta como contraseña el **DNI** de la propia
cuenta y las claves heredadas `@2026` y `admin`.

| Código institucional | DNI | Nombre | Correo | Rol |
|---|---|---|---|---|
| `ADMIN-2026` | 00000001 | Administrador General de TI | `admin.ti@ieargentina.edu.pe` | Admin |
| `DIR-2026` | 10000001 | Manuel Alvarado Carranza | `direccion@ieargentina.edu.pe` | Director |
| `COORD-DSI` | 20000001 | Carlos Mendoza Rivas | `coord.sistemas@ieargentina.edu.pe` | Coordinador |
| `SEC-ACAD` | 30000001 | Rosa Morales Salazar | `secretaria.academica@ieargentina.edu.pe` | Secretaria |
| `TES-2026` | 40000001 | Elena Ramos Palacios | `tesoreria@ieargentina.edu.pe` | Tesorería |
| `DOC-DSI-01` | 12345678 | Roberto Sánchez Benítez | `rsanchez@ieargentina.edu.pe` | Docente |
| `EST-DSI-001` | 87654321 | Felipe Ostos | `felipe.ostos@ieargentina.edu.pe` | Alumno, Docente y Admin |
| `EST-DSI-002` | 77654321 | Ana García Flores | `ana.garcia@ieargentina.edu.pe` | Alumno |
| `EST-CONT-001` | 66554433 | Luis Torres Quispe | `luis.torres@ieargentina.edu.pe` | Alumno |

El login acepta **código institucional, DNI o correo** (sin distinguir mayúsculas).

La pantalla incluye además **seis botones de acceso de demostración en 1 clic** —Director,
Docente, Estudiante, Coordinador, Secretaría y Tesorería— que envían el **DNI** de la cuenta
y la contraseña `123456`:

| Botón | DNI enviado | Cuenta con la que entra |
|---|---|---|
| Director | `10000001` | Manuel Alvarado Carranza |
| Docente | `12345678` | Roberto Sánchez Benítez |
| Estudiante | `87654321` | Felipe Ostos |
| Coordinador | `20000001` | Carlos Mendoza Rivas |
| Secretaría | `30000001` | Rosa Morales Salazar |
| Tesorería | `40000001` | Elena Ramos Palacios |

> El nombre pequeño que muestran esos botones (Felipe, Sheyla (M02), Mendoza, Rodriguez,
> Flores, Perez) es solo una etiqueta de la interfaz y **no coincide** con el nombre de la
> persona sembrada en la base: la sesión la abre el DNI indicado en el botón. Esos alias sí
> aparecen cuando la base no responde y la BLL cae al catálogo de respaldo en memoria.

<details>
<summary><strong>Modo sin base de datos (respaldo en memoria)</strong></summary>

Si la conexión falla, `UsuarioLinqBLL` no lanza error: autentica contra un catálogo demo.
Además del DNI, el código institucional o el correo de cada entrada, acepta estas claves
(contraseña `123456`, el DNI correspondiente o `admin`):

| Clave de acceso | Perfil que devuelve |
|---|---|
| `admin` · `director` | Director Institucional (todos los roles) |
| `coordinador` | Coordinador y Docente |
| `secretaria` | Secretaría |
| `tesoreria` | Tesorería |
| `docente` · `sheyla` | Docente y Alumno |
| `alumno` · `mendoza` | Alumno |
| `montero` · `10000003` | Docente |
| `felipe` · `felipeostosb` | Director (todos los roles) |

Los DNI de ese catálogo son `10000001`, `20000001`, `30000001`, `40000001`, `12345678`,
`87654321`, `10000003` y `47915633`. Este respaldo solo entra en juego cuando la base no
responde; con la base creada, mandan los usuarios reales de la tabla anterior.

</details>

---

## Estructura del repositorio

```
IntranetInstitucional_Capas_PostgreSQL/
├── db_intranet_capas.sql                     # Script completo de SQL Server (esquema + datos)
├── README.md                                 # Documentación del proyecto
├── iniciar-localhost (linux).sh              # Arranque con Mono/xsp4 en el puerto 5189
├── IntranetInstitucional_Capas.sln
│
├── IntranetInstitucional_Capas.Entidades/            # DTOs y modelos
│   ├── Usuario.cs · Persona.cs · Rol.cs
│   ├── SesionUsuario.cs · ResultadoLogin.cs · RespuestaTransaccion.cs
│   └── Modulo02/  (AsistenciaDtos, ClaseSesionDtos, JustificacionDtos, MatrizDtos)
│
├── IntranetInstitucional_Capas.AccesoDatos/          # DAL (LINQ to SQL)
│   ├── BD_INTRANET.dbml · BD_INTRANET.designer.cs   # modelo + DataContext (15 objetos)
│   ├── ConexionBD.cs · UsuarioLinqDAL.cs
│   └── Modulo02/AsistenciaLinqDAL.cs
│
├── IntranetInstitucional_Capas.LogicaNegocio/        # BLL
│   ├── UsuarioLinqBLL.cs
│   └── Modulo02/AsistenciaLinqBLL.cs
│
└── IntranetInstitucional_Capas.PresentacionWeb/      # UI WebForms
    ├── Web.config
    ├── Global.asax · Global.asax.cs
    ├── Sitio.master
    ├── Login.aspx · Default.aspx · SinAcceso.aspx
    ├── Modulo02*.aspx  (8 páginas)
    └── App_Start/SesionPagina.cs
```

---

## Anexo — Ejecutar en Linux con Mono (opcional)

> Solo para quien no use Windows. El equipo trabaja con Visual Studio, así que este anexo se
> puede ignorar.

Mono 6.x incluye `xbuild` (compilador) y `xsp4` (servidor web). Con la base ya creada y la
cadena de conexión configurada:

```bash
xbuild IntranetInstitucional_Capas.sln /p:Configuration=Debug
./"iniciar-localhost (linux).sh"      # levanta http://localhost:5189
```

El sitio redirige a `Login.aspx` si no hay sesión activa.

Detalles propios de Mono:

- Las cadenas del repositorio usan `Encrypt=False`, así que no hay problema con certificados
  autofirmados. Si se cambia a `Encrypt=True`, hay que agregar `TrustServerCertificate=True`.
- Evitar agregar `MinPoolSize` o `MaxPoolSize` en Mono: su `System.Data.SqlClient` puede
  rechazarlos. No son necesarios para ejecutar el sitio.
- LINQ to SQL requiere `System.Data.Linq.dll`: el proyecto `AccesoDatos` ya declara la
  referencia y `xbuild` la resuelve desde la instalación de Mono.

---

**IESTP "Argentina"** — Ley 30512 & MPA 2024 · Periodo académico 2026-II
