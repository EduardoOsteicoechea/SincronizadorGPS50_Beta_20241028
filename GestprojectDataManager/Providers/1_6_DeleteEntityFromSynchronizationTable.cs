//using System;
//using System.Data.SqlClient;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class DeleteEntityFromSynchronizationTable
//   {
//      public DeleteEntityFromSynchronizationTable
//      (
//         System.Data.SqlClient.SqlConnection connection,
//         int? entityId,
//         string entitySageGuid,
//         string tableName,
//         string entityIdColumnName,
//         string entitySage50GuidColumnName
//      )
//      {
//         try
//         {
//            connection.Open();

//            string sqlString = $@"
//            DELETE FROM 
//               {tableName} 
//            WHERE 
//               {entityIdColumnName}={entityId}
//            AND
//               {entitySage50GuidColumnName}='{entitySageGuid}'
//            ;";

//            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
//            {
//               sqlCommand.ExecuteNonQuery();
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
