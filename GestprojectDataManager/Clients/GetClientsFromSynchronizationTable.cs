using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class GetClientsFromSynchronizationTable
   {
      public List<GestprojectCustomer> GestprojectClientList { get; set; } = new List<GestprojectCustomer>();
      public GetClientsFromSynchronizationTable
      (
         System.Data.SqlClient.SqlConnection connection,
         List<int> SynchronizationTableclientsIds,
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               {tableSchema.GestprojectClientNameColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientCIFNIFColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientAddressColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientPostalCodeColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientLocalityColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientProvinceColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientCountryColumn.ColumnDatabaseName},
               {tableSchema.SynchronizationStatusColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientCompanyGroupNameColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientCompanyGroupCodeColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientCompanyGroupMainCodeColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnDatabaseName},
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientCodeColumn.ColumnDatabaseName},
               {tableSchema.Sage50ClientGuidIdColumn.ColumnDatabaseName},
               {tableSchema.CommentsColumn.ColumnDatabaseName}
            FROM 
               {tableSchema.TableName} 
            WHERE 
               {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName} IN ({string.Join(",", SynchronizationTableclientsIds)});";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     GestprojectDataManager.GestprojectCustomer client = new GestprojectCustomer();

                     client.PAR_NOMBRE = Convert.ToString(reader.GetValue(0));
                     client.PAR_CIF_NIF = Convert.ToString(reader.GetValue(1));
                     client.PAR_DIRECCION_1 = Convert.ToString(reader.GetValue(2));
                     client.PAR_CP_1 = Convert.ToString(reader.GetValue(3));
                     client.PAR_LOCALIDAD_1 = Convert.ToString(reader.GetValue(4));
                     client.PAR_PROVINCIA_1 = Convert.ToString(reader.GetValue(5));
                     client.PAR_PAIS_1 = Convert.ToString(reader.GetValue(6));
                     client.synchronization_status = Convert.ToString(reader.GetValue(7));
                     client.sage50_company_group_name = Convert.ToString(reader.GetValue(8));
                     client.sage50_company_group_code = Convert.ToString(reader.GetValue(9));
                     client.sage50_company_group_main_code = Convert.ToString(reader.GetValue(10));
                     client.sage50_company_group_guid_id = Convert.ToString(reader.GetValue(11));
                     client.PAR_ID = Convert.ToInt32(reader.GetValue(12));

                     client.sage50_client_code = System.Convert.ToString(reader.GetValue(13)) == "" || Convert.ToString(reader.GetValue(13)) == null || Convert.ToString(reader.GetValue(13)) == null ? "" : System.Convert.ToString(reader.GetValue(13));

                     client.sage50_guid_id = System.Convert.ToString(reader.GetValue(14)) == "" || Convert.ToString(reader.GetValue(14)) == null || Convert.ToString(reader.GetValue(14)) == null ? "" : System.Convert.ToString(reader.GetValue(14));

                     client.comments = System.Convert.ToString(reader.GetValue(15)) == "" || Convert.ToString(reader.GetValue(15)) == null || Convert.ToString(reader.GetValue(15)) == null ? "" : System.Convert.ToString(reader.GetValue(15));

                     GestprojectClientList.Add(client);
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
