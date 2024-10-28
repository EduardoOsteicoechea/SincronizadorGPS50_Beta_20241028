using SincronizadorGPS50.Workflows.Clients;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal static class TypeProtector<T>
   {
      public static T Scrutinize(SqlDataReader reader, int ordinal, T defaultValue)
      {
         try
         {
            if(reader == null)
            {
               throw new ArgumentNullException(nameof(reader));
            }

            if(reader.IsDBNull(ordinal))
            {
               return defaultValue;
            };

            if(reader.GetFieldType(ordinal) != typeof(T))
            {
               throw new InvalidOperationException($"Type mismatch: Expected {typeof(T)}, but got {reader.GetFieldType(ordinal)} at iteration \"{ordinal}\". The value was: \"{reader.GetValue(ordinal)}\" and the column name: \"{reader.GetSchemaTable().Rows[ordinal]["ColumnName"]}\"");
            };

            T value = reader.GetFieldValue<T>(ordinal);

            return value;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception,
               $"Error processing ordinal {ordinal} (expected {typeof(T)})"
            );
         };         
      }
   }
}
