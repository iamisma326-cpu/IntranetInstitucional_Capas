<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Sitio.master" CodeBehind="SinAcceso.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.SinAcceso" ValidateRequest="false" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContenidoPrincipal" runat="server">
    <div class="min-h-[70vh] flex items-center justify-center px-4">
        <div class="max-w-md w-full bg-white dark:bg-[#111827] rounded-3xl border border-slate-200/80 dark:border-slate-800 shadow-sm p-8 text-center">
            <div class="w-16 h-16 rounded-2xl bg-rose-50 dark:bg-rose-950/50 text-rose-600 dark:text-rose-400 flex items-center justify-center text-3xl mx-auto mb-5">
                &#128274;
            </div>
            <h1 class="text-lg sm:text-xl font-black text-slate-900 dark:text-slate-100 tracking-tight">Acceso no autorizado</h1>
            <p class="text-xs text-slate-500 dark:text-slate-400 mt-3 leading-relaxed">
                Tu rol activo no tiene permiso para realizar esta acci&oacute;n en el m&oacute;dulo.
                Si crees que deber&iacute;as tener acceso, cambia tu modo/rol activo desde el selector superior
                o consulta con la administraci&oacute;n del instituto.
            </p>
            <div class="mt-6 flex flex-col sm:flex-row gap-2 justify-center">
                <a href="Default.aspx" class="btn btn-outline btn-sm rounded-xl font-bold">Ir al inicio</a>
                <a href="#" class="btn btn-ghost btn-sm rounded-xl font-bold text-slate-500">Cambiar de cuenta</a>
            </div>
        </div>
    </div>
</asp:Content>
