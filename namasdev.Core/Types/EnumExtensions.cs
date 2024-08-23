using System;
using System.ComponentModel;

using namasdev.Core.Reflection;

namespace namasdev.Core.Types
{
    public static class EnumExtensions
    {
        //public static string Description<TEnum>(this TEnum value) where TEnum : Enum
        public static string Description(this Enum valor)
        {
            if (valor == null)
            {
                return String.Empty;
            }

            DescriptionAttribute atributo = null;
            if (IsValid(valor))
            {
                atributo = ReflectionHelper.ObtenerAtributoDeCampo<DescriptionAttribute>(valor.GetType(), valor.ToString());
            }

            return atributo != null
                ? atributo.Description
                : valor.ToString();
        }

        public static bool IsValid(this Enum valor)
        {
            //  La "ausencia de valor" en un Enum es considerado válido.
            if (valor == null)
            {
                return true;
            }

            //  Para verificar que un valor de un Enum es válido, intentamos hacer el cast del valor a un número, 
            //  en caso de poder hacerlo asumimos que el valor NO está definido en el Enum.
            return !long.TryParse(valor.ToString(), out _);
        }
    }
}
