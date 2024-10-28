using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class GestprojectUserIdAndDevice
   {
      public IdsAndDeviceTerminalNames Get(
      System.Data.SqlClient.SqlConnection connection,
      string userName,
      string deviceName
      )
      {

         try
         {
            connection.Open();


            List<int> userIds = new List<int>();
            List<string> deviceTerminalNames = new List<string>();

            string sqlString1 = "SELECT USU_ID FROM USUARIO";
            using(SqlCommand command1 = new SqlCommand(sqlString1, connection))
            {
               using(SqlDataReader reader1 = command1.ExecuteReader())
               {
                  while(reader1.Read())
                  {
                     userIds.Add(reader1.GetInt32(0));
                  }
               }
            };


            ////// Where par_id!!!! matches par_id in usuario
            string sqlString2 = $"SELECT CNX_EQUIPO FROM CONEXIONES_ACTIVAS WHERE CNX_USUARIO={userName}";
            using(SqlCommand command2 = new SqlCommand(sqlString2, connection))
            {
               using(SqlDataReader reader2 = command2.ExecuteReader())
               {
                  while(reader2.Read())
                  {
                     deviceTerminalNames.Add(reader2.GetString(0));
                  }
               }
            }
            IdsAndDeviceTerminalNames containerClass = new IdsAndDeviceTerminalNames();
            containerClass.UserIds = userIds;
            containerClass.deviceTerminalNames = deviceTerminalNames;
            return containerClass;
         }
         catch(SqlException ex)
         {
            MessageBox.Show($"Error: \n\n{ex.Message}");
            return null;
         }
         finally
         {
            connection.Close();
         };



      }
   }
}
