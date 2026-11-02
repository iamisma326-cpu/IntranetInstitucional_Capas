<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IntranetInstitucional_Capas.PresentacionWeb.Login" %>

<!DOCTYPE html>
<html lang="es" data-theme="light">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar Sesion - AQUILA A-ERP | IESTP Argentina</title>

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800;900&display=swap" rel="stylesheet" />

    <link href="https://cdn.jsdelivr.net/npm/daisyui@4.7.2/dist/full.min.css" rel="stylesheet" type="text/css" />
    <script src="https://cdn.tailwindcss.com"></script>

    <script>
        tailwind.config = {
            darkMode: ['class', '[data-theme="dark"]'],
            theme: {
                extend: {
                    fontFamily: {
                        sans: ['"Plus Jakarta Sans"', 'system-ui', 'sans-serif'],
                    },
                    colors: {
                        aquila: {
                            50: '#f0f7ff',
                            100: '#e0effe',
                            200: '#bae0fd',
                            500: '#0ea5e9',
                            700: '#0369a1',
                            900: '#0c4a6e',
                            950: '#082f49',
                        }
                    }
                }
            }
        }
    </script>

    <script>
        (function () {
            try {
                var savedTheme = localStorage.getItem('aquila-theme') || 'light';
                document.documentElement.setAttribute('data-theme', savedTheme);
                if (savedTheme === 'dark') {
                    document.documentElement.classList.add('dark');
                } else {
                    document.documentElement.classList.remove('dark');
                }
            } catch (e) { }
        })();
    </script>

    <meta name="theme-color" content="#0b132b" />

    <style>
        body {
            font-family: 'Plus Jakarta Sans', sans-serif;
            transition: background-color 0.2s ease, color 0.2s ease;
        }
        [data-theme="dark"] { color-scheme: dark; }
        [data-theme="light"] { color-scheme: light; }
    </style>
</head>

