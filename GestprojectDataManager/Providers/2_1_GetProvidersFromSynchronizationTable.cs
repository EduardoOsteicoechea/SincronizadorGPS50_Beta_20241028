//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class GetProvidersFromSynchronizationTable
//   {
//      public List<GestprojectProviderModel> GestprojectEntityList { get; set; } = new List<GestprojectProviderModel>();
//      public GetProvidersFromSynchronizationTable
//      (
//         System.Data.SqlClient.SqlConnection connection,
//         List<int> SynchronizationTableEntitiesIds,

//         string synchronizationTableName,

//         string fullNameColumnName,
//         string cifColumnName,
//         string addressColumnName,
//         string postalCodeColumnName,
//         string LocalityColumnName,
//         string ProvinceColumnName,
//         string countryColumnName,
//         string synchronizationStatusColumnName,
//         string companyGroupNameColumnName,
//         string companyGroupCodeColumnName,
//         string companyGroupMainCodeColumnName,
//         string companyGroupGuidColumnName,
//         string gestprojectIdColumnName,
//         string sage50CodeColumnName,
//         string sage50GuidColumnName,
//         string commentsColumnName
//      )
//      {
//         try
//         {
//            connection.Open();

//            string sqlString = $@"
//            SELECT 
//               {fullNameColumnName},
//               {cifColumnName},
//               {addressColumnName},
//               {postalCodeColumnName},
//               {LocalityColumnName},
//               {ProvinceColumnName},
//               {countryColumnName},
//               {synchronizationStatusColumnName},
//               {companyGroupNameColumnName},
//               {companyGroupCodeColumnName},
//               {companyGroupMainCodeColumnName},
//               {companyGroupGuidColumnName},
//               {gestprojectIdColumnName},
//               {sage50CodeColumnName},
//               {sage50GuidColumnName},
//               {commentsColumnName}
//            FROM 
//               {synchronizationTableName} 
//            WHERE 
//               {gestprojectIdColumnName} IN ({string.Join(",", SynchronizationTableEntitiesIds)});";

//            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
//            {
//               using(SqlDataReader reader = sqlCommand.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     GestprojectProviderModel Provider = new GestprojectProviderModel();

//                     Provider.PAR_NOMBRE = Convert.ToString(reader.GetValue(0));
//                     Provider.PAR_CIF_NIF = Convert.ToString(reader.GetValue(1));
//                     Provider.PAR_DIRECCION_1 = Convert.ToString(reader.GetValue(2));
//                     Provider.PAR_CP_1 = Convert.ToString(reader.GetValue(3));
//                     Provider.PAR_LOCALIDAD_1 = Convert.ToString(reader.GetValue(4));
//                     Provider.PAR_PROVINCIA_1 = Convert.ToString(reader.GetValue(5));
//                     Provider.PAR_PAIS_1 = Convert.ToString(reader.GetValue(6));
//                     Provider.synchronization_status = Convert.ToString(reader.GetValue(7));
//                     Provider.sage50_company_group_name = Convert.ToString(reader.GetValue(8));
//                     Provider.sage50_company_group_code = Convert.ToString(reader.GetValue(9));
//                     Provider.sage50_company_group_main_code = Convert.ToString(reader.GetValue(10));
//                     Provider.sage50_company_group_guid_id = Convert.ToString(reader.GetValue(11));
//                     Provider.PAR_ID = Convert.ToInt32(reader.GetValue(12));

//                     Provider.sage50_code = System.Convert.ToString(reader.GetValue(13)) == "" || Convert.ToString(reader.GetValue(13)) == null || Convert.ToString(reader.GetValue(13)) == null ? "" : System.Convert.ToString(reader.GetValue(13));

//                     Provider.sage50_guid_id = System.Convert.ToString(reader.GetValue(14)) == "" || Convert.ToString(reader.GetValue(14)) == null || Convert.ToString(reader.GetValue(14)) == null ? "" : System.Convert.ToString(reader.GetValue(14));

//                     Provider.comments = System.Convert.ToString(reader.GetValue(15)) == "" || Convert.ToString(reader.GetValue(15)) == null || Convert.ToString(reader.GetValue(15)) == null ? "" : System.Convert.ToString(reader.GetValue(15));

//                     GestprojectEntityList.Add(Provider);
//                  };
//               };
//            };
//         }
//         catch(System.Exception exception)
//         {
//            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
//         }
//         finally
//         {
//            connection.Close();
//         };
//      }
//   }
//}
