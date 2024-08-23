using System;
using System.ComponentModel.DataAnnotations;

using namasdev.Core.Types;

namespace namasdev.Core.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EnumValidoAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //  La "ausencia de valor" en un Enum es considerado válido.
            if (value == null)
            {
                return true;
            }

            //  Si el valor NO es un Enum, no aplica la validación. Para no lanzar un error, decimos que NO es válido
            if (!(value is Enum))
            {
                return false;
            }

            return ((Enum)value).IsValid();
        }
    }
}
