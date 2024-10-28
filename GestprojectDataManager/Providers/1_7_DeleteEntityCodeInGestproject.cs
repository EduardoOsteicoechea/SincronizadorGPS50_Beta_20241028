//using System;
//using System.Data.SqlClient;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class DeleteEntityCodeInGestproject
//   {
//      public DeleteEntityCodeInGestproject
//      (
//         SqlConnection connection,
//         int? entityId,
//         string tableName,
//         string columnToClearName,
//         string entityIdColumnName
//      ) 
//      {
//         try
//         {
//            connection.Open();

//            string sqlString = $@"
//            UPDATE 
//               {tableName}
//            SET
//               {columnToClearName}=''
//            WHERE
//               {entityIdColumnName}={entityId}
//            ;";

//            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
//            {
//               sqlCommand.ExecuteNonQuery();
//            };
//         }
//         catch(SqlException exception)
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
