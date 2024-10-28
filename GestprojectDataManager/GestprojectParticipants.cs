using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class GestprojectParticipants
   {
      private string tableColumnsNames { get; } = $@"
         PAR_ID,
         PAR_SUBCTA_CONTABLE,
         PAR_SUBCTA_CONTABLE_2,
         PAR_NOMBRE,
         PAR_NOMBRE_COMERCIAL,
         PAR_CIF_NIF,
         PAR_DIRECCION_1,
         PAR_CP_1,
         PAR_LOCALIDAD_1,
         PAR_PROVINCIA_1,
         PAR_PAIS_1,
         PAR_APELLIDO_1,
         PAR_APELLIDO_2
      ";
      private string tableName { get; } = "PARTICIPANTE";
      public List<GestprojectParticipantModel> Get(System.Data.SqlClient.SqlConnection connection, List<int> IdList = null)
      {
         try
         {
            connection.Open();

            List<GestprojectParticipantModel> gestprojectParticipantList = new List<GestprojectParticipantModel>();
            string sqlString = "";

            if(IdList == null)
            {
               sqlString = $@"
               SELECT
                  {tableColumnsNames}
               FROM
                  {tableName}
               ;";
            }
            else
            {
               sqlString = $@"
               SELECT
                  {tableColumnsNames}
               FROM
                  {tableName}
               WHERE PAR_ID IN ({string.Join(",", IdList)})
               ;";
            };

            using(System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sqlString, connection))
            {
               using(System.Data.SqlClient.SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     GestprojectParticipantModel participant = new GestprojectParticipantModel();

                     participant.PAR_ID = Convert.ToInt32(reader.GetValue(0).GetType().Name == "DBNull" ? -1 : reader.GetValue(0));
                     participant.PAR_SUBCTA_CONTABLE = Convert.ToString(reader.GetValue(1).GetType().Name == "DBNull" ? "" : reader.GetValue(1));
                     participant.PAR_SUBCTA_CONTABLE_2 = Convert.ToString(reader.GetValue(2).GetType().Name == "DBNull" ? "" : reader.GetValue(2));
                     participant.PAR_NOMBRE = Convert.ToString(reader.GetValue(3).GetType().Name == "DBNull" ? "" : reader.GetValue(3));
                     participant.PAR_NOMBRE_COMERCIAL = Convert.ToString(reader.GetValue(4).GetType().Name == "DBNull" ? "" : reader.GetValue(4));
                     participant.PAR_CIF_NIF = Convert.ToString(reader.GetValue(5).GetType().Name == "DBNull" ? "" : reader.GetValue(5));
                     participant.PAR_DIRECCION_1 = Convert.ToString(reader.GetValue(6).GetType().Name == "DBNull" ? "" : reader.GetValue(6));
                     participant.PAR_CP_1 = Convert.ToString(reader.GetValue(7).GetType().Name == "DBNull" ? "" : reader.GetValue(7));
                     participant.PAR_LOCALIDAD_1 = Convert.ToString(reader.GetValue(8).GetType().Name == "DBNull" ? "" : reader.GetValue(8));
                     participant.PAR_PROVINCIA_1 = Convert.ToString(reader.GetValue(9).GetType().Name == "DBNull" ? "" : reader.GetValue(9));
                     participant.PAR_PAIS_1 = Convert.ToString(reader.GetValue(10).GetType().Name == "DBNull" ? "" : reader.GetValue(10));
                     participant.PAR_APELLIDO_1 = Convert.ToString(reader.GetValue(11).GetType().Name == "DBNull" ? "" : reader.GetValue(11));
                     participant.PAR_APELLIDO_2 = Convert.ToString(reader.GetValue(12).GetType().Name == "DBNull" ? "" : reader.GetValue(12));

                     gestprojectParticipantList.Add(participant);
                  };
               };
            };
            return gestprojectParticipantList;
         }
         catch(SqlException exception)
         {
            throw new System.Exception(
               $"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.GestprojectParticipants\n.Get\r\n:\n\n{exception.Message}"
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
