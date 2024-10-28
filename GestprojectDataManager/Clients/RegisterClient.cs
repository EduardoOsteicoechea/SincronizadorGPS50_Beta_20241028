using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class RegisterClient
   {
      public RegisterClient
      (
         System.Data.SqlClient.SqlConnection connection, 
         GestprojectDataManager.GestprojectCustomer client,
         CustomerSyncronizationTableSchema tableSchema

      )
      {
         try
         {
            connection.Open();
            string sqlString2 = $@"
            INSERT INTO {tableSchema.TableName} 
            (
               {tableSchema.SynchronizationStatusColumn.ColumnDatabaseName}, 
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientNameColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientCIFNIFColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientAddressColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientPostalCodeColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientLocalityColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientProvinceColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientCountryColumn.ColumnDatabaseName}
            ) 
            VALUES 
            (
               'Nunca ha sido sincronizado',
               {client.PAR_ID},
               '{client.fullName}',
               '{client.PAR_CIF_NIF}', 
               '{client.PAR_DIRECCION_1}', 
               '{client.PAR_CP_1}', 
               '{client.PAR_LOCALIDAD_1}', 
               '{client.PAR_PROVINCIA_1}', 
               '{client.PAR_PAIS_1}'
            );";

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
