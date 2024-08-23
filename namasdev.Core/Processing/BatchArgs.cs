using System.Collections.Generic;

namespace namasdev.Core.Processing
{
    public class BatchArgs<T>
    {
        public int Numero { get; set; }
        public IEnumerable<T> Items { get; set; }
        public bool CancelarProcesamiento { get; set; }
    }
}
