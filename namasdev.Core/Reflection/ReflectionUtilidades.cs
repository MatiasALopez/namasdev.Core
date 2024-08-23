using System;
using System.Linq;
using System.Reflection;

using namasdev.Core.Validation;

namespace namasdev.Core.Reflection
{
    public class ReflectionHelper
    {
        public static object ObtenerValorDefault<T>()
        {
            return ObtenerValorDefault(typeof(T));
        }

        public static object ObtenerValorDefault(Type tipo)
        {
            Validador.ValidarArgumentRequeridoYThrow(tipo, nameof(tipo));

            if (tipo.IsValueType)
            {
                return Activator.CreateInstance(tipo);
            }

            return null;
        }

        public static bool LasPropiedadesDeLosObjetosSonIguales<T>(T obj1, T obj2) 
            where T : class
        {
            Validador.ValidarArgumentRequeridoYThrow(obj1, nameof(obj1));
            Validador.ValidarArgumentRequeridoYThrow(obj2, nameof(obj2));

            if (obj1 == obj2)
            {
                return true;
            }

            Type tipo = typeof(T);
            
            foreach (var pi in ObtenerPropiedades(tipo))
            {
                object obj1Value = tipo.GetProperty(pi.Name).GetValue(obj1, null);
                object obj2Value = tipo.GetProperty(pi.Name).GetValue(obj2, null);

                if (obj1Value != obj2Value && (obj1Value == null || !obj1Value.Equals(obj2Value)))
                {
                    return false;
                }
            }

            return true;
        }

        public static PropertyInfo[] ObtenerPropiedades<T>()
        {
            return ObtenerPropiedades(typeof(T));
        }

