using System.Data.SqlClient;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class DeleteClientCodeInGestproject
   {
      public DeleteClientCodeInGestproject
      (
         System.Data.SqlClient.SqlConnection connection,
         GestprojectCustomer customer,
         CustomerSyncronizationTableSchema tableSchema
      ) 
      {
         try
         {
            connection.Open();

            string sqlString2 = $@"
            UPDATE 
               {"PARTICIPANTE"}
            SET
               {tableSchema.GestprojectClientAccountableSubaccountColumn.ColumnDatabaseName}=''
            WHERE
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={customer.PAR_ID}
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString2, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };
         }
         catch(SqlException exception)
         {
            throw new System.Exception(
               $"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.RegisterClient:\n\n{exception.Message}"
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
