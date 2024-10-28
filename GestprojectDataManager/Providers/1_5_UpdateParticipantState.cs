//using System;
//using System.Data.SqlClient;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class UpdateParticipantState
//   {
//      public UpdateParticipantState
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

//         string participantSynchronizationStatus,
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

//            string sqlString = $@"
//            UPDATE 
//               {tableName} 
//            SET
//               {synchronizationStatusColumnName}='{participantSynchronizationStatus}',
//               {fullNameColumnName}='{participantFullName}',
//               {cifColumnName}='{participantCif}',
//               {addressColumnName}='{participantAddress}',
//               {postalCodeColumnName}='{participantPostalCode}',
//               {localityColumnName}='{participantLocality}',
//               {provinceColumnName}='{participantProvince}',
//               {countryColumnName}='{participantCountry}'
//            WHERE
//               {gestprojectIdColumnName}={participantGestprojectId}
//            ;";

//            //MessageBox.Show(sqlString);

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
