using System;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class Persona
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string EmailPersonal { get; set; }
        public string Telefono { get; set; }

        public string NombreCompleto
        {
            get { return (Nombres + " " + Apellidos).Trim(); }
        }
    }
}
