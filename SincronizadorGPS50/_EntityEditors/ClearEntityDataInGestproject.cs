using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace SincronizadorGPS50
{
   public class ClearEntityDataInGestproject
   {
      public ClearEntityDataInGestproject
      (
         SqlConnection connection,
         string tableName,
         List<string> columnsToClear,
         (string contitionColumnName,dynamic conditionColumnValue) conditionKeyValuePair
      ) 
      {
         try
         {
            connection.Open();

            StringBuilder columnsToClearStringBuilder = new StringBuilder();

            for(global::System.Int32 i = 0; i < columnsToClear.Count; i++)
            {
               columnsToClearStringBuilder.Append($"{columnsToClear[i]}='',");
            };

            string sqlString = $@"
            UPDATE 
               {tableName}
            SET
               {columnsToClearStringBuilder.ToString().TrimEnd(',')}
            WHERE
               {conditionKeyValuePair.contitionColumnName}={conditionKeyValuePair.conditionColumnValue}
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
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
