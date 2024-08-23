using System.Collections.Generic;
using System.Linq;

namespace namasdev.Core.Types
{
    public static class IEnumerableExtensiones
    {
        public static bool IsNotNullAndNotEmpty<T>(this IEnumerable<T> lista)
        {
            return lista != null && lista.Any();
        }
    }
}
