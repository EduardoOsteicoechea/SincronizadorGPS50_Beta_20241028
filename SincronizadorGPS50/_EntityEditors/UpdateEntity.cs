using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class UpdateEntity
   {
      public UpdateEntity
      (
         System.Data.SqlClient.SqlConnection connection,
         string tableName,
         List<(string columnName, dynamic columnValue)> columnsNamesAndValues,
         (string columnName, dynamic value) condition1,
         (string columnName, dynamic value) condition2 = default
      )
      {
         try
         {
            connection.Open();

            StringBuilder columnsNamesAndValuesStringBuilder = new StringBuilder();

            for(global::System.Int32 i = 0; i < columnsNamesAndValues.Count; i++)
            {
            //   string name = columnsNamesAndValues[i].columnName;
            //   dynamic value = columnsNamesAndValues[i].columnValue;

            //   columnsNamesAndValuesStringBuilder.Append($"{name}={DynamicValuesFormatters.Formatters[value.GetType()](value)},");
            
               string name = columnsNamesAndValues[i].columnName;

               if(columnsNamesAndValues[i].columnValue == null && name == "FCE_OBSERVACIONES")
               {
                  dynamic value = "";
                  columnsNamesAndValuesStringBuilder.Append($"{name}={DynamicValuesFormatters.Formatters[value.GetType()](value)},");
               }
               else
               {
                  dynamic value = columnsNamesAndValues[i].columnValue;
                  columnsNamesAndValuesStringBuilder.Append($"{name}={DynamicValuesFormatters.Formatters[value.GetType()](value)},");
               };
            };

            StringBuilder sqlCondition = new StringBuilder();
            
            if(condition1.value.GetType() == typeof(string))
            {
               if(condition1.value == "")
               {
                  sqlCondition.Append($"{condition2.columnName}={DynamicValuesFormatters.Formatters[condition2.value.GetType()](condition2.value)}");
               }
               else
               {
                  sqlCondition.Append($"{condition1.columnName}={DynamicValuesFormatters.Formatters[condition1.value.GetType()](condition1.value)}");            
               };
            }
            else
            {
               if(condition1.value == null || condition1.value == 0)
               {
                  sqlCondition.Append($"{condition2.columnName}={DynamicValuesFormatters.Formatters[condition2.value.GetType()](condition2.value)}");
               }
               else
               {
                  sqlCondition.Append($"{condition1.columnName}={DynamicValuesFormatters.Formatters[condition1.value.GetType()](condition1.value)}");            
               };
            };

            //StringBuilder conditionStringBuilder = new StringBuilder();
            //conditionStringBuilder.Append($"{conditionKeyValuePair.columnName}={DynamicValuesFormatters.Formatters[conditionKeyValuePair.columnValue.GetType()](conditionKeyValuePair.columnValue)}");

            string sqlString = $@"
            UPDATE 
               {tableName} 
            SET
               {columnsNamesAndValuesStringBuilder.ToString().TrimEnd(',')}
            WHERE
               {sqlCondition.ToString()}
            ;";

            //MessageBox.Show(sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
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
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
