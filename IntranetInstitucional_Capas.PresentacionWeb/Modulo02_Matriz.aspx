<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Modulo02_Matriz.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Modulo02_Matriz" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="space-y-3 pb-28">

        <div class="rounded-[22px] border border-slate-200 dark:border-slate-800 bg-white dark:bg-[#111827] px-4 py-3 shadow-[0_10px_24px_rgba(15,23,42,0.04)]">
            <div class="flex flex-col gap-2.5 xl:flex-row xl:items-center xl:justify-between">
                <div class="flex items-center gap-3">
                    <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-slate-900 dark:bg-slate-100 text-base font-black text-white dark:text-slate-900 shadow-sm">A</div>
                    <div class="min-w-0">
                        <div class="mb-0.5 flex flex-wrap items-center gap-2 text-[10px] font-black uppercase tracking-[0.18em] text-slate-500 dark:text-slate-400">
                            <span><%: matriz.UnidadDidacticaCodigo %></span>
                            <span class="rounded-full bg-slate-100 dark:bg-slate-800 px-2 py-0.5 text-slate-600 dark:text-slate-300">Ciclo <%: matriz.Ciclo %> &bull; <%: matriz.Turno %> (Secc <%: matriz.Seccion %>)</span>
                            <span class="rounded-full bg-emerald-100 dark:bg-emerald-950 px-2 py-0.5 text-emerald-700 dark:text-emerald-300"><%= filas.Count %> alumnos</span>
                        </div>
                        <h1 class="text-lg font-black tracking-tight text-slate-900 dark:text-slate-100"><%: matriz.UnidadDidacticaNombre %></h1>
                    </div>
                </div>

                <div class="flex flex-wrap items-center gap-1.5">
                    <button type="button" onclick="abrirModalPegarExcel()" class="btn bg-emerald-50 hover:bg-emerald-100 text-emerald-800 dark:bg-emerald-950 dark:text-emerald-300 btn-xs rounded-lg font-bold border border-emerald-200 px-2.5 shadow-sm">&#128267; Pegar <span class="hidden xl:inline">(Ctrl+V)</span></button>
                    <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click" CausesValidation="false"
                        CssClass="btn bg-white dark:bg-slate-800 hover:bg-slate-50 text-slate-700 dark:text-slate-200 btn-xs rounded-lg font-bold border border-slate-200 px-2.5 shadow-sm">&#128261; Excel</asp:LinkButton>
                    <asp:LinkButton ID="btnExportarRegistra" runat="server" OnClick="btnExportarRegistra_Click" CausesValidation="false"
                        CssClass="btn bg-indigo-50 dark:bg-indigo-950 hover:bg-indigo-100 text-indigo-700 dark:text-indigo-300 btn-xs rounded-lg font-bold border border-indigo-200 px-2.5 shadow-sm">&#128209; REGISTRA</asp:LinkButton>
                    <button onclick="window.print()" class="btn bg-white dark:bg-slate-800 hover:bg-slate-50 text-slate-700 dark:text-slate-200 btn-xs rounded-lg font-bold border border-slate-200 px-2.5 shadow-sm" title="Imprimir">&#128424;&#65039;</button>
                </div>
            </div>

            <nav class="mt-3 flex w-full flex-wrap gap-1.5 border-t border-slate-200 dark:border-slate-700 pt-2.5 text-[10px] font-black uppercase tracking-[0.18em]"
                 aria-label="Ficha de curso">
                <a href="<%= ResolveUrl("~/Modulo02_Matriz.aspx?claseId=" + claseId) %>" class="rounded-lg border border-sky-200 dark:border-sky-800 bg-sky-50 dark:bg-sky-950/60 px-3 py-1.5 text-sky-700 dark:text-sky-300">Asistencia</a>
                <a href="<%= ResolveUrl("~/Modulo02_Clase.aspx?claseId=" + claseId) %>" class="rounded-lg border border-transparent px-3 py-1.5 text-slate-500 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800">Detalle por Semana</a>
                <a href="<%= ResolveUrl("~/Modulo02_Justificaciones.aspx") %>" class="rounded-lg border border-transparent px-3 py-1.5 text-slate-500 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800">Justificaciones</a>
                <a href="<%= ResolveUrl("~/Modulo02_ReportesDpi.aspx") %>" class="rounded-lg border border-transparent px-3 py-1.5 text-slate-500 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800">Reporte DPI</a>
            </nav>
        </div>

        <div class="bg-white dark:bg-[#111827] px-3 py-2 rounded-2xl border border-slate-200/80 dark:border-slate-800 shadow-sm flex flex-col md:flex-row items-center justify-between gap-2 text-[11px]">
            <div class="flex flex-wrap items-center gap-3">
                <span class="font-bold text-slate-700 dark:text-slate-300">Convenciones:</span>
                <span class="flex items-center gap-1.5"><span class="w-5 h-5 rounded-md bg-emerald-500 text-white font-bold flex items-center justify-center text-[10px]">P</span> Presente</span>
                <span class="flex items-center gap-1.5"><span class="w-5 h-5 rounded-md bg-amber-500 text-white font-bold flex items-center justify-center text-[10px]">T</span> Tardanza</span>
                <span class="flex items-center gap-1.5"><span class="w-5 h-5 rounded-md bg-rose-500 text-white font-bold flex items-center justify-center text-[10px]">F</span> Falta</span>
                <span class="flex items-center gap-1.5"><span class="w-5 h-5 rounded-md bg-blue-500 text-white font-bold flex items-center justify-center text-[10px]">J</span> Justificada</span>
                <span class="flex items-center gap-1.5"><span class="w-5 h-5 rounded-md bg-slate-200 dark:bg-slate-700 text-slate-600 dark:text-slate-300 font-bold flex items-center justify-center text-[10px]">-</span> Sin Registro</span>
            </div>

            <div class="flex items-center gap-3 w-full md:w-auto justify-end">
                <input type="text" id="inputBuscarMatriz" placeholder="&#128269; Filtrar estudiante o DNI..." onkeyup="filtrarMatriz()" class="input input-xs input-bordered w-48 sm:w-60 rounded-xl text-xs bg-slate-50 dark:bg-slate-900" />
                <label class="cursor-pointer label gap-2 py-0">
                    <input type="checkbox" id="checkSoloRiesgo" onchange="filtrarMatriz()" class="checkbox checkbox-xs checkbox-error rounded" />
                    <span class="label-text text-[11px] font-bold text-rose-600">Solo Riesgo / DPI</span>
                </label>
            </div>
        </div>

        <div class="bg-white dark:bg-[#111827] rounded-3xl border border-slate-200/80 dark:border-slate-800 shadow-sm overflow-hidden">
            <div class="overflow-x-auto max-h-[calc(100vh-235px)] min-h-[400px]" id="contenedorMatriz" tabindex="0">
                <table class="table table-pin-rows table-pin-cols table-xs w-full" id="tablaMatriz">
                    <thead>
                        <tr class="bg-slate-100 dark:bg-slate-800 text-slate-700 dark:text-slate-200 font-bold uppercase text-[10px] tracking-wider">

                            <th class="w-8 text-center bg-slate-100 dark:bg-slate-800">#</th>

                            <th class="min-w-[210px] bg-slate-100 dark:bg-slate-800">Estudiante</th>

                            <asp:Repeater ID="repColumnas" runat="server">
                                <ItemTemplate>
                                    <th class='text-center min-w-[48px] p-1 <%# Eval("ClaseColumna") %> <%# (bool)Eval("TieneSesion") ? "" : "opacity-50" %>' title='<%# Eval("TituloSemana") %>'>
                                        <asp:PlaceHolder ID="phColConSesion" runat="server" Visible='<%# (bool)Eval("TieneSesion") %>'>
                                            <a href='<%# Eval("UrlTomar") %>' class="hover:underline flex flex-col items-center">
                                                <span class="font-extrabold text-[10px]"><%# Eval("Etiqueta") %></span>
                                                <span class="text-[8px] font-mono text-slate-400 font-normal"><%# Eval("FechaTexto") %></span>
                                            </a>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID="phColSinSesion" runat="server" Visible='<%# !(bool)Eval("TieneSesion") %>'>
                                            <span class="flex flex-col items-center">
                                                <span class="font-extrabold text-[10px]"><%# Eval("Etiqueta") %></span>
                                                <span class="text-[8px] font-mono text-slate-400 font-normal">--/--</span>
                                            </span>
                                        </asp:PlaceHolder>
                                    </th>
                                </ItemTemplate>
                            </asp:Repeater>

                            <th class="text-center bg-emerald-50 dark:bg-emerald-950 text-emerald-800 dark:text-emerald-300 font-extrabold">P</th>
                            <th class="text-center bg-amber-50 dark:bg-amber-950 text-amber-800 dark:text-amber-300 font-extrabold">T</th>
                            <th class="text-center bg-rose-50 dark:bg-rose-950 text-rose-800 dark:text-rose-300 font-extrabold">F</th>
                            <th class="text-center bg-blue-50 dark:bg-blue-950 text-blue-800 dark:text-blue-300 font-extrabold">J</th>
                            <th class="text-center bg-slate-100 dark:bg-slate-800 text-slate-700 dark:text-slate-200 font-extrabold">% DPI</th>
                            <th class="text-center bg-slate-100 dark:bg-slate-800 text-slate-700 dark:text-slate-200 font-extrabold">Condici&oacute;n</th>
                        </tr>
                    </thead>
                    <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
                        <asp:Repeater ID="repFilas" runat="server" OnItemDataBound="repFilas_ItemDataBound">
                            <ItemTemplate>
                                <tr class="hover:bg-slate-50 dark:hover:bg-slate-900/50 transition fila-matriz"
                                    data-estudiante-id='<%# Eval("EstudianteId") %>'
                                    data-dni='<%# Eval("Dni") %>'
                                    data-codigo='<%# Eval("CodigoEstudiante") %>'
                                    data-nombre='<%# Eval("NombreBusqueda") %>'
                                    data-es-riesgo='<%# Eval("EsRiesgoTexto") %>'>

                                    <th class="text-center font-bold text-slate-400 bg-white dark:bg-[#111827]"><%# Eval("NumeroFila") %></th>

                                    <th class="bg-white dark:bg-[#111827]">
                                        <p class="font-bold text-slate-900 dark:text-slate-100 text-xs truncate max-w-[210px]"><%# Eval("NombreCompleto") %></p>
                                        <span class="text-[10px] font-mono text-slate-400 font-normal">DNI: <%# Eval("Dni") %></span>
                                        <asp:HiddenField ID="hidEstudiante" runat="server" Value='<%# Eval("EstudianteId") %>' />
                                    </th>

                                    <asp:Repeater ID="repCeldas" runat="server">
                                        <ItemTemplate>
                                            <asp:PlaceHolder ID="phCeldaVacia" runat="server" Visible='<%# !(bool)Eval("TieneSesion") %>'>

                                                <td class="text-center p-1 bg-slate-50/60 dark:bg-slate-900/40">
                                                    <span class="w-7 h-7 rounded-lg bg-slate-100 dark:bg-slate-800 text-slate-300 dark:text-slate-600 font-bold text-[10px] inline-flex items-center justify-center">-</span>
                                                </td>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="phCelda" runat="server" Visible='<%# (bool)Eval("TieneSesion") %>'>
                                                <td class="text-center p-1 celda-asistencia focus:outline-none"
                                                    data-sesion-id='<%# Eval("SesionId") %>'
                                                    data-sesion-ids='<%# Eval("SesionIdsCsv") %>'
                                                    data-estado='<%# Eval("Estado") %>'
                                                    onclick="seleccionarYRotarCelda(this)">
                                                    <button type="button" class='w-7 h-7 rounded-lg <%# Eval("BadgeClase") %> font-bold text-[10px] inline-flex items-center justify-center transition transform active:scale-90 shadow-xs cursor-pointer hover:ring-2 hover:ring-indigo-400'>
                                                        <%# Eval("BadgeChar") %>
                                                    </button>
                                                    <asp:HiddenField ID="hidEstado" runat="server" Value='<%# Eval("Estado") %>' />
                                                    <asp:HiddenField ID="hidSesiones" runat="server" Value='<%# Eval("SesionIdsCsv") %>' />
                                                </td>
                                            </asp:PlaceHolder>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                    <td class="text-center font-bold text-emerald-700 bg-emerald-50/50 total-p"><%# Eval("TotalPresentes") %></td>
                                    <td class="text-center font-bold text-amber-700 bg-amber-50/50 total-t"><%# Eval("TotalTardanzas") %></td>
                                    <td class="text-center font-bold text-rose-700 bg-rose-50/50 total-f"><%# Eval("TotalFaltasInjustificadas") %></td>
                                    <td class="text-center font-bold text-blue-700 bg-blue-50/50 total-j"><%# Eval("TotalFaltasJustificadas") %></td>

                                    <td class='text-center font-mono font-extrabold text-xs celda-porcentaje <%# Eval("ClasePorcentaje") %>'>
                                        <%# Eval("PorcentajeTexto") %>%
                                    </td>
                                    <td class="text-center celda-condicion">
                                        <asp:PlaceHolder ID="phDpi" runat="server" Visible='<%# (bool)Eval("EsDpi") %>'>
                                            <span class="badge badge-error badge-xs font-black text-white py-2 px-2 shadow-sm" title="Inhabilitado para Semana 17">&#128680; DPI</span>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID="phRiesgo" runat="server" Visible='<%# (bool)Eval("EsRiesgoAlto") %>'>
                                            <span class="badge badge-warning badge-xs font-bold text-white py-2 px-2 shadow-sm">&#9888;&#65039; Riesgo</span>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID="phRegular" runat="server" Visible='<%# !(bool)Eval("EsDpi") && !(bool)Eval("EsRiesgoAlto") %>'>
                                            <span class="badge badge-success badge-xs font-semibold py-2 px-2 bg-emerald-50 text-emerald-700 border-emerald-200">&#9989; Regular</span>
                                        </asp:PlaceHolder>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="fixed bottom-4 left-4 right-4 md:left-72 md:right-8 bg-slate-900/95 backdrop-blur-md text-white p-4 rounded-2xl shadow-2xl border border-slate-700 flex flex-col sm:flex-row items-center justify-between gap-3 z-50">
            <div class="flex items-center gap-3 text-xs">
                <span class="w-3 h-3 rounded-full bg-emerald-400"></span>
                <div>
                    <p class="font-bold text-slate-100">S&aacute;bana Live Grid de 18 Semanas</p>
                    <p class="text-[10px] text-slate-400">Total Alumnos: <%= filas.Count %> &bull; Atajos de teclado activos (P, T, F, J, Flechas)</p>
                </div>
            </div>

            <div class="flex items-center gap-3 w-full sm:w-auto justify-end">
                <asp:LinkButton ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" CausesValidation="false"
                    Text="&#128190; Guardar Toda la Matriz"
                    CssClass="btn bg-emerald-500 hover:bg-emerald-400 text-slate-950 btn-sm rounded-xl font-black text-xs px-6 shadow-lg" />
            </div>
        </div>
    </div>

    <dialog id="modal_pegar_excel" class="modal">
        <div class="modal-box max-w-2xl bg-white dark:bg-[#111827] rounded-3xl p-6 shadow-2xl border border-slate-200 dark:border-slate-800">
            <div class="flex items-center gap-3 mb-3">
                <div class="w-10 h-10 rounded-2xl bg-emerald-100 dark:bg-emerald-950 text-emerald-700 dark:text-emerald-300 flex items-center justify-center font-bold text-xl">
                    &#128267;
                </div>
                <div>
                    <h3 class="text-base font-bold text-slate-900 dark:text-slate-100">
                        Smart Copy-Paste desde Excel Personal (Ctrl+V)
                    </h3>
                    <p class="text-xs text-slate-500 dark:text-slate-400">
                        Copia la columna de asistencia de tu archivo Excel y p&eacute;gala aqu&iacute; para sincronizar toda la n&oacute;mina en 1 segundo.
                    </p>
                </div>
            </div>

            <div class="space-y-3 text-xs">
                <div>
                    <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">
                        Selecciona la Semana Destino
                    </label>
                    <select id="selectSemanaDestino" class="select select-sm select-bordered w-full rounded-xl text-xs bg-slate-50 dark:bg-slate-900">
                        <% foreach (IntranetInstitucional_Capas.PresentacionWeb.ColumnaMatrizVm col in columnas)
                           {
                               if (!col.TieneSesion) continue; %>
                            <option value="<%= col.SesionId %>">Semana <%= col.NumeroSemana %> (<%= col.FechaLarga %>)<% if (col.NumeroSemana == 17)
                                                                                                                      { %> - Recuperaci&oacute;n<% }
                                                                                                                    else if (col.NumeroSemana == 18)
                                                                                                                      { %> - Cierre REGISTRA<% } %></option>
                        <% } %>
                    </select>
                </div>

                <div>
                    <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">
                        Pega con Ctrl+V los valores (Acepta: P, T, F, J o 1, 0)
                    </label>
                    <textarea id="textareaPegarExcel" rows="8" class="textarea textarea-bordered w-full rounded-2xl text-xs font-mono bg-slate-50 dark:bg-slate-900" placeholder="Pega con Ctrl+V aqu&iacute;... Ejemplo:
