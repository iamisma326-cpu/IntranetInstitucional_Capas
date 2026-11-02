<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Modulo02_TomarAsistencia.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Modulo02_TomarAsistencia" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="space-y-4">

        <div class="rounded-[22px] border border-slate-200 bg-white p-4 shadow-[0_10px_24px_rgba(15,23,42,0.04)]">
            <div class="flex flex-col gap-3 xl:flex-row xl:items-center xl:justify-between">
                <div class="flex items-center gap-3">
                    <div class="flex h-12 w-12 items-center justify-center rounded-xl bg-slate-900 text-lg font-black text-white shadow-sm">A</div>
                    <div>
                        <div class="mb-1 flex items-center gap-2 text-[10px] font-black uppercase tracking-[0.18em] text-slate-500">
                            <span><asp:Literal ID="litCodigoCurso" runat="server" /></span>
                            <span class="rounded-full bg-slate-100 px-2 py-0.5 text-slate-600">A-ERP</span>
                        </div>
                        <h1 class="text-2xl font-black tracking-tight text-slate-900">
                            <asp:Literal ID="litNombreCurso" runat="server" />
                        </h1>
                        <p class="text-sm text-slate-500">
                            <asp:Literal ID="litDocenteAula" runat="server" />
                        </p>
                    </div>
                </div>

                <div class="flex flex-wrap items-center gap-2 text-[11px] font-bold">
                    <span class="rounded-full bg-slate-100 px-3 py-2 text-slate-700">CURSO: <asp:Literal ID="litCursoChip" runat="server" /></span>
                    <span class="rounded-full bg-slate-100 px-3 py-2 text-slate-700">SEMANA <asp:Literal ID="litSemanaChip" runat="server" /> / 18</span>
                    <span class="rounded-full bg-emerald-100 px-3 py-2 text-emerald-700"><asp:Literal ID="litPresentesChip" runat="server" /> presentes</span>
                </div>
            </div>

            <div class="mt-4 flex flex-wrap gap-2 border-t border-slate-200 pt-3 text-[11px] font-black uppercase tracking-[0.18em] text-slate-500">
                <button type="button" class="rounded-xl border border-sky-200 bg-sky-50 px-4 py-2 text-sky-700">Asistencia</button>
                <button type="button" class="rounded-xl px-4 py-2 text-slate-500 hover:bg-slate-100">Detalle por Semana</button>
                <button type="button" class="rounded-xl px-4 py-2 text-slate-500 hover:bg-slate-100">Justificaciones</button>
                <button type="button" class="rounded-xl px-4 py-2 text-slate-500 hover:bg-slate-100">Reporte DPI</button>
            </div>
        </div>

        <div class="space-y-4">

            <section class="rounded-[20px] border border-slate-200 bg-white p-4 shadow-[0_10px_24px_rgba(15,23,42,0.04)]">
                <div class="flex flex-col gap-3 xl:flex-row xl:items-center xl:justify-between">
                    <div>
                        <div class="mb-2 flex items-center gap-2 text-[10px] font-black uppercase tracking-[0.18em] text-slate-500">
                            <span class="inline-flex h-6 w-6 items-center justify-center rounded-full bg-amber-100 text-amber-700">&#9889;</span>
                            <span>Clase de hoy</span>
                        </div>
                        <h2 class="text-3xl font-black tracking-tight text-slate-900">SEMANA <asp:Literal ID="litSemanaTitulo" runat="server" /></h2>
                    </div>

                    <div class="flex flex-wrap items-center gap-2 text-[11px] font-bold">
                        <span class="rounded-full bg-emerald-100 px-3 py-1.5 text-emerald-700">&#9679; <asp:Literal ID="litPresentesResumen" runat="server" /> Presentes</span>
                        <span class="rounded-full bg-rose-100 px-3 py-1.5 text-rose-700">&#9679; <asp:Literal ID="litFaltasResumen" runat="server" /> Faltas</span>
                        <span class="rounded-full bg-amber-100 px-3 py-1.5 text-amber-700">&#9679; <asp:Literal ID="litTardanzasResumen" runat="server" /> Tardanzas</span>
                    </div>
                </div>

                <div class="mt-4 flex flex-col gap-3 rounded-2xl border border-slate-200 bg-slate-50 p-3 md:flex-row md:items-center md:justify-between">
                    <div class="flex items-center gap-3">
                        <div class="flex h-11 w-11 items-center justify-center rounded-xl bg-sky-100 text-lg">&#128218;</div>
                        <div>
                            <div class="text-[10px] font-black uppercase tracking-[0.18em] text-slate-500">Aula</div>
                            <div class="text-base font-bold text-slate-800"><asp:Literal ID="litAula" runat="server" /></div>
                        </div>
                    </div>

                    <div class="flex flex-wrap items-center gap-2">
                        <button type="button" onclick="marcarTodos('PRESENTE')" class="rounded-xl border border-emerald-200 bg-emerald-50 px-4 py-2 text-xs font-black text-emerald-700 hover:bg-emerald-100">Pasar asistencia</button>
                        <button type="button" onclick="copiarRegistra()" class="rounded-xl border border-slate-200 bg-white px-4 py-2 text-xs font-bold text-slate-700 hover:bg-slate-100">Copiar para REGISTRA</button>
                    </div>
                </div>
            </section>

            <section class="overflow-hidden rounded-[20px] border border-slate-200 bg-white shadow-[0_10px_24px_rgba(15,23,42,0.04)]">
                <asp:Panel ID="pnlTabla" runat="server">
                    <div class="overflow-x-auto">
                        <table class="min-w-full table-auto border-collapse text-left">
                            <thead class="bg-slate-100 text-[10px] font-black uppercase tracking-[0.18em] text-slate-500">
                                <tr>
                                    <th class="px-3 py-3">N&deg;</th>
                                    <th class="px-3 py-3">Estudiante</th>
                                    <th class="px-3 py-3">DPI</th>
                                    <th class="px-3 py-3">Asistencia</th>
                                    <th class="px-3 py-3">Observaci&oacute;n</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y divide-slate-200 text-sm">
                                <asp:DataList ID="dlAlumnos" runat="server"
                                    RepeatLayout="Flow"
                                    RepeatDirection="Horizontal"
                                    CssClass="hidden"
                                    ItemStyle-CssClass="hidden"
                                    OnItemDataBound="dlAlumnos_ItemDataBound">
                                    <ItemTemplate>
                                        <tr runat="server" id="filaAlumno" class="fila-alumno transition hover:bg-slate-50">
                                            <td class="px-3 py-3 font-bold text-slate-400">
                                                <asp:Literal ID="litNumero" runat="server" />
                                            </td>
                                            <td class="px-3 py-3">
                                                <div class="flex items-center gap-3">
                                                    <div class="flex h-9 w-9 items-center justify-center rounded-xl bg-slate-200 text-[10px] font-black text-slate-700">
                                                        <asp:Literal ID="litIniciales" runat="server" />
                                                    </div>
                                                    <div>
                                                        <div class="font-bold text-slate-800">
                                                            <asp:Literal ID="litNombre" runat="server" />
                                                        </div>
                                                        <div class="text-[10px] text-slate-500">
                                                            <asp:Literal ID="litDni" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="px-3 py-3">
                                                <asp:Literal ID="litDpi" runat="server" />
                                            </td>
                                            <td class="px-3 py-3">
                                                <div class="flex items-center gap-2">
                                                    <button type="button" class="btn-estado flex h-8 w-8 items-center justify-center rounded-full border border-slate-200 bg-white text-slate-600 text-xs font-black shadow-sm transition hover:scale-105" data-estado="PRESENTE" onclick="setEstado(this, 'PRESENTE')">P</button>
                                                    <button type="button" class="btn-estado flex h-8 w-8 items-center justify-center rounded-full border border-slate-200 bg-white text-slate-600 text-xs font-black shadow-sm transition hover:scale-105" data-estado="TARDANZA" onclick="setEstado(this, 'TARDANZA')">T</button>
                                                    <button type="button" class="btn-estado flex h-8 w-8 items-center justify-center rounded-full border border-slate-200 bg-white text-slate-600 text-xs font-black shadow-sm transition hover:scale-105" data-estado="FALTA_INJUSTIFICADA" onclick="setEstado(this, 'FALTA_INJUSTIFICADA')">F</button>
                                                    <button type="button" class="btn-estado flex h-8 w-8 items-center justify-center rounded-full border border-slate-200 bg-white text-slate-600 text-xs font-black shadow-sm transition hover:scale-105" data-estado="FALTA_JUSTIFICADA" onclick="setEstado(this, 'FALTA_JUSTIFICADA')">J</button>
                                                </div>
                                            </td>
                                            <td class="px-3 py-3">
                                                <div class="flex items-center gap-2">
                                                    <asp:HiddenField ID="hdnEstado" runat="server" />
                                                    <asp:TextBox ID="txtMinutos" runat="server" TextMode="Number" min="0" max="180"
                                                        CssClass="w-14 rounded-lg border border-slate-200 bg-slate-50 px-2 py-1 text-center text-xs" />
                                                    <asp:TextBox ID="txtObservacion" runat="server"
                                                        placeholder="Observaci&#243;n"
                                                        CssClass="w-full max-w-[180px] rounded-lg border border-slate-200 bg-slate-50 px-2 py-1 text-xs" />
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:DataList>
                            </tbody>
                        </table>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlVacio" runat="server" Visible="false">
                    <div class="p-8 text-center text-xs text-slate-400">
                        <asp:Literal ID="litVacio" runat="server" />
                    </div>
                </asp:Panel>
            </section>
        </div>

        <div class="sticky bottom-4 z-20">
            <div class="mx-auto flex max-w-[1500px] items-center justify-between gap-3 rounded-2xl border border-slate-700 bg-slate-900/95 px-5 py-3 text-white shadow-2xl backdrop-blur">
                <div class="flex items-center gap-3 text-xs">
                    <span class="inline-block h-2.5 w-2.5 rounded-full bg-emerald-400"></span>
                    <span>Sesi&oacute;n lista para guardar</span>
                </div>

                <div class="flex items-center gap-2">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar borrador"
                        OnClick="btnGuardar_Click"
                        UseSubmitBehavior="false"
                        CssClass="rounded-xl border border-slate-600 bg-slate-800 px-4 py-2 text-xs font-bold text-white hover:bg-slate-700" />
                    <asp:Button ID="btnOficializar" runat="server" Text="Oficializar y cerrar"
                        OnClick="btnOficializar_Click"
                        UseSubmitBehavior="false"
                        CssClass="rounded-xl bg-emerald-500 px-4 py-2 text-xs font-black text-slate-950 hover:bg-emerald-400" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="ContentEstilos" ContentPlaceHolderID="EstilosPagina" runat="server">
