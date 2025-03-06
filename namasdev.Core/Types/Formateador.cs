using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

using namasdev.Core.Validation;

namespace namasdev.Core.Types
{
    public class Formateador
    {
        public const string TEXTO_SI_CON_ACENTO = "Sí";
        public const string TEXTO_SI_SIN_ACENTO = "Si";

        public static string FormatoDia(DateTime fecha,
            CultureInfo cultura = null)
        {
            return FormatoDia(fecha.DayOfWeek, cultura);
        }

        public static string FormatoDia(int diaSemana,
            CultureInfo cultura = null)
        {
            return FormatoDia((DayOfWeek)diaSemana, cultura);
        }

        public static string FormatoDia(DayOfWeek diaSemana,
            CultureInfo cultura = null)
        {
            return (cultura ?? CultureInfo.CurrentCulture).DateTimeFormat.GetDayName(diaSemana);
        }

        public static string FormatoLista<T>(IEnumerable<T> valores, 
            string separador = ",",
            bool excluirNulosOVacios = false)
        {
            if (excluirNulosOVacios)
            {
                valores = valores.Where(v => !string.IsNullOrWhiteSpace(Convert.ToString(v)));
            }

            return String.Join(separador, valores);
        }

        public static string FormatoListaHoras(IEnumerable<TimeSpan> horas,
            string separador = ",")
        {
            return horas != null
                ? FormatoLista(horas.Select(h => FormatoHora(h)), separador)
                : String.Empty;
        }

        public static string FormatoClaveValor<TKey, TValue>(IDictionary<TKey, TValue> diccionario,
            string separadorClaveValor = " : ",
            string separadorItems = ", ")
        {
            Validador.ValidarArgumentRequeridoYThrow(diccionario, nameof(diccionario));

            return FormatoLista(diccionario.Select(it => $"{it.Key}{separadorClaveValor}{it.Value}"), separador: separadorItems);
        }

        public static string FormatoTexto(IEnumerable<string> lineas)
        {
            return lineas != null
                ? String.Join(Environment.NewLine, lineas)
                : String.Empty;
        }

        public static string FormatoTextoEnCantLineas(string texto, int cantLineas,
            int? cantMaximaCaracteresPorLinea = null)
        {
            int textoCantLineas = 1;
            if (cantMaximaCaracteresPorLinea.HasValue)
            {
                textoCantLineas = (int)System.Math.Ceiling((decimal)texto.Length / cantMaximaCaracteresPorLinea.Value);
            }
            return $"{texto}{String.Join("", Enumerable.Repeat(Environment.NewLine, cantLineas > textoCantLineas ? cantLineas - textoCantLineas : 0))}";
        }

        public static string FormatoNombreArchivoDesdeUrl(string url)
        {
            return !String.IsNullOrWhiteSpace(url)
                ? WebUtility.UrlDecode(Path.GetFileName(url))
                : String.Empty;
        }

        public static string FormatoNumero(int numero,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? numero.ToString($"N0", cultura)
                : numero.ToString($"N0");
        }

        public static string FormatoNumero(int? numero,
            CultureInfo cultura = null)
        {
            return numero.HasValue
                ? FormatoNumero(numero.Value, cultura)
                : String.Empty;
        }

        public static string FormatoNumero(long numero,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? numero.ToString($"N0", cultura)
                : numero.ToString($"N0");
        }

        public static string FormatoNumero(long? numero,
            CultureInfo cultura = null)
        {
            return numero.HasValue
                ? FormatoNumero(numero.Value, cultura)
                : String.Empty;
        }

        public static string FormatoNumero(decimal numero, 
            int cantDecimales = 2,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? numero.ToString($"N{cantDecimales}", cultura)
                : numero.ToString($"N{cantDecimales}");
        }

        public static string FormatoNumero(decimal? numero,
            int cantDecimales = 2,
            CultureInfo cultura = null)
        {
            return numero.HasValue 
                ? FormatoNumero(numero.Value, cantDecimales) 
                : String.Empty;
        }

        public static string FormatoListaEmails(IEnumerable<string> emails)
        {
            return FormatoLista(emails);
        }

        public static string FormatoMoneda(decimal? valor,
            int cantDecimales = 2,
            CultureInfo cultura = null)
        {
            return valor.HasValue
                ? FormatoMoneda(valor.Value, cantDecimales: cantDecimales, cultura: cultura)
                : String.Empty;
        }

        public static string FormatoMoneda(decimal valor,
            int cantDecimales = 0,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? valor.ToString($"C{cantDecimales}", cultura)
                : valor.ToString($"C{cantDecimales}");
        }

        public static string FormatoHtml(string valor)
        {
            return !String.IsNullOrWhiteSpace(valor)
                ? valor.Replace(Environment.NewLine, "<br/>")
                : null;
        }

