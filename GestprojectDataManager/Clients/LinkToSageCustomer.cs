using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class LinkToSageCustomer
   {
      public LinkToSageCustomer
      (
         System.Data.SqlClient.SqlConnection connection,
         GestprojectCustomer gestprojectCustomer,
         CustomerSyncronizationTableSchema tableSchema

      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            UPDATE
               {tableSchema.TableName} 
            SET
               {tableSchema.Sage50ClientCodeColumn.ColumnDatabaseName}='{gestprojectCustomer.sage50_client_code}',
               {tableSchema.Sage50ClientGuidIdColumn.ColumnDatabaseName}='{gestprojectCustomer.sage50_guid_id}',
               {tableSchema.Sage50ClientCompanyGroupNameColumn.ColumnDatabaseName}='{gestprojectCustomer.sage50_company_group_name}',
               {tableSchema.Sage50ClientCompanyGroupCodeColumn.ColumnDatabaseName}='{gestprojectCustomer.sage50_company_group_code}',
               {tableSchema.Sage50ClientCompanyGroupMainCodeColumn.ColumnDatabaseName}='{gestprojectCustomer.sage50_company_group_main_code}',
               {tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnDatabaseName}='{gestprojectCustomer.sage50_company_group_guid_id}'
            WHERE
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={gestprojectCustomer.PAR_ID}              
            ;";

            MessageBox.Show(sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };
         }
         catch(System.Exception exception)
         {
            throw new System.Exception($"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.LinkToSageCustomer:\n\n{exception.Message}");
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