</asp:Content>

<asp:Content ID="ContentScripts" ContentPlaceHolderID="ScriptsPagina" runat="server">
    <script>
        function setEstado(button, estado) {
            const row = button.closest('.fila-alumno');
            if (!row) return;

            const buttons = row.querySelectorAll('.btn-estado');

            buttons.forEach(btn => {
                btn.classList.remove('is-active', 'bg-emerald-500', 'bg-amber-500', 'bg-rose-500', 'bg-blue-500', 'text-white');
                btn.classList.add('bg-white', 'text-slate-600');
                btn.style.backgroundColor = '';
                btn.style.color = '';
            });

            const palette = {
                PRESENTE: 'bg-emerald-500 text-white',
                TARDANZA: 'bg-amber-500 text-white',
                FALTA_INJUSTIFICADA: 'bg-rose-500 text-white',
                FALTA_JUSTIFICADA: 'bg-blue-500 text-white'
            };

            button.classList.add(...palette[estado].split(' '));
            button.classList.add('is-active');
            row.dataset.estado = estado;

            const hdnEstado = row.querySelector('input[type="hidden"]');
            if (hdnEstado) {
                hdnEstado.value = estado;
            }

            const inputMinutos = row.querySelector('input[type="number"]');
            if (inputMinutos) {
                const esTardanza = estado === 'TARDANZA';
                inputMinutos.disabled = !esTardanza;
                inputMinutos.classList.toggle('opacity-50', !esTardanza);
                if (esTardanza && (!inputMinutos.value || Number(inputMinutos.value) <= 0)) {
                    inputMinutos.value = '15';
                }
            }
        }

        function marcarTodos(estado) {
            document.querySelectorAll('.fila-alumno').forEach(row => {
                const target = row.querySelector(`.btn-estado[data-estado="${estado}"]`);
                if (target) setEstado(target, estado);
            });
        }

        function copiarRegistra() {
            const lines = [];
            document.querySelectorAll('.fila-alumno').forEach(row => {
                const nombre = row.querySelector('div.font-bold')?.textContent?.trim() ?? '';
                const estado = row.dataset.estado || 'PRESENTE';
                lines.push(`${nombre}\t${estado}`);
            });

            const text = lines.join('\n');
            navigator.clipboard.writeText(text).then(() => {
                alert('Se copi\u00f3 la lista en formato REGISTRA.');
            }).catch(() => {
                alert('No se pudo copiar autom\u00e1ticamente.');
            });
        }

        function aplicarEstadoInicial() {
            document.querySelectorAll('.fila-alumno').forEach(row => {
                const hdnEstado = row.querySelector('input[type="hidden"]');
                const estado = (hdnEstado && hdnEstado.value) ? hdnEstado.value : 'PRESENTE';
                const target = row.querySelector(`.btn-estado[data-estado="${estado}"]`);
                if (target) setEstado(target, estado);
            });
        }

        document.addEventListener('DOMContentLoaded', aplicarEstadoInicial);
    </script>
</asp:Content>
