using System;
using System.Data.SqlClient;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class WasParticipantRegistered
   {
      public bool ItIs { get; set; } = false;
      public WasParticipantRegistered
      (
         SqlConnection connection,
         string tableName,
         string participanSage50GuidColumnName,
         string participantGestprojectIdColumnName,
         int? participantGestprojectId
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
               SELECT 
                  {participantGestprojectIdColumnName}
               FROM 
                  {tableName}
               WHERE 
                  {participantGestprojectIdColumnName}={participantGestprojectId}
            ";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     if(reader.GetValue(0).GetType().Name != "DBNull" || System.Convert.ToString(reader.GetValue(0)) != "")
                     {
                        ItIs = true;
                        break;
                     }
                  };
               };
            };
         }
         catch(SqlException exception)
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
