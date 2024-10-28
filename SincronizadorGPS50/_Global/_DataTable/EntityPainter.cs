using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class EntityPainter<T1> : ISynchronizableEntityPainter<T1>
   {
      public void PaintEntityListOnDataTable
      (
         List<T1> proccessedGestprojectEntities,
         DataTable dataTable,
         List<(string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue)> tableFieldsTupleList
      )
      {
         try
         {
            foreach(T1 item in proccessedGestprojectEntities)
            {
               //StringBuilder stringBuilder = new StringBuilder();
               //foreach(var item2 in item.GetType().GetProperties())
               //{
               //   stringBuilder.Append($"{item2.Name}: {item2.GetValue(item)}\n");
               //};
               //MessageBox.Show(stringBuilder.ToString());
               
               DataRow row = dataTable.NewRow();

               for(global::System.Int32 i = 0; i < tableFieldsTupleList.Count; i++)
               {
                  var propertyName = tableFieldsTupleList[i].columnName;
                  var propertyValue = item.GetType().GetProperty(propertyName)?.GetValue(item);

                  if(tableFieldsTupleList[i].columnType == typeof(decimal))
                  {
                     row[i] = propertyValue ?? 0.00;
                  }
                  if(tableFieldsTupleList[i].columnType == typeof(string))
                  {
                     row[i] = propertyValue ?? "";
                  }
                  else if(tableFieldsTupleList[i].columnType == typeof(int))
                  {
                     row[i] = propertyValue ?? DBNull.Value;
                  }
                  else if(tableFieldsTupleList[i].columnType == typeof(DateTime))
                  {
                     row[i] = propertyValue ?? DBNull.Value;
                  };
               };

               dataTable.Rows.Add(row);
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
