//using System;
//using System.Data.SqlClient;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class RegisterParticipant
//   {
//      public RegisterParticipant
//      (
//         System.Data.SqlClient.SqlConnection connection,
//         string tableName,
         
//         string synchronizationStatusColumnName,
//         string gestprojectIdColumnName,
//         string fullNameColumnName,
//         string cifColumnName,
//         string addressColumnName,
//         string postalCodeColumnName,
//         string localityColumnName,
//         string provinceColumnName,
//         string countryColumnName,

//         int? participantGestprojectId,
//         string participantFullName,
//         string participantCif,
//         string participantAddress,
//         string participantPostalCode,
//         string participantLocality,
//         string participantProvince,
//         string participantCountry
//      )
//      {
//         try
//         {
//            connection.Open();
//            string sqlString2 = $@"
//            INSERT INTO {tableName} 
//            (
//               {synchronizationStatusColumnName}, 
//               {gestprojectIdColumnName},
//               {fullNameColumnName},
//               {cifColumnName},
//               {addressColumnName},
//               {postalCodeColumnName},
//               {localityColumnName},
//               {provinceColumnName},
//               {countryColumnName}
//            ) 
//            VALUES 
//            (
//               'Desincronizado',
//               {participantGestprojectId},
//               '{participantFullName}',
//               '{participantCif}', 
//               '{participantAddress}', 
//               '{participantPostalCode}', 
//               '{participantLocality}', 
//               '{participantProvince}', 
//               '{participantCountry}'
//            );";

//            using(SqlCommand sqlCommand = new SqlCommand(sqlString2, connection))
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
