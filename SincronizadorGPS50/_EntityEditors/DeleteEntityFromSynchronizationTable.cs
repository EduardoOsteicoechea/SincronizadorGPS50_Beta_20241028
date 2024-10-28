using System.Data.SqlClient;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class DeleteEntityFromSynchronizationTable
   {
      public DeleteEntityFromSynchronizationTable
      (
         SqlConnection connection,
         string tableName,
         (string columnName, dynamic columnValue) condition1,
         (string columnName, dynamic columnValue) condition2
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            DELETE FROM 
               {tableName} 
            WHERE 
               {condition1.columnName}={DynamicValuesFormatters.Formatters[condition1.columnValue.GetType()](condition1.columnValue)}
            AND
               {condition2.columnName}={DynamicValuesFormatters.Formatters[condition2.columnValue.GetType()](condition2.columnValue)}
            ;";

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

      public DeleteEntityFromSynchronizationTable
      (
         SqlConnection connection,
         string tableName,
         (string columnName, dynamic columnValue) condition1
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            DELETE FROM 
               {tableName} 
            WHERE 
               {condition1.columnName}={DynamicValuesFormatters.Formatters[condition1.columnValue.GetType()](condition1.columnValue)}
            ;";

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
