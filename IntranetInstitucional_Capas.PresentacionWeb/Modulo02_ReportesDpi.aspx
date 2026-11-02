<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Modulo02_ReportesDpi.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Modulo02_ReportesDpi" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="ContentEstilos" ContentPlaceHolderID="EstilosPagina" runat="server">
    <style>

        .tabla-radar tbody > tr + tr {
            border-top: 1px solid #f1f5f9;
        }
        .tabla-radar th {
            text-align: left;
            vertical-align: middle;
        }

        .indicadores-radar {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
            gap: 1rem;
            border: 0;
            border-collapse: separate;
        }
        .indicadores-radar > tbody,
        .indicadores-radar > tbody > tr {
            display: contents;
        }
        .indicadores-radar td {
            padding: 0;
            border: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="space-y-6 pb-20">

        <div class="bg-white dark:bg-[#111827] rounded-3xl p-6 sm:p-7 border border-slate-200/80 dark:border-slate-800 shadow-sm flex flex-col sm:flex-row sm:flex-wrap justify-between items-start sm:items-center gap-4">
            <div class="flex items-center gap-4">
                <div class="w-14 h-14 rounded-2xl bg-rose-100 dark:bg-rose-950 text-rose-700 dark:text-rose-300 border border-rose-200 dark:border-rose-800 flex items-center justify-center shrink-0 shadow-sm font-bold text-2xl">
                    &#128680;
                </div>
                <div>
                    <div class="flex items-center gap-2 mb-1.5">
                        <span class="text-xs font-bold bg-rose-100 dark:bg-rose-950 text-rose-800 dark:text-rose-300 px-2.5 py-0.5 rounded-full border border-rose-200 dark:border-rose-800">Supervisi&oacute;n Acad&eacute;mica y Direcci&oacute;n</span>
                        <span class="text-xs text-slate-400 font-medium">L&iacute;mite 30% Inasistencias (Art. 8.8.1 MPA 2024)</span>
                    </div>
                    <h1 class="text-xl sm:text-2xl font-black text-slate-900 dark:text-slate-100 tracking-tight">
                        Radar Preventivo de Deserci&oacute;n &amp; Casos DPI
                    </h1>
                    <p class="text-xs text-slate-500 dark:text-slate-400">
                        Detecci&oacute;n temprana de estudiantes en zona de riesgo (&ge;20%) e inhabilitados por DPI (&ge;30%) sin derecho a recuperaci&oacute;n en Semana 17.
                    </p>
                </div>
            </div>
            <div class="flex gap-2">
                <button type="button" onclick="window.print()" class="btn bg-white dark:bg-slate-800 hover:bg-slate-50 text-slate-700 dark:text-slate-200 btn-sm rounded-xl font-bold border border-slate-200 text-xs px-3.5 shadow-sm">
                    &#128424;&#65039; Imprimir Radar
                </button>
                <a href="Modulo02.aspx" class="btn bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 text-slate-700 dark:text-slate-300 btn-sm rounded-xl font-bold border-0 text-xs px-3.5">
                    Volver
                </a>
            </div>

            <asp:Panel ID="pnlTabs" runat="server">
                <nav class="mt-3 flex w-full flex-wrap gap-1.5 border-t border-slate-200 dark:border-slate-700 pt-2.5 text-[10px] font-black uppercase tracking-[0.18em]"
                     aria-label="Ficha de curso">
                    <a href="<%= RutaAsistencia %>" class="<%= TabAsistencia %>">Asistencia</a>
                    <a href="<%= RutaDetalle %>" class="<%= TabDetalle %>">Detalle por Semana</a>
                    <a href="<%= RutaJustificaciones %>" class="<%= TabJustificaciones %>">Justificaciones</a>
                    <a href="<%= RutaReporte %>" class="<%= TabReporte %>">Reporte DPI</a>
                </nav>
            </asp:Panel>
        </div>

        <asp:DataList ID="dlIndicadores" runat="server"
            RepeatLayout="Table"
            RepeatDirection="Horizontal"
            CssClass="indicadores-radar">
            <ItemTemplate>
                <div class="bg-white dark:bg-[#111827] rounded-3xl p-5 border border-slate-200/80 dark:border-slate-800 shadow-sm">
                    <span class="text-[10px] font-bold text-slate-400 uppercase tracking-wider block"><%# Eval("Etiqueta") %></span>
                    <div class='text-2xl font-black mt-1 <%# Eval("ClaseValor") %>'><%# Eval("Valor") %></div>
                    <p class='text-[10px] font-bold mt-0.5 <%# Eval("ClaseNota") %>'><%# Eval("Nota") %></p>
                </div>
            </ItemTemplate>
        </asp:DataList>

        <div class="bg-white dark:bg-[#111827] p-4 rounded-3xl border border-slate-200/80 dark:border-slate-800 shadow-sm flex flex-col sm:flex-row items-center justify-between gap-4">
            <input type="text" id="inputBuscarRadar" placeholder="&#128269; Buscar estudiante, DNI o curso..." onkeyup="filtrarRadar()" class="input input-xs input-bordered w-full sm:w-72 rounded-xl text-xs bg-slate-50 dark:bg-slate-900" />

            <div class="flex items-center gap-2">
                <select id="selectFiltroCarrera" onchange="filtrarRadar()" class="select select-xs select-bordered rounded-xl text-xs bg-slate-50 dark:bg-slate-900">
                    <option value="TODAS">Todas las Carreras</option>
                    <option value="SISTEMAS">Desarrollo de Sistemas (DSI)</option>
                    <option value="CONTABILIDAD">Contabilidad (CONT)</option>
                    <option value="ADMINISTRACION">Administraci&oacute;n (ADM)</option>
                </select>
            </div>
        </div>

        <div class="bg-white dark:bg-[#111827] rounded-3xl border border-slate-200/80 dark:border-slate-800 shadow-sm overflow-hidden">
            <div class="overflow-x-auto">
                <asp:GridView ID="tablaRadarDpi" runat="server"
                    AutoGenerateColumns="false"
                    UseAccessibleHeader="true"
                    GridLines="None"
                    CssClass="min-w-full table-auto border-collapse text-left text-xs tabla-radar"
                    RowStyle-CssClass="fila-radar hover:bg-slate-50 dark:hover:bg-slate-900/50 transition"
                    OnRowDataBound="tablaRadarDpi_RowDataBound"
                    OnRowCommand="tablaRadarDpi_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Estudiante">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider" />
                            <ItemStyle CssClass="px-3 py-3" />
                            <ItemTemplate>
                                <p class="font-bold text-slate-900 dark:text-slate-100"><%# Eval("EstudianteNombre") %></p>
                                <span class="text-[10px] font-mono text-slate-400"><%# Eval("EstudianteCodigo") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Carrera / Ciclo / Turno">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider" />
                            <ItemStyle CssClass="px-3 py-3" />
                            <ItemTemplate>
                                <p class="font-semibold text-slate-800 dark:text-slate-200"><%# Eval("CarreraNombre") %></p>
                                <span class="text-[10px] text-slate-500">Ciclo <%# Eval("Ciclo") %> &bull; <%# Eval("Turno") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unidad Did&aacute;ctica">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider" />
                            <ItemStyle CssClass="px-3 py-3" />
                            <ItemTemplate>
                                <p class="font-semibold text-slate-800 dark:text-slate-200"><%# Eval("UnidadDidacticaNombre") %></p>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Docente Asignado">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider" />
                            <ItemStyle CssClass="px-3 py-3 text-slate-600 dark:text-slate-400" />
                            <ItemTemplate>
                                <%# Eval("DocenteNombre") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Horas Falta / Total">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider text-center" />
                            <ItemStyle CssClass="px-3 py-3 text-center font-mono font-bold" />
                            <ItemTemplate>
                                <span class="text-rose-600"><%# Eval("HorasFalta") %> hrs</span> / <span class="text-slate-400"><%# Eval("TotalHoras") %> hrs</span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="% Inasistencia">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider text-center" />
                            <ItemStyle CssClass="px-3 py-3 text-center" />
                            <ItemTemplate>
                                <div class='font-black text-xs <%# ClasePorcentaje(Eval("PorcentajeInasistencia")) %>'>
                                    <%# PorcentajeTexto(Eval("PorcentajeInasistencia")) %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Estado Oficial">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider text-center" />
                            <ItemStyle CssClass="px-3 py-3 text-center" />
                            <ItemTemplate>
                                <span class='inline-flex items-center rounded-full px-2 py-0.5 text-[10px] shadow-xs <%# ClaseSemaforo(Eval("SemaforoEstado")) %>'>
                                    <%# EmojiSemaforo(Eval("SemaforoEstado")) %> <%# EtiquetaSemaforo(Eval("SemaforoEstado")) %>
                                </span>
                                <div style="margin-top: 4px;">
                                    <asp:LinkButton ID="lnkDetalle" runat="server" CommandName="VerDetalle" CommandArgument='<%# Eval("EstudianteId") %>'
                                        CssClass="text-[10px] font-bold text-indigo-600 dark:text-indigo-400 hover:underline">Ver Detalle</asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="p-8 text-center text-xs text-slate-400">
                            &#9989; No se registran estudiantes en riesgo cr&iacute;tico de inasistencia en este momento.
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

    <script>
        function filtrarRadar() {
            const texto = document.getElementById('inputBuscarRadar').value.toLowerCase().trim();
            const carrera = document.getElementById('selectFiltroCarrera').value;

            document.querySelectorAll('#tablaRadarDpi tbody tr.fila-radar').forEach(fila => {
                const estudiante = fila.getAttribute('data-estudiante') || '';
                const codigo = fila.getAttribute('data-codigo') || '';
                const curso = fila.getAttribute('data-curso') || '';
                const car = fila.getAttribute('data-carrera') || '';

                let matchTexto = !texto || estudiante.includes(texto) || codigo.includes(texto) || curso.includes(texto);
                let matchCarrera = true;

                if (carrera === 'SISTEMAS') matchCarrera = car.includes('sistema');
                else if (carrera === 'CONTABILIDAD') matchCarrera = car.includes('contab');
                else if (carrera === 'ADMINISTRACION') matchCarrera = car.includes('admin');

                fila.style.display = (matchTexto && matchCarrera) ? '' : 'none';
            });
        }
    </script>
</asp:Content>
