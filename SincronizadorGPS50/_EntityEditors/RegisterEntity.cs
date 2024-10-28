using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class RegisterEntity
   {
      public RegisterEntity
      (
         System.Data.SqlClient.SqlConnection connection,
         string tableName,
         List<(string columnName, dynamic columnValue)> columnsNamesAndValues
      )
      {
         try
         {
            connection.Open();

            StringBuilder  dynamicColumns = new StringBuilder();
            StringBuilder  dynamicValues = new StringBuilder();

            for (global::System.Int32 i = 0; i < columnsNamesAndValues.Count; i++)
            {
               string name = columnsNamesAndValues[i].columnName;

               if(columnsNamesAndValues[i].columnValue == null && name == "FCE_OBSERVACIONES")
               {
                  dynamic value = "";
                  dynamicColumns.Append($"{name},");
                  dynamicValues.Append($"{DynamicValuesFormatters.Formatters[value.GetType()](value)},"); 
               }
               else
               {
                  dynamic value = columnsNamesAndValues[i].columnValue;
                  dynamicColumns.Append($"{name},");
                  dynamicValues.Append($"{DynamicValuesFormatters.Formatters[value.GetType()](value)},"); 
               };
            };

            string sqlString2 = $@"INSERT INTO {tableName} ({dynamicColumns.ToString().TrimEnd(',')}) VALUES ({dynamicValues.ToString().TrimEnd(',')});";

            //MessageBox.Show(sqlString2);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString2, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };

            //MessageBox.Show($"Registered {columnsNamesAndValues[3].columnValue}");
         }
         catch(SqlException exception)
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