        public static PropertyInfo[] ObtenerPropiedades(Type tipoClase)
        {
            Validador.ValidarArgumentRequeridoYThrow(tipoClase, nameof(tipoClase));

            return tipoClase.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static PropertyInfo[] ObtenerPropiedadesDeTipo<TClase, TPropiedad>()
        {
            return ObtenerPropiedadesDeTipo(typeof(TClase), typeof(TPropiedad));
        }

        public static PropertyInfo[] ObtenerPropiedadesDeTipo(Type tipoClase, Type tipoPropiedad)
        {
            Validador.ValidarArgumentRequeridoYThrow(tipoPropiedad, nameof(tipoPropiedad));

            return ObtenerPropiedades(tipoClase)
                .Where(p => p.PropertyType == tipoPropiedad)
                .ToArray();
        }

        public static T CrearInstancia<T>(params object[] parametrosConstructor)
            where T : class
        {
            return (T)Activator.CreateInstance(typeof(T), parametrosConstructor);
        }

        public static bool TodasLasPropiedadesDelObjetoTienenValorNull<T>(T objeto)
            where T : class
        {
            Validador.ValidarArgumentRequeridoYThrow(objeto, nameof(objeto));

            return !ObtenerPropiedades(typeof(T))
                .Select(p => p.GetValue(objeto))
                .Any(v => v != null && !String.IsNullOrWhiteSpace(Convert.ToString(v)));
        }

        public static bool TodasLasPropiedadesDelObjetoTienenValorDefault<T>(T objeto)
            where T : class
        {
            Validador.ValidarArgumentRequeridoYThrow(objeto, nameof(objeto));

            return !ObtenerPropiedades(typeof(T))
                .Any(p => !object.Equals(p.GetValue(objeto), ObtenerValorDefault(p.PropertyType)));
        }

        public static TAtributo ObtenerAtributoDeCampo<TAtributo>(Type tipoClase, string nombreCampo)
            where TAtributo : Attribute
        {
            Validador.ValidarArgumentRequeridoYThrow(tipoClase, nameof(tipoClase));
            Validador.ValidarArgumentRequeridoYThrow(nombreCampo, nameof(nombreCampo));

            var campo = tipoClase.GetField(nombreCampo);
            if (campo == null)
            {
                throw new MissingMemberException(tipoClase.FullName, nombreCampo);
            }

            return ObtenerAtributoDeMiembro<TAtributo>(campo);
        }

        public static TAtributo ObtenerAtributoDePropiedad<TAtributo>(Type tipoClase, string nombrePropiedad)
           where TAtributo : Attribute
        {
            Validador.ValidarArgumentRequeridoYThrow(tipoClase, nameof(tipoClase));
            Validador.ValidarArgumentRequeridoYThrow(nombrePropiedad, nameof(nombrePropiedad));

            var propiedad = tipoClase.GetProperty(nombrePropiedad);
            if (propiedad == null)
            {
                throw new MissingMemberException(tipoClase.FullName, nombrePropiedad);
            }

            return ObtenerAtributoDeMiembro<TAtributo>(propiedad);
        }

        private static TAtributo ObtenerAtributoDeMiembro<TAtributo>(MemberInfo miembro,
            Func<TAtributo, bool> condicionAtributo = null)
          where TAtributo : Attribute
        {
            Validador.ValidarArgumentRequeridoYThrow(miembro, nameof(miembro));

            var atributos = miembro.GetCustomAttributes(typeof(TAtributo), false);
            if (atributos == null || atributos.Length == 0)
            {
                return null;
            }

            var atributo = (TAtributo)atributos.FirstOrDefault();
            if (condicionAtributo != null && !condicionAtributo(atributo))
            {
                return null;
            }

            return atributo;
        }

        public static PropertyInfo[] ObtenerPropiedadesConAtributo<TClase, TAtributo>(
            Func<TAtributo, bool> condicionAtributo = null)
            where TAtributo : Attribute
        {
            return ObtenerPropiedadesConAtributo<TAtributo>(typeof(TClase), condicionAtributo);
        }

        public static PropertyInfo[] ObtenerPropiedadesConAtributo<TAtributo>(Type tipoClase,
            Func<TAtributo, bool> condicionAtributo = null)
            where TAtributo : Attribute
        {
            Validador.ValidarArgumentRequeridoYThrow(tipoClase, nameof(tipoClase));

            return ObtenerPropiedades(tipoClase)
                .Where(p => ObtenerAtributoDeMiembro<TAtributo>(p, condicionAtributo) != null)
                .ToArray();
        }

        public static PropertyInfo[] ObtenerPropiedadesDeTipoConAtributo<TClase, TPropiedad, TAtributo>(
            Func<TAtributo, bool> condicionAtributo = null)
           where TAtributo : Attribute
        {
            return ObtenerPropiedadesDeTipoConAtributo<TAtributo>(typeof(TClase), typeof(TPropiedad), condicionAtributo);
        }

        public static PropertyInfo[] ObtenerPropiedadesDeTipoConAtributo<TAtributo>(Type tipoClase, Type tipoPropiedad,
            Func<TAtributo, bool> condicionAtributo = null)
           where TAtributo : Attribute
        {
            Validador.ValidarArgumentRequeridoYThrow(tipoClase, nameof(tipoClase));

            return ObtenerPropiedadesDeTipo(tipoClase, tipoPropiedad)
                .Where(p => ObtenerAtributoDeMiembro<TAtributo>(p, condicionAtributo) != null)
                .ToArray();
        }

        public static T ObtenerValorPropiedad<T>(Type tipo, object objeto, string nombrePropiedad)
        {
            var p = tipo.GetProperty(nombrePropiedad);
            if (p == null)
            {
                throw new MissingMemberException(tipo.Name, nombrePropiedad);
            }

            return (T)p.GetValue(objeto);
        }

        public static void EstablecerValorPropiedad(Type tipo, object objeto, string nombrePropiedad, object valor)
        {
            var p = tipo.GetProperty(nombrePropiedad);
            if (p == null)
            {
                throw new MissingMemberException(tipo.Name, nombrePropiedad);
            }

            p.SetValue(objeto, valor);
        }
    }
}
