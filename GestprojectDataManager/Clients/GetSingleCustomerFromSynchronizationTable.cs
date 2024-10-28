using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class GetSingleCustomerFromSynchronizationTable
   {
      public GestprojectCustomer GestprojectCustomer { get; set; } = new GestprojectCustomer();
      public GetSingleCustomerFromSynchronizationTable
      (
         System.Data.SqlClient.SqlConnection connection,
         GestprojectCustomer gestprojectCustomer,
         string companyGroupGuid,
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               {tableSchema.SynchronizationTableClientIdColumn.ColumnDatabaseName},
               {tableSchema.SynchronizationStatusColumn.ColumnDatabaseName},

               {tableSchema.Sage50ClientCodeColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientGuidIdColumn.ColumnDatabaseName},

               {tableSchema.Sage50ClientCompanyGroupNameColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientCompanyGroupCodeColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientCompanyGroupMainCodeColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnDatabaseName},

               {tableSchema.CommentsColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientParentUserIdColumn.ColumnDatabaseName},
               {tableSchema.ClientLastUpdateTerminalColumn.ColumnDatabaseName},
            FROM 
               {tableSchema.TableName} 
            WHERE 
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={gestprojectCustomer.PAR_ID}
            AND
               {tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnDatabaseName}='{companyGroupGuid}'               
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     GestprojectCustomer.synchronization_table_id = Convert.ToInt32(reader.GetValue(0).GetType().Name == "DBNull" ? -1 : reader.GetValue(0));
                     GestprojectCustomer.synchronization_status = Convert.ToString(reader.GetValue(1).GetType().Name == "DBNull" ? "" : reader.GetValue(1));

                     GestprojectCustomer.sage50_client_code = Convert.ToString(reader.GetValue(2).GetType().Name == "DBNull" ? "" : reader.GetValue(2));
                     GestprojectCustomer.sage50_client_code = Convert.ToString(reader.GetValue(3).GetType().Name == "DBNull" ? "" : reader.GetValue(3));

                     GestprojectCustomer.sage50_company_group_name = Convert.ToString(reader.GetValue(4).GetType().Name == "DBNull" ? "" : reader.GetValue(4));
                     GestprojectCustomer.sage50_company_group_code = Convert.ToString(reader.GetValue(5).GetType().Name == "DBNull" ? "" : reader.GetValue(5));
                     GestprojectCustomer.sage50_company_group_main_code = Convert.ToString(reader.GetValue(6).GetType().Name == "DBNull" ? "" : reader.GetValue(6));
                     GestprojectCustomer.sage50_company_group_guid_id = Convert.ToString(reader.GetValue(7).GetType().Name == "DBNull" ? "" : reader.GetValue(7));

                     GestprojectCustomer.comments = Convert.ToString(reader.GetValue(8).GetType().Name == "DBNull" ? "" : reader.GetValue(8));
                     GestprojectCustomer.parent_gesproject_user_id = Convert.ToInt32(reader.GetValue(9).GetType().Name == "DBNull" ? -1 : reader.GetValue(9));
                     GestprojectCustomer.last_record = Convert.ToDateTime(reader.GetValue(10).GetType().Name == "DBNull" ? DateTime.Now : reader.GetValue(10));
                  };
               };
            };
         }
         catch(System.Exception exception)
         {
            throw new System.Exception(
               $"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.GetClientsFromSynchronizationTable:\n\n{exception.Message}"
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
