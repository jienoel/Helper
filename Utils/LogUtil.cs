using System;
using System.Collections.Generic;
using System.Text;

public static class LogUtil
{
      public delegate string ToString<T>(T value);
      public static string IEnumerableToString<T>(this IEnumerable<T> values, ToString<T> func = null) 
      {
            StringBuilder builder = new StringBuilder();
            foreach (var value in values)
            {
                  builder.AppendLine(func == null ? value.ToString() : func(value));
            }
            return builder.ToString();
      }

      public static string DicToString<T1, T2>(this Dictionary<T1, T2> values, ToString<T1> func1  = null, ToString<T2> func2  = null)
      {
            StringBuilder builder = new StringBuilder();
            foreach (var pair in values)
            {
                  builder.AppendLine(string.Format("{0} : {1}", func1 == null ? pair.Key.ToString() : func1(pair.Key), func2 == null ? pair.Value.ToString() : func2(pair.Value)));
            }
            return builder.ToString();
      }
}