using System.Data.SqlClient;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class DeleteFromSynchronizationTable
   {
      public DeleteFromSynchronizationTable
      (
         System.Data.SqlClient.SqlConnection connection,
         GestprojectCustomer customer,
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            DELETE FROM 
               {tableSchema.TableName} 
            WHERE 
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={customer.PAR_ID}
            AND
               {tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnDatabaseName}='{customer.sage50_company_group_guid_id}'
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };
         }
         catch(System.Exception exception)
         {
            throw new System.Exception(
               $"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.DeleteFromSynchronizationTable:\n\n{exception.Message}"
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
