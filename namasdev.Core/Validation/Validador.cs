using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

using namasdev.Core.Exceptions;
using namasdev.Core.IO;
using namasdev.Core.Reflection;
using namasdev.Core.Types;

namespace namasdev.Core.Validation
{
    public class Validador
    {
        public const string REQUERIDO_TEXTO_FORMATO = "{0} es un dato requerido.";
        public const string TAMAÑO_MAXIMO_TEXTO_FORMATO = "{0} debe tener hasta {1} caracteres.";
        public const string TAMAÑO_EXACTO_TEXTO_FORMATO = "{0} debe tener exactamente {1} caracteres.";
        public const string TIPO_NO_VALIDO_TEXTO_FORMATO = "{0} no es un dato de tipo {1} válido.";
        public const string ENTIDAD_INEXISTENTE_TEXTO_FORMATO = "{0} inexistente ({1}).";
        public const string ENTIDAD_BORRADA_TEXTO_FORMATO = "{0} se encontraba borrad{1} con anterioridad ({2}).";
        public const string RANGO_FECHAS_NO_VALIDO_TEXTO_FORMATO = "El rango de fechas no es válido ({0} - {1}).";
        public const string RANGO_FECHAS_CANT_MAX_MESES_TEXTO_FORMATO = "El rango de fechas debe incluir hasta {0} meses como máximo.";
        public const string FORMATO_NO_VALIDO_TEXTO_FORMATO = "{0} no tiene el formato válido.";
        public const string FORMATO_EMAIL_NO_VALIDO_TEXTO_FORMATO = "{0} no es un correo válido.";
        public const string FORMATO_DEBE_SER_VACIO = "{0} debe ser vacío.";
        public const string REQUERIDO_LISTA_FORMATO_DEBE_SER_VACIO = "{0} debe contener al menos un elemento válido.";
        public const string IP_NO_VALIDA_TEXTO_FORMATO = "{0} no es una IP válida";
        public const string NUMERO_MAYOR_A_TEXTO_FORMATO = "{0} debe ser un número mayor a {1}.";
        public const string NUMERO_MENOR_A_TEXTO_FORMATO = "{0} debe ser un número menor a {1}.";
        public const string NUMERO_RANGO_TEXTO_FORMATO = "{0} debe ser un número entre {1} y {2}.";
        public const string FECHA_MAYOR_A_TEXTO_FORMATO = "{0} debe ser una {1} posterior a {2}.";
        public const string FECHA_MENOR_A_TEXTO_FORMATO = "{0} debe ser una {1} anterior a {2}.";
        public const string FECHA_RANGO_TEXTO_FORMATO = "{0} debe ser una {1} entre {2} y {3}.";
        public const string RANGO_TIEMPO_NO_VALIDO_TEXTO_FORMATO = "{0} no es un rango de tiempo válido ({1} - {2}).";
        public const string RANGO_TIEMPO_MIN_TEXTO_FORMATO = "{0} debe ser un rango de tiempo de {1} como mínimo.";
        public const string RANGO_TIEMPO_MAX_TEXTO_FORMATO = "{0} debe ser un rango de tiempo de {1} como máximo.";
        public const string RANGO_TIEMPO_RANGO_TEXTO_FORMATO = "{0} debe ser un rango de tiempo entre {1} y {2}.";
        public const string RANGO_TIEMPO_RANGO_EXACTO_TEXTO_FORMATO = "{0} debe ser un rango de tiempo de {1}.";

        public static void Validar<TObjeto>(TObjeto objeto,
            string mensajeEncabezado = null)
            where TObjeto : class
        {
            Validar<TObjeto, ExcepcionMensajeAlUsuario>(objeto, mensajeEncabezado: mensajeEncabezado);
        }

        public static void Validar<TObjeto, TException>(TObjeto objeto,
            string mensajeEncabezado = null)
            where TObjeto : class
            where TException : Exception
        {
            var errores = ObtenerValidationResults(objeto);
            if (errores.Any())
            {
                throw ReflectionHelper.CrearInstancia<TException>($"{(!String.IsNullOrWhiteSpace(mensajeEncabezado) ? $"{mensajeEncabezado}{Environment.NewLine}" : string.Empty)}{Formateador.Lista(errores, separador: Environment.NewLine)}");
            }
        }

