using System;

namespace namasdev.Core.Exceptions
{
    public class ExceptionHelper
    {
        public static string ObtenerMensajesRecursivamente(Exception ex)
        {
            string mensaje = ex.Message;
            if (ex.InnerException != null)
            {
                mensaje += Environment.NewLine + ObtenerMensajesRecursivamente(ex.InnerException);
            }
            return mensaje;
        }
    }
}
