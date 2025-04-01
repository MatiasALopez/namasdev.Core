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

        public static string Dia(DateTime fecha,
            CultureInfo cultura = null)
        {
            return Dia(fecha.DayOfWeek, cultura);
        }

        public static string Dia(int diaSemana,
            CultureInfo cultura = null)
        {
            return Dia((DayOfWeek)diaSemana, cultura);
        }

        public static string Dia(DayOfWeek diaSemana,
            CultureInfo cultura = null)
        {
            return (cultura ?? CultureInfo.CurrentCulture).DateTimeFormat.GetDayName(diaSemana);
        }

        public static string Lista<T>(IEnumerable<T> valores, 
            string separador = ",",
            bool excluirNulosOVacios = false)
        {
            if (excluirNulosOVacios)
            {
                valores = valores.Where(v => !string.IsNullOrWhiteSpace(Convert.ToString(v)));
            }

            return String.Join(separador, valores);
        }

        public static string ListaHoras(IEnumerable<TimeSpan> horas,
            string separador = ",")
        {
            return horas != null
                ? Lista(horas.Select(h => Hora(h)), separador)
                : String.Empty;
        }

        public static string ClaveValor<TKey, TValue>(IDictionary<TKey, TValue> diccionario,
            string separadorClaveValor = " : ",
            string separadorItems = ", ")
        {
            Validador.ValidarArgumentRequeridoYThrow(diccionario, nameof(diccionario));

            return Lista(diccionario.Select(it => $"{it.Key}{separadorClaveValor}{it.Value}"), separador: separadorItems);
        }

        public static string Texto(IEnumerable<string> lineas)
        {
            return lineas != null
                ? String.Join(Environment.NewLine, lineas)
                : String.Empty;
        }

        public static string TextoEnCantLineas(string texto, int cantLineas,
            int? cantMaximaCaracteresPorLinea = null)
        {
            int textoCantLineas = 1;
            if (cantMaximaCaracteresPorLinea.HasValue)
            {
                textoCantLineas = (int)System.Math.Ceiling((decimal)texto.Length / cantMaximaCaracteresPorLinea.Value);
            }
            return $"{texto}{String.Join("", Enumerable.Repeat(Environment.NewLine, cantLineas > textoCantLineas ? cantLineas - textoCantLineas : 0))}";
        }

        public static string NombreArchivoDesdeUrl(string url)
        {
            return !String.IsNullOrWhiteSpace(url)
                ? WebUtility.UrlDecode(Path.GetFileName(url))
                : String.Empty;
        }

        public static string Numero(int numero,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? numero.ToString($"N0", cultura)
                : numero.ToString($"N0");
        }

        public static string Numero(int? numero,
            CultureInfo cultura = null)
        {
            return numero.HasValue
                ? Numero(numero.Value, cultura)
                : String.Empty;
        }

        public static string Numero(long numero,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? numero.ToString($"N0", cultura)
                : numero.ToString($"N0");
        }

        public static string Numero(long? numero,
            CultureInfo cultura = null)
        {
            return numero.HasValue
                ? Numero(numero.Value, cultura)
                : String.Empty;
        }

        public static string Numero(decimal numero, 
            int cantDecimales = 2,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? numero.ToString($"N{cantDecimales}", cultura)
                : numero.ToString($"N{cantDecimales}");
        }

        public static string Numero(decimal? numero,
            int cantDecimales = 2,
            CultureInfo cultura = null)
        {
            return numero.HasValue 
                ? Numero(numero.Value, cantDecimales) 
                : String.Empty;
        }

        public static string Numero(double numero,
            int cantDecimales = 2,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? numero.ToString($"N{cantDecimales}", cultura)
                : numero.ToString($"N{cantDecimales}");
        }

        public static string Numero(double? numero,
            int cantDecimales = 2,
            CultureInfo cultura = null)
        {
            return numero.HasValue
                ? Numero(numero.Value, cantDecimales)
                : String.Empty;
        }

        public static string ListaEmails(IEnumerable<string> emails)
        {
            return Lista(emails);
        }

        public static string Moneda(decimal? valor,
            int cantDecimales = 2,
            CultureInfo cultura = null)
        {
            return valor.HasValue
                ? Moneda(valor.Value, cantDecimales: cantDecimales, cultura: cultura)
                : String.Empty;
        }

        public static string Moneda(decimal valor,
            int cantDecimales = 0,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? valor.ToString($"C{cantDecimales}", cultura)
                : valor.ToString($"C{cantDecimales}");
        }

        public static string Porcentaje(double valor,
            int cantDecimales = 0,
            CultureInfo cultura = null)
        {
            return cultura != null
                ? valor.ToString($"P{cantDecimales}", cultura)
                : valor.ToString($"P{cantDecimales}");
        }

        public static string Porcentaje(double? valor,
            int cantDecimales = 0,
            CultureInfo cultura = null)
        {
            return valor.HasValue
                ? Porcentaje(valor.Value, cantDecimales, cultura)
                : "";
        }

        public static string Porcentaje(decimal valor,
           int cantDecimales = 0,
           CultureInfo cultura = null)
        {
            return cultura != null
                ? valor.ToString($"P{cantDecimales}", cultura)
                : valor.ToString($"P{cantDecimales}");
        }

        public static string Porcentaje(decimal? valor,
            int cantDecimales = 0,
            CultureInfo cultura = null)
        {
            return valor.HasValue
                ? Porcentaje(valor.Value, cantDecimales, cultura)
                : "";
        }

        public static string Html(string valor)
        {
            return !String.IsNullOrWhiteSpace(valor)
                ? valor.Replace(Environment.NewLine, "<br/>")
                : null;
        }

        public static string SiNo(bool valor,
            bool usarAcento = true)
        {
            return valor ? (usarAcento ? TEXTO_SI_CON_ACENTO : TEXTO_SI_SIN_ACENTO) : "No";
        }

        public static string NombreDiaYFecha(DateTime fecha,
            string separador = " ")
        {
            var diaSemana = Dia((int)fecha.DayOfWeek);
            return $"{diaSemana}{separador}{Fecha(fecha)}";
        }

        public static string FechaTexto(DateTime fecha,
            bool incluirNombreDia = true)
        {
            return fecha.ToString($@"{(incluirNombreDia ? "dddd, " : String.Empty)}dd \de MMMM \de yyyy");
        }

        public static string Fecha(DateTime? fecha,
             string textoVacio = "",
             string separador = "/")
        {
            return fecha.HasValue
                ? Fecha(fecha.Value, separador)
                : textoVacio;
        }

        public static string Fecha(DateTime fecha,
            string separador = "/")
        {
            return fecha.ToString($"dd{separador}MM{separador}yyyy");
        }

        public static string FechaInvertido(DateTime? fecha,
            string separador = "",
            string textoVacio = "")
        {
            return fecha.HasValue
                ? FechaInvertido(fecha.Value, separador)
                : textoVacio;
        }

        public static string FechaInvertido(DateTime fecha,
            string separador = "")
        {
            return $"{fecha:yyyy}{separador}{fecha:MM}{separador}{fecha:dd}";
        }

        public static string FechaYHora(DateTime? fechaHora,
            string separadorFechaPartes = "/", string separadorHoraPartes = ":", string separadorFechaHora = " ",
            bool incluirSegundos = false,
            string textoVacio = "")
        {
            return fechaHora.HasValue
                ? FechaYHora(fechaHora.Value, separadorFechaPartes, separadorHoraPartes, separadorFechaHora, incluirSegundos)
                : textoVacio;
        }

        public static string FechaYHora(DateTime fechaHora,
            string separadorFechaPartes = "/", string separadorHoraPartes = ":", string separadorFechaHora = " ",
            bool incluirSegundos = false)
        {
            return fechaHora.ToString($"dd{separadorFechaPartes}MM{separadorFechaPartes}yyyy{separadorFechaHora}HH{separadorHoraPartes}mm{(incluirSegundos ? $"{separadorHoraPartes}ss" : String.Empty)}");
        }

        public static string FechaYHoraInvertido(DateTime fechaHora,
            string separadorFechaPartes = "-", string separadorHoraPartes = ":", string separadorFechaHora = " ",
            bool incluirSegundos = false)
        {
            return fechaHora.ToString($"yyyy{separadorFechaPartes}MM{separadorFechaPartes}dd{separadorFechaHora}HH{separadorHoraPartes}mm{(incluirSegundos ? $"{separadorHoraPartes}ss" : String.Empty)}");
        }

        public static string Hora(DateTime? fechaHora,
            string textoVacio = "")
        {
            return fechaHora.HasValue
                ? Hora(fechaHora.Value)
                : textoVacio;
        }

        public static string Hora(DateTime fechaHora)
        {
            return fechaHora.ToString("HH:mm");
        }

        public static string Hora(TimeSpan? hora,
            string textoVacio = "")
        {
            return hora.HasValue
                ? Hora(hora.Value)
                : textoVacio;
        }

        public static string Hora(TimeSpan hora)
        {
            return hora.ToString("hh\\:mm");
        }

        public static string FechaYHoraStandard(DateTime fechaHora)
        {
            return fechaHora.ToString("s");
        }

        public static string Tiempo(TimeSpan? tiempo,
            string textoVacio = "")
        {
            return tiempo.HasValue
                ? Tiempo(tiempo)
                : textoVacio;
        }

        public static string Tiempo(TimeSpan tiempo)
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
            return Lista(partes, " ");
        }
    }
}
