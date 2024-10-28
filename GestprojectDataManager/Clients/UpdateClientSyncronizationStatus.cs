using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class UpdateClientSyncronizationStatus
   {
      public UpdateClientSyncronizationStatus
      (
         System.Data.SqlClient.SqlConnection connection,
         int? gestprojectClientId,
         string sage50ClientGuid,
         string sage50CompanyGroupGuid,
         bool isSynchronized,
         CustomerSyncronizationTableSchema tableSchema
      ) 
      {
         try
         {
            connection.Open();

            string synchronizationStatus = isSynchronized ? "Sincronizado" : "Desincronizado";

            string whereClause = "";
            if(sage50ClientGuid != null && sage50ClientGuid != "") 
            {
               whereClause = $@"
               {tableSchema.Sage50ClientGuidIdColumn.ColumnDatabaseName}='{sage50ClientGuid}'
               AND
               {tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnDatabaseName}='{sage50CompanyGroupGuid}'
               ";
            } 
            else
            {
               whereClause = $@"
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={gestprojectClientId}
               AND
               {tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnDatabaseName}='{sage50CompanyGroupGuid}'
               ";
            };

            string sqlString1 = $@"
            UPDATE 
               {tableSchema.TableName} 
            SET 
               {tableSchema.SynchronizationStatusColumn.ColumnDatabaseName}='{synchronizationStatus}',
               {tableSchema.CommentsColumn.ColumnDatabaseName}=''
            WHERE
               {whereClause}
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString1, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };
         }
         catch(System.Exception exception)
         {
            throw new System.Exception(
               $"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.UpdateClientSyncronizationStatus:\n\n{exception.Message}"
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
