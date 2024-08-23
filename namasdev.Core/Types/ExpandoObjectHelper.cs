using System.Collections.Generic;
using System.Dynamic;

namespace namasdev.Core.Types
{
    public class ExpandoObjectHelper
    {
        public static ExpandoObject CrearDesdeDiccionario<TValue>(Dictionary<string, TValue> diccionario)
        {
            var expObj = new ExpandoObject();
            var expDic = (IDictionary<string, object>)expObj;
            foreach (var item in diccionario)
            {
                expDic.Add(item.Key, item.Value);
            }
            return expObj;
        }

        public static void Actualizar<TValue>(ExpandoObject original, Dictionary<string, TValue> nuevosValores)
        {
            var expDic = (IDictionary<string, object>)original;
            foreach (var item in nuevosValores)
            {
                expDic.AddOrSetValue(item.Key, item.Value);
            }
        }
    }
}
