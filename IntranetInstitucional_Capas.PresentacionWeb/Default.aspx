<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Default.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Default" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
<div class="space-y-6">

    <div class="bg-gradient-to-r from-slate-900 via-blue-950 to-indigo-950 text-white rounded-3xl p-6 sm:p-8 border border-white/10 shadow-lg relative overflow-hidden">
        <div class="relative z-10 flex flex-col md:flex-row md:items-center justify-between gap-4">
            <div class="space-y-1">
                <div class="flex items-center gap-2">
                    <span class="text-[11px] bg-cyan-400/20 text-cyan-200 border border-cyan-400/30 px-2.5 py-0.5 rounded-full font-bold">Periodo 2026-II</span>
                    <span class="text-xs text-slate-400">&bull; IESTP "Argentina"</span>
                </div>
                <h1 class="text-2xl sm:text-3xl font-black tracking-tight text-white">
                    &iexcl;Hola, <%: NombreUsuario %>! &#128075;
                </h1>
                <p class="text-xs sm:text-sm text-slate-300 max-w-xl">
                    <% if (EsDocenteModo) { %>
                        Est&aacute;s en Modo Docente. Gestiona tus clases, asistencia de 18 semanas y actas.
                    <% } else if (EsDirectorModo) { %>
                        Est&aacute;s en Modo Direcci&oacute;n General. Supervisi&oacute;n integral del ecosistema institucional.
                    <% } else if (EsCoordinadorModo) { %>
                        Est&aacute;s en Modo Coordinaci&oacute;n. Gesti&oacute;n de aulas, horarios y carga docente.
                    <% } else if (EsSecretariaModo) { %>
                        Est&aacute;s en Modo Secretar&iacute;a. Validaci&oacute;n de matr&iacute;culas, expedientes, actas oficiales y tr&aacute;mites.
                    <% } else if (EsTesoreriaModo) { %>
                        Est&aacute;s en Modo Tesorer&iacute;a. Conciliaci&oacute;n de pagos, validaci&oacute;n de vouchers y tasas TUPA.
                    <% } else { %>
                        Est&aacute;s en Modo Estudiante. Consulta tu r&eacute;cord de asistencia, matr&iacute;cula y tr&aacute;mites.
                    <% } %>
                </p>
            </div>

            <div class="flex items-center gap-2 shrink-0">
                <a href="#" class="btn btn-sm bg-emerald-500 hover:bg-emerald-400 text-slate-950 font-bold rounded-xl text-xs gap-1.5 shadow-sm border-0" title="Herramienta r&aacute;pida de especificaci&oacute;n y descarga A4">
                    <span>&#128203; Ficha PO</span>
                </a>
                <a href="#" class="btn btn-sm btn-ghost bg-white/10 hover:bg-white/20 text-white rounded-xl text-xs font-semibold">
                    &#128218; Gu&iacute;as &amp; Videos
                </a>
            </div>
        </div>
        <div class="absolute -right-10 -bottom-10 w-60 h-60 bg-cyan-500/10 rounded-full blur-3xl pointer-events-none"></div>
    </div>

    <div class="space-y-3">
        <div class="flex items-center justify-between">
            <h2 class="text-xs font-bold text-slate-500 dark:text-slate-400 uppercase tracking-wider">
                Acciones Frecuentes &bull; Modo <%: RolActivo %>
            </h2>
            <span class="text-[11px] text-slate-400">4 accesos directos priorizados</span>
        </div>

        <div class="grid grid-cols-2 sm:grid-cols-4 gap-3">
            <% if (EsDocenteModo) { %>
                <a href="Modulo02_TomarAsistencia.aspx" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128203;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Tomar Asistencia</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Registro en 1 clic</p>
                </a>

                <a href="Modulo02_Matriz.aspx" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128202;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Matriz 18 Semanas</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">S&aacute;bana de asistencia</p>
                </a>

                <a href="Modulo02_ReportesDpi.aspx" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#9888;&#65039;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Alertas DPI 30%</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Control de inasistencias</p>
                </a>

                <a href="Modulo02_Justificaciones.aspx" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128221;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Justificaciones</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Bandeja de solicitudes</p>
                </a>
            <% } else if (EsDirectorModo) { %>
                <a href="Modulo02.aspx" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128202;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Panel Asistencia</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Supervisi&oacute;n institucional</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128737;&#65039;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Seguridad &amp; Roles</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Credenciales y auditor&iacute;a</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128203;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Mesa de Partes</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Expedientes y tr&aacute;mites</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128179;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Tesorer&iacute;a &amp; Pagos</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Recaudaci&oacute;n TUPA</p>
                </a>
            <% } else if (EsCoordinadorModo) { %>
                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128230;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">M&oacute;dulo 04</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Gesti&oacute;n M&oacute;dulo 04</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128104;&#8205;&#127979;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Carga Docente</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Asignaci&oacute;n lectiva</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128202;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Calificaciones</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Actas y ponderados</p>
                </a>

                <a href="Modulo02_ReportesDpi.aspx" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#9888;&#65039;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Alertas DPI</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Deserci&oacute;n temprana</p>
                </a>
            <% } else if (EsSecretariaModo) { %>
                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128203;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Panel Matr&iacute;cula</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Validaci&oacute;n de fichas</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#127891;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Egresados &amp; Grados</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Carpetas de titulaci&oacute;n</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128236;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Mesa de Tr&aacute;mites</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Atenci&oacute;n TUPA y FUT</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128209;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Actas Oficiales</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Certificados de estudio</p>
                </a>
            <% } else if (EsTesoreriaModo) { %>
                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128179;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Bandeja Vouchers</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Validaci&oacute;n de pagos</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#127963;&#65039;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Tasas TUPA</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Cobros de conceptos</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128176;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Pagos Matr&iacute;cula</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Filtro de vouchers M01</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128202;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Caja &amp; Recaudaci&oacute;n</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Resumen financiero</p>
                </a>
            <% } else { %>
                <a href="Modulo02_MiAsistencia.aspx" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#127891;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Mi Asistencia</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Ver r&eacute;cord y faltas DPI</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128218;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Mi Matr&iacute;cula</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Cursos inscritos</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128203;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Encuestas</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Evaluaci&oacute;n docente</p>
                </a>

                <a href="#" class="p-4 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group">
                    <div class="text-2xl mb-2">&#128179;</div>
                    <p class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">Tr&aacute;mites TUPA</p>
                    <p class="text-[11px] text-slate-400 dark:text-slate-500 mt-0.5">Certificados y pagos</p>
                </a>
            <% } %>
        </div>
    </div>

    <div class="space-y-3">
        <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-1">
            <h2 class="text-xs font-bold text-slate-500 dark:text-slate-400 uppercase tracking-wider">
                10 M&oacute;dulos del Sistema &bull; Mapa de Responsables de Base de Datos
            </h2>
            <span class="text-[11px] text-slate-400">Esquemas aislados mod00..mod09</span>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#128737;&#65039;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-slate-100 dark:bg-slate-800 text-slate-700 dark:text-slate-300 px-2 py-0.5 rounded">mod00</span>
                            <span class="text-[10px] bg-slate-200/70 dark:bg-slate-700 text-slate-700 dark:text-slate-200 font-semibold px-2 py-0.5 rounded">Core</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        00. Seguridad Core &amp; Auditor&iacute;a
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Control central de sesiones SSO, tokens de recuperaci&oacute;n y auditor&iacute;a de eventos.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; Esquema: <code class="font-mono text-cyan-600 dark:text-cyan-400">mod00</code></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#128221;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-sky-100 dark:bg-sky-950 text-sky-700 dark:text-sky-300 px-2 py-0.5 rounded">mod01</span>
                            <span class="text-[10px] bg-sky-50 dark:bg-sky-900/60 text-sky-700 dark:text-sky-300 border border-sky-200 dark:border-sky-800 font-bold px-2 py-0.5 rounded">BD: Ismael</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        01. Matr&iacute;cula &amp; Admisi&oacute;n
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Matr&iacute;cula acad&eacute;mica regular, convalidaciones y reserva de vacantes.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; DBA: <strong class="text-slate-700 dark:text-slate-300">Ismael</strong></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>

            <a href="Modulo02.aspx" class="p-5 rounded-2xl bg-white dark:bg-[#111827] border border-slate-200/80 dark:border-slate-800 shadow-sm hover:shadow-md transition flex flex-col justify-between group">
                <div class="space-y-3">
                    <div class="flex items-center justify-between">
                        <span class="p-2.5 rounded-xl bg-indigo-50 dark:bg-indigo-950/60 text-indigo-600 dark:text-indigo-400 text-xl">&#128197;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] bg-indigo-50 dark:bg-indigo-900/60 text-indigo-700 dark:text-indigo-300 border border-indigo-200 dark:border-indigo-800 font-bold px-2 py-0.5 rounded">BD: Sheyla</span>
                            <span class="text-[10px] font-mono text-slate-400">mod02</span>
                        </div>
                    </div>
                    <div>
                        <h3 class="text-sm font-bold text-slate-900 dark:text-slate-100 group-hover:text-indigo-600 dark:group-hover:text-indigo-400 transition">02. Asistencia &amp; Deserci&oacute;n (DPI)</h3>
                        <p class="text-xs text-slate-500 dark:text-slate-400 mt-1">Control de 18 semanas acad&eacute;micas, toma de lista r&aacute;pida, justificaciones en 72h y c&aacute;lculo autom&aacute;tico de DPI.</p>
                    </div>
                </div>
                <div class="pt-4 border-t border-slate-100 dark:border-slate-800/60 flex items-center justify-between text-xs mt-3">
                    <span class="text-indigo-600/80 dark:text-indigo-400 font-medium">&#128452;&#65039; DBA: <strong>Sheyla</strong></span>
                    <span class="text-indigo-600 dark:text-indigo-400 font-bold group-hover:translate-x-0.5 transition-transform">Ver m&oacute;dulo &#8594;</span>
                </div>
            </a>

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#128230;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-emerald-100 dark:bg-emerald-950 text-emerald-700 dark:text-emerald-300 px-2 py-0.5 rounded">mod03</span>
                            <span class="text-[10px] bg-emerald-50 dark:bg-emerald-900/60 text-emerald-700 dark:text-emerald-300 border border-emerald-200 dark:border-emerald-800 font-bold px-2 py-0.5 rounded">BD: Brenda</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        03. Inventario &amp; Equipos
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Control patrimonial, inventario de laboratorios y asignaci&oacute;n de equipos inform&aacute;ticos.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; DBA: <strong class="text-slate-700 dark:text-slate-300">Brenda</strong></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#128230;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-amber-100 dark:bg-amber-950 text-amber-700 dark:text-amber-300 px-2 py-0.5 rounded">mod04</span>
                            <span class="text-[10px] bg-amber-50 dark:bg-amber-900/60 text-amber-700 dark:text-amber-300 border border-amber-200 dark:border-amber-800 font-bold px-2 py-0.5 rounded">BD: Eq 04</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        04. M&oacute;dulo 04
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Espacio operativo y servicios del M&oacute;dulo 04 institucional.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; DBA: <strong class="text-slate-700 dark:text-slate-300">Equipo 04</strong></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#128736;&#65039;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-purple-100 dark:bg-purple-950 text-purple-700 dark:text-purple-300 px-2 py-0.5 rounded">mod05</span>
                            <span class="text-[10px] bg-purple-50 dark:bg-purple-900/60 text-purple-700 dark:text-purple-300 border border-purple-200 dark:border-purple-800 font-bold px-2 py-0.5 rounded">BD: Oliva</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        05. Incidencias &amp; Requerimientos
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Mesa de ayuda TI, tickets de soporte t&eacute;cnico, incidentes de red y requerimientos.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; DBA: <strong class="text-slate-700 dark:text-slate-300">Oliva</strong></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#127891;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-rose-100 dark:bg-rose-950 text-rose-700 dark:text-rose-300 px-2 py-0.5 rounded">mod06</span>
                            <span class="text-[10px] bg-rose-50 dark:bg-rose-900/60 text-rose-700 dark:text-rose-300 border border-rose-200 dark:border-rose-800 font-bold px-2 py-0.5 rounded">BD: Sandra</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        06. Egresados &amp; Titulaci&oacute;n
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Seguimiento a graduados, tr&aacute;mites de titulaci&oacute;n profesional, grados y constancias.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; DBA: <strong class="text-slate-700 dark:text-slate-300">Sandra</strong></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#128202;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-teal-100 dark:bg-teal-950 text-teal-700 dark:text-teal-300 px-2 py-0.5 rounded">mod07</span>
                            <span class="text-[10px] bg-teal-50 dark:bg-teal-900/60 text-teal-700 dark:text-teal-300 border border-teal-200 dark:border-teal-800 font-bold px-2 py-0.5 rounded">BD: Brayan</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        07. Encuestas &amp; Calidad Docente
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Evaluaci&oacute;n del desempe&ntilde;o docente, encuestas de satisfacci&oacute;n y calidad institucional.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; DBA: <strong class="text-slate-700 dark:text-slate-300">Brayan</strong></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#128272;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-orange-100 dark:bg-orange-950 text-orange-700 dark:text-orange-300 px-2 py-0.5 rounded">mod08</span>
                            <span class="text-[10px] bg-orange-50 dark:bg-orange-900/60 text-orange-700 dark:text-orange-300 border border-orange-200 dark:border-orange-800 font-bold px-2 py-0.5 rounded">BD: Toro</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        08. Login, Seguridad &amp; Roles
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Gesti&oacute;n de credenciales, pol&iacute;ticas de acceso institucional y asignaci&oacute;n de roles.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; DBA: <strong class="text-slate-700 dark:text-slate-300">Toro</strong></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>

            <a href="#" class="p-5 rounded-2xl bg-white dark:bg-[#111827] hover:bg-slate-50 dark:hover:bg-slate-800/80 border border-slate-200/80 dark:border-slate-800 shadow-sm transition group flex flex-col justify-between">
                <div>
                    <div class="flex items-center justify-between mb-3">
                        <span class="text-xl">&#128179;</span>
                        <div class="flex items-center gap-1.5">
                            <span class="text-[10px] font-mono font-bold bg-cyan-100 dark:bg-cyan-950 text-cyan-700 dark:text-cyan-300 px-2 py-0.5 rounded">mod09</span>
                            <span class="text-[10px] bg-cyan-50 dark:bg-cyan-900/60 text-cyan-700 dark:text-cyan-300 border border-cyan-200 dark:border-cyan-800 font-bold px-2 py-0.5 rounded">BD: Eq 09</span>
                        </div>
                    </div>
                    <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 group-hover:text-blue-600 dark:group-hover:text-cyan-400 transition">
                        09. Tesorer&iacute;a &amp; Pagos TUPA
                    </h3>
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-1 leading-relaxed">
                        Recaudaci&oacute;n de tasas institucionales, constancias, caja chica y estado de cuenta.
                    </p>
                </div>
                <div class="mt-4 pt-2 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between text-[11px]">
                    <span class="text-slate-400 font-medium">&#128452;&#65039; DBA: <strong class="text-slate-700 dark:text-slate-300">Equipo 09</strong></span>
                    <span class="text-blue-600 dark:text-cyan-400 font-semibold group-hover:translate-x-0.5 transition">Ingresar &#8594;</span>
                </div>
            </a>
        </div>
    </div>
</div>
</asp:Content>
