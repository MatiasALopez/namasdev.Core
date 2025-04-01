using System;
using System.Collections.Generic;
using System.Linq;

using namasdev.Core.Exceptions;

namespace namasdev.Core.Types
{
    public static class FechaHoraHelper
    {
        public static DateTime? CrearDesdeFechaYHora(DateTime? fecha, TimeSpan? hora)
        {
            return fecha.HasValue
                ? fecha.Value.Add(hora ?? TimeSpan.Zero)
                : (DateTime?)null;
        }

        public static DateTime CrearDesdeFechaYHora(DateTime fecha, TimeSpan hora)
        {
            return fecha.Date.Add(hora);
        }

        public static DateTime CrearUtcDesdeUnixTime(long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
        }

        public static long CrearUnixTimeFechaHora(DateTime fechaHora)
        {
            return (long)(fechaHora.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public static DateTime Min(DateTime fecha1, DateTime fecha2)
        {
            return fecha1 < fecha2
                ? fecha1
                : fecha2;
        }

        public static DateTime Min(DateTime? fecha1, DateTime? fecha2, DateTime fechaDefault)
        {
            return Min(fecha1 ?? fechaDefault, fecha2 ?? fechaDefault);
        }

        public static DateTime Max(DateTime fecha1, DateTime fecha2)
        {
            return fecha1 > fecha2
                ? fecha1
                : fecha2;
        }

        public static IEnumerable<DateTime> ObtenerFechasDeRango(DateTime desde, DateTime hasta)
        {
            ValidarRangoYThrow(desde, hasta);

            var res = new List<DateTime>();

            DateTime fecha = desde;
            while (fecha <= hasta)
            {
                res.Add(fecha);

                fecha = fecha.AddDays(1);
            }

            return res;
        }

        public static int DiferenciaEnDias(DateTime desde, DateTime hasta)
        {
            if (desde > hasta)
            {
                return (int)(desde - hasta.Date).TotalDays;
            }

            return (int)(hasta - desde.Date).TotalDays;
        }

        public static TimeSpan DiferenciaEnHoras(DateTime desde, DateTime hasta)
        {
            ValidarRangoYThrow(desde, hasta);

            return hasta.Subtract(desde);
        }

        public static void ValidarRangoYThrow(DateTime desde, DateTime hasta)
        {
            if (hasta < desde)
            {
                throw new ExcepcionMensajeAlUsuario($"Rango no válido ({Formateador.FechaYHora(desde)} - {Formateador.FechaYHora(hasta)}).");
            }
        }

        public static DateTime ObtenerDiaHabilDeMes(int mes, int año, int diaHabil)
        {
            DateTime fecha = new DateTime(año, mes, 1),
                fechaHabil = DateTime.MinValue;
            int contDias = 0;
            while (fecha.Month == mes)
            {
                if (fecha.IsWorkday())
                {
                    fechaHabil = fecha;
                    contDias++;
                }

                if (contDias == diaHabil)
                {
                    return fecha;
                }

                fecha = fecha.AddDays(1);
            }

            // NOTA (ML): si llega a este punto es porque no encontró el día, en ese caso
            // devuelvo el ultimo dia habil del mes
            return fechaHabil;
        }

        public static bool HoraDentroDeRango(DateTime fechaHora, TimeSpan horaInicio, TimeSpan horaFin)
        {
            return horaInicio < horaFin
                ? fechaHora.TimeOfDay >= horaInicio && fechaHora.TimeOfDay <= horaFin
                : fechaHora.TimeOfDay >= horaInicio || fechaHora.TimeOfDay <= horaFin;
        }

        public static int ContarDias(DateTime desde, DateTime hasta,
            DayOfWeek[] diasDeSemanaAExcluir = null)
        {
            return ContarDias(ObtenerFechasDeRango(desde, hasta), diasDeSemanaAExcluir);
        }

        public static int ContarDias(IEnumerable<DateTime> listadoDias,
            DayOfWeek[] diasDeSemanaAExcluir = null)
        {
            int cant = 0;
            foreach (var fecha in listadoDias)
            {
                if (diasDeSemanaAExcluir == null
                    || !diasDeSemanaAExcluir.Contains(fecha.DayOfWeek))
                {
                    cant++;
                }
            }
            return cant;
        }

        public static int ContarDias(IEnumerable<DateTime> fechas, DateTime desde, DateTime hasta,
            DayOfWeek[] diasDeSemanaAExcluir = null)
        {
            return fechas
                .Count(fecha =>
                    fecha <= hasta
                    && fecha >= desde
                    && !diasDeSemanaAExcluir.Contains(fecha.DayOfWeek));
        }

        public static int ContarDias(List<DateTime> fechas, List<DateTime> listadoDias,
            DayOfWeek[] diasDeSemanaAExcluir = null)
        {
            fechas.RemoveAll(item => !listadoDias.Contains(item));
            return fechas
                .Count(fecha => !diasDeSemanaAExcluir.Contains(fecha.DayOfWeek));
        }
    }
}
