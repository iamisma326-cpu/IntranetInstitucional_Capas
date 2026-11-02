using System;
using System.Collections.Generic;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class SesionUsuario
    {
        public int UsuarioId { get; set; }
        public int PersonaId { get; set; }
        public int? DocenteId { get; set; }
        public int? EstudianteId { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string CodigoInstitucional { get; set; }
        public string Email { get; set; }
        public string RolActivo { get; set; }
        public List<string> Roles { get; set; }

        public SesionUsuario()
        {
            Roles = new List<string>();
        }

        public bool EsDirector
        {
            get { return Es(Rol.Director) || Es(Rol.Admin); }
        }

        public bool EsCoordinador
        {
            get { return Es(Rol.Coordinador) || EsDirector; }
        }

        public bool EsDocente
        {
            get { return Es(Rol.Docente) || EsCoordinador; }
        }

        public bool EsAlumno
        {
            get { return Es(Rol.Alumno) || Es(Rol.Estudiante); }
        }

        public bool EsSecretaria
        {
            get { return Es(Rol.Secretaria); }
        }

        public bool EsTesoreria
        {
            get { return Es(Rol.Tesoreria); }
        }

        private bool Es(string rol)
        {
            return string.Equals(RolActivo, rol, StringComparison.OrdinalIgnoreCase);
        }

        public bool TieneRol(string rol)
        {
            if (Roles == null) return false;
            foreach (string r in Roles)
            {
                if (string.Equals(r, rol, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        public bool ModoAlumno
        {
            get { return EsAlumno && !EsDocente; }
        }

        public bool AccedeVistaDocente
        {
            get { return EsDocente; }
        }

        public bool PuedeGuardarAsistencia
        {
            get { return EsDocente; }
        }

        public bool PuedeSolicitarJustificacion
        {
            get { return EsAlumno || EsDocente || EsSecretaria; }
        }

        public bool PuedeResolverJustificacion
        {
            get { return EsDocente; }
        }
    }
}
