using System;

namespace namasdev.Core.Types
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, 1);
        }

        public static DateTime AddWeeks(this DateTime dateTime, int numberOfWeeks)
        {
            return dateTime.AddDays(numberOfWeeks * 7);
        }

        public static DateTime? ToLocalTime(this DateTime? fechaHora)
        {
            return fechaHora.HasValue ? fechaHora.Value.ToLocalTime() : (DateTime?)null;
        }

        public static DateTime? ToUniversalTime(this DateTime? fechaHora)
        {
            return fechaHora.HasValue ? fechaHora.Value.ToUniversalTime() : (DateTime?)null;
        }

        public static DateTime ChangeKind(this DateTime fechaHora, DateTimeKind nuevoTipo)
        {
            return new DateTime(fechaHora.Ticks, nuevoTipo);
        }

        public static DateTime TruncateToHour(this DateTime fechaHora)
        {
            return new DateTime(fechaHora.Year, fechaHora.Month, fechaHora.Day, fechaHora.Hour, 0, 0);
        }

        public static bool IsWorkday(this DateTime fecha)
        {
            return fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday;
        }

        public static DateTime AddWorkingDays(this DateTime fecha, int valor)
        {
            if (valor == 0)
            {
                return fecha;
            }

            bool sumar = valor > 0;
            int cont = 0;
            while (cont <= valor)
            {
                fecha = fecha.AddDays(sumar ? 1 : -1);

                if (fecha.IsWorkday())
                {
                    cont++;
                }
            }

            return fecha;
        }
    }
}
