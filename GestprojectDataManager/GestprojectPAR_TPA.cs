using SincronizadorGPS50.GestprojectDataManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestprojectDataManager
{
   public class GestprojectPAR_TPA
   {
      public List<int> Get(System.Data.SqlClient.SqlConnection connection, string participantTypeNumber, List<int> IdList = null)
      {
         try
         {
            connection.Open();

            List<int> gestProjectClientIdList = new List<int>();

            string sqlString = "";

            if(IdList == null)
            {
               sqlString = "SELECT * FROM PAR_TPA;";
            }
            else
            {
               sqlString = $"SELECT * FROM PAR_TPA WHERE ID IN ({string.Join(",", IdList)});";
            };

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     if(reader.GetValue(0).ToString() == participantTypeNumber)
                     {
                        gestProjectClientIdList.Add(Convert.ToInt32(reader.GetValue(1)));
                     };
                  };
                  gestProjectClientIdList.Distinct().ToList();
               };
            };

            return gestProjectClientIdList;
         }
         catch(SqlException exception)
         {
            throw new System.Exception(
               $"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.GestprojectPAR_TPA\n.Get\r\n:\n\n{exception.Message}"
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