P
P
F
T
P
..."></textarea>
                </div>

                <div class="p-3 bg-indigo-50 dark:bg-indigo-950/40 rounded-xl text-indigo-900 dark:text-indigo-300 text-[11px] flex items-center gap-2">
                    <span>&#128161;</span>
                    <span>El sistema mapea las filas ordenadamente seg&uacute;n la n&oacute;mina de estudiantes.</span>
                </div>

                <div class="modal-action mt-6 flex gap-2">
                    <button type="button" onclick="modal_pegar_excel.close()" class="btn btn-ghost btn-sm rounded-xl">Cancelar</button>
                    <button type="button" onclick="procesarPegadoExcel()" class="btn bg-indigo-600 hover:bg-indigo-700 text-white btn-sm rounded-xl font-bold border-0 px-5">
                        Aplicar a la S&aacute;bana
                    </button>
                </div>
            </div>
        </div>
    </dialog>

    <script>
        let celdaActiva = null;

        function seleccionarYRotarCelda(td) {
            seleccionarCelda(td);
            const estadoActual = td.getAttribute('data-estado');
            let nuevoEstado = 'PRESENTE';
            if (estadoActual === 'PRESENTE') nuevoEstado = 'FALTA_INJUSTIFICADA';
            else if (estadoActual === 'FALTA_INJUSTIFICADA') nuevoEstado = 'TARDANZA';
            else if (estadoActual === 'TARDANZA') nuevoEstado = 'FALTA_JUSTIFICADA';
            else if (estadoActual === 'FALTA_JUSTIFICADA') nuevoEstado = 'PRESENTE';
            else nuevoEstado = 'PRESENTE';

            aplicarEstadoACelda(td, nuevoEstado);
            recalcularFila(td.closest('tr'));
        }

        function seleccionarCelda(td) {
            if (celdaActiva) {
                celdaActiva.classList.remove('ring-2', 'ring-indigo-500', 'ring-offset-1');
            }
            celdaActiva = td;
            if (celdaActiva) {
                celdaActiva.classList.add('ring-2', 'ring-indigo-500', 'ring-offset-1');
            }
        }

        function aplicarEstadoACelda(td, estado) {
            td.setAttribute('data-estado', estado);

            const hdnEstado = td.querySelector('input[type="hidden"]');
            if (hdnEstado) { hdnEstado.value = estado; }

            const btn = td.querySelector('button');
            if (!btn) return;

            if (estado === 'PRESENTE') {
                btn.className = 'w-7 h-7 rounded-lg bg-emerald-500 text-white font-bold text-[10px] inline-flex items-center justify-center transition shadow-xs';
                btn.innerText = 'P';
            } else if (estado === 'TARDANZA') {
                btn.className = 'w-7 h-7 rounded-lg bg-amber-500 text-white font-bold text-[10px] inline-flex items-center justify-center transition shadow-xs';
                btn.innerText = 'T';
            } else if (estado === 'FALTA_INJUSTIFICADA') {
                btn.className = 'w-7 h-7 rounded-lg bg-rose-500 text-white font-black text-[10px] inline-flex items-center justify-center transition shadow-xs';
                btn.innerText = 'F';
            } else if (estado === 'FALTA_JUSTIFICADA') {
                btn.className = 'w-7 h-7 rounded-lg bg-blue-500 text-white font-bold text-[10px] inline-flex items-center justify-center transition shadow-xs';
                btn.innerText = 'J';
            } else {
                btn.className = 'w-7 h-7 rounded-lg bg-slate-100 dark:bg-slate-800 text-slate-400 font-bold text-[10px] inline-flex items-center justify-center transition shadow-xs';
                btn.innerText = '-';
            }
        }

        function recalcularFila(fila) {
            const celdas = fila.querySelectorAll('.celda-asistencia');
            let pres = 0, tard = 0, falt = 0, just = 0;

            celdas.forEach(c => {
                const st = c.getAttribute('data-estado');
                if (st === 'PRESENTE') pres++;
                else if (st === 'TARDANZA') tard++;
                else if (st === 'FALTA_INJUSTIFICADA') falt++;
                else if (st === 'FALTA_JUSTIFICADA') just++;
            });

            fila.querySelector('.total-p').innerText = pres;
            fila.querySelector('.total-t').innerText = tard;
            fila.querySelector('.total-f').innerText = falt;
            fila.querySelector('.total-j').innerText = just;

            const horasFalta = falt * 4;
            const totalHoras = 72;
            const pct = Math.round((horasFalta / totalHoras) * 10000) / 100;

            const celdaPct = fila.querySelector('.celda-porcentaje');
            celdaPct.innerText = pct.toFixed(2) + '%';
            if (pct >= 30) {
                celdaPct.className = 'text-center font-mono font-extrabold text-xs celda-porcentaje text-rose-600 bg-rose-50 dark:bg-rose-950';
                fila.querySelector('.celda-condicion').innerHTML = '<span class="badge badge-error badge-xs font-black text-white py-2 px-2 shadow-sm" title="Inhabilitado para Semana 17">&#128680; DPI</span>';
                fila.setAttribute('data-es-riesgo', 'true');
            } else if (pct >= 20) {
                celdaPct.className = 'text-center font-mono font-extrabold text-xs celda-porcentaje text-amber-600';
                fila.querySelector('.celda-condicion').innerHTML = '<span class="badge badge-warning badge-xs font-bold text-white py-2 px-2 shadow-sm">&#9888;&#65039; Riesgo</span>';
                fila.setAttribute('data-es-riesgo', 'true');
            } else {
                celdaPct.className = 'text-center font-mono font-extrabold text-xs celda-porcentaje text-slate-800 dark:text-slate-200';
                fila.querySelector('.celda-condicion').innerHTML = '<span class="badge badge-success badge-xs font-semibold py-2 px-2 bg-emerald-50 text-emerald-700 border-emerald-200">&#9989; Regular</span>';
                fila.setAttribute('data-es-riesgo', 'false');
            }
        }

        function filtrarMatriz() {
            const texto = document.getElementById('inputBuscarMatriz').value.toLowerCase().trim();
            const soloRiesgo = document.getElementById('checkSoloRiesgo').checked;

            document.querySelectorAll('#tablaMatriz tbody tr.fila-matriz').forEach(fila => {
                const nombre = fila.getAttribute('data-nombre') || '';
                const dni = fila.getAttribute('data-dni') || '';
                const esRiesgo = fila.getAttribute('data-es-riesgo') === 'true';

                let matchTexto = !texto || nombre.includes(texto) || dni.includes(texto);
                let matchRiesgo = !soloRiesgo || esRiesgo;

                fila.style.display = (matchTexto && matchRiesgo) ? '' : 'none';
            });
        }

        document.addEventListener('keydown', function (e) {
            if (!celdaActiva || document.activeElement.tagName === 'INPUT' || document.activeElement.tagName === 'TEXTAREA') return;

            const filaActual = celdaActiva.closest('tr');
            const celdasFila = Array.from(filaActual.querySelectorAll('.celda-asistencia'));
            const indexCelda = celdasFila.indexOf(celdaActiva);

            if (e.key === 'ArrowRight' && indexCelda < celdasFila.length - 1) {
                seleccionarCelda(celdasFila[indexCelda + 1]);
                e.preventDefault();
            } else if (e.key === 'ArrowLeft' && indexCelda > 0) {
                seleccionarCelda(celdasFila[indexCelda - 1]);
                e.preventDefault();
            } else if (e.key === 'ArrowDown') {
                const siguienteFila = filaActual.nextElementSibling;
                if (siguienteFila && siguienteFila.classList.contains('fila-matriz')) {
                    const celdasSig = siguienteFila.querySelectorAll('.celda-asistencia');
                    if (celdasSig[indexCelda]) seleccionarCelda(celdasSig[indexCelda]);
                }
                e.preventDefault();
            } else if (e.key === 'ArrowUp') {
                const anteriorFila = filaActual.previousElementSibling;
                if (anteriorFila && anteriorFila.classList.contains('fila-matriz')) {
                    const celdasAnt = anteriorFila.querySelectorAll('.celda-asistencia');
                    if (celdasAnt[indexCelda]) seleccionarCelda(celdasAnt[indexCelda]);
                }
                e.preventDefault();
            } else if (['p', 'P', '1'].includes(e.key)) {
                aplicarEstadoACelda(celdaActiva, 'PRESENTE');
                recalcularFila(filaActual);
                avanzarAbajo(filaActual, indexCelda);
                e.preventDefault();
            } else if (['f', 'F', '0'].includes(e.key)) {
                aplicarEstadoACelda(celdaActiva, 'FALTA_INJUSTIFICADA');
                recalcularFila(filaActual);
                avanzarAbajo(filaActual, indexCelda);
                e.preventDefault();
            } else if (['t', 'T'].includes(e.key)) {
                aplicarEstadoACelda(celdaActiva, 'TARDANZA');
                recalcularFila(filaActual);
                avanzarAbajo(filaActual, indexCelda);
                e.preventDefault();
            } else if (['j', 'J'].includes(e.key)) {
                aplicarEstadoACelda(celdaActiva, 'FALTA_JUSTIFICADA');
                recalcularFila(filaActual);
                avanzarAbajo(filaActual, indexCelda);
                e.preventDefault();
            }
        });

        function avanzarAbajo(filaActual, indexCelda) {
            const siguienteFila = filaActual.nextElementSibling;
            if (siguienteFila && siguienteFila.classList.contains('fila-matriz')) {
                const celdasSig = siguienteFila.querySelectorAll('.celda-asistencia');
                if (celdasSig[indexCelda]) seleccionarCelda(celdasSig[indexCelda]);
            }
        }

        function abrirModalPegarExcel() {
            document.getElementById('modal_pegar_excel').showModal();
            document.getElementById('textareaPegarExcel').focus();
        }

        function procesarPegadoExcel() {
            const sesionId = parseInt(document.getElementById('selectSemanaDestino').value);
            const texto = document.getElementById('textareaPegarExcel').value.trim();
            if (!texto) return;

            const lineas = texto.split(/\r?\n/).map(l => l.trim()).filter(l => l.length > 0);
            const filas = document.querySelectorAll('#tablaMatriz tbody tr.fila-matriz');

            let procesados = 0;
            filas.forEach((fila, idx) => {
                if (idx < lineas.length) {
                    const valorRaw = lineas[idx].toUpperCase();
                    let estado = 'PRESENTE';
                    if (valorRaw.includes('F') || valorRaw === '0') estado = 'FALTA_INJUSTIFICADA';
                    else if (valorRaw.includes('T')) estado = 'TARDANZA';
                    else if (valorRaw.includes('J')) estado = 'FALTA_JUSTIFICADA';
                    else if (valorRaw.includes('P') || valorRaw === '1') estado = 'PRESENTE';

                    const celda = fila.querySelector(`.celda-asistencia[data-sesion-id="${sesionId}"]`);
                    if (celda) {
                        aplicarEstadoACelda(celda, estado);
                        recalcularFila(fila);
                        procesados++;
                    }
                }
            });

            modal_pegar_excel.close();
            document.getElementById('textareaPegarExcel').value = '';
            alert(`&#9989; ${procesados} registros sincronizados desde Excel con \u00e9xito.`);
        }
    </script>
</asp:Content>
