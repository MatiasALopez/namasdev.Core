using System;
using System.Collections.Generic;

namespace namasdev.Core.Types
{
    public class RangoFechasHoras
    {
        public RangoFechasHoras(DateTime desde, DateTime hasta)
        {
            FechaHoraHelper.ValidarRangoYThrow(desde, hasta);

            Desde = desde;
            Hasta = hasta;
        }

        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }

        public IEnumerable<DateTime> ObtenerFechas()
        {
            return FechaHoraHelper.ObtenerFechasDeRango(Desde, Hasta);
        }

        public TimeSpan DiferenciaEnHoras()
        {
            return FechaHoraHelper.DiferenciaEnHoras(Desde, Hasta);
        }

        public int DiferenciaEnDias()
        {
            return FechaHoraHelper.DiferenciaEnDias(Desde, Hasta);
        }

        public int ContarDias(
            DayOfWeek[] diasDeSemanaAExcluir = null)
        {
            return FechaHoraHelper.ContarDias(Desde, Hasta, diasDeSemanaAExcluir);
        }
    }
}
