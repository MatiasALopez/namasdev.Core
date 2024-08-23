using System;
using System.Collections.Generic;
using System.Linq;

namespace namasdev.Core.Processing
{
    public class ProcesadorHelper
    {
        public static void ProcesarEnBatch<T>(IEnumerable<T> lista, int tamañoBatch, Action<BatchArgs<T>> procesamientoItems)
        {
            BatchArgs<T> batchArgs;
            IEnumerable<T> items;
            int itemsCount;
            int numero = 1;
            bool finDeLista = false;
            while (!finDeLista)
            {
                items = lista.Skip((numero - 1) * tamañoBatch).Take(tamañoBatch);
                itemsCount = items.Count();

                if (itemsCount > 0)
                {
                    batchArgs = new BatchArgs<T> { Numero = numero, Items = items };
                    procesamientoItems(batchArgs);

                    if (batchArgs.CancelarProcesamiento)
                    {
                        break;
                    }
                }

                finDeLista = itemsCount == 0 || itemsCount < tamañoBatch;

                numero++;
            }
        }
    }
}
