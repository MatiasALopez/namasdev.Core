using System;
using System.Collections.Generic;
using System.Globalization;

namespace namasdev.Core.Types
{
    public class TrimestreYAño : IComparable<TrimestreYAño>
    {
        public TrimestreYAño()
            : this(DateTime.Now)
        {
        }

        public TrimestreYAño(int trimestre, int año)
            : this((short)trimestre, (short)año)
        {
        }

        public TrimestreYAño(short trimestre, short año)
        {
            ValidarTrimestreYAño(trimestre, año);

            this.Trimestre = trimestre;
            this.Año = año;
        }

        public TrimestreYAño(string añoYTrimestre)
        {
            if (String.IsNullOrWhiteSpace(añoYTrimestre))
            {
                throw new ArgumentNullException("añoYTrimestre");
            }

            try
            {
                Año = short.Parse(añoYTrimestre.Substring(0, 4));
                Trimestre = short.Parse(añoYTrimestre.Substring(4, 1));

                ValidarTrimestreYAño(Trimestre, Año);
            }
            catch (Exception)
            {
                throw new FormatException("Formato de año y trimestre inválido.");
            }
        }

        public TrimestreYAño(DateTime fecha)
        {
            Año = (short)fecha.Year;
            Trimestre = Convert.ToInt16(Math.Ceiling(fecha.Month / 3m));
        }

        public short Trimestre { get; set; }
        public short Año { get; set; }

        public DateTime PrimerDia
        {
            get { return new DateTime(Año, ((Trimestre - 1) * 3) + 1, 1); }
        }

        public DateTime UltimoDia
        {
            get
            {
                int mes = Trimestre * 3;
                return new DateTime(Año, mes, CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(Año, mes));
            }
        }

        public string AñoYTrimestre
        {
            get { return FormatearAñoYTrimestre(Trimestre, Año); }
        }

        public string TrimestreNombre
        {
            get
            {
                string sufijo;
                switch (Trimestre)
                {
                    case 1:
                    case 3:
                        sufijo = "er";
                        break;

                    case 2:
                        sufijo = "do";
                        break;

                    case 4:
                        sufijo = "to";
                        break;

                    default:
                        throw new Exception($"Trimestre inválido ({Trimestre}).");
                }

                return $"{Trimestre}{sufijo} trimestre";
            }
        }

        public string TrimestreNombreYAño
        {
            get { return $"{TrimestreNombre} de {Año}"; }
        }

        private void ValidarTrimestreYAño(short trimestre, short año)
        {
            if (trimestre < 1 || trimestre > 4)
            {
                throw new Exception($"Trimestre inválido ({trimestre}).");
            }
            if (año <= 0)
            {
                throw new Exception($"Año inválido ({año}).");
            }
        }

        public TrimestreYAño CrearTrimestreSiguiente()
        {
            return CrearTrimestreSumandoNumero(1);
        }

        public TrimestreYAño CrearTrimestreAnterior()
        {
            return CrearTrimestreSumandoNumero(-1);
        }

        public TrimestreYAño CrearTrimestreSumandoNumero(int numero)
        {
            return new TrimestreYAño(PrimerDia.AddMonths(numero * 3));
        }

        public IEnumerable<MesYAño> ObtenerMesesDeTrimestre()
        {
            var primerMes = new MesYAño(PrimerDia);
            return new MesYAño[] 
            {
                primerMes,
                primerMes.CrearMesSiguiente(),
                new MesYAño(UltimoDia)
            };
        }

        public MesYAño ObtenerMesDeTrimestre(int numero)
        {
            return new MesYAño(PrimerDia).CrearMesSumandoMeses(numero - 1);
        }

        public override string ToString()
        {
            return TrimestreNombreYAño;
        }

        public int CompareTo(TrimestreYAño other)
        {
            if (other == null)
            {
                return 1;
            }

            return String.Compare(this.AñoYTrimestre, other.AñoYTrimestre);
        }

        public override int GetHashCode()
        {
            return this.AñoYTrimestre.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otro = obj as TrimestreYAño;
            if (otro == null)
            {
                return false;
            }

            return this.GetHashCode().Equals(otro.GetHashCode());
        }

        public static string FormatearAñoYTrimestre(short trimestre, short año)
        {
            return String.Format("{0:D4}{1:D1}", año, trimestre);
        }

        public static string FormatearAñoYTrimestre(int trimestre, int año)
        {
            return FormatearAñoYTrimestre((short)trimestre, (short)año);
        }

        public static string FormatearTrimestreYAño(int trimestre, int año)
        {
            return FormatearAñoYTrimestre((short)trimestre, (short)año);
        }
    }
}