        public static IEnumerable<ValidationResult> ObtenerValidationResults<T>(T objeto)
            where T : class
        {
            var res = new List<ValidationResult>();
            Validator.TryValidateObject(objeto, new ValidationContext(objeto), res, true);
            return res;
        }

        public static void ValidarArgumentRequeridoYThrow(object valor, string nombre)
        {
            ValidarRequeridoYThrow<ArgumentNullException>(valor, nombre);
        }

        public static void ValidarArgumentListaRequeridaYThrow<TEntidad>(IEnumerable<TEntidad> lista, string nombre,
            bool validarNoVacia = true,
            Func<TEntidad, bool> validacionItem = null)
        {
            ValidarListaRequeridaYThrow<ArgumentNullException, TEntidad>(lista, nombre,
                validarNoVacia: validarNoVacia,
                validacionItem: validacionItem);
        }

        public static void ValidarRequeridoYThrow<TException>(object valor, string mensaje)
            where TException : Exception
        {
            if (valor == null
                || (valor is string && String.IsNullOrWhiteSpace((string)valor)))
            {
                throw ReflectionHelper.CrearInstancia<TException>(mensaje);
            }
        }

        public static void ValidarListaRequeridaYThrow<TException, TEntidad>(IEnumerable<TEntidad> lista, string mensaje,
            bool validarNoVacia = true,
            Func<TEntidad, bool> validacionItem = null)
            where TException : Exception
        {
            validacionItem = validacionItem ?? (e => true);

            if (lista == null
                || (validarNoVacia && !lista.Any(i => validacionItem(i))))
            {
                throw ReflectionHelper.CrearInstancia<TException>(mensaje);
            }
        }

        public static void ValidarEntidadExistenteYThrow(object entidad, object id, string nombreEntidad)
        {
            if (entidad == null)
            {
                throw new Exception(String.Format(ENTIDAD_INEXISTENTE_TEXTO_FORMATO, nombreEntidad, id));
            }
        }

        public static bool ValidarArchivo(Archivo archivo, bool requerido,
            out string mensaje,
            string descripcion = null,
            string extensiones = null)
        {
            IEnumerable<string> extensionesLista =
                !string.IsNullOrWhiteSpace(extensiones)
                ? ObtenerListaExtensiones(extensiones)
                : null;

            return ValidarArchivo(archivo, requerido,
                out mensaje,
                descripcion, extensionesLista);
        }

        public static bool ValidarArchivoYAgregarAListaErrores(Archivo archivo, bool requerido, 
            List<string> errores,
            string descripcion = null,
            string extensiones = null)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarArchivo(archivo, requerido,
                        out string mensajeError,
                        descripcion: descripcion,
                        extensiones: extensiones);

                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarArchivo(Archivo archivo, bool requerido,
            out string mensaje,
            string descripcion = null,
            IEnumerable<string> extensiones = null)
        {
            mensaje = null;

            descripcion = $"Archivo{(!String.IsNullOrWhiteSpace(descripcion) ? $" {descripcion}" : "")}";

            if (archivo == null || archivo.Contenido == null || archivo.Contenido.Length == 0)
            {
                if (requerido)
                {
                    mensaje = Validador.MensajeRequerido(descripcion);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (extensiones != null)
            {
                return ValidarExtensionDeArchivo(archivo.Nombre, extensiones,
                    out mensaje);
            }

            return true;
        }

        public static bool ValidarArchivoYAgregarAListaErrores(Archivo archivo, bool requerido, 
            List<string> errores,
            string descripcion = null,
            IEnumerable<string> extensiones = null)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarArchivo(archivo, requerido,
                        out string mensajeError,
                        descripcion: descripcion,
                        extensiones: extensiones);

                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarExtensionDeArchivo(string nombreArchivo, string extensiones,
            out string mensaje)
        {
            return ValidarExtensionDeArchivo(nombreArchivo, ObtenerListaExtensiones(extensiones), out mensaje);
        }

        public static bool ValidarExtensionDeArchivoYAgregarAListaErrores(string nombreArchivo, string extensiones, 
            List<string> errores)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarExtensionDeArchivo(nombreArchivo, extensiones,
                        out string mensajeError);

                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarExtensionDeArchivo(string nombreArchivo, IEnumerable<string> extensiones,
            out string mensaje)
        {
            if (!EsExtensionDeArchivoValida(nombreArchivo, extensiones))
            {
                mensaje = String.Format("El archivo '{0}' no es válido. Extensiones válidas: {1}.", Path.GetFileName(nombreArchivo), String.Join(", ", extensiones));
                return false;
            }

            mensaje = null;
            return true;
        }

        public static bool ValidarExtensionDeArchivoYAgregarAListaErrores(string nombreArchivo, IEnumerable<string> extensiones, 
            List<string> errores)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarExtensionDeArchivo(nombreArchivo, extensiones,
                        out string mensajeError);

                    return mensajeError;
                },
                errores);
        }

