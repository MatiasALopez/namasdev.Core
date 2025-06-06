namespace namasdev.Core.Types
{
    public class NumerosHelper
    {
        public static string CrearNumeroDesdeDigitos(int digitosEnteros, int digitosDecimales)
        {
            return $"{new string('9', digitosEnteros)}.{new string('9', digitosDecimales)}";
        }
    }
}