        public static string FormatoSiNo(bool valor,
            bool usarAcento = true)
        {
            return valor ? (usarAcento ? TEXTO_SI_CON_ACENTO : TEXTO_SI_SIN_ACENTO) : "No";
        }

        public static string FormatoNombreDiaYFecha(DateTime fecha,
            string separador = " ")
        {
            var diaSemana = FormatoDia((int)fecha.DayOfWeek);
            return $"{diaSemana}{separador}{FormatoFecha(fecha)}";
        }

        public static string FormatoFechaTexto(DateTime fecha,
            bool incluirNombreDia = true)
        {
            return fecha.ToString($@"{(incluirNombreDia ? "dddd, " : String.Empty)}dd \de MMMM \de yyyy");
        }

        public static string FormatoFecha(DateTime? fecha,
             string textoVacio = "",
             string separador = "/")
        {
            return fecha.HasValue
                ? FormatoFecha(fecha.Value, separador)
                : textoVacio;
        }

        public static string FormatoFecha(DateTime fecha,
            string separador = "/")
        {
            return fecha.ToString($"dd{separador}MM{separador}yyyy");
        }

        public static string FormatoFechaInvertido(DateTime? fecha,
            string separador = "",
            string textoVacio = "")
        {
            return fecha.HasValue
                ? FormatoFechaInvertido(fecha.Value, separador)
                : textoVacio;
        }

        public static string FormatoFechaInvertido(DateTime fecha,
            string separador = "")
        {
            return $"{fecha:yyyy}{separador}{fecha:MM}{separador}{fecha:dd}";
        }

        public static string FormatoFechaYHora(DateTime? fechaHora,
            string separadorFechaPartes = "/", string separadorHoraPartes = ":", string separadorFechaHora = " ",
            bool incluirSegundos = false,
            string textoVacio = "")
        {
            return fechaHora.HasValue
                ? FormatoFechaYHora(fechaHora.Value, separadorFechaPartes, separadorHoraPartes, separadorFechaHora, incluirSegundos)
                : textoVacio;
        }

        public static string FormatoFechaYHora(DateTime fechaHora,
            string separadorFechaPartes = "/", string separadorHoraPartes = ":", string separadorFechaHora = " ",
            bool incluirSegundos = false)
        {
            return fechaHora.ToString($"dd{separadorFechaPartes}MM{separadorFechaPartes}yyyy{separadorFechaHora}HH{separadorHoraPartes}mm{(incluirSegundos ? $"{separadorHoraPartes}ss" : String.Empty)}");
        }

        public static string FormatoFechaYHoraInvertido(DateTime fechaHora,
            string separadorFechaPartes = "-", string separadorHoraPartes = ":", string separadorFechaHora = " ",
            bool incluirSegundos = false)
        {
            return fechaHora.ToString($"yyyy{separadorFechaPartes}MM{separadorFechaPartes}dd{separadorFechaHora}HH{separadorHoraPartes}mm{(incluirSegundos ? $"{separadorHoraPartes}ss" : String.Empty)}");
        }

        public static string FormatoHora(DateTime? fechaHora,
            string textoVacio = "")
        {
            return fechaHora.HasValue
                ? FormatoHora(fechaHora.Value)
                : textoVacio;
        }

        public static string FormatoHora(DateTime fechaHora)
        {
            return fechaHora.ToString("HH:mm");
        }

        public static string FormatoHora(TimeSpan? hora,
            string textoVacio = "")
        {
            return hora.HasValue
                ? FormatoHora(hora.Value)
                : textoVacio;
        }

        public static string FormatoHora(TimeSpan hora)
        {
            return hora.ToString("hh\\:mm");
        }

        public static string FormatoFechaYHoraStandard(DateTime fechaHora)
        {
            return fechaHora.ToString("s");
        }

        public static string FormatoTiempo(TimeSpan? tiempo,
            string textoVacio = "")
        {
            return tiempo.HasValue
                ? FormatoTiempo(tiempo)
                : textoVacio;
        }

        public static string FormatoTiempo(TimeSpan tiempo)
        {
            var partes = new List<string>();
            if (tiempo.Hours > 0)
            {
                partes.Add($"{tiempo:%h}h");
            }
            if (tiempo.Minutes > 0)
            {
                partes.Add($"{tiempo:mm}m");
            }
            if (tiempo.Seconds > 0)
            {
                partes.Add($"{tiempo:ss}s");
            }
            return FormatoLista(partes, " ");
        }

        public static string FormatoPorcentaje(double valor, 
            int cantDecimales = 0)
        {
            return valor.ToString($"P{cantDecimales}");
        }
    }
}
