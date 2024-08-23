using System.Linq;

namespace Disercoin.Core.Math
{
    public class MathHelper
    {
        public static decimal? Max(params decimal?[] valores)
        {
            if (valores.Length == 0)
            {
                return null;
            }

            return valores.Max();
        }

        public static double? Max(params double?[] valores)
        {
            if (valores.Length == 0)
            {
                return null;
            }

            return valores.Max();
        }

        public static decimal? Min(params decimal?[] valores)
        {
            if (valores.Length == 0)
            {
                return null;
            }

            return valores.Min();
        }

        public static double? Min(params double?[] valores)
        {
            if (valores.Length == 0)
            {
                return null;
            }

            return valores.Min();
        }

        public static double Sumar(double[] valores,
            int? digitosRedondeo = null)
        {
            return AplicarDigitosRedondeo(valores.Sum(), digitosRedondeo);
        }

        public static double? Sumar(double?[] valores,
            int? digitosRedondeo = null)
        {
            var suma = valores.Sum();
            return suma.HasValue
                ? AplicarDigitosRedondeo(suma.Value, digitosRedondeo)
                : (double?)null;
        }

        public static decimal? Sumar(decimal?[] valores,
            int? digitosRedondeo = null)
        {
            var suma = valores.Sum();
            return suma.HasValue
                ? AplicarDigitosRedondeo(suma.Value, digitosRedondeo)
                : (decimal?)null;
        }

        public static double Promedio(double[] valores,
            int? digitosRedondeo = null)
        {
            return AplicarDigitosRedondeo(valores.Average(), digitosRedondeo);
        }

        public static double? Promedio(double?[] valores,
            int? digitosRedondeo = null)
        {
            var promedio = valores.Average();
            return promedio.HasValue
                ? AplicarDigitosRedondeo(promedio.Value, digitosRedondeo)
                : (double?)null;
        }

        public static decimal? Promedio(decimal?[] valores, 
            int? digitosRedondeo = null)
        {
            var promedio = valores.Average();
            return promedio.HasValue
                ? AplicarDigitosRedondeo(promedio.Value, digitosRedondeo)
                : (decimal?)null;
        }

        public static decimal? Promedio(decimal? suma, int? cantTotal,
            int? digitosRedondeo = null)
        {
            return suma.HasValue && cantTotal.HasValue
                ? Promedio(suma.Value, cantTotal.Value, digitosRedondeo)
                : (decimal?)null;
        }

        public static decimal Promedio(decimal suma, int cantTotal,
            int? digitosRedondeo = null)
        {
            return cantTotal > 0
                ? AplicarDigitosRedondeo(suma / cantTotal, digitosRedondeo) 
                : 0;
        }

        public static double? Promedio(double? suma, int? cantTotal,
            int? digitosRedondeo = null)
        {
            return suma.HasValue && cantTotal.HasValue
                ? Promedio(suma.Value, cantTotal.Value, digitosRedondeo)
                : (double?)null;
        }

        public static double Promedio(double suma, int cantTotal,
            int? digitosRedondeo = null)
        {
            return cantTotal > 0
                ? AplicarDigitosRedondeo(suma / cantTotal, digitosRedondeo)
                : 0;
        }

        public static decimal? Porcentaje(int? contador, int cantTotal,
            int? digitosRedondeo = null)
        {
            return contador.HasValue
                ? Porcentaje(contador.Value, cantTotal, digitosRedondeo)
                : (decimal?)null;
        }

        public static decimal Porcentaje(int contador, int cantTotal,
            int? digitosRedondeo = null)
        {
            return cantTotal > 0 
                ? AplicarDigitosRedondeo((decimal)contador / cantTotal, digitosRedondeo)
                : 0;
        }

        public static decimal? PorcentajeSi(bool condicion, int? contador, int cantTotal,
            int? digitosRedondeo = null)
        {
            return condicion && contador.HasValue
                ? Porcentaje(contador.Value, cantTotal, digitosRedondeo)
                : (decimal?)null;
        }

        public static int ContarNoNull(params decimal?[] valores)
        {
            return valores.Count(v => v.HasValue);
        }

        public static int ContarNoNull(params double?[] valores)
        {
            return valores.Count(v => v.HasValue);
        }

        public static int ContarTrue(params bool[] valores)
        {
            return valores.Count(v => v);
        }

        private static decimal AplicarDigitosRedondeo(decimal valor, int? digitosRedondeo)
        {
            return digitosRedondeo.HasValue
                ? System.Math.Round(valor, digitosRedondeo.Value)
                : valor;
        }

        private static double AplicarDigitosRedondeo(double valor, int? digitosRedondeo)
        {
            return digitosRedondeo.HasValue
                ? System.Math.Round(valor, digitosRedondeo.Value)
                : valor;
        }
    }
}
