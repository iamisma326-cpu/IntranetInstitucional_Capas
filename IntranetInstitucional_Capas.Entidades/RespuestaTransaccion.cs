namespace IntranetInstitucional_Capas.Entidades
{
    public class RespuestaTransaccion
    {
        public bool EsExitosa { get; set; }

        public string Mensaje { get; set; }

        public int? IdGenerado { get; set; }

        public static RespuestaTransaccion Exito(string mensaje, int? idGenerado = null)
        {
            return new RespuestaTransaccion { EsExitosa = true, Mensaje = mensaje, IdGenerado = idGenerado };
        }

        public static RespuestaTransaccion Error(string mensaje)
        {
            return new RespuestaTransaccion { EsExitosa = false, Mensaje = mensaje };
        }
    }
}
