using System;
using System.Linq;

namespace namasdev.Core.Types
{
    public class Conversor
    {
        public static Guid[] ConvertirAListaGuids(string lista,
            string separador = ",")
        {
            return string.IsNullOrWhiteSpace(lista)
                ? null
                : lista.Split(new[] { separador }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(it => Guid.Parse(it))
                    .ToArray();
        }

        public static short[] ConvertirAListaShort(string lista,
            string separador = ",")
        {
            return string.IsNullOrWhiteSpace(lista)
                ? null
                : lista.Split(new[] { separador }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(it => short.Parse(it))
                    .ToArray();
        }

        public static string[] ConvertirALista(string lista,
            string separador = ",")
        {
            return string.IsNullOrWhiteSpace(lista)
                ? null
                : lista.Split(new[] { separador }, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
        }
    }
}
