using System.Collections.Generic;
using System.Linq;

namespace YaEngine.Core
{
    public static class CopyUtils
    {
        public static Dictionary<TKey, TValue> Copy<TKey, TValue>(this Dictionary<TKey, TValue> source)
        {
            return source.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}