namespace namasdev.Core.Types
{
    public class NumerosHelper
    {
        protected string CrearNumeroDesdeDigitos(int digitosEnteros, int digitosDecimales)
        {
            return $"{new string('9', digitosEnteros)}.{new string('9', digitosDecimales)}";
        }
    }
}
