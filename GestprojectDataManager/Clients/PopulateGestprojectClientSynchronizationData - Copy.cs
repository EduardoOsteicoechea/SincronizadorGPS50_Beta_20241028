//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using static sage.ew.docsven.FirmaElectronica;

//namespace SincronizadorGPS50.Workflows.Clients
//{
//   internal class PopulateGestprojectClientSynchronizationData
//   {
//      internal PopulateGestprojectClientSynchronizationData(GestprojectDataManager.GestprojectClient client)
//      {
//         using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
//         {
//            try
//            {
//               connection.Open();

//               string sqlString = @"
//                    SELECT 
//                        id, 
//                        synchronization_status, 
//                        sage50_code, 
//                        sage50_guid_id 
//                    FROM 
//                        INT_SAGE_SINC_CLIENTE "
//                    +
//                    $"WHERE gestproject_id={client.PAR_ID};";

//               using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
//               {
//                  using(SqlDataReader reader = sqlCommand.ExecuteReader())
//                  {
//                     while(reader.Read())
//                     {
//                        client.synchronization_table_id = (int)reader.GetValue(0);
//                        client.synchronization_status = (string)reader.GetValue(1);
//                        client.PAR_SUBCTA_CONTABLE = (string)reader.GetValue(2);
//                        client.sage50_client_code = (string)reader.GetValue(2);
//                        client.sage50_guid_id = (string)reader.GetValue(3);
//                     };
//                  };
//               };
//            }
//            catch(SqlException ex)
//            {
//               MessageBox.Show($"Error during data retrieval: \n\n{ex.Message}");
//            }
//            finally
//            {
//               connection.Close();
//            };
//         };
//      }
//   }
//}
