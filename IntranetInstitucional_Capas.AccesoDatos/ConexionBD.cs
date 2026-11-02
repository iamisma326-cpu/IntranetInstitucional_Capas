using System;
using System.Configuration;
using System.Data.SqlClient;

namespace IntranetInstitucional_Capas.AccesoDatos
{
    public static class ConexionBD
    {
        private const string NOMBRE_CONEXION = "CnxBD_Intranet";

        public static SqlConnection ObtenerConexion()
        {
            ConnectionStringSettings config =
                ConfigurationManager.ConnectionStrings[NOMBRE_CONEXION];

            if (config == null)
            {
                throw new ConfigurationErrorsException(
                    "No se encontro la cadena de conexion '" + NOMBRE_CONEXION + "' en el Web.config.");
            }

            return new SqlConnection(config.ConnectionString);
        }
    }
}
