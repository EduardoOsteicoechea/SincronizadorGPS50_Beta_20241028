using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class EntitySyncronizationTableStatusManager : ISynchronizationDatabaseTableManager
   {
      public bool TableExists(SqlConnection connection, string tableName)
      {
         try
         {
            connection.Open();

            string sqlString = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE \"TABLE_NAME\" = '{tableName}'";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               int? Sage50SincronizationTableCount = (int)sqlCommand.ExecuteScalar();
               if(Sage50SincronizationTableCount != null)
               {
                  return Sage50SincronizationTableCount > 0;
               }
               else
               {
                  return false;
               };
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

      public void CreateTable(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema)
      {
         try
         {
            connection.Open();

            string columnsDefinition = "";

            for (global::System.Int32 i = 0; i < tableSchema.ColumnsTuplesList.Count; i++)
            {
               (string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue) currentTuple = tableSchema.ColumnsTuplesList[i];
               string columnDatabaseName = currentTuple.columnName;
               string columnDefinition = currentTuple.columnDefinition;
               string columnFullDefinition = $"{columnDatabaseName} {columnDefinition}";

               if(i < tableSchema.ColumnsTuplesList.Count - 1)
               {
                  columnsDefinition += $"{columnFullDefinition}, ";
               }
               else
               {
                  columnsDefinition += $"{columnFullDefinition}";
               };
            };

            string sqlString = $"CREATE TABLE {tableSchema.TableName} ({columnsDefinition});";

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
