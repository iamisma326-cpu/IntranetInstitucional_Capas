<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Modulo02.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Modulo02" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="space-y-6">

        <div class="bg-white dark:bg-[#111827] rounded-3xl p-6 border border-slate-200/80 dark:border-slate-800 shadow-sm flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
            <div class="flex items-center gap-4">
                <div class="w-12 h-12 rounded-2xl bg-indigo-50 dark:bg-indigo-950/50 text-indigo-600 dark:text-indigo-400 flex items-center justify-center text-xl shrink-0 font-bold">
                    &#128197;
                </div>
                <div>
                    <div class="flex items-center gap-2 mb-1">
                        <span class="text-[10px] font-mono font-bold bg-indigo-100 dark:bg-indigo-950 text-indigo-800 dark:text-indigo-300 px-2 py-0.5 rounded">02</span>
                        <span class="text-xs font-bold text-indigo-700 dark:text-indigo-300 bg-indigo-50 dark:bg-indigo-900/40 px-2 py-0.5 rounded-md border border-indigo-200 dark:border-indigo-800">&#127963;&#65039; IESTP "Argentina" &mdash; Ley 30512 &amp; MPA 2024</span>
                    </div>
                    <h1 class="text-lg sm:text-xl font-black text-slate-900 dark:text-slate-100 tracking-tight">
                        Control de Asistencia &amp; Prevenci&oacute;n de Deserci&oacute;n DPI
                    </h1>
                    <p class="text-xs text-slate-500 dark:text-slate-400">
                        S&aacute;bana de 18 semanas, toma flash en 1 clic, atajos de teclado y sincronizaci&oacute;n con REGISTRA MINEDU.
                    </p>
                </div>
            </div>
            <div class="flex flex-wrap gap-2">
                <a href="Modulo02_MiAsistencia.aspx" class="btn btn-ghost btn-sm rounded-xl text-xs font-semibold">
                    &#128737;&#65039; Mi Asistencia (Alumno)
                </a>
                <a href="Modulo02_Justificaciones.aspx" class="btn btn-ghost btn-sm rounded-xl text-xs font-semibold">
                    &#128196; Justificaciones
                    <% if (_vm.TotalJustificacionesPendientes > 0) { %>
                    <span class="badge badge-error badge-xs text-white font-bold"><%= _vm.TotalJustificacionesPendientes %></span>
                    <% } %>
                </a>
                <a href="Modulo02_ReportesDpi.aspx" class="btn bg-rose-600 hover:bg-rose-700 text-white btn-sm rounded-xl text-xs font-bold shadow-sm">
                    &#128680; Radar DPI (&ge;30%)
                </a>
            </div>
        </div>

        <% if (_claseActiva != null) { %>
        <div class="bg-gradient-to-r from-indigo-900 via-indigo-800 to-slate-900 text-white rounded-3xl p-6 sm:p-7 shadow-lg border border-indigo-700/50 flex flex-col md:flex-row justify-between items-start md:items-center gap-5 relative overflow-hidden">
            <div class="space-y-2 relative z-10">
                <div class="flex flex-wrap items-center gap-2">
                    <span class="badge bg-emerald-500 text-white font-black text-[10px] px-2.5 py-1 tracking-wider uppercase animate-pulse border-0">
                        &#9889; Clase Activa Hoy
                    </span>
                    <span class="text-xs font-mono bg-indigo-950/80 text-indigo-200 px-2 py-0.5 rounded border border-indigo-700/60">
                        <%: _claseActiva.UnidadDidacticaCodigo %>
                    </span>
                    <span class="text-xs text-indigo-200">
                        Semana <%= _claseActiva.NumeroSemana %> &bull; <%: _claseActiva.Ciclo %> <%: _claseActiva.Turno %> (Secc <%: _claseActiva.Seccion %>)
                    </span>
                    <span class="text-xs font-bold bg-indigo-700/80 text-white px-2 py-0.5 rounded">
                        &#127979; <%: _claseActiva.AulaCodigo %>
                    </span>
                </div>
                <h2 class="text-xl sm:text-2xl font-black tracking-tight text-white">
                    <%: _claseActiva.UnidadDidacticaNombre %>
                </h2>
                <p class="text-xs text-indigo-200/90 flex items-center gap-2">
                    <span>&#9200; Horario: <strong><%= Hora(_claseActiva.HoraInicio) %> &ndash; <%= Hora(_claseActiva.HoraFin) %></strong></span>
                    <span>&bull;</span>
                    <span>&#128101; N&oacute;mina: <strong><%= _claseActiva.TotalAlumnos %> alumnos</strong></span>
                    <span>&bull;</span>
                    <span>Estado: <strong class="text-emerald-300"><%: _claseActiva.EstadoSesion %></strong></span>
                </p>
            </div>

            <div class="flex flex-wrap items-center gap-3 relative z-10 w-full md:w-auto">
                <a href='Modulo02_TomarAsistencia.aspx?sesionId=<%= _claseActiva.SesionId %>&amp;claseId=<%= _claseActiva.ClaseDocenteId %>' class="btn bg-emerald-500 hover:bg-emerald-400 text-slate-950 font-black text-sm px-6 rounded-2xl border-0 shadow-xl transition transform hover:scale-105 w-full md:w-auto flex items-center justify-center gap-2">
                    <span>&#9889; Tomar Asistencia Ahora (1 Clic)</span>
                </a>
                <a href='Modulo02_Matriz.aspx?claseId=<%= _claseActiva.ClaseDocenteId %>' class="btn bg-indigo-950/80 hover:bg-indigo-900 text-indigo-100 font-bold text-xs px-4 rounded-2xl border border-indigo-600/60 w-full md:w-auto">
                    &#128202; Ver S&aacute;bana 18 Sem
                </a>
            </div>

            <div class="absolute -right-10 -bottom-10 w-48 h-48 bg-indigo-600/10 rounded-full blur-2xl pointer-events-none"></div>
        </div>
        <% } %>

        <div class="grid grid-cols-2 lg:grid-cols-4 gap-4">
            <div class="bg-white dark:bg-[#111827] rounded-2xl p-4 border border-slate-200/80 dark:border-slate-800 shadow-sm">
                <span class="text-[10px] font-bold text-slate-400 uppercase tracking-wider block">Cursos Asignados</span>
                <div class="text-xl font-bold text-indigo-600 dark:text-indigo-400 mt-1"><%= _vm.TotalClasesAsignadas %></div>
                <p class="text-[10px] text-slate-400 mt-0.5">Semestre 2026-I (18 semanas)</p>
            </div>
            <div class="bg-white dark:bg-[#111827] rounded-2xl p-4 border border-slate-200/80 dark:border-slate-800 shadow-sm">
                <span class="text-[10px] font-bold text-slate-400 uppercase tracking-wider block">Sesiones Hoy</span>
                <div class="text-xl font-bold text-emerald-600 dark:text-emerald-400 mt-1"><%= _vm.TotalSesionesHoy %></div>
                <p class="text-[10px] text-slate-400 mt-0.5">Programadas</p>
            </div>
            <div class="bg-white dark:bg-[#111827] rounded-2xl p-4 border border-slate-200/80 dark:border-slate-800 shadow-sm">
                <span class="text-[10px] font-bold text-slate-400 uppercase tracking-wider block">Alumnos en DPI</span>
                <div class="text-xl font-bold text-rose-600 dark:text-rose-400 mt-1 flex items-center gap-1.5">
                    <span><%= _vm.TotalAlumnosDpi %></span>
                    <span class="text-[10px] font-semibold bg-rose-50 dark:bg-rose-950/50 text-rose-700 dark:text-rose-300 px-1.5 py-0.5 rounded">&ge;30% Faltas</span>
                </div>
                <p class="text-[10px] text-slate-400 mt-0.5">Inhabilitados para Recup. Sem 17</p>
            </div>
            <div class="bg-white dark:bg-[#111827] rounded-2xl p-4 border border-slate-200/80 dark:border-slate-800 shadow-sm">
                <span class="text-[10px] font-bold text-slate-400 uppercase tracking-wider block">Justificaciones 72h</span>
                <div class="text-xl font-bold text-amber-600 dark:text-amber-400 mt-1"><%= _vm.TotalJustificacionesPendientes %></div>
                <p class="text-[10px] text-slate-400 mt-0.5">Por Evaluar</p>
            </div>
        </div>

        <% if (_vm.EsDocente || _vm.MisClasesDocente.Count > 0) { %>
        <div class="space-y-3">
            <div class="flex items-center justify-between">
                <h2 class="text-xs font-bold text-slate-500 dark:text-slate-400 uppercase tracking-wider">
                    Mis Asignaturas Asignadas
                </h2>
                <span class="text-[11px] text-indigo-600 dark:text-indigo-400 font-semibold">
                    Ciclo de 18 Semanas: Sem 1..16 Regular &bull; Sem 17 Recuperaci&oacute;n &bull; Sem 18 Cierre
                </span>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                <asp:Repeater ID="repClasesDocente" runat="server">
                    <ItemTemplate>
                        <div class="bg-white dark:bg-[#111827] rounded-2xl p-5 border border-slate-200/80 dark:border-slate-800 shadow-sm flex flex-col justify-between space-y-3 hover:shadow-md transition">
                            <div class="space-y-2">
                                <div class="flex items-center justify-between">
                                    <span class="text-[10px] font-mono font-bold bg-indigo-50 dark:bg-indigo-950 text-indigo-700 dark:text-indigo-300 px-2 py-0.5 rounded"><%# Eval("UnidadDidacticaCodigo") %></span>
                                    <span class="text-[10px] text-slate-400">Ciclo <%# Eval("Ciclo") %> &bull; <%# Eval("Turno") %> (Secc <%# Eval("Seccion") %>)</span>
                                </div>
                                <h3 class="font-bold text-xs text-slate-900 dark:text-slate-100 leading-snug">
                                    <%# Eval("UnidadDidacticaNombre") %>
                                </h3>
                                <p class="text-[11px] text-slate-400"><%# Eval("CarreraNombre") %> &bull; &#127979; <%# AulaDe(Eval("AulaCodigo")) %></p>

                                <div class="space-y-1 pt-1">
                                    <div class="flex justify-between text-[11px] text-slate-500 dark:text-slate-400">
                                        <span>Avance Acad&eacute;mico</span>
                                        <span class="font-mono font-bold"><%# Eval("SemanasRegistradas") %> / 18 sem</span>
                                    </div>
                                    <div class="w-full bg-slate-100 dark:bg-slate-800 h-2 rounded-full overflow-hidden flex">
                                        <div class="bg-indigo-600 h-full transition-all" style='width: <%# Avance(Eval("SemanasRegistradas")) %>%'></div>
                                    </div>
                                </div>
                            </div>

                            <div class="pt-3 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between gap-2">
                                <a href='Modulo02_Matriz.aspx?claseId=<%# Eval("ClaseId") %>' class="btn btn-ghost btn-xs text-slate-700 dark:text-slate-300 font-bold hover:bg-slate-100 dark:hover:bg-slate-800 rounded-lg">
                                    &#128202; S&aacute;bana Live Grid
                                </a>
                                <a href='Modulo02_Clase.aspx?claseId=<%# Eval("ClaseId") %>' class="btn bg-indigo-50 hover:bg-indigo-100 text-indigo-700 dark:bg-indigo-950/60 dark:text-indigo-300 btn-xs font-bold rounded-lg border border-indigo-200 dark:border-indigo-800">
                                    18 Semanas &#8594;
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <% } %>

        <div class="grid grid-cols-1 lg:grid-cols-3 gap-4">

            <div class="lg:col-span-2 space-y-3">
                <h2 class="text-xs font-bold text-slate-500 dark:text-slate-400 uppercase tracking-wider">
                    Sesiones Dictadas &amp; En Curso
                </h2>

                <div class="bg-white dark:bg-[#111827] rounded-2xl border border-slate-200/80 dark:border-slate-800 shadow-sm overflow-hidden divide-y divide-slate-100 dark:divide-slate-800">
                    <asp:Repeater ID="repSesiones" runat="server">
                        <ItemTemplate>
                            <div class="p-4 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-3 hover:bg-slate-50/50 dark:hover:bg-slate-900/40 transition">
                                <div class="flex items-start gap-3">
                                    <div class="w-9 h-9 rounded-xl bg-indigo-50 dark:bg-indigo-950/50 text-indigo-700 dark:text-indigo-300 flex flex-col items-center justify-center shrink-0 font-bold">
                                        <span class="text-[8px] uppercase">S</span>
                                        <span class="text-xs leading-none"><%# Eval("NumeroSemana") %></span>
                                    </div>
                                    <div>
                                        <div class="flex items-center gap-2">
                                            <span class="font-bold text-xs text-slate-900 dark:text-slate-100"><%# Eval("UnidadDidacticaNombre") %></span>
                                            <span class='<%# EsCerrada(Eval("Estado")) ? "badge badge-success badge-xs font-semibold text-white" : "badge badge-warning badge-xs font-semibold text-white animate-pulse" %>'><%# EsCerrada(Eval("Estado")) ? "Oficializada" : "Abierta" %></span>
                                        </div>
                                        <p class="text-[11px] text-slate-500 dark:text-slate-400 mt-0.5">
                                            Docente: <%# Eval("DocenteNombreCompleto") %> &bull; <%# AulaDe(Eval("AulaCodigo")) %> &bull; <%# HoraDe(Eval("HoraInicio")) %> a <%# HoraDe(Eval("HoraFin")) %>
                                        </p>
                                    </div>
                                </div>

                                <div class="flex items-center gap-2 w-full sm:w-auto justify-end">
                                    <a href='Modulo02_TomarAsistencia.aspx?sesionId=<%# Eval("Id") %>&amp;claseId=<%# Eval("ClaseDocenteId") %>' class="btn bg-indigo-600 hover:bg-indigo-700 text-white btn-xs rounded-lg font-bold border-0 px-3">
                                        <%# EsCerrada(Eval("Estado")) ? "Ver Asistencia" : "Tomar Lista" %>
                                    </a>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlSesionesVacio" runat="server" Visible="false">
                        <div class="p-6 text-center text-xs text-slate-400">
                            No hay sesiones registradas hoy.
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <div class="space-y-3">
                <div class="flex items-center justify-between">
                    <h2 class="text-xs font-bold text-slate-500 dark:text-slate-400 uppercase tracking-wider">
                        Radar DPI (&ge;20% Faltas)
                    </h2>
                    <a href="Modulo02_ReportesDpi.aspx" class="text-[10px] text-indigo-600 font-bold hover:underline">Ver Todos</a>
                </div>

                <div class="bg-white dark:bg-[#111827] rounded-2xl border border-slate-200/80 dark:border-slate-800 shadow-sm p-4 space-y-3">
                    <asp:Repeater ID="repCasosDpi" runat="server">
                        <ItemTemplate>
                            <div class="p-2.5 rounded-xl bg-slate-50 dark:bg-slate-900/60 border border-slate-200/60 dark:border-slate-800 flex items-center justify-between gap-2 text-xs">
                                <div>
                                    <p class="font-bold text-slate-900 dark:text-slate-100 text-[11px] leading-tight"><%# Eval("EstudianteNombre") %></p>
                                    <p class="text-[10px] text-slate-400"><%# Eval("UnidadDidacticaNombre") %></p>
                                </div>
                                <div class="text-right shrink-0">
                                    <span class='<%# EsDpi(Eval("PorcentajeInasistencia")) ? "badge badge-error badge-xs font-black text-white" : "badge badge-warning badge-xs font-bold text-white" %>'><%# EsDpi(Eval("PorcentajeInasistencia")) ? "DPI " : "Riesgo " %><%# Pct(Eval("PorcentajeInasistencia")) %>%</span>
                                    <span class='<%# EsDpi(Eval("PorcentajeInasistencia")) ? "text-[9px] text-rose-600 font-bold block" : "text-[9px] text-amber-600 font-semibold block" %>'><%# EsDpi(Eval("PorcentajeInasistencia")) ? "Inhabilitado Sem 17" : "Alerta temprana" %></span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlCasosDpiVacio" runat="server" Visible="false">
                        <p class="text-xs text-slate-400 text-center py-4">Cero estudiantes en condici&oacute;n cr&iacute;tica de DPI.</p>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
