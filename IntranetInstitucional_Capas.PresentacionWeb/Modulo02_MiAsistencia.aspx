<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Modulo02_MiAsistencia.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Modulo02_MiAsistencia" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="space-y-6">

        <div class="bg-white dark:bg-[#111827] rounded-3xl p-6 sm:p-7 border border-slate-200/80 dark:border-slate-800 shadow-sm flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
            <div class="flex items-center gap-4">
                <div class="w-14 h-14 rounded-2xl bg-indigo-100 dark:bg-indigo-950 text-indigo-700 dark:text-indigo-300 border border-indigo-200 dark:border-indigo-800 flex items-center justify-center shrink-0 shadow-sm font-bold text-2xl">
                    &#128737;&#65039;
                </div>
                <div>
                    <div class="flex items-center gap-2 mb-1">
                        <span class="text-xs font-bold bg-indigo-100 dark:bg-indigo-950 text-indigo-800 dark:text-indigo-300 px-2.5 py-0.5 rounded-full border border-indigo-200 dark:border-indigo-800">Portal del Estudiante</span>
                        <span class="text-xs text-slate-400 font-medium">Ciclo 2026-I (18 Semanas)</span>
                    </div>
                    <h1 class="text-xl sm:text-2xl font-black text-slate-900 dark:text-slate-100 tracking-tight">
                        Mi Asistencia &amp; Radar Anti-DPI
                    </h1>
                    <p class="text-xs text-slate-500 dark:text-slate-400">
                        Control de inasistencias en tiempo real con c&aacute;lculo del l&iacute;mite de <strong>30% DPI (Art. 8.8.1 MPA)</strong>.
                    </p>
                </div>
            </div>
            <div class="flex gap-2">
                <a href="Modulo02_Justificaciones.aspx" class="btn bg-indigo-50 dark:bg-indigo-950 hover:bg-indigo-100 text-indigo-700 dark:text-indigo-300 btn-sm rounded-xl font-bold border border-indigo-200 dark:border-indigo-800 text-xs px-4">
                    &#128196; Ver Mis Justificaciones
                </a>
                <a href="Default.aspx" class="btn bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 text-slate-700 dark:text-slate-300 btn-sm rounded-xl font-bold border-0 text-xs px-3">
                    Volver al Inicio
                </a>
            </div>
        </div>

        <div class="bg-indigo-50/70 dark:bg-indigo-950/40 border border-indigo-200/80 dark:border-indigo-800 rounded-2xl p-4 flex items-start gap-3 text-xs text-indigo-900 dark:text-indigo-200">
            <span class="text-2xl shrink-0">&#128161;</span>
            <div class="space-y-1">
                <p class="font-bold">Reglamento Oficial de Asistencia y Semana 17 de Recuperaci&oacute;n (Art. 8.8.1 MPA):</p>
                <p class="text-[11px] text-indigo-700/90 dark:text-indigo-300/90">
                    Las faltas injustificadas acumulan horas pedag&oacute;gicas. Si tus horas de inasistencia alcanzan o superan el <strong>30% de las horas totales de la Unidad Did&aacute;ctica</strong>, quedas autom&aacute;ticamente en condici&oacute;n <strong>DPI (Desaprobado por Inasistencia) con nota 00</strong> y <strong>pierdes el derecho a rendir la evaluaci&oacute;n de recuperaci&oacute;n en la Semana 17</strong>.
                </p>
            </div>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
            <asp:Repeater ID="repCursos" runat="server">
                <ItemTemplate>
                    <div class="bg-white dark:bg-[#111827] rounded-3xl border border-slate-200/80 dark:border-slate-800 shadow-sm p-5 flex flex-col justify-between hover:shadow-md transition">
                        <div>

                            <div class="flex justify-between items-start gap-2 mb-3">
                                <div>
                                    <span class="text-[10px] font-mono font-bold text-slate-400 bg-slate-100 dark:bg-slate-800 px-2 py-0.5 rounded"><%# Eval("UnidadDidacticaCodigo") %></span>
                                    <h3 class="font-bold text-sm text-slate-900 dark:text-slate-100 mt-1 leading-snug"><%# Eval("UnidadDidacticaNombre") %></h3>
                                </div>
                                <span class='badge <%# BadgeSemaforo(Eval("SemaforoEstado")) %> badge-xs font-bold py-2 px-2.5 shrink-0'>
                                    <%# Eval("SemaforoEstado") %>
                                </span>
                            </div>

                            <div class="mt-4 mb-4">
                                <div class="flex justify-between text-[11px] font-semibold text-slate-600 dark:text-slate-400 mb-1">
                                    <span>Inasistencia acumulada</span>
                                    <span class="font-bold text-slate-900 dark:text-slate-100"><%# Pct(Eval("PorcentajeInasistencia")) %>% / 30% max</span>
                                </div>
                                <div class="w-full bg-slate-100 dark:bg-slate-800 h-2.5 rounded-full overflow-hidden">
                                    <div class='<%# BarraSemaforo(Eval("SemaforoEstado")) %> h-full rounded-full transition-all duration-500' style='width: <%# AnchoDpi(Eval("PorcentajeInasistencia")) %>%'></div>
                                </div>
                            </div>

                            <div class="grid grid-cols-4 gap-1.5 bg-slate-50 dark:bg-slate-900/60 p-2.5 rounded-2xl text-center text-xs mb-4 border border-slate-100 dark:border-slate-800">
                                <div>
                                    <span class="text-[9px] text-slate-400 font-bold block">PRES</span>
                                    <span class="font-bold text-emerald-600 dark:text-emerald-400 text-sm"><%# Eval("CantPresentes") %></span>
                                </div>
                                <div>
                                    <span class="text-[9px] text-slate-400 font-bold block">TARD</span>
                                    <span class="font-bold text-amber-600 dark:text-amber-400 text-sm"><%# Eval("CantTardanzas") %></span>
                                </div>
                                <div>
                                    <span class="text-[9px] text-slate-400 font-bold block">JUST</span>
                                    <span class="font-bold text-blue-600 dark:text-blue-400 text-sm"><%# Eval("CantFaltasJustificadas") %></span>
                                </div>
                                <div>
                                    <span class="text-[9px] text-slate-400 font-bold block">FALTAS</span>
                                    <span class="font-bold text-rose-600 dark:text-rose-400 text-sm"><%# Eval("CantFaltasInjustificadas") %></span>
                                </div>
                            </div>

                            <div class="text-[11px] text-slate-500 dark:text-slate-400 mb-2 space-y-0.5">
                                <p>&#9201;&#65039; Total Horas Asignatura: <strong><%# Eval("TotalHorasSemestre") %> hrs</strong></p>
                                <p>&#9888;&#65039; Margen de Falta antes de DPI: <strong class='<%# Convert.ToInt32(Eval("HorasFaltaDisponibles")) <= 4 ? "text-rose-600 font-black" : "text-emerald-700 dark:text-emerald-400 font-bold" %>'><%# Eval("HorasFaltaDisponibles") %> hrs</strong></p>
                            </div>
                        </div>

                        <div class="pt-3 border-t border-slate-100 dark:border-slate-800 flex justify-end">
                            <a href='Modulo02_DetalleCurso.aspx?unidadId=<%# Eval("UnidadDidacticaId") %>&amp;estudianteId=<%= _estudianteId %>' class="btn bg-indigo-50 dark:bg-indigo-950 hover:bg-indigo-100 text-indigo-700 dark:text-indigo-300 btn-xs rounded-xl font-bold border border-indigo-200 dark:border-indigo-800 px-3">
                                Ver Sesiones &amp; Justificar (72h) &#8594;
                            </a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlCursosVacio" runat="server" Visible="false" CssClass="col-span-full">
                <div class="col-span-full bg-white dark:bg-[#111827] rounded-3xl p-10 text-center border border-slate-200 dark:border-slate-800">
                    <p class="text-xs text-slate-400">No se encontraron asignaturas con registro de asistencia en este periodo.</p>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
