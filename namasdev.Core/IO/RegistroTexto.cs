using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using namasdev.Core.Types;

namespace namasdev.Core.IO
{
    public abstract class RegistroTexto
    {
        private List<string> _errores;

        public RegistroTexto(int linea,
            bool incluirPosicionEnError = false)
        {
            Linea = linea;
            IncluirPosicionEnError = incluirPosicionEnError;

            _errores = new List<string>();
        }

        public int Linea { get; private set; }
        public IEnumerable<string> Errores
        {
            get { return _errores.AsReadOnly(); }
        }
        private bool IncluirPosicionEnError { get; set; }

        public bool EsValido
        {
            get { return _errores.Count == 0; }
        }

        protected bool TieneAlgunDato(string[] datos)
        {
            return datos != null
                && datos.Any()
                && datos.Any(d => !String.IsNullOrWhiteSpace(d));
        }

        protected string ObtenerString(string[] datos, int posicion, string descripcionDato,
            bool esRequerido = true, int? tamañoMaximo = null,
            bool convertirCadenaVaciaANull = true)
        {
            string dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido, tamañoMaximo);
            return String.IsNullOrWhiteSpace(dato) && convertirCadenaVaciaANull
                ? null
                : dato;
        }

        protected int? ObtenerInt(string[] datos, int posicion, string descripcionDato, bool esRequerido = true)
        {
            var dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido);

            if (!String.IsNullOrWhiteSpace(dato))
            {
                try
                {
                    return int.Parse(dato);
                }
                catch (Exception)
                {
                    AgregarError(posicion, $"{descripcionDato} no es un entero válido.");
                }
            }

            return null;
        }

        protected short? ObtenerShort(string[] datos, int posicion, string descripcionDato, bool esRequerido = true)
        {
            var dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido);

            if (!String.IsNullOrWhiteSpace(dato))
            {
                try
                {
                    return short.Parse(dato);
                }
                catch (Exception)
                {
                    AgregarError(posicion, $"{descripcionDato} no es un entero corto válido.");
                }
            }

            return null;
        }

        protected long? ObtenerLong(string[] datos, int posicion, string descripcionDato, bool esRequerido = true)
        {
            var dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido);

            if (!String.IsNullOrWhiteSpace(dato))
            {
                try
                {
                    return long.Parse(dato);
                }
                catch (Exception)
                {
                    AgregarError(posicion, $"{descripcionDato} no es un entero largo válido.");
                }
            }

            return null;
        }

        protected double? ObtenerDouble(string[] datos, int posicion, string descripcionDato, bool esRequerido = true)
        {
            var dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido);

            if (!String.IsNullOrWhiteSpace(dato))
            {
                try
                {
                    return double.Parse(dato);
                }
                catch (Exception)
                {
                    AgregarError(posicion, $"{descripcionDato} no es un número válido.");
                }
            }

            return null;
        }

        protected decimal? ObtenerDecimal(string[] datos, int posicion, string descripcionDato, bool esRequerido = true)
        {
            var dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido);

            if (!String.IsNullOrWhiteSpace(dato))
            {
                try
                {
                    return decimal.Parse(dato);
                }
                catch (Exception)
                {
                    AgregarError(posicion, $"{descripcionDato} no es un número válido.");
                }
            }

            return null;
        }

        protected DateTime? ObtenerDateTime(string[] datos, int posicion, string descripcionDato,
            bool esRequerido = true, string formato = null)
        {
            var dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido);

            if (!String.IsNullOrWhiteSpace(dato))
            {
                try
                {
                    return String.IsNullOrWhiteSpace(formato)
                        ? DateTime.Parse(dato)
                        : DateTime.ParseExact(dato, formato, CultureInfo.CurrentCulture);
                }
                catch (Exception)
                {
                    AgregarError(posicion, $"{descripcionDato} no es una fecha/hora válida.");
                }
            }

            return null;
        }

        protected TimeSpan? ObtenerTimeSpan(string[] datos, int posicion, string descripcionDato,
            bool esRequerido = true, string formato = null)
        {
            var dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido);

            if (!String.IsNullOrWhiteSpace(dato))
            {
                try
                {
                    return String.IsNullOrWhiteSpace(formato)
                        ? TimeSpan.Parse(dato)
                        : TimeSpan.ParseExact(dato, formato, CultureInfo.CurrentCulture);
                }
                catch (Exception)
                {
                    AgregarError(posicion, $"{descripcionDato} no es una hora válida.");
                }
            }

            return null;
        }

        protected bool? ObtenerBoolean(string[] datos, int posicion, string descripcionDato, bool esRequerido = true)
        {
            var dato = ObtenerDatoEnPosicion(datos, posicion, descripcionDato, esRequerido);

            if (!String.IsNullOrWhiteSpace(dato))
            {
                try
                {
                    return bool.Parse(dato);
                }
                catch (Exception)
                {
                    try
                    {
                        return string.Equals(dato, Formateador.TEXTO_SI_SIN_ACENTO, StringComparison.CurrentCultureIgnoreCase)
                            || string.Equals(dato, Formateador.TEXTO_SI_CON_ACENTO, StringComparison.CurrentCultureIgnoreCase);
                    }
                    catch (Exception)
                    {
                        AgregarError(posicion, $"{descripcionDato} no es dato booleano válido.");
                    }
                }
            }

            return null;
        }

        private string ObtenerDatoEnPosicion(string[] datos, int posicion, string descripcion, bool esRequerido = true, int? tamañoMaximo = null)
        {
            int indice = posicion - 1;
            if (posicion < 1 || posicion > datos.Length)
            {
                AgregarError(posicion, $"Posición inválida (posición: {posicion}, posición máxima: {datos.Length}).");
                return null;
            }

            string dato = datos[indice];

            if (String.IsNullOrWhiteSpace(dato))
            {
                if (esRequerido)
                {
                    AgregarError(posicion, $"{descripcion} es un dato mandatorio.");
                }
            }
            else if (dato.Length > tamañoMaximo)
            {
                AgregarError(posicion, $"{descripcion} debe tener hasta {tamañoMaximo} caracteres.");
            }

            return dato;
        }

        protected void AgregarError(int posicion, string error)
        {
            string posicionTexto = $"[Posición {posicion}] ";
            _errores.Add($"{(IncluirPosicionEnError ? posicionTexto : String.Empty)}{error}");
        }
    }
}
