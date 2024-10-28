using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class WasGestprojectClientRegistered
   {
      public bool ItIs { get; set; } = false;
      public int? GP_USU_ID { get; set; } = null;
      public WasGestprojectClientRegistered
      (
         System.Data.SqlClient.SqlConnection connection, 
         GestprojectDataManager.GestprojectCustomer client,
         string currentCompanyGroupGuid,
         GestprojectDataManager.CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
         {
            connection.Open();

            string whereClause = "";

            if(client.sage50_guid_id != "" && client.sage50_guid_id != null) 
            {
               whereClause = $@"
                  {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={client.PAR_ID}
                  AND
                  {tableSchema.Sage50ClientGuidIdColumn.ColumnDatabaseName}='{client.sage50_guid_id}'
               ";
            }
            else 
            {
               whereClause = $@"
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName}={client.PAR_ID}
               ";
            };

            string sqlString = $@"
               SELECT 
                  {tableSchema.SynchronizationTableClientIdColumn.ColumnDatabaseName}
               FROM 
                  {tableSchema.TableName}
               WHERE 
                  {whereClause}
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     if(reader.GetValue(0).GetType().Name != "DBNull")
                     {
                        ItIs = true;
                        break;
                     }
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
