using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class RegisterUpdatedSage50ClientData
   {
      public RegisterUpdatedSage50ClientData
      (
         System.Data.SqlClient.SqlConnection connection,
         int gestprojectClientId,
         string sage50ClientCode,
         string country,
         string name,
         string cif,
         string postalCode,
         string address,
         string province,
         CustomerSyncronizationTableSchema tableSchema
      ) 
      {
         try
         {
            connection.Open();

            string sqlString1 = $@"
            UPDATE {tableSchema.TableName} 
            SET 
               {tableSchema.SynchronizationStatusColumn.ColumnDatabaseName}='Sincronizado', 
               {tableSchema.GestprojectClientCountryColumn.ColumnDatabaseName}='{country}', 
               {tableSchema.GestprojectClientNameColumn.ColumnDatabaseName}='{name}', 
               {tableSchema.GestprojectClientCIFNIFColumn.ColumnDatabaseName}='{cif}',
               {tableSchema.GestprojectClientPostalCodeColumn.ColumnDatabaseName}='{postalCode}',
               {tableSchema.GestprojectClientAddressColumn.ColumnDatabaseName}='{address}',
               {tableSchema.GestprojectClientProvinceColumn.ColumnDatabaseName}='{province}',
               {tableSchema.GestprojectClientAccountableSubaccountColumn.ColumnDatabaseName}='{sage50ClientCode}',
               {tableSchema.Sage50ClientCodeColumn.ColumnDatabaseName}='{sage50ClientCode}'
            WHERE
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={gestprojectClientId}
            ;";


            using(SqlCommand sqlCommand = new SqlCommand(sqlString1, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };

            string sqlString2 = $@"
            UPDATE {"PARTICIPANTE"}
            SET
               {tableSchema.GestprojectClientAccountableSubaccountColumn.ColumnDatabaseName}='{sage50ClientCode}'
            WHERE
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={gestprojectClientId}
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
