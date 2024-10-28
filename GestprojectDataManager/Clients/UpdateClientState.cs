using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class UpdateClientState
   {
      public UpdateClientState
      (
         System.Data.SqlClient.SqlConnection connection,
         GestprojectCustomer customer,
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
         {
            connection.Open();

            string sqlString1 = $@"
            UPDATE 
               {tableSchema.TableName} 
            SET
               {tableSchema.SynchronizationStatusColumn.ColumnDatabaseName}='{customer.synchronization_status}',
               {tableSchema.GestprojectClientNameColumn.ColumnDatabaseName}='{customer.fullName}',
               {tableSchema.GestprojectClientCIFNIFColumn.ColumnDatabaseName}='{customer.PAR_CIF_NIF}',
               {tableSchema.GestprojectClientAddressColumn.ColumnDatabaseName}='{customer.PAR_DIRECCION_1}',
               {tableSchema.GestprojectClientPostalCodeColumn.ColumnDatabaseName}='{customer.PAR_CP_1}',
               {tableSchema.GestprojectClientLocalityColumn.ColumnDatabaseName}='{customer.PAR_LOCALIDAD_1}',
               {tableSchema.GestprojectClientProvinceColumn.ColumnDatabaseName}='{customer.PAR_PROVINCIA_1}',
               {tableSchema.GestprojectClientCountryColumn.ColumnDatabaseName}='{customer.PAR_PAIS_1}'
            WHERE
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}='{customer.PAR_ID}'
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString1, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };
         }
         catch(System.Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
