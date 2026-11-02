<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Modulo02_Clase.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Modulo02_Clase" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="space-y-3 pb-20">

        <div class="rounded-[22px] border border-slate-200 dark:border-slate-800 bg-white dark:bg-[#111827] px-4 py-3 shadow-[0_10px_24px_rgba(15,23,42,0.04)]">
            <div class="flex flex-col gap-2.5 xl:flex-row xl:items-center xl:justify-between">
                <div class="flex items-center gap-3">
                    <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-slate-900 dark:bg-slate-100 text-base font-black text-white dark:text-slate-900 shadow-sm">A</div>
                    <div class="min-w-0">
                        <div class="mb-0.5 flex flex-wrap items-center gap-2 text-[10px] font-black uppercase tracking-[0.18em] text-slate-500 dark:text-slate-400">
                            <span><%: CodigoCurso %></span>
                            <span class="rounded-full bg-slate-100 dark:bg-slate-800 px-2 py-0.5 text-slate-600 dark:text-slate-300">Ciclo <%: Ciclo %> &bull; <%: Turno %></span>
                            <span class="rounded-full bg-emerald-100 dark:bg-emerald-950 px-2 py-0.5 text-emerald-700 dark:text-emerald-300">18 semanas</span>
                        </div>
                        <h1 class="text-lg font-black tracking-tight text-slate-900 dark:text-slate-100"><%: NombreCurso %></h1>
                    </div>
                </div>

                <div class="flex flex-wrap items-center gap-1.5">
                    <button type="button" onclick="modal_reprogramar.showModal()" class="btn bg-amber-50 hover:bg-amber-100 text-amber-800 dark:bg-amber-950 dark:text-amber-300 btn-xs rounded-lg font-bold border border-amber-200 px-2.5 shadow-sm">&#128260; Reprogramar</button>
                    <a href="<%= RutaSabana %>" class="btn bg-indigo-50 dark:bg-indigo-950 hover:bg-indigo-100 text-indigo-700 dark:text-indigo-300 btn-xs rounded-lg font-bold border border-indigo-200 px-2.5 shadow-sm">&#128202; S&aacute;bana</a>
                    <a href="<%= RutaVolver %>" class="btn bg-white dark:bg-slate-800 hover:bg-slate-50 text-slate-700 dark:text-slate-200 btn-xs rounded-lg font-bold border border-slate-200 px-2.5 shadow-sm">&#8592; Volver</a>
                </div>
            </div>

            <nav class="mt-3 flex w-full flex-wrap gap-1.5 border-t border-slate-200 dark:border-slate-700 pt-2.5 text-[10px] font-black uppercase tracking-[0.18em]"
                 aria-label="Ficha de curso">
                <a href="<%= RutaAsistencia %>" class="<%= TabAsistencia %>">Asistencia</a>
                <a href="<%= RutaDetalle %>" class="<%= TabDetalle %>">Detalle por Semana</a>
                <a href="<%= RutaJustificaciones %>" class="<%= TabJustificaciones %>">Justificaciones</a>
                <a href="<%= RutaReporte %>" class="<%= TabReporte %>">Reporte DPI</a>
            </nav>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
            <asp:Repeater ID="repSemanas" runat="server">
                <ItemTemplate>
                    <div class='rounded-3xl p-5 border <%# Eval("TarjetaBorder") %> shadow-sm transition flex flex-col justify-between group'>
                        <div>

                            <div class="flex justify-between items-start mb-3">
                                <div class="flex items-center gap-2.5">
                                    <div class='w-10 h-10 rounded-2xl <%# Eval("ClaseIcono") %> flex items-center justify-center font-extrabold text-sm shadow-inner'>
                                        <%# Eval("NumeroSemana") %>
                                    </div>
                                    <div>
                                        <h3 class="font-extrabold text-sm text-slate-900 dark:text-slate-100 leading-tight">
                                            <%# Eval("TituloSemanaHtml") %>
                                        </h3>
                                        <span class="text-[11px] text-slate-400 font-medium">
                                            <%# Eval("SubtituloSemanaHtml") %>
                                        </span>
                                    </div>
                                </div>
                                <span class='badge <%# Eval("BadgeSemana") %> badge-xs py-2 px-2.5'>
                                    <%# Eval("EstadoSemana") %>
                                </span>
                            </div>

                            <div class="space-y-2 mt-3">
                                <asp:Repeater ID="repSesiones" runat="server" DataSource='<%# Eval("Sesiones") %>'>
                                    <ItemTemplate>
                                        <div class="bg-slate-50/80 dark:bg-slate-900/60 hover:bg-slate-100 dark:hover:bg-slate-800 p-3 rounded-2xl border border-slate-100 dark:border-slate-800 transition flex flex-col gap-1.5 text-xs">
                                            <div class="flex justify-between items-center font-semibold text-slate-700 dark:text-slate-300">
                                                <span class="flex items-center gap-1.5">
                                                    <span>&#128197; <%# Eval("FechaTexto") %></span>
                                                    <span class="text-[10px] text-slate-400 font-normal">(<%# Eval("HoraInicioTexto") %> &ndash; <%# Eval("HoraFinTexto") %>)</span>
                                                </span>
                                                <span class="font-mono text-[11px] bg-slate-200/60 dark:bg-slate-800 px-1.5 py-0.5 rounded text-slate-600 dark:text-slate-300"><%# Eval("HorasPedagogicas") %> hrs</span>
                                            </div>

                                            <%# Eval("TemaHtml") %>

                                            <asp:Panel ID="pnlMetricas" runat="server" Visible='<%# Eval("MostrarMetricas") %>'
                                                CssClass="flex flex-wrap items-center gap-2 text-[10px] font-bold pt-1 border-t border-slate-200/60 dark:border-slate-800">
                                                <span class="text-emerald-700 dark:text-emerald-400">&#10003; <%# Eval("TotalPresentes") %> Presentes</span>
                                                <span class="text-amber-700 dark:text-amber-400">&#9201;&#65039; <%# Eval("TotalTardanzas") %> Tardanzas</span>
                                                <span class="text-rose-700 dark:text-rose-400">&#10060; <%# Eval("TotalFaltas") %> Faltas</span>
                                                <span class="text-blue-700 dark:text-blue-400">&#128998; <%# Eval("TotalJustificadas") %> Justificadas</span>
                                                <%# Eval("EnCursoHtml") %>
                                            </asp:Panel>

                                            <div class="pt-1 flex justify-end">
                                                <a href='<%# Eval("UrlAsistencia") %>' class='<%# Eval("ClaseBoton") %> btn-xs rounded-lg font-bold border-0 px-3'>
                                                    <%# Eval("TextoBotonHtml") %>
                                                </a>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <dialog id="modal_reprogramar" class="modal">
        <div class="modal-box max-w-lg bg-white dark:bg-[#111827] rounded-3xl p-6 shadow-2xl border border-slate-200 dark:border-slate-800">
            <h3 class="text-base font-bold text-slate-900 dark:text-slate-100 mb-1">
                Reprogramaci&oacute;n El&aacute;stica de Horarios Futuros
            </h3>
            <p class="text-xs text-slate-400 mb-4">
                Modifica el d&iacute;a, hora o aula a partir de una semana $N$. Las sesiones de semanas anteriores cerradas quedan blindadas e intactas.
            </p>

            <div class="space-y-3 text-xs">
                <div>
                    <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Aplicar a partir de la Semana</label>
                    <asp:DropDownList ID="ddlDesdeSemana" runat="server"
                        CssClass="select select-sm select-bordered w-full rounded-xl text-xs" />
                </div>

                <div class="grid grid-cols-2 gap-3">
                    <div>
                        <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Nuevo D&iacute;a</label>
                        <asp:DropDownList ID="ddlNuevoDia" runat="server"
                            CssClass="select select-sm select-bordered w-full rounded-xl text-xs">
                            <asp:ListItem Value="1" Text="Lunes" />
                            <asp:ListItem Value="2" Text="Martes" />
                            <asp:ListItem Value="3" Text="Mi&eacute;rcoles" />
                            <asp:ListItem Value="4" Text="Jueves" />
                            <asp:ListItem Value="5" Text="Viernes" />
                            <asp:ListItem Value="6" Text="S&aacute;bado" />
                        </asp:DropDownList>
                    </div>
                    <div>
                        <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Nueva Aula / Lab</label>
                        <asp:TextBox ID="txtAula" runat="server" placeholder="LAB-402"
                            CssClass="input input-sm input-bordered w-full rounded-xl text-xs" />
                    </div>
                </div>

                <div class="grid grid-cols-2 gap-3">
                    <div>
                        <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Hora Inicio</label>
                        <asp:TextBox ID="txtHoraInicio" runat="server" TextMode="Time" Text="18:35"
                            CssClass="input input-sm input-bordered w-full rounded-xl text-xs" />
                    </div>
                    <div>
                        <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Hora Fin</label>
                        <asp:TextBox ID="txtHoraFin" runat="server" TextMode="Time" Text="21:45"
                            CssClass="input input-sm input-bordered w-full rounded-xl text-xs" />
                    </div>
                </div>

                <div>
                    <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Motivo Institucional</label>
                    <asp:TextBox ID="txtMotivo" runat="server"
                        placeholder="Ej: Adecuaci&oacute;n de disponibilidad de laboratorio..."
                        required="required"
                        CssClass="input input-sm input-bordered w-full rounded-xl text-xs" />
                </div>

                <div class="modal-action mt-6 flex gap-2">
                    <button type="button" onclick="modal_reprogramar.close()" class="btn btn-ghost btn-sm rounded-xl">Cancelar</button>
                    <asp:Button ID="btnReprogramar" runat="server" Text="Aplicar Reprogramaci&oacute;n"
                        OnClick="btnReprogramar_Click"
                        CssClass="btn bg-amber-500 hover:bg-amber-600 text-slate-950 btn-sm rounded-xl font-bold border-0 px-4" />
                </div>
            </div>
        </div>
    </dialog>
</asp:Content>