<body class="bg-slate-50 dark:bg-[#0b0f19] text-slate-800 dark:text-slate-200 selection:bg-blue-600 selection:text-white transition-colors duration-200">
    <form id="formPrincipal" runat="server">

        <div class="min-h-screen flex flex-col justify-between bg-slate-50 dark:bg-[#0b0f19] transition-colors">

            <header class="navbar bg-slate-900/95 dark:bg-[#0f172a]/95 backdrop-blur-md text-white shadow-sm px-6 h-16 min-h-0 border-b border-slate-800">
                <div class="flex-1 items-center gap-3">
                    <div class="w-9 h-9 rounded-xl bg-gradient-to-br from-slate-900 to-blue-950 border border-cyan-400/40 p-1.5 flex items-center justify-center shadow-sm shrink-0">
                        <svg viewBox="0 0 100 100" class="w-full h-full drop-shadow-[0_0_6px_rgba(56,189,248,0.7)]" fill="none">
                            <defs>
                                <linearGradient id="unauthWingL" x1="0%" y1="0%" x2="100%" y2="100%"><stop offset="0%" stop-color="#38bdf8" /><stop offset="100%" stop-color="#1d4ed8" /></linearGradient>
                                <linearGradient id="unauthWingR" x1="100%" y1="0%" x2="0%" y2="100%"><stop offset="0%" stop-color="#60a5fa" /><stop offset="100%" stop-color="#2563eb" /></linearGradient>
                                <linearGradient id="unauthCore" x1="0%" y1="0%" x2="0%" y2="100%"><stop offset="0%" stop-color="#ffffff" /><stop offset="100%" stop-color="#93c5fd" /></linearGradient>
                            </defs>
                            <polygon points="48,10 4,68 32,60 46,38" fill="url(#unauthWingL)" />
                            <polygon points="52,10 96,68 68,60 54,38" fill="url(#unauthWingR)" />
                            <polygon points="50,6 43,44 50,88 57,44" fill="url(#unauthCore)" />
                            <polygon points="50,22 46,44 50,70 54,44" fill="#0284c7" />
                        </svg>
                    </div>
                    <div class="flex items-center gap-2">
                        <span class="text-base font-black tracking-tight text-white">AQUILA</span>
                        <span class="text-[10px] bg-cyan-500/20 text-cyan-300 px-2 py-0.5 rounded font-bold border border-cyan-500/30">A-ERP</span>
                    </div>
                </div>

                <div class="flex-none items-center gap-2">

                    <button type="button" onclick="toggleAquilaTheme()" class="btn btn-ghost btn-sm btn-circle text-amber-300 hover:bg-white/10 transition" title="Alternar Modo Oscuro">
                        <svg class="theme-icon-sun w-5 h-5 hidden text-amber-300" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
                        <svg class="theme-icon-moon w-5 h-5 text-cyan-200" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z" /></svg>
                    </button>
                </div>
            </header>

            <main class="flex-1 flex items-center justify-center p-4">
                <div class="max-w-md w-full mx-auto my-6 sm:my-12 space-y-6">

                    <div class="bg-white dark:bg-[#111827] rounded-3xl p-7 sm:p-9 border border-slate-200/80 dark:border-slate-800 shadow-sm space-y-6">
                        <div class="text-center space-y-3">

                            <div class="w-16 h-16 rounded-2xl bg-gradient-to-br from-slate-900 to-blue-950 border border-cyan-400/40 p-2.5 mx-auto shadow-sm flex items-center justify-center">
                                <svg viewBox="0 0 100 100" class="w-full h-full drop-shadow-[0_0_8px_rgba(56,189,248,0.7)]" fill="none">
                                    <defs>
                                        <linearGradient id="loginWingL" x1="0%" y1="0%" x2="100%" y2="100%"><stop offset="0%" stop-color="#38bdf8" /><stop offset="100%" stop-color="#1d4ed8" /></linearGradient>
                                        <linearGradient id="loginWingR" x1="100%" y1="0%" x2="0%" y2="100%"><stop offset="0%" stop-color="#60a5fa" /><stop offset="100%" stop-color="#2563eb" /></linearGradient>
                                        <linearGradient id="loginCore" x1="0%" y1="0%" x2="0%" y2="100%"><stop offset="0%" stop-color="#ffffff" /><stop offset="100%" stop-color="#93c5fd" /></linearGradient>
                                    </defs>
                                    <polygon points="48,10 4,68 32,60 46,38" fill="url(#loginWingL)" />
                                    <polygon points="52,10 96,68 68,60 54,38" fill="url(#loginWingR)" />
                                    <polygon points="50,6 43,44 50,88 57,44" fill="url(#loginCore)" />
                                    <polygon points="50,22 46,44 50,70 54,44" fill="#0284c7" />
                                </svg>
                            </div>

                            <div>
                                <div class="flex items-center justify-center gap-2">
                                    <h1 class="text-xl font-black text-slate-900 dark:text-slate-100 tracking-tight">AQUILA</h1>
                                    <span class="text-[10px] bg-blue-100 dark:bg-blue-950 text-blue-800 dark:text-cyan-300 font-bold px-2 py-0.5 rounded border border-blue-200 dark:border-blue-800">A-ERP</span>
                                </div>
                                <p class="text-xs text-slate-500 dark:text-slate-400 mt-1">
                                    Intranet Institucional &bull; IESTP "Argentina"
                                </p>
                            </div>
                        </div>

                        <asp:Panel ID="pnlError" runat="server" Visible="false"
                            CssClass="p-3 rounded-2xl bg-rose-50 dark:bg-rose-950/40 text-rose-700 dark:text-rose-300 border border-rose-200 dark:border-rose-900 text-xs">
                            <asp:Label ID="lblError" runat="server" />
                        </asp:Panel>

                        <div class="space-y-4 text-xs">
                            <asp:HiddenField ID="hdnReturnUrl" runat="server" />

                            <div>
                                <label for="<%= txtIdentificador.ClientID %>" class="font-bold text-slate-700 dark:text-slate-300 block mb-1 text-[11px]">
                                    DNI / Identificador
                                </label>
                                <asp:TextBox ID="txtIdentificador" runat="server"
                                    placeholder="87654321 o 10000001"
                                    required="required"
                                    CssClass="input input-sm input-bordered w-full rounded-2xl bg-slate-50 dark:bg-slate-800/80 text-xs font-semibold text-slate-800 dark:text-slate-100 border-slate-200 dark:border-slate-700 h-10" />
                            </div>

                            <div>
                                <label for="<%= txtPassword.ClientID %>" class="font-bold text-slate-700 dark:text-slate-300 block mb-1 text-[11px]">
                                    Contrasena
                                </label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                    placeholder="&bull;&bull;&bull;&bull;&bull;&bull;"
                                    required="required"
                                    CssClass="input input-sm input-bordered w-full rounded-2xl bg-slate-50 dark:bg-slate-800/80 text-xs font-semibold text-slate-800 dark:text-slate-100 border-slate-200 dark:border-slate-700 h-10" />
                            </div>

                            <asp:Button ID="btnIngresar" runat="server" Text="Ingresar al Sistema &#8594;"
                                OnClick="btnIngresar_Click"
                                CssClass="btn btn-primary w-full rounded-2xl font-bold text-xs h-10 shadow-sm" />
                        </div>
                    </div>

                    <div class="bg-white dark:bg-[#111827] rounded-3xl p-5 border border-slate-200/80 dark:border-slate-800 shadow-sm space-y-3">
                        <span class="text-[10px] font-bold text-slate-400 uppercase tracking-wider block">
                            Accesos de Demostracion (1 Clic)
                        </span>

                        <div class="grid grid-cols-2 gap-2 text-xs">

                            <button type="button" onclick="quickLogin('10000001', '123456')" class="p-2.5 rounded-xl bg-slate-50 dark:bg-slate-800/60 hover:bg-slate-100 dark:hover:bg-slate-800 text-slate-800 dark:text-slate-200 border border-slate-200/60 dark:border-slate-700/60 text-left transition flex items-center justify-between">
                                <div>
                                    <p class="font-bold text-[11px]">&#127963;&#65039; Director</p>
                                    <p class="text-[10px] text-slate-400">Felipe</p>
                                </div>
                                <span class="text-[10px] text-blue-600 dark:text-cyan-400 font-semibold">Entrar</span>
                            </button>

                            <button type="button" onclick="quickLogin('12345678', '123456')" class="p-2.5 rounded-xl bg-slate-50 dark:bg-slate-800/60 hover:bg-slate-100 dark:hover:bg-slate-800 text-slate-800 dark:text-slate-200 border border-slate-200/60 dark:border-slate-700/60 text-left transition flex items-center justify-between">
                                <div>
                                    <p class="font-bold text-[11px]">&#128105;&#8205;&#127979; Docente</p>
                                    <p class="text-[10px] text-slate-400">Sheyla (M02)</p>
                                </div>
                                <span class="text-[10px] text-blue-600 dark:text-cyan-400 font-semibold">Entrar</span>
                            </button>

                            <button type="button" onclick="quickLogin('87654321', '123456')" class="p-2.5 rounded-xl bg-slate-50 dark:bg-slate-800/60 hover:bg-slate-100 dark:hover:bg-slate-800 text-slate-800 dark:text-slate-200 border border-slate-200/60 dark:border-slate-700/60 text-left transition flex items-center justify-between">
                                <div>
                                    <p class="font-bold text-[11px]">&#127890; Estudiante</p>
                                    <p class="text-[10px] text-slate-400">Mendoza</p>
                                </div>
                                <span class="text-[10px] text-blue-600 dark:text-cyan-400 font-semibold">Entrar</span>
                            </button>

                            <button type="button" onclick="quickLogin('20000001', '123456')" class="p-2.5 rounded-xl bg-slate-50 dark:bg-slate-800/60 hover:bg-slate-100 dark:hover:bg-slate-800 text-slate-800 dark:text-slate-200 border border-slate-200/60 dark:border-slate-700/60 text-left transition flex items-center justify-between">
                                <div>
                                    <p class="font-bold text-[11px]">&#128203; Coordinador</p>
                                    <p class="text-[10px] text-slate-400">Rodriguez</p>
                                </div>
                                <span class="text-[10px] text-blue-600 dark:text-cyan-400 font-semibold">Entrar</span>
                            </button>

                            <button type="button" onclick="quickLogin('30000001', '123456')" class="p-2.5 rounded-xl bg-slate-50 dark:bg-slate-800/60 hover:bg-slate-100 dark:hover:bg-slate-800 text-slate-800 dark:text-slate-200 border border-slate-200/60 dark:border-slate-700/60 text-left transition flex items-center justify-between">
                                <div>
                                    <p class="font-bold text-[11px]">&#128193; Secretaria</p>
                                    <p class="text-[10px] text-slate-400">Flores</p>
                                </div>
                                <span class="text-[10px] text-blue-600 dark:text-cyan-400 font-semibold">Entrar</span>
                            </button>

                            <button type="button" onclick="quickLogin('40000001', '123456')" class="p-2.5 rounded-xl bg-slate-50 dark:bg-slate-800/60 hover:bg-slate-100 dark:hover:bg-slate-800 text-slate-800 dark:text-slate-200 border border-slate-200/60 dark:border-slate-700/60 text-left transition flex items-center justify-between">
                                <div>
                                    <p class="font-bold text-[11px]">&#128179; Tesoreria</p>
                                    <p class="text-[10px] text-slate-400">Perez</p>
                                </div>
                                <span class="text-[10px] text-blue-600 dark:text-cyan-400 font-semibold">Entrar</span>
                            </button>
                        </div>
                    </div>

                </div>
            </main>

            <footer class="p-4 bg-white dark:bg-[#0f172a] border-t border-slate-200 dark:border-slate-800 text-center text-xs text-slate-500 dark:text-slate-400">
                <p class="font-medium">&#129413; AQUILA A-ERP &bull; IESTP "Argentina" 2026</p>
            </footer>
        </div>

    </form>

    <script>
        function toggleAquilaTheme() {
            var currentTheme = document.documentElement.getAttribute('data-theme') || 'light';
            var newTheme = currentTheme === 'dark' ? 'light' : 'dark';
            document.documentElement.setAttribute('data-theme', newTheme);
            if (newTheme === 'dark') {
                document.documentElement.classList.add('dark');
            } else {
                document.documentElement.classList.remove('dark');
            }
            localStorage.setItem('aquila-theme', newTheme);
            updateAquilaThemeIcons();
        }

        function updateAquilaThemeIcons() {
            var isDark = document.documentElement.getAttribute('data-theme') === 'dark';
            var suns = document.querySelectorAll('.theme-icon-sun');
            var moons = document.querySelectorAll('.theme-icon-moon');
            for (var i = 0; i < suns.length; i++) { suns[i].classList.toggle('hidden', !isDark); }
            for (var j = 0; j < moons.length; j++) { moons[j].classList.toggle('hidden', isDark); }
        }

        document.addEventListener('DOMContentLoaded', updateAquilaThemeIcons);

        function quickLogin(dni, pass) {
            document.getElementById('<%= txtIdentificador.ClientID %>').value = dni;
            document.getElementById('<%= txtPassword.ClientID %>').value = pass;
            var btn = document.getElementById('<%= btnIngresar.ClientID %>');
            if (btn) { btn.focus(); }
        }
    </script>
</body>
</html>
