using System;

namespace namasdev.Core.Exceptions
{
    [Serializable]
    public class ExcepcionMensajeAlUsuario : Exception
    {
        public ExcepcionMensajeAlUsuario()
            : base() { }

        public ExcepcionMensajeAlUsuario(string mensaje)
            : this(mensaje, null, null) { }

        public ExcepcionMensajeAlUsuario(string mensaje, string mensajeInterno)
            : this(mensaje, mensajeInterno, null) { }

        public ExcepcionMensajeAlUsuario(string mensaje, Exception inner)
            : this(mensaje, null, inner) { }

        public ExcepcionMensajeAlUsuario(string mensaje, string mensajeInterno, Exception inner)
            : base(mensaje, inner)
        {
            MensajeInterno = mensajeInterno;
        }

        protected ExcepcionMensajeAlUsuario(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public string MensajeInterno { get; private set; }

        public bool SeDebeRegistrarElError
        {
            get { return !String.IsNullOrWhiteSpace(MensajeInterno); }
        }
    }
}
