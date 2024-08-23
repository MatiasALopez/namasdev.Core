using System;
using System.Collections.Generic;
using System.Linq;

namespace namasdev.Core.Types
{
    public class HtmlHelper
    {
        /// <summary>
        /// Crea lista HTML (ul).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valores"></param>
        /// <param name="esListaOrdenada"></param>
        /// <returns></returns>
        public static string CrearListaHtml<T>(IEnumerable<T> valores,
            bool esListaOrdenada = false)
        {
            if (valores == null || !valores.Any())
            {
                return String.Empty;
            }

            string tagLista = esListaOrdenada ? "ol" : "ul";
            return $"<{tagLista}>{Formateador.FormatoLista(valores.Select(v => $"<li>{v}</li>"), separador: string.Empty)}</{tagLista}>";
        }
    }
}
