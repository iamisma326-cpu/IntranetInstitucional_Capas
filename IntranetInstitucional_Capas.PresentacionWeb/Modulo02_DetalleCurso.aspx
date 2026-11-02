<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Modulo02_DetalleCurso.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Modulo02_DetalleCurso" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="ContentEstilos" ContentPlaceHolderID="EstilosPagina" runat="server">
    <style>

        .tabla-historial tbody > tr + tr {
            border-top: 1px solid #f1f5f9;
        }
        .tabla-historial th {
            text-align: left;
            vertical-align: middle;
        }

        .wizard-pasos > tbody > tr > td {
            padding: 0;
            border: 0;
        }
        .wizard-pasos .wizardNavigationBar {
            margin-top: 1rem;
            display: flex;
            justify-content: flex-end;
            gap: 0.5rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="space-y-6 pb-20">

        <div class="bg-white dark:bg-[#111827] rounded-3xl p-6 sm:p-7 border border-slate-200/80 dark:border-slate-800 shadow-sm flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
            <div class="flex items-start gap-4">
                <a href="Modulo02_MiAsistencia.aspx" class="w-11 h-11 rounded-2xl bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 text-slate-700 dark:text-slate-300 flex items-center justify-center shrink-0 transition mt-1">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
                    </svg>
                </a>
                <div>
                    <div class="flex items-center gap-2 mb-1.5">
                        <span class="text-xs font-bold bg-indigo-100 dark:bg-indigo-950 text-indigo-800 dark:text-indigo-300 px-2.5 py-0.5 rounded-full border border-indigo-200 dark:border-indigo-800">Historial de 18 Semanas</span>
                        <span class="text-xs text-slate-400 font-medium">Plazo de Justificaci&oacute;n: 72 horas h&aacute;biles</span>
                    </div>
                    <h1 class="text-xl sm:text-2xl font-black text-slate-900 dark:text-slate-100 tracking-tight">
                        Detalle de Asistencias por Sesi&oacute;n
                    </h1>
                    <p class="text-xs text-slate-500 dark:text-slate-400">
                        Registro individual clase por clase. Las faltas injustificadas pueden justificarse con sustento m&eacute;dico o laboral dentro de los 3 d&iacute;as h&aacute;biles.
                    </p>
                </div>
            </div>
            <div>
                <a href="Modulo02_MiAsistencia.aspx" class="btn bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 text-slate-700 dark:text-slate-300 btn-sm rounded-xl font-bold border-0 text-xs px-4">
                    Volver al Sem&aacute;foro
                </a>
            </div>
        </div>

        <div class="bg-white dark:bg-[#111827] rounded-3xl border border-slate-200/80 dark:border-slate-800 shadow-sm overflow-hidden">
            <div class="overflow-x-auto">
                <asp:GridView ID="gvHistorial" runat="server"
                    AutoGenerateColumns="false"
                    UseAccessibleHeader="true"
                    GridLines="None"
                    DataKeyNames="AsistenciaId"
                    CssClass="min-w-full table-auto border-collapse text-left text-xs tabla-historial"
                    OnRowDataBound="gvHistorial_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sem">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider" />
                            <ItemStyle CssClass="w-12 px-3 py-3 text-center" />
                            <ItemTemplate>
                                <span class='font-black <%# ClaseSemana(Eval("NumeroSemana")) %>'><%# EtiquetaSemana(Eval("NumeroSemana")) %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Fecha de Clase">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider" />
                            <ItemStyle CssClass="px-3 py-3" />
                            <ItemTemplate>
                                <span class="font-medium text-slate-900 dark:text-slate-100 font-mono"><%# FechaTexto(Eval("FechaClase")) %></span>
                                <span class="block text-[10px] text-slate-400"><%# HorasTexto(Eval("HorasPedagogicas")) %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tema Desarrollado (S&iacute;labo)">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider" />
                            <ItemStyle CssClass="px-3 py-3" />
                            <ItemTemplate>
                                <p class="font-semibold text-slate-800 dark:text-slate-200"><%# TemaTexto(Eval("TemaDesarrollado")) %></p>
                                <asp:Panel ID="pnlNota" runat="server" Visible="false">
                                    <p class="text-[11px] text-slate-400 italic mt-0.5">Nota: <%# Eval("Observacion") %></p>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Docente">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider" />
                            <ItemStyle CssClass="px-3 py-3 text-slate-600 dark:text-slate-400" />
                            <ItemTemplate>
                                <%# Eval("DocenteNombre") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Estado Oficial">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider text-center" />
                            <ItemStyle CssClass="px-3 py-3 text-center" />
                            <ItemTemplate>
                                <span class='inline-flex items-center rounded-full px-2 py-0.5 text-[10px] font-bold text-white shadow-xs <%# ClaseEstado(Eval("Estado")) %>'>
                                    <%# PuntoEstado(Eval("Estado")) %> <%# EtiquetaEstado(Eval("Estado"), Eval("MinutosTardanza")) %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tr&aacute;mite de Justificaci&oacute;n">
                            <HeaderStyle CssClass="bg-slate-100/80 px-3 py-3 text-slate-600 font-bold uppercase text-[10px] tracking-wider text-right" />
                            <ItemStyle CssClass="px-3 py-3 text-right" />
                            <ItemTemplate>
                                <asp:Panel ID="pnlSinTramite" runat="server" Visible="false">
                                    <span class="text-slate-300 dark:text-slate-700 text-[11px]">&mdash;</span>
                                </asp:Panel>

                                <asp:Panel ID="pnlFalta" runat="server" Visible="false">
                                    <asp:Panel ID="pnlTramitePendiente" runat="server" Visible="false">
                                        <span class="inline-flex items-center rounded-full border border-indigo-200 px-2 py-0.5 text-[10px] font-bold text-indigo-600 dark:text-indigo-400">
                                            &#8987; <%# Eval("EstadoJustificacion") %>
                                        </span>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlBoton72" runat="server" Visible="false" CssClass="flex items-center justify-end gap-2">
                                        <asp:Button ID="btnJustificar" runat="server" UseSubmitBehavior="false"
                                            CssClass="btn bg-indigo-600 hover:bg-indigo-700 text-white btn-xs rounded-xl font-bold border-0 px-3 shadow-sm" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPlazoVencido" runat="server" Visible="false">
                                        <span class="text-[10px] text-slate-400 font-semibold" title="Han pasado m&aacute;s de 72 horas h&aacute;biles">&#9203; Plazo 72h Vencido</span>
                                    </asp:Panel>
                                </asp:Panel>

                                <asp:Panel ID="pnlJustificada" runat="server" Visible="false">
                                    <span class="text-emerald-600 dark:text-emerald-400 font-bold text-[11px]">&#10003; Justificaci&oacute;n Aprobada</span>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="p-8 text-center text-xs text-slate-400">
                            No se registran asistencias para esta unidad did&aacute;ctica a&uacute;n.
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

    <dialog id="modal_justificar" class="modal">
        <div class="modal-box max-w-md bg-white dark:bg-[#111827] rounded-3xl p-6 shadow-2xl border border-slate-200 dark:border-slate-800">
            <div class="flex items-center gap-3 mb-3">
                <div class="w-10 h-10 rounded-2xl bg-indigo-100 dark:bg-indigo-950 text-indigo-700 dark:text-indigo-300 flex items-center justify-center font-bold text-xl">
                    &#128206;
                </div>
                <div>
                    <h3 class="text-base font-bold text-slate-900 dark:text-slate-100">
                        Solicitud de Justificaci&oacute;n Digital (72h)
                    </h3>
                    <p class="text-xs text-slate-400" id="modalFechaTexto">Fecha seleccionada...</p>
                </div>
            </div>

            <input type="hidden" id="inputAsistenciaId" runat="server" />

            <asp:Wizard ID="wizJustificar" runat="server"
                DisplaySideBar="false"
                CssClass="wizard-pasos w-full text-xs"
                OnActiveStepChanged="wizJustificar_ActiveStepChanged"
                OnNextButtonClick="wizJustificar_NextButtonClick"
                OnFinishButtonClick="wizJustificar_FinishButtonClick">
                <WizardSteps>
                    <asp:WizardStep ID="pasoMotivo" runat="server" Title="Motivo">
                        <div class="space-y-3">
                            <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Motivo Oficial</label>
                            <select id="ddlMotivo" runat="server" class="select select-sm select-bordered w-full rounded-xl text-xs bg-slate-50 dark:bg-slate-900"></select>
                            <p class="text-[11px] text-slate-400">Paso 1 de 3: selecciona el motivo oficial de la inasistencia.</p>
                        </div>
                    </asp:WizardStep>

                    <asp:WizardStep ID="pasoExplicacion" runat="server" Title="Explicaci&oacute;n">
                        <div class="space-y-3">
                            <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Explicaci&oacute;n y Sustento Detallado</label>
                            <textarea id="txtDescripcion" runat="server" rows="3"
                                class="textarea textarea-bordered w-full rounded-2xl text-xs bg-slate-50 dark:bg-slate-900"
                                placeholder="Describe los motivos espec&iacute;ficos de tu inasistencia..."></textarea>
                            <p class="text-[11px] text-slate-400">Paso 2 de 3: detalla por qu&eacute; no asististe a la sesi&oacute;n.</p>
                        </div>
                    </asp:WizardStep>

                    <asp:WizardStep ID="pasoDocumento" runat="server" Title="Documento y Confirmaci&oacute;n">
                        <div class="space-y-3">
                            <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Enlace a Documento / Certificado Escaneado</label>
                            <input type="text" id="txtDocumento" runat="server"
                                placeholder="https://drive.google.com/... o enlace de certificado"
                                class="input input-sm input-bordered w-full rounded-xl text-xs bg-slate-50 dark:bg-slate-900" />

                            <div class="p-3 bg-amber-50 dark:bg-amber-950/40 rounded-xl text-amber-900 dark:text-amber-300 text-[11px] flex items-center gap-2">
                                <span>&#9888;&#65039;</span>
                                <span>La solicitud ser&aacute; evaluada directamente por el docente de la asignatura dentro del plazo reglamentario.</span>
                            </div>
                            <p class="text-[11px] text-slate-400">Paso 3 de 3: confirma el env&iacute;o de la justificaci&oacute;n.</p>
                        </div>
                    </asp:WizardStep>
                </WizardSteps>
            </asp:Wizard>

            <div class="modal-action mt-6 flex gap-2">
                <button type="button" onclick="modal_justificar.close()" class="btn btn-ghost btn-sm rounded-xl">Cancelar</button>
            </div>
        </div>
    </dialog>

    <script>
        function abrirModalJustificar(asistenciaId, fechaTexto, semana) {
            document.getElementById('inputAsistenciaId').value = asistenciaId;
            document.getElementById('modalFechaTexto').innerText = 'Semana ' + semana + ' \u2022 Fecha: ' + fechaTexto;
            document.getElementById('modal_justificar').showModal();
            return false;
        }
    </script>
</asp:Content>
