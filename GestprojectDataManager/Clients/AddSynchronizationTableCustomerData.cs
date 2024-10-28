using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class AddSynchronizationTableCustomerData
   {
      public AddSynchronizationTableCustomerData
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
               {tableSchema.ClientLastUpdateTerminalColumn.ColumnDatabaseName}
            FROM 
               {tableSchema.TableName} 
            WHERE 
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={gestprojectCustomer.PAR_ID}              
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     gestprojectCustomer.synchronization_table_id = Convert.ToInt32(reader.GetValue(0).GetType().Name == "DBNull" ? null : reader.GetValue(0));
                     gestprojectCustomer.synchronization_status = Convert.ToString(reader.GetValue(1).GetType().Name == "DBNull" ? "" : reader.GetValue(1));

                     gestprojectCustomer.sage50_client_code = Convert.ToString(reader.GetValue(2).GetType().Name == "DBNull" ? "" : reader.GetValue(2));
                     gestprojectCustomer.sage50_guid_id = Convert.ToString(reader.GetValue(3).GetType().Name == "DBNull" ? "" : reader.GetValue(3));

                     gestprojectCustomer.sage50_company_group_name = Convert.ToString(reader.GetValue(4).GetType().Name == "DBNull" ? "" : reader.GetValue(4));
                     gestprojectCustomer.sage50_company_group_code = Convert.ToString(reader.GetValue(5).GetType().Name == "DBNull" ? "" : reader.GetValue(5));
                     gestprojectCustomer.sage50_company_group_main_code = Convert.ToString(reader.GetValue(6).GetType().Name == "DBNull" ? "" : reader.GetValue(6));
                     gestprojectCustomer.sage50_company_group_guid_id = Convert.ToString(reader.GetValue(7).GetType().Name == "DBNull" ? "" : reader.GetValue(7));

                     gestprojectCustomer.comments = Convert.ToString(reader.GetValue(8).GetType().Name == "DBNull" ? "" : reader.GetValue(8));
                     gestprojectCustomer.comments = gestprojectCustomer.comments.Length > 2000 ? gestprojectCustomer.comments.Substring(0,2000) : gestprojectCustomer.comments;

                     gestprojectCustomer.parent_gesproject_user_id = Convert.ToInt32(reader.GetValue(9).GetType().Name == "DBNull" ? null : reader.GetValue(9));
                     gestprojectCustomer.last_record = Convert.ToDateTime(reader.GetValue(10).GetType().Name == "DBNull" ? null : reader.GetValue(10));
                  };
               };
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
