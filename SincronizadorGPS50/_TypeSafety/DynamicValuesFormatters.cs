using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public static class DynamicValuesFormatters
   {
      public static Dictionary<Type, Func<object, string>> Formatters = new Dictionary<Type, Func<object, string>>
      {
         { typeof(decimal), value => $"CAST(REPLACE('{value ?? 0.00}',',','.') AS NUMERIC)" },
         { typeof(int), value => value.ToString() ?? (0).ToString() },
         { typeof(string), value => $"'{value}'" ?? "''" },
         { typeof(DateTime), value => $"'{value:yyyy-MM-dd HH:mm:ss}'" }
      };
   }
}