        public static bool EsExtensionDeArchivoValida(string nombreArchivo, string extensiones)
        {
            return EsExtensionDeArchivoValida(nombreArchivo, ObtenerListaExtensiones(extensiones));
        }

        public static bool EsExtensionDeArchivoValida(string nombreArchivo, IEnumerable<string> extensiones)
        {
            if (extensiones == null)
            {
                throw new ArgumentNullException("extensiones");
            }

            return extensiones.Contains(Path.GetExtension(nombreArchivo).ToLower());
        }

        private static string[] ObtenerListaExtensiones(string extensiones)
        {
            if (extensiones == null)
            {
                throw new ArgumentNullException("extensiones");
            }

            return extensiones.Split(',');
        }

        public static bool ValidarString(string valor, string nombre, bool requerido,
            out string mensajeError,
            int? tamañoMaximo = null, int? tamañoExacto = null, string regEx = null)
        {
            if (String.IsNullOrWhiteSpace(valor))
            {
                if (requerido)
                {
                    mensajeError = MensajeRequerido(nombre);
                    return false;
                }
            }
            else
            {
                if (tamañoExacto.HasValue && valor.Length != tamañoExacto.Value)
                {
                    mensajeError = String.Format(TAMAÑO_EXACTO_TEXTO_FORMATO, nombre, tamañoExacto);
                    return false;
                }
                else if (tamañoMaximo.HasValue && valor.Length > tamañoMaximo.Value)
                {
                    mensajeError = MensajeTextoTamañoMaximo(nombre, tamañoMaximo.Value);
                    return false;
                }

                if (!String.IsNullOrWhiteSpace(regEx))
                {
                    if (!Regex.IsMatch(valor, regEx))
                    {
                        mensajeError = String.Format(FORMATO_NO_VALIDO_TEXTO_FORMATO, nombre);
                        return false;
                    }
                }
            }

            mensajeError = null;
            return true;
        }

        public static bool ValidarStringYAgregarAListaErrores(string valor, string nombre, bool requerido,
            List<string> errores,
            int? tamañoMaximo = null, int? tamañoExacto = null, string regEx = null)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarString(valor, nombre, requerido,
                        out string mensajeError,
                        tamañoMaximo: tamañoMaximo,
                        tamañoExacto: tamañoExacto,
                        regEx: regEx);

                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarRangoFechas(DateTime fechaDesde, DateTime fechaHasta,
            out string mensajeError,
            int? cantMesesMax = null)
        {
            if (fechaDesde > fechaHasta)
            {
                mensajeError = String.Format(RANGO_FECHAS_NO_VALIDO_TEXTO_FORMATO, Formateador.Fecha(fechaDesde), Formateador.Fecha(fechaHasta));
                return false;
            }
            else
            {
                if (cantMesesMax.HasValue)
                {
                    var cantMeses = new MesYAño(fechaDesde)
                        .CalcularDiferenciaEnMeses(new MesYAño(fechaHasta)) + 1;

                    if (cantMeses > cantMesesMax)
                    {
                        mensajeError = String.Format(RANGO_FECHAS_CANT_MAX_MESES_TEXTO_FORMATO, cantMesesMax);
                        return false;
                    }
                }
            }

            mensajeError = null;
            return true;
        }

        public static bool ValidarRangoFechasYAgregarAListaErrores(DateTime fechaDesde, DateTime fechaHasta,
            List<string> errores,
            int? cantMesesMax = null)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarRangoFechas(fechaDesde, fechaHasta, 
                        out string mensajeError, 
                        cantMesesMax: cantMesesMax);

                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarEmail(string email, string nombre, bool requerido,
            out string mensajeError)
        {
            if (!ValidarString(email, nombre, requerido, 
                out mensajeError))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return true;
            }

