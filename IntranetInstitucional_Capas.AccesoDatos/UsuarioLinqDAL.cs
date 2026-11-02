using System;
using System.Collections.Generic;
using System.Linq;
using IntranetInstitucional_Capas.Entidades;

namespace IntranetInstitucional_Capas.AccesoDatos
{
    public class UsuarioLinqDAL
    {
        public Usuario BuscarPorIdentificador(string identificador)
        {
            if (string.IsNullOrWhiteSpace(identificador)) return null;

            string limpio = identificador.Trim().ToLower();

            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    var fila = (from u in db.Usuarios
                                join p in db.Personas on u.PersonaId equals p.Id
                                where u.Estado
                                      && (p.Dni.ToLower() == limpio
                                          || (p.EmailPersonal ?? string.Empty).ToLower() == limpio
                                          || u.CodigoInstitucional.ToLower() == limpio
                                          || u.Email.ToLower() == limpio)
                                select new { Usuario = u, Persona = p }).FirstOrDefault();

                    if (fila == null) return null;

                    return MapearUsuario(fila.Usuario, fila.Persona);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al buscar el usuario: " + ex.Message);
                }
            }
        }

        public List<string> ListarRoles(int usuarioId)
        {
            List<string> roles = new List<string>();

            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    List<string> nombres = (from ur in db.UsuarioRoles
                                            join r in db.Roles on ur.RolId equals r.Id
                                            where ur.UsuarioId == usuarioId && ur.EsActivo == true
                                            orderby r.Nombre
                                            select r.Nombre).ToList();

                    foreach (string nombre in nombres)
                    {
                        if (!string.IsNullOrWhiteSpace(nombre)) roles.Add(nombre.Trim());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al listar roles: " + ex.Message);
                }
            }

            return roles;
        }

        public int ObtenerDocenteId(int personaId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    Docentes docente = db.Docentes.FirstOrDefault(d => d.PersonaId == personaId);
                    return docente == null ? 0 : docente.Id;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener el docente: " + ex.Message);
                }
            }
        }

        public int ObtenerEstudianteId(int personaId)
        {
            using (BD_INTRANETDataContext db = new BD_INTRANETDataContext())
            {
                try
                {
                    Estudiantes estudiante = db.Estudiantes.FirstOrDefault(e => e.PersonaId == personaId);
                    return estudiante == null ? 0 : estudiante.Id;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error tecnico en DAL (LINQ to SQL) al obtener el estudiante: " + ex.Message);
                }
            }
        }

        private static Usuario MapearUsuario(Usuarios usuarioLinq, Personas personaLinq)
        {
            Usuario usuario = new Usuario();
            usuario.Id = usuarioLinq.Id;
            usuario.PersonaId = usuarioLinq.PersonaId;
            usuario.CodigoInstitucional = usuarioLinq.CodigoInstitucional;
            usuario.Email = usuarioLinq.Email;
            usuario.PasswordHash = usuarioLinq.PasswordHash;
            usuario.Estado = usuarioLinq.Estado;
            usuario.UltimoAcceso = usuarioLinq.UltimoAcceso;

            Persona persona = new Persona();
            persona.Id = personaLinq.Id;
            persona.Dni = personaLinq.Dni;
            persona.Nombres = personaLinq.Nombres;
            persona.Apellidos = personaLinq.Apellidos;
            persona.EmailPersonal = personaLinq.EmailPersonal;
            persona.Telefono = personaLinq.Telefono;

            usuario.Persona = persona;
            return usuario;
        }
    }
}
