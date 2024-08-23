using System;

namespace namasdev.Core
{
    public static class GuidExtensions
    {
        public static Guid? ValueOrNullIfEmpty(this Guid valor)
        {
            return valor != Guid.Empty
                ? valor
                : (Guid?)null;
        }
    }
}
