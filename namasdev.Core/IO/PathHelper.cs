using System.IO;

namespace namasdev.Core.IO
{
    public class PathHelper
    {
        public static string CambiarNombreArchivoManteniendoExtension(string nombreActualConExtension, string nombreNuevoSinExtension)
        {
            return $"{nombreNuevoSinExtension}{Path.GetExtension(nombreActualConExtension)}";
        }
    }
}
