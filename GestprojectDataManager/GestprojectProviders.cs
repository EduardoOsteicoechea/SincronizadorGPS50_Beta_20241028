//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class GestprojectProviders
//   {
//      public bool IsSuccssesful { get; set; } = false;
//      public List<GestprojectProviderModel> Get(System.Data.SqlClient.SqlConnection connection, List<int> IdList = null)
//      {
//         try
//         {
//            connection.Open();

//            List<int> gestProjectProviderIdList = new List<int>();
//            List<GestprojectProviderModel> gestprojectProviderList = new List<GestprojectProviderModel>();

//            string sqlString = "";

//            if(IdList == null)
//            {
//               sqlString = "SELECT * FROM PAR_TPA;";
//            }
//            else
//            {
//               sqlString = $"SELECT * FROM PAR_TPA WHERE ID IN ({string.Join(",", IdList)});";
//            };

//            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
//            {
//               using(SqlDataReader reader = sqlCommand.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     if(reader.GetValue(0).ToString() == "12")
//                     {
//                        gestProjectProviderIdList.Add(Convert.ToInt32(reader.GetValue(1)));
//                     };
//                  };
//                  gestProjectProviderIdList.Distinct().ToList();
//               };
//            };

//            List<GestprojectParticipantModel> gestprojectClientParticipantList = new GestprojectParticipants().Get(connection, gestProjectProviderIdList);

//            for(global::System.Int32 i = 0; i < gestprojectClientParticipantList.Count; i++)
//            {
//               GestprojectParticipantModel gestprojectClientParticipant = gestprojectClientParticipantList[i];
//               GestprojectProviderModel gestprojectProvider = new GestprojectProviderModel();

//               gestprojectProvider.PAR_ID = gestprojectClientParticipant.PAR_ID;
//               gestprojectProvider.PAR_SUBCTA_CONTABLE_2 = gestprojectClientParticipant.PAR_SUBCTA_CONTABLE_2;
//               gestprojectProvider.PAR_NOMBRE = gestprojectClientParticipant.PAR_NOMBRE;
//               gestprojectProvider.PAR_NOMBRE_COMERCIAL = gestprojectClientParticipant.PAR_NOMBRE_COMERCIAL;
//               gestprojectProvider.PAR_CIF_NIF = gestprojectClientParticipant.PAR_CIF_NIF;
//               gestprojectProvider.PAR_DIRECCION_1 = gestprojectClientParticipant.PAR_DIRECCION_1;
//               gestprojectProvider.PAR_CP_1 = gestprojectClientParticipant.PAR_CP_1;
//               gestprojectProvider.PAR_LOCALIDAD_1 = gestprojectClientParticipant.PAR_LOCALIDAD_1;
//               gestprojectProvider.PAR_PROVINCIA_1 = gestprojectClientParticipant.PAR_PROVINCIA_1;
//               gestprojectProvider.PAR_PAIS_1 = gestprojectClientParticipant.PAR_PAIS_1;

//               gestprojectProviderList.Add(gestprojectProvider);
//            };

//            IsSuccssesful = true;

//            return gestprojectProviderList;
//         }
//         catch(SqlException ex)
//         {
//            MessageBox.Show($"Error: \n\n{ex.Message}");
//            return null;
//         }
//         finally
//         {
//            connection.Close();
//         };
//      }
//   }
//}