            try
            {
                new MailAddress(email);

                mensajeError = null;
                return true;
            }
            catch (Exception)
            {
                mensajeError = String.Format(FORMATO_EMAIL_NO_VALIDO_TEXTO_FORMATO, nombre);
                return false;
            }
        }

        public static bool ValidarEmailYAgregarAListaErrores(string email, string nombre, bool requerido,
            List<string> errores)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarEmail(email, nombre, requerido,
                        out string mensajeError);

                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarIP(string ipAddress, string nombre,
            out string mensajeError)
        {
            IPAddress oIPAddress;
            if (!IPAddress.TryParse(ipAddress, out oIPAddress)
                || !String.Equals(ipAddress, oIPAddress.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                mensajeError = String.Format(IP_NO_VALIDA_TEXTO_FORMATO, nombre);
                return false;
            }

            mensajeError = null;
            return true;
        }

        public static bool ValidarIPYAgregarAListaErrores(string ipAddress, string nombre,
            List<string> errores)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarIP(ipAddress, nombre,
                        out string mensajeError);

                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarNumeroYAgregarAListaErrores(decimal? valor, string nombre, bool requerido,
            List<string> errores,
            decimal? valorMinimo = null, decimal? valorMaximo = null,
            int cantDecimales = 2)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarNumero(valor, nombre, requerido,
                        out string mensajeError,
                        valorMinimo: valorMinimo, valorMaximo: valorMaximo,
                        cantDecimales: cantDecimales);

                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarNumero(decimal? valor, string nombre, bool requerido,
            out string mensajeError,
            decimal? valorMinimo = null, decimal? valorMaximo = null,
            int cantDecimales = 2)
        {
            if (!valor.HasValue)
            {
                if (requerido)
                {
                    mensajeError = String.Format(REQUERIDO_TEXTO_FORMATO, nombre);
                    return false;
                }
            }
            else
            {
                if (valorMinimo.HasValue && valorMaximo.HasValue)
                {
                    if (valor < valorMinimo
                        || valor > valorMaximo)
                    {
                        mensajeError = String.Format(NUMERO_RANGO_TEXTO_FORMATO, nombre, Formateador.Numero(valorMinimo, cantDecimales: cantDecimales), Formateador.Numero(valorMaximo, cantDecimales: cantDecimales));
                        return false;
                    }
                }
                else if (valorMinimo.HasValue)
                {
                    if (valor < valorMinimo)
                    {
                        mensajeError = String.Format(NUMERO_MAYOR_A_TEXTO_FORMATO, nombre, Formateador.Numero(valorMinimo, cantDecimales: cantDecimales));
                        return false;
                    }
                }
                else if (valorMaximo.HasValue)
                {
                    if (valor > valorMaximo)
                    {
                        mensajeError = String.Format(NUMERO_MENOR_A_TEXTO_FORMATO, nombre, Formateador.Numero(valorMaximo, cantDecimales: cantDecimales));
                        return false;
                    }
                }
            }

            mensajeError = null;
            return true;
        }

        public static bool ValidarNumeroYAgregarAListaErrores(int? valor, string nombre, bool requerido,
            List<string> errores,
            int? valorMinimo = null, int? valorMaximo = null)
        {
            return ValidarNumeroYAgregarAListaErrores((decimal?)valor, nombre, requerido,
                errores,
                valorMinimo: valorMinimo, valorMaximo: valorMaximo);
        }

        public static bool ValidarNumero(int? valor, string nombre, bool requerido,
            out string mensajeError,
            int? valorMinimo = null, int? valorMaximo = null)
        {
            return ValidarNumero((decimal?)valor, nombre, requerido,
                out mensajeError,
                valorMinimo: valorMinimo, valorMaximo: valorMaximo);
        }

        public static bool ValidarNumeroYAgregarAListaErrores(short? valor, string nombre, bool requerido,
            List<string> errores,
            short? valorMinimo = null, short? valorMaximo = null)
        {
            return ValidarNumeroYAgregarAListaErrores((decimal?)valor, nombre, requerido,
                errores,
                valorMinimo: valorMinimo, valorMaximo: valorMaximo);
        }

        public static bool ValidarNumero(short? valor, string nombre, bool requerido,
            out string mensajeError,
            short? valorMinimo = null, short? valorMaximo = null)
        {
            return ValidarNumero((decimal?)valor, nombre, requerido,
                out mensajeError,
                valorMinimo: valorMinimo, valorMaximo: valorMaximo);
        }

        public static bool ValidarNumeroYAgregarAListaErrores(long? valor, string nombre, bool requerido,
            List<string> errores,
            long? valorMinimo = null, long? valorMaximo = null)
        {
            return ValidarNumeroYAgregarAListaErrores((decimal?)valor, nombre, requerido,
                errores,
                valorMinimo: valorMinimo, valorMaximo: valorMaximo);
        }

        public static bool ValidarNumero(long? valor, string nombre, bool requerido,
            out string mensajeError,
            long? valorMinimo = null, long? valorMaximo = null)
        {
            return ValidarNumero((decimal?)valor, nombre, requerido,
                out mensajeError,
                valorMinimo: valorMinimo, valorMaximo: valorMaximo);
        }

        public static bool ValidarNumeroYAgregarAListaErrores(double? valor, string nombre, bool requerido,
            List<string> errores,
            double? valorMinimo = null, double? valorMaximo = null,
            int cantDecimales = 2)
        {
            return ValidarNumeroYAgregarAListaErrores((decimal?)valor, nombre, requerido,
                errores,
                valorMinimo: (decimal?)valorMinimo, valorMaximo: (decimal?)valorMaximo,
                cantDecimales: cantDecimales);
        }

        public static bool ValidarNumero(double? valor, string nombre, bool requerido,
            out string mensajeError,
            double? valorMinimo = null, double? valorMaximo = null,
            int cantDecimales = 2)
        {
            return ValidarNumero((decimal?)valor, nombre, requerido,
                out mensajeError,
                valorMinimo: (decimal?)valorMinimo, valorMaximo: (decimal?)valorMaximo,
                cantDecimales: cantDecimales);
        }

        public static bool ValidarFecha(
            DateTime? fecha, string nombre, bool requerido,
            out string mensajeError,
            DateTime? fechaMinima = null, DateTime? fechaMaxima = null,
            bool incluirHora = false)
        {
            if (!fecha.HasValue)
            {
                if (requerido)
                {
                    mensajeError = String.Format(REQUERIDO_TEXTO_FORMATO, nombre);
                    return false;
                }
            }
            else
            {
                string textoFecha = $"Fecha{(incluirHora ? "/Hora" : string.Empty)}";

                Func<DateTime?, string> formato;
                if (incluirHora)
                {
                    formato = (f) => Formateador.FechaYHora(f);
                }
                else
                {
                    fecha = fecha.Value.Date;
                    fechaMinima = fechaMinima?.Date;
                    fechaMaxima = fechaMaxima?.Date;

                    formato = (f) => Formateador.Fecha(f);
                }

                if (fechaMinima.HasValue && fechaMaxima.HasValue)
                {
                    if (fecha < fechaMinima
                        || fecha > fechaMaxima)
                    {
                        mensajeError = String.Format(FECHA_RANGO_TEXTO_FORMATO, nombre, textoFecha, formato(fechaMinima), formato(fechaMaxima));
                        return false;
                    }
                }
                else if (fechaMinima.HasValue)
                {
                    if (fecha < fechaMinima)
                    {
                        mensajeError = String.Format(FECHA_MAYOR_A_TEXTO_FORMATO, nombre, textoFecha, formato(fechaMinima));
                        return false;
                    }
                }
                else if (fechaMaxima.HasValue)
                {
                    if (fecha > fechaMaxima)
                    {
                        mensajeError = String.Format(FECHA_MENOR_A_TEXTO_FORMATO, nombre, textoFecha, formato(fechaMaxima));
                        return false;
                    }
                }
            }

            mensajeError = null;
            return true;
        }

        public static bool ValidarFechaYAgregarAListaErrores(
            DateTime? fecha, string nombre, bool requerido,
            List<string> errores,
            DateTime? fechaMinima = null, DateTime? fechaMaxima = null,
            bool incluirHora = false)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarFecha(fecha, nombre, requerido,
                        out string mensajeError,
                        fechaMinima: fechaMinima, fechaMaxima: fechaMaxima,
                        incluirHora: incluirHora);
                    
                    return mensajeError;
                },
                errores);
        }

        public static bool ValidarRangoTiempo(
            TimeSpan desde, TimeSpan hasta, string nombre,
            out string mensajeError,
            TimeSpan? tiempoMinimo = null, TimeSpan? tiempoMaximo = null)
        {
            if (desde > hasta)
            {
                mensajeError = String.Format(RANGO_TIEMPO_NO_VALIDO_TEXTO_FORMATO, nombre, Formateador.Hora(desde), Formateador.Hora(hasta));
                return false;
            }

            var diffTiempo = hasta - desde;
            if (tiempoMinimo.HasValue && tiempoMaximo.HasValue)
            {
                if (diffTiempo < tiempoMinimo
                    || diffTiempo > tiempoMaximo)
                {
                    string formato =
                        tiempoMinimo == tiempoMaximo
                        ? RANGO_TIEMPO_RANGO_EXACTO_TEXTO_FORMATO
                        : RANGO_TIEMPO_RANGO_TEXTO_FORMATO;

                    mensajeError = String.Format(formato, nombre, Formateador.Tiempo(tiempoMinimo.Value), Formateador.Tiempo(tiempoMaximo.Value));
                    return false;
                }
            }
            else if (tiempoMinimo.HasValue)
            {
                if (diffTiempo < tiempoMinimo.Value)
                {
                    mensajeError = String.Format(RANGO_TIEMPO_MIN_TEXTO_FORMATO, nombre, Formateador.Tiempo(tiempoMinimo.Value));
                    return false;
                }
            }
            else if (tiempoMaximo.HasValue)
            {
                if (diffTiempo > tiempoMaximo.Value)
                {
                    mensajeError = String.Format(RANGO_TIEMPO_MAX_TEXTO_FORMATO, nombre, Formateador.Tiempo(tiempoMaximo.Value));
                    return false;
                }
            }

            mensajeError = null;
            return true;
        }

        public static bool ValidarRangoTiempoYAgregarAListaErrores(
            TimeSpan desde, TimeSpan hasta, string nombre,
            List<string> errores,
            TimeSpan? tiempoMinimo = null, TimeSpan? tiempoMaximo = null)
        {
            return ValidarYAgregarAListaErrores(
                () =>
                {
                    ValidarRangoTiempo(desde, hasta, nombre,
                        out string mensajeError,
                        tiempoMinimo: tiempoMinimo, tiempoMaximo: tiempoMaximo);

                    return mensajeError;
                },
                errores);
        }

        private static bool ValidarYAgregarAListaErrores(Func<string> validacion, List<string> errores)
        {
            ValidarArgumentListaRequeridaYThrow(errores, nameof(errores), validarNoVacia: false);

            string mensajeError = validacion();
            if (!string.IsNullOrWhiteSpace(mensajeError))
            {
                errores.Add(mensajeError);
                return false;
            }

            return true;
        }

        public static void LanzarExcepcionMensajeAlUsuarioSiExistenErrores(List<string> errores)
        {
            if (errores == null)
            {
                return;
            }

            if (errores.Any())
            {
                throw new ExcepcionMensajeAlUsuario(Formateador.Lista(errores, Environment.NewLine));
            }
        }

        #region Mensajes

        public static string MensajeDebeSerVacio(string nombre)
        {
            return String.Format(FORMATO_DEBE_SER_VACIO, nombre);
        }

        public static string MensajeEntidadInexistente(string entidad, object valorBusqueda)
        {
            return String.Format(ENTIDAD_INEXISTENTE_TEXTO_FORMATO, entidad, Convert.ToString(valorBusqueda));
        }

        public static string MensajeEntidadBorrada(string entidad, object valorBusqueda,
            bool nombreEntidadEsFemenino = false)
        {
            return String.Format(ENTIDAD_BORRADA_TEXTO_FORMATO, entidad, nombreEntidadEsFemenino ? "a" : "o", Convert.ToString(valorBusqueda));
        }

        public static string MensajeRequerido(string nombre)
        {
            return String.Format(REQUERIDO_TEXTO_FORMATO, nombre);
        }

        public static string MensajeTextoTamañoMaximo(string nombre, int tamañoMaximo)
        {
            return String.Format(TAMAÑO_MAXIMO_TEXTO_FORMATO, nombre, tamañoMaximo);
        }

        #endregion
    }
}
