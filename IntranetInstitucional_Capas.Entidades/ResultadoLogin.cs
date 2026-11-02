using System;
using System.Collections.Generic;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class ResultadoLogin
    {
        public bool Ok { get; set; }
        public string Mensaje { get; set; }
        public int UsuarioId { get; set; }
        public int PersonaId { get; set; }
        public int? DocenteId { get; set; }
        public int? EstudianteId { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string CodigoInstitucional { get; set; }
        public string Email { get; set; }
        public string RolPrincipal { get; set; }
        public List<string> Roles { get; set; }

        public ResultadoLogin()
        {
            Roles = new List<string>();
        }

        public SesionUsuario CrearSesion()
        {
            SesionUsuario sesion = new SesionUsuario
            {
                UsuarioId = UsuarioId,
                PersonaId = PersonaId,
                DocenteId = DocenteId,
                EstudianteId = EstudianteId,
                NombreCompleto = NombreCompleto,
                Dni = Dni,
                CodigoInstitucional = CodigoInstitucional,
                Email = Email,
                RolActivo = string.IsNullOrEmpty(RolPrincipal) ? Rol.Alumno : RolPrincipal,
                Roles = new List<string>(Roles)
            };
            return sesion;
        }
    }
}
