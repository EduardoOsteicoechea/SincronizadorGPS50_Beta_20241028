using System.Collections.Generic;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class ClearEntitySynchronizationData<T> where T : new()
   {
      public ClearEntitySynchronizationData
      (
         T entity,
         List<(string propertyNames, dynamic values)> properties
      )
      {
         try
         {
            for(global::System.Int32 i = 0; i < properties.Count; i++)
            {
               typeof(T).GetProperty(properties[i].propertyNames).SetValue(entity, properties[i].values);
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
   }
}
