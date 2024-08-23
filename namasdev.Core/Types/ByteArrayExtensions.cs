using System;
using System.Collections.Generic;

namespace namasdev.Core.Types
{
    public static class ByteArrayExtensions
    {
        public static int? ObtenerEntero(this byte[] bytes, int indice, int cantBytesEntero)
        {
            int result = 0;

            if (cantBytesEntero > 3)
            {
                result |= bytes[indice++] << 24;
            }
            if (cantBytesEntero > 2)
            {
                result |= bytes[indice++] << 16;
            }
            if (cantBytesEntero > 1)
            {
                result |= bytes[indice++] << 8;
            }
            result |= bytes[indice];

            return result;
        }

        public static decimal?[] ObtenerListaDecimales(this byte[] bytes, int tamañoBloques, int cantDecimales)
        {
            var lista = new List<decimal?>();

            int cantBloques = (bytes.Length - 4) / tamañoBloques;
            int pos;
            for (int i = 0; i < cantBloques; i++)
            {
                pos = (i * tamañoBloques) + 4;

                lista.Add(ObtenerDecimal(bytes, pos, tamañoBloques - 1, cantDecimales));
            }

            return lista.ToArray();
        }

        public static decimal? ObtenerDecimal(this byte[] bytes, int indice, int cantBytesEntero, int cantDecimales)
        {
            int? entero = bytes[indice] == 0
                ? ObtenerEntero(bytes, indice + 1, cantBytesEntero)
                : null;

            if (entero.HasValue)
            {
                return entero.Value * (decimal)Math.Pow(10, -cantDecimales);
            }
            else
            {
                return null;
            }
        }
    }
}
