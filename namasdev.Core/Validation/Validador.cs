﻿using System;
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

        public static void ValidarArgumentRequeridoYThrow(object valor, string nombre)
        {
            ValidarRequeridoYThrow<ArgumentNullException>(valor, nombre);
        }

        public static void ValidarRequeridoYThrow<TException>(object valor, string mensaje)
            where TException : Exception
        {
            if (String.IsNullOrWhiteSpace(Convert.ToString(valor)))
            {
                throw ReflectionHelper.CrearInstancia<TException>(mensaje);
            }
        }

        public static IEnumerable<ValidationResult> ObtenerValidationResults<T>(T objeto)
            where T : class
        {
            var res = new List<ValidationResult>();
            Validator.TryValidateObject(objeto, new ValidationContext(objeto), res, true);
            return res;
        }

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
                throw ReflectionHelper.CrearInstancia<TException>($"{(!String.IsNullOrWhiteSpace(mensajeEncabezado) ? $"{mensajeEncabezado}{Environment.NewLine}" : string.Empty)}{Formateador.FormatoLista(errores, separador: Environment.NewLine)}");
            }
        }

        public static bool ValidarArchivoYAgregarAListaErrores(Archivo archivo, bool requerido, List<string> errores,
            string descripcion = null,
            IEnumerable<string> extensiones = null)
        {
            if (errores == null)
            {
                throw new ArgumentNullException(nameof(errores));
            }

            string mensajeError;
            if (!ValidarArchivo(archivo, requerido,
                out mensajeError,
                descripcion, extensiones))
            {
                errores.Add(mensajeError);
                return false;
            }

            return true;
        }

        public static bool ValidarArchivoYAgregarAListaErrores(Archivo archivo, bool requerido, List<string> errores,
            string descripcion = null,
            string extensiones = null)
        {
            if (errores == null)
            {
                throw new ArgumentNullException(nameof(errores));
            }

            string mensajeError;
            if (!ValidarArchivo(archivo, requerido,
                out mensajeError,
                descripcion, extensiones))
            {
                errores.Add(mensajeError);
                return false;
            }

            return true;
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

        public static bool ValidarExtensionDeArchivo(string nombreArchivo, string extensiones,
            out string mensaje)
        {
            return ValidarExtensionDeArchivo(nombreArchivo, ObtenerListaExtensiones(extensiones), out mensaje);
        }

        public static bool ValidarExtensionDeArchivo(string nombreArchivo, IEnumerable<string> extensiones,
            out string mensaje)
        {
            mensaje = null;

            if (!EsExtensionDeArchivoValida(nombreArchivo, extensiones))
            {
                mensaje = String.Format("El archivo '{0}' no es válido. Extensiones válidas: {1}.", Path.GetFileName(nombreArchivo), String.Join(", ", extensiones));
                return false;
            }

            return true;
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
        
        public static string MensajeDebeSerVacio(string nombre)
        {
            return String.Format(FORMATO_DEBE_SER_VACIO, nombre);
        }

        public static void ValidarEntidadExistente(object entidad, object id, string nombreEntidad)
        {
            if (entidad == null)
            {
                throw new Exception(String.Format(ENTIDAD_INEXISTENTE_TEXTO_FORMATO, nombreEntidad, id));
            }
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

        public static bool ValidarStringYAgregarAListaErrores(string valor, string nombre, bool requerido,
            List<string> errores,
            int? tamañoMaximo = null, int? tamañoExacto = null, string regEx = null)
        {
            if (errores == null)
            {
                throw new ArgumentNullException(nameof(errores));
            }

            string mensajeError;
            if (!ValidarString(valor, nombre, requerido,
                out mensajeError,
                tamañoMaximo: tamañoMaximo, tamañoExacto: tamañoExacto, regEx: regEx))
            {
                errores.Add(mensajeError);
                return false;
            }

            return true;
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

        public static bool ValidarRangoFechas(DateTime fechaDesde, DateTime fechaHasta,
            out string mensajeError,
            int? cantMesesMax = null)
        {
            if (fechaDesde > fechaHasta)
            {
                mensajeError = String.Format(RANGO_FECHAS_NO_VALIDO_TEXTO_FORMATO, Formateador.FormatoFecha(fechaDesde), Formateador.FormatoFecha(fechaHasta));
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

        public static bool ValidarRangoFechasMaxMeses(DateTime fechaDesde, DateTime fechaHasta, int cantMesesMax,
            out string mensajeError)
        {
            var cantMeses = new MesYAño(fechaDesde)
                .CalcularDiferenciaEnMeses(new MesYAño(fechaHasta)) + 1;

            if (cantMeses > cantMesesMax)
            {
                mensajeError = String.Format(RANGO_FECHAS_CANT_MAX_MESES_TEXTO_FORMATO, cantMesesMax);
                return false;
            }

            mensajeError = null;
            return true;
        }

        public static bool ValidarEmail(string valor, string nombre,
            out string mensajeError)
        {
            try
            {
                new MailAddress(valor);

                mensajeError = null;
                return true;
            }
            catch (Exception)
            {
                mensajeError = String.Format(FORMATO_EMAIL_NO_VALIDO_TEXTO_FORMATO, nombre);
                return false;
            }
        }

        public static bool EsIPValida(string ipAddress)
        {
            IPAddress oIPAddress;
            return IPAddress.TryParse(ipAddress, out oIPAddress)
                && String.Equals(ipAddress, oIPAddress.ToString(), StringComparison.CurrentCultureIgnoreCase);
        }

        public static void LanzarExcepcionMensajeAlUsuarioSiExistenErrores(List<string> errores)
        {
            if (errores == null)
            {
                return;
            }

            if (errores.Any())
            {
                throw new ExcepcionMensajeAlUsuario(Formateador.FormatoLista(errores, Environment.NewLine));
            }
        }
    }
}