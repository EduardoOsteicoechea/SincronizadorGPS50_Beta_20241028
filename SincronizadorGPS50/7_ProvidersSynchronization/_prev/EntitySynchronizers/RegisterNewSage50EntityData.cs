using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class RegisterNewSage50EntityData
   {
      public RegisterNewSage50EntityData
      (
         System.Data.SqlClient.SqlConnection connection,
         string tableName,
         List<(string columnName, dynamic columnValue)> columnsAndValues,
         (string columnName, dynamic columnValue) conditionKeyValuePair,
         string tableName2,
         (string columnName, dynamic columnValue) columnsAndValues2,
         (string columnName, dynamic columnValue) conditionKeyValuePair2
      )
      {
         try
         {
            connection.Open();

            StringBuilder columnsAndValuesStringBuilder = new StringBuilder();

            Dictionary<Type, Func<object, string>> formatters = new Dictionary<Type, Func<object, string>>
            {
                { typeof(int), value => value.ToString() },
                { typeof(string), value => $"'{value}'" },
                { typeof(DateTime), value => $"'{value:yyyy-MM-dd HH:mm:ss}'" }
            };

            for(global::System.Int32 i = 0; i < columnsAndValues.Count; i++)
            {
               string name = columnsAndValues[i].columnName;
               dynamic value = columnsAndValues[i].columnValue;

               columnsAndValuesStringBuilder.Append($"{name}={formatters[value.GetType()](value)},");
            };

            StringBuilder conditionStringBuilder = new StringBuilder();
            conditionStringBuilder.Append($"{conditionKeyValuePair.columnName}={formatters[conditionKeyValuePair.columnValue.GetType()](conditionKeyValuePair.columnValue)}");


            string sqlString = $@"
            UPDATE 
               {tableName} 
            SET
               {columnsAndValuesStringBuilder.ToString().TrimEnd(',')}
            WHERE
               {conditionStringBuilder.ToString()}
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };

            string sqlString2 = $@"
            UPDATE {tableName2}
            SET
               {columnsAndValues2.columnName}='{columnsAndValues2.columnValue}'
            WHERE
               {conditionKeyValuePair2.columnName}={conditionKeyValuePair2.columnValue}
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString2, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };
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
