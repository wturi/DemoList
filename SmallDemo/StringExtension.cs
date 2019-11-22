using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallDemo
{
    public static class StringExtension
    {
        public static string MidStrEx(this string source, string startStr, string endStr)
        {
            var result = string.Empty;
            try
            {
                var startIndexOf = source.IndexOf(startStr, StringComparison.Ordinal);
                if (startIndexOf == -1)
                    return result;
                var sempstress = source.Substring(startIndexOf + startStr.Length);
                var endIndexOf = sempstress.IndexOf(endStr, StringComparison.Ordinal);
                if (endIndexOf == -1)
                    return result;
                result = sempstress.Remove(endIndexOf);
            }
            catch (Exception e)
            {
                // ignored
            }

            return result;
        }

        public static string StringJoin<T>(this IEnumerable<T> objEnumerable, string join)
        {
            return string.Join(join, objEnumerable);
        }
    }
}
