using System.Collections.Generic;

namespace namasdev.Core.Types
{
    public static class DictionaryExtensions
    {
        public static void AddOrSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}
