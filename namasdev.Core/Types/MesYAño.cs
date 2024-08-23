using System;
using System.Collections.Generic;
using System.Globalization;

namespace namasdev.Core.Types
{
    public class MesYAño : IComparable<MesYAño>
    {
        public MesYAño()
            : this(DateTime.Today)
        {
        }

        public MesYAño(short mes, short año)
        {
            ValidarMesYAño(mes, año);

            this.Mes = mes;
            this.Año = año;
        }

        public MesYAño(string mesNombre, short año)
        {
            if (String.IsNullOrWhiteSpace(mesNombre))
            {
                throw new ArgumentNullException("mesNombre");
            }

            try
            {
                Mes = (short)(Array.IndexOf(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames, mesNombre.ToLower()) + 1);
            }
            catch (Exception)
            {
                throw new FormatException("Nombre de mes inválido.");
            }
                
            Año = año;

            ValidarMesYAño(Mes, Año);
        }

        public MesYAño(string añoYMes)
        {
            if (String.IsNullOrWhiteSpace(añoYMes))
            {
                throw new ArgumentNullException("añoYMes");
            }

            try 
	        {	      
                Año = short.Parse(añoYMes.Substring(0, 4));
                Mes = short.Parse(añoYMes.Substring(4, 2));

                ValidarMesYAño(Mes, Año);
	        }
	        catch (Exception)
	        {
		        throw new FormatException("Formato de año y mes inválido.");
	        }
        }

        public MesYAño(DateTime fecha)
        {
            Año = (short)fecha.Year;
            Mes = (short)fecha.Month;
        }

        public short Mes { get; set; }
        public short Año { get; set; }

        public DateTime PrimerDia
        {
            get { return new DateTime(Año, Mes, 1); }
        }

        public DateTime UltimoDia
        {
            get { return new DateTime(Año, Mes, CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(Año, Mes)); }
        }

        public string AñoYMes
        {
            get { return FormatearAñoYMes(Mes, Año); }
        }

        public string AñoYMesConSeparador
        {
            get { return FormatearAñoYMes(Mes, Año, conSeparador: true); }
        }

        public string MesNombre
        {
            get { return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes); }
        }

        public string MesNombreYAño
        {
            get { return String.Format("{0} de {1}", MesNombre, Año); }
        }

        private void ValidarMesYAño(short mes, short año)
        {
            if (mes < 1 || mes > 12)
            {
                throw new Exception($"Mes inválido ({mes}).");
            }
            if (año <= 0)
            {
                throw new Exception($"Año inválido ({año}).");
            }
        }


        public MesYAño CrearMesSiguiente()
        {
            return CrearMesSumandoMeses(1);
        }

        public MesYAño CrearMesAnterior()
        {
            return CrearMesSumandoMeses(-1);
        }

        public MesYAño CrearMesSumandoMeses(int numeroMeses)
        {
            return new MesYAño(PrimerDia.AddMonths(numeroMeses));
        }

        public DateTime ObtenerFechaDiaHabil(int numeroDiaHabil)
        {
            var fecha = PrimerDia;
            var ultimoDia = UltimoDia;
            int diaHabil = 1;
            while (fecha < ultimoDia && diaHabil < numeroDiaHabil)
            {
                fecha = fecha.AddDays(1);

                if (!(fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday))
                {
                    diaHabil++;
                }
            }

            return fecha;
        }

        public IEnumerable<DateTime> ObtenerFechas()
        {
            var dias = new List<DateTime>();
            DateTime dia = PrimerDia;
            while (dia <= UltimoDia)
            {
                dias.Add(dia);
                dia = dia.AddDays(1);
            }
            return dias;
        }

        public DateTime ObtenerFecha(int dia)
        {
            return new DateTime(Año, Mes, dia);
        }

        public int CalcularDiferenciaEnMeses(MesYAño hasta)
        {
            bool invertido = this.CompareTo(hasta) > 0;
            int cant = 0;
            MesYAño mes = this;
            while (!mes.Equals(hasta))
            {
                mes = invertido
                    ? mes.CrearMesAnterior()
                    : mes.CrearMesSiguiente();

                cant++;
            }
            return invertido ? cant * -1 : cant;
        }

        public override string ToString()
        {
            return MesNombreYAño;
        }

        public int CompareTo(MesYAño other)
        {
            if (other == null)
            {
                return 1;
            }

            return String.Compare(this.AñoYMes, other.AñoYMes);
        }

        public override int GetHashCode()
        {
            return this.AñoYMes.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otro = obj as MesYAño;
            if (otro == null)
            {
                return false;
            }

            return this.GetHashCode().Equals(otro.GetHashCode());
        }

        public static string FormatearAñoYMes(short mes, short año,
            bool conSeparador = false)
        {
            return $"{año:D4}{(conSeparador ? "-" : "")}{mes:D2}";
        }

        public static IEnumerable<MesYAño> ObtenerMeses(MesYAño desde, MesYAño hasta)
        {
            var meses = new List<MesYAño>();

            bool invertido = desde.CompareTo(hasta) > 0;
            MesYAño mes = desde;
            if (mes.Equals(hasta))
            {
                meses.Add(desde);
            }
            while (!mes.Equals(hasta))
            {
                meses.Add(mes);

                mes = invertido
                    ? mes.CrearMesAnterior()
                    : mes.CrearMesSiguiente();
            }

            return meses;
        }

        public static IEnumerable<MesYAño> ObtenerMesesParaAños(int añoDesde, int añoHasta)
        {
            var meses = new List<MesYAño>();
            for (int año = añoDesde; año <= añoHasta; año++)
            {
                for (short mes = 1; mes <= 12; mes++)
                {
                    meses.Add(new MesYAño { Año = (short)año, Mes = mes });
                }
            }
            return meses;
        }
    }
}
