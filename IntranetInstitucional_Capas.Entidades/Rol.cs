using System;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class Rol
    {
        public const string Director = "Director";
        public const string Admin = "Admin";
        public const string Coordinador = "Coordinador";
        public const string Docente = "Docente";
        public const string Alumno = "Alumno";
        public const string Estudiante = "Estudiante";
        public const string Secretaria = "Secretaria";
        public const string Tesoreria = "Tesoreria";

        public static readonly string[] Orden = new string[]
        {
            Director,
            Docente,
            Alumno,
            Coordinador,
            Secretaria,
            Tesoreria
        };

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
