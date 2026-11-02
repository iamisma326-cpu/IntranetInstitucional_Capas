using System;

namespace IntranetInstitucional_Capas.Entidades
{
    [Serializable]
    public class Usuario
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }
        public string CodigoInstitucional { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool Estado { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        public Persona Persona { get; set; }
    }
}
