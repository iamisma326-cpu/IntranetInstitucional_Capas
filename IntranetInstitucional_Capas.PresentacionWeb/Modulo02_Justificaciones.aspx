<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="Modulo02_Justificaciones.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Modulo02_Justificaciones" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="ContentEstilos" ContentPlaceHolderID="EstilosPagina" runat="server">
    <style>

        #gvJustificaciones tbody tr + tr {
            border-top: 1px solid #f1f5f9;
        }

        .dark #gvJustificaciones tbody tr + tr,
        [data-theme="dark"] #gvJustificaciones tbody tr + tr {
            border-top-color: #1e293b;
        }

        .wiz-zen {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
        }

        .wiz-zen td {
            border: none;
            padding: 0;
            vertical-align: top;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="space-y-6">

        <div class="bg-white dark:bg-[#111827] rounded-3xl p-6 sm:p-7 border border-slate-200/80 dark:border-slate-800 shadow-sm flex flex-col sm:flex-row sm:flex-wrap justify-between items-start sm:items-center gap-4">
            <div class="flex items-center gap-4">
                <div class="w-14 h-14 rounded-2xl bg-indigo-100 dark:bg-indigo-950 text-indigo-700 dark:text-indigo-300 border border-indigo-200 dark:border-indigo-800 flex items-center justify-center shrink-0 shadow-sm font-bold text-2xl">
                    &#128196;
                </div>
                <div>
                    <div class="flex items-center gap-2 mb-1">
                        <span class="text-xs font-bold bg-indigo-100 dark:bg-indigo-950 text-indigo-800 dark:text-indigo-300 px-2.5 py-0.5 rounded-full border border-indigo-200 dark:border-indigo-800">Tr&aacute;mites de Asistencia</span>
                        <span class="text-xs text-slate-400 font-medium">Ventana de 72h H&aacute;biles (Art. 8.8.1 MPA)</span>
                    </div>
                    <h1 class="text-xl sm:text-2xl font-black text-slate-900 dark:text-slate-100 tracking-tight">
                        Bandeja de Justificaciones de Inasistencia
                    </h1>
                    <p class="text-xs text-slate-500 dark:text-slate-400">
                        <%= Subtitulo %>
                    </p>
                </div>
            </div>

            <div class="flex gap-2">
                <a href="<%= RutaVuelta %>" class="btn bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 text-slate-700 dark:text-slate-300 btn-sm rounded-xl font-bold border-0 text-xs px-3.5">
                    Volver
                </a>
            </div>

            <% if (!_esSoloAlumno) { %>

            <nav class="mt-3 flex w-full flex-wrap gap-1.5 border-t border-slate-200 dark:border-slate-700 pt-2.5 text-[10px] font-black uppercase tracking-[0.18em]"
                 aria-label="Ficha de curso">
                <a href="<%= RutaAsistencia %>" class="<%= TabAsistencia %>">Asistencia</a>
                <a href="<%= RutaDetalle %>" class="<%= TabDetalle %>">Detalle por Semana</a>
                <a href="<%= RutaJustificaciones %>" class="<%= TabJustificaciones %>">Justificaciones</a>
                <a href="<%= RutaReporte %>" class="<%= TabReporte %>">Reporte DPI</a>
            </nav>
            <% } %>
        </div>

        <div class="flex flex-wrap gap-2 text-xs">
            <a href="Modulo02_Justificaciones.aspx" class="btn btn-sm rounded-xl font-bold <%= ClaseTodas %>">
                Todas (<%= TotalTodas %>)
            </a>
            <a href="Modulo02_Justificaciones.aspx?estado=PENDIENTE" class="btn btn-sm rounded-xl font-bold <%= ClasePendientes %>">
                Pendientes (<%= TotalPendientes %>)
            </a>
            <a href="Modulo02_Justificaciones.aspx?estado=APROBADA" class="btn btn-sm rounded-xl font-bold <%= ClaseAprobadas %>">
                Aprobadas (<%= TotalAprobadas %>)
            </a>
            <a href="Modulo02_Justificaciones.aspx?estado=RECHAZADA" class="btn btn-sm rounded-xl font-bold <%= ClaseRechazadas %>">
                Rechazadas (<%= TotalRechazadas %>)
            </a>
        </div>

        <asp:Panel ID="pnlSolicitud" runat="server">
            <div class="bg-white dark:bg-[#111827] rounded-3xl p-6 border border-slate-200/80 dark:border-slate-800 shadow-sm">
                <div class="flex items-center gap-4 mb-5">
                    <div class="w-14 h-14 rounded-2xl bg-amber-100 dark:bg-amber-950 text-amber-700 dark:text-amber-300 border border-amber-200 dark:border-amber-800 flex items-center justify-center shrink-0 shadow-sm font-bold text-2xl">
                        &#128206;
                    </div>
                    <div>
                        <p class="font-bold text-slate-900 dark:text-slate-100 text-sm">Solicitud de Justificaci&oacute;n Digital (72h)</p>
                        <p class="text-xs text-slate-500 dark:text-slate-400">Complete los tres pasos del tr&aacute;mite para enviarlo a evaluaci&oacute;n docente.</p>
                    </div>
                </div>

                <asp:Wizard ID="wizJustificacion" runat="server"
                    DisplaySideBar="false"
                    CssClass="wiz-zen"
                    OnFinishButtonClick="wizJustificacion_FinishButtonClick"
                    StartNextButtonText="Continuar"
                    StepNextButtonText="Continuar"
                    StepPreviousButtonText="Volver"
                    FinishButtonText="Confirmar Solicitud"
                    FinishPreviousButtonText="Volver">
                    <StartNavigationTemplate>
                        <div class="flex flex-wrap gap-3 pt-4 mt-4 border-t border-slate-100 dark:border-slate-800">
                            <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Continuar"
                                CssClass="btn bg-indigo-600 hover:bg-indigo-700 text-white btn-sm rounded-xl font-bold border-0 px-4" />
                        </div>
                    </StartNavigationTemplate>
                    <StepNavigationTemplate>
                        <div class="flex flex-wrap gap-3 pt-4 mt-4 border-t border-slate-100 dark:border-slate-800">
                            <asp:Button ID="StepPreviousButton" runat="server" CommandName="MovePrevious" Text="Volver"
                                CausesValidation="false"
                                CssClass="btn btn-ghost btn-sm rounded-xl font-bold border border-slate-300 dark:border-slate-700" />
                            <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Continuar"
                                CssClass="btn bg-indigo-600 hover:bg-indigo-700 text-white btn-sm rounded-xl font-bold border-0 px-4" />
                        </div>
                    </StepNavigationTemplate>
                    <FinishNavigationTemplate>
                        <div class="flex flex-wrap gap-3 pt-4 mt-4 border-t border-slate-100 dark:border-slate-800">
                            <asp:Button ID="FinishPreviousButton" runat="server" CommandName="MovePrevious" Text="Volver"
                                CausesValidation="false"
                                CssClass="btn btn-ghost btn-sm rounded-xl font-bold border border-slate-300 dark:border-slate-700" />
                            <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" Text="Confirmar Solicitud"
                                CssClass="btn bg-indigo-600 hover:bg-indigo-700 text-white btn-sm rounded-xl font-bold border-0 px-4" />
                        </div>
                    </FinishNavigationTemplate>

                    <WizardSteps>
                        <asp:WizardStep ID="stpMotivo" runat="server" Title="Motivo">
                        <div class="space-y-3 text-xs">
                            <div>
                                <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1" for="<%= ddlMotivo.ClientID %>">Motivo de la Inasistencia</label>
                                <asp:DropDownList ID="ddlMotivo" runat="server"
                                    CssClass="select select-sm select-bordered w-full rounded-xl text-xs font-bold"></asp:DropDownList>
                            </div>
                            <div>
                                <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1" for="<%= txtDescripcion.ClientID %>">Descripci&oacute;n de los Hechos</label>
                                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="4"
                                    CssClass="textarea textarea-bordered w-full rounded-xl text-xs"></asp:TextBox>
                            </div>
                        </div>
                    </asp:WizardStep>

                    <asp:WizardStep ID="stpFechas" runat="server" Title="Fechas y adjunto">
                        <div class="space-y-3 text-xs">
                            <div>
                                <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1" for="<%= txtAsistenciaId.ClientID %>">Registro de Asistencia (ID)</label>
                                <asp:TextBox ID="txtAsistenciaId" runat="server" TextMode="Number"
                                    CssClass="input input-sm input-bordered w-full rounded-xl text-xs"></asp:TextBox>
                            </div>
                            <div>
                                <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1" for="txtDocumentoUrl">Documento de Sustento (URL)</label>
                                <input id="txtDocumentoUrl" runat="server" type="text"
                                    class="input input-sm input-bordered w-full rounded-xl text-xs" />
                            </div>
                            <div class="rounded-xl border border-amber-200 dark:border-amber-800 bg-amber-50 dark:bg-amber-950/40 p-3 text-[11px] text-amber-800 dark:text-amber-300">
                                &#9888;&#65039; La solicitud debe registrarse dentro de las 72 horas h&aacute;biles de la fecha de clase (Art. 8.8.1 MPA). Adjunte el certificado m&eacute;dico o constancia en un enlace compartible (Google Drive).
                            </div>
                        </div>
                    </asp:WizardStep>

                    <asp:WizardStep ID="stpConfirmacion" runat="server" Title="Confirmaci&#243;n">
                        <div class="text-xs space-y-3">
                            <p class="text-slate-600 dark:text-slate-400">Revise los datos del tr&aacute;mite antes de confirmar el env&iacute;o:</p>
                            <ul class="list-disc pl-5 space-y-1 text-slate-700 dark:text-slate-300">
                                <li><strong>Motivo:</strong> <%= ResumenMotivo %></li>
                                <li><strong>Descripci&oacute;n:</strong> <%= ResumenDescripcion %></li>
                                <li><strong>Registro de asistencia:</strong> <%= ResumenAsistencia %></li>
                                <li><strong>Documento de sustento:</strong> <%= ResumenDocumento %></li>
                                <li>El tr&aacute;mite quedar&aacute; en estado <span class="badge badge-warning badge-sm font-bold text-white">Pendiente</span> hasta el dictamen docente.</li>
                            </ul>
                        </div>
                    </asp:WizardStep>
                    </WizardSteps>
                </asp:Wizard>
            </div>
        </asp:Panel>

        <div class="bg-white dark:bg-[#111827] rounded-3xl border border-slate-200/80 dark:border-slate-800 shadow-sm overflow-hidden">
            <div class="overflow-x-auto">
                <asp:GridView ID="gvJustificaciones" runat="server"
                    AutoGenerateColumns="false"
                    GridLines="None"
                    CellPadding="0"
                    CellSpacing="0"
                    ClientIDMode="Predictable"
                    DataKeyNames="Id"
                    CssClass="table w-full text-xs"
                    HeaderStyle-CssClass="bg-slate-100/80 dark:bg-slate-800 text-slate-600 dark:text-slate-300 font-bold uppercase text-[10px] tracking-wider"
                    RowStyle-CssClass="hover:bg-slate-50 dark:hover:bg-slate-900/50 transition"
                    OnRowCommand="gvJustificaciones_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Estudiante">
                            <ItemTemplate>
                                <p class="font-bold text-slate-900 dark:text-slate-100 leading-tight"><%# Eval("EstudianteNombre") %></p>
                                <span class="text-[10px] font-mono text-slate-400"><%# Eval("EstudianteCodigo") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unidad Did&aacute;ctica / Fecha">
                            <ItemTemplate>
                                <p class="font-semibold text-slate-800 dark:text-slate-200"><%# Eval("UnidadDidacticaNombre") %></p>
                                <span class="text-[10px] text-slate-400 font-mono">&#128197; <%# FechaTexto(Eval("FechaClase")) %> (Sem <%# Eval("NumeroSemana") %>)</span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Motivo &amp; Explicaci&oacute;n">
                            <ItemStyle CssClass="max-w-xs" />
                            <ItemTemplate>
                                <span class="badge badge-ghost badge-xs font-bold text-indigo-700 dark:text-indigo-300 mb-1"><%# Eval("Motivo") %></span>
                                <p class="text-[11px] text-slate-600 dark:text-slate-400 truncate"><%# Eval("Descripcion") %></p>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sustento Adjunto">
                            <ItemTemplate>
                                <a href='<%# UrlAdjunto(Eval("DocumentoSustentoUrl")) %>' target="_blank"
                                    style='<%# EstiloAdjunto(Eval("DocumentoSustentoUrl")) %>'
                                    class="text-indigo-600 dark:text-indigo-400 hover:underline font-semibold flex items-center gap-1 text-[11px]">
                                    <span>&#128206; Ver Certificado</span>
                                </a>
                                <span class="text-slate-300 text-[11px]" style='<%# EstiloSinAdjunto(Eval("DocumentoSustentoUrl")) %>'>Sin adjunto</span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Estado">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <span class="badge badge-warning badge-sm font-bold text-white" style='<%# EstiloEstado(Eval("Estado"), "PENDIENTE") %>'>Pendiente</span>
                                <span class="badge badge-success badge-sm font-bold text-white" style='<%# EstiloEstado(Eval("Estado"), "APROBADA") %>'>Aprobada</span>
                                <span class="badge badge-error badge-sm font-bold text-white" style='<%# EstiloEstado(Eval("Estado"), "RECHAZADA") %>'>Rechazada</span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acci&oacute;n">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <span class="text-[11px] text-slate-400 font-medium" style='<%# _esSoloAlumno ? "" : "display:none;" %>'><%# TextoAccionAlumno(Eval("Estado")) %></span>
                                <asp:LinkButton ID="lnkEvaluar" runat="server" CommandName="Evaluar" CommandArgument='<%# Eval("Id") %>'
                                    Visible='<%# !_esSoloAlumno && EsEstado(Eval("Estado"), "PENDIENTE") %>'
                                    CssClass="btn bg-indigo-600 hover:bg-indigo-700 text-white btn-xs rounded-xl font-bold border-0 px-3">Evaluar</asp:LinkButton>
                                <span class="text-[10px] text-slate-400" style='<%# (_esSoloAlumno || EsEstado(Eval("Estado"), "PENDIENTE")) ? "display:none;" : "" %>'>Dictaminado</span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <asp:Panel ID="pnlVacio" runat="server" Visible="false">
                <div class="p-8 text-center text-xs text-slate-400">
                    No se encontraron solicitudes de justificaci&oacute;n.
                </div>
            </asp:Panel>
        </div>
    </div>

    <dialog id="modal_resolver" class="modal">
        <div class="modal-box max-w-md bg-white dark:bg-[#111827] rounded-3xl p-6 shadow-2xl border border-slate-200 dark:border-slate-800">
            <h3 class="text-base font-bold text-slate-900 dark:text-slate-100 mb-1">
                Dictamen de Justificaci&oacute;n
            </h3>
            <p class="text-xs text-slate-400 mb-4" id="modalResolverEstudiante"><%= TextoModalEstudiante %></p>

            <div class="space-y-3 text-xs">
                <div>
                    <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1">Resoluci&oacute;n Docente</label>
                    <asp:DropDownList ID="ddlResolucion" runat="server"
                        CssClass="select select-sm select-bordered w-full rounded-xl text-xs font-bold"></asp:DropDownList>
                </div>

                <div>
                    <label class="font-bold text-slate-700 dark:text-slate-300 block mb-1" for="<%= txtObservacion.ClientID %>">Observaci&oacute;n / Sustento del Dictamen</label>
                    <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" Rows="3"
                        CssClass="textarea textarea-bordered w-full rounded-xl text-xs"></asp:TextBox>
                </div>

                <div class="modal-action mt-6 flex gap-2">
                    <button type="button" onclick="modal_resolver.close()" class="btn btn-ghost btn-sm rounded-xl">Cancelar</button>
                    <asp:Button ID="btnEmitirDictamen" runat="server" Text="Emitir Dictamen" OnClick="btnEmitirDictamen_Click"
                        CssClass="btn bg-indigo-600 hover:bg-indigo-700 text-white btn-sm rounded-xl font-bold border-0 px-4" />
                </div>
            </div>
        </div>
    </dialog>

    <% if (_abrirModal) { %>
    <script type="text/javascript">
        (function () {
            var dlg = document.getElementById('modal_resolver');
            if (dlg && typeof dlg.showModal === 'function') dlg.showModal();
        })();
    </script>
    <% } %>
</asp:Content>
