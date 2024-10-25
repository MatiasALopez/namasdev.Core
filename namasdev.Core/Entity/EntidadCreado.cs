using System;

namespace namasdev.Core.Entity
{
    public abstract class EntidadCreado<TId> : Entidad<TId>, IEntidadCreado
        where TId : IEquatable<TId>
    {
		public string CreadoPor { get; set; }
		public DateTime CreadoFecha { get; set; }
	}
}
