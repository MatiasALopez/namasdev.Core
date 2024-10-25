using System;

namespace namasdev.Core.Entity
{
    public abstract class EntidadCreadoModificado<TId> : EntidadCreado<TId>, IEntidadModificacion
        where TId : IEquatable<TId>
    {
		public string UltimaModificacionPor { get; set; }
		public DateTime UltimaModificacionFecha { get; set; }
	}
}
