//using System;
//using System.Data.SqlClient;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class AppendSynchronizationTableDataToProvider
//   {
//      public AppendSynchronizationTableDataToProvider
//      (
//         SqlConnection connection,
//         GestprojectProviderModel entity,
//         string tableName,
//         string syncronizationTableIdColumnName,
//         string syncronizationStatusColumnName,
//         string syncronizationSage50CodeTableIdColumnName,
//         string syncronizationSage50GuidColumnName,
//         string syncronizationCompanyGroupNameColumnName,
//         string syncronizationCompanyGroupCodeColumnName,
//         string syncronizationCompanyGroupMainCodeColumnName,
//         string syncronizationCompanyGroupGuidColumnName,
//         string syncronizationTableParentUserColumnName,
//         string syncronizationTableLastUpdateColumnName,
//         string syncronizationCommentsColumnName,
//         string gestprojectProviderColumnName,
//         int? entityId
//      )
//      {
//         try
//         {
//            connection.Open();

//            string sqlString = $@"
//            SELECT 
//               {syncronizationTableIdColumnName},
//               {syncronizationStatusColumnName},

//               {syncronizationSage50CodeTableIdColumnName},
//               {syncronizationSage50GuidColumnName},

//               {syncronizationCompanyGroupNameColumnName},
//               {syncronizationCompanyGroupCodeColumnName},
//               {syncronizationCompanyGroupMainCodeColumnName},
//               {syncronizationCompanyGroupGuidColumnName},

//               {syncronizationTableParentUserColumnName},
//               {syncronizationTableLastUpdateColumnName},
//               {syncronizationCommentsColumnName}
//            FROM 
//               {tableName} 
//            WHERE 
//               {gestprojectProviderColumnName}={entityId}              
//            ;";

//            //MessageBox.Show(sqlString);

//            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
//            {
//               using(SqlDataReader reader = sqlCommand.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     entity.synchronization_table_id = Convert.ToInt32(reader.GetValue(0).GetType().Name == "DBNull" ? null : reader.GetValue(0));
//                     entity.synchronization_status = Convert.ToString(reader.GetValue(1).GetType().Name == "DBNull" ? "" : reader.GetValue(1));

//                     entity.sage50_code = Convert.ToString(reader.GetValue(2).GetType().Name == "DBNull" ? "" : reader.GetValue(2));
//                     entity.sage50_guid_id = Convert.ToString(reader.GetValue(3).GetType().Name == "DBNull" ? "" : reader.GetValue(3));

//                     entity.sage50_company_group_name = Convert.ToString(reader.GetValue(4).GetType().Name == "DBNull" ? "" : reader.GetValue(4));
//                     entity.sage50_company_group_code = Convert.ToString(reader.GetValue(5).GetType().Name == "DBNull" ? "" : reader.GetValue(5));
//                     entity.sage50_company_group_main_code = Convert.ToString(reader.GetValue(6).GetType().Name == "DBNull" ? "" : reader.GetValue(6));
//                     entity.sage50_company_group_guid_id = Convert.ToString(reader.GetValue(7).GetType().Name == "DBNull" ? "" : reader.GetValue(7));

//                     entity.parent_gesproject_user_id = Convert.ToInt32(reader.GetValue(8).GetType().Name == "DBNull" ? null : reader.GetValue(8));
//                     entity.last_record = Convert.ToDateTime(reader.GetValue(9).GetType().Name == "DBNull" ? null : reader.GetValue(9));

//                     entity.comments = Convert.ToString(reader.GetValue(10).GetType().Name == "DBNull" ? "" : reader.GetValue(10));
//                     entity.comments = entity.comments.Length > 2000 ? entity.comments.Substring(0, 2000) : entity.comments;
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
