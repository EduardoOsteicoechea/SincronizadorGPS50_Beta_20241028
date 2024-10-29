using System;
using System.Collections.Generic;
using System.IO;

namespace SincronizadorGPS50
{
   public class GestprojectSessionSettings
   {
      private static string BaseLocalApplicationGestprojectDataFolder { get; set; } = 
      System.IO.Path.Combine(
         Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
         @"Micad\Gestproject"
      );
      public UserSessionData userSessionData { get; set; } = null;
      public GestprojectSessionSettings
      (
         System.Data.SqlClient.SqlConnection connection, 
         string currentUser
      )
      {
         try
         {
            connection.Open();

            //0.After connecting to Gestproject database,
            //1.Get distinct "CNX_USUARIO" rows "FROM [GESTPROJECT2020].[dbo].[CONEXIONES]".

            string sql1 = "SELECT DISTINCT CNX_USUARIO FROM [GESTPROJECT2020].[dbo].[CONEXIONES]; ";
            List<string> distinctUsersList = new List<string>();
            using(System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql1, connection))
            {
               using(System.Data.SqlClient.SqlDataReader reader2 = sqlCommand.ExecuteReader())
               {
                  while(reader2.Read())
                  {
                     distinctUsersList.Add(reader2.GetString(0));
                  };
               };
            };

            // 2.2. Get "CNX_EQUIPO" matching to obtain it's lenght

            string sql1_2 = $"SELECT TOP 1 CNX_EQUIPO FROM [GESTPROJECT2020].[dbo].[CONEXIONES] WHERE CNX_USUARIO='{currentUser}';";
            string connectionUserDevice = "";
            using(System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql1_2, connection))
            {
               using(System.Data.SqlClient.SqlDataReader reader2 = sqlCommand.ExecuteReader())
               {
                  while(reader2.Read())
                  {
                     connectionUserDevice = reader2.GetString(0);
                     break;
                  };
               };
            };

            //5.Get and store "CNX_EQUIPO" matching that "CNX_CODIGO" "FROM [GESTPROJECT2020].[dbo].[CONEXIONES]".

            string sql3 = $"SELECT CNX_PERSONAL, CNX_PERFIL, CNX_ID FROM [GESTPROJECT2020].[dbo].[CONEXIONES] WHERE CNX_EQUIPO='{connectionUserDevice}';";
            string connectedPersonalUserName = "";
            string connectedProfile = "";
            int connectionId = 0;
            using(System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql3, connection))
            {
               using(System.Data.SqlClient.SqlDataReader reader2 = sqlCommand.ExecuteReader())
               {
                  while(reader2.Read())
                  {
                     connectedPersonalUserName = reader2.GetString(0);
                     connectedProfile = reader2.GetString(1);
                     connectionId = reader2.GetInt32(2);
                     break;
                  };
               };
            };

            //6.Get and store "CNX_EQUIPO" matching that "CNX_CODIGO" "FROM [GESTPROJECT2020].[dbo].[CONEXIONES]".

            string sql4 = $"SELECT USU_ID FROM [GESTPROJECT2020].[dbo].[USUARIO] WHERE USU_LOGIN='{currentUser.ToLower()}';";
            int? gestprojectSessionUserId = null;
            using(System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sql4, connection))
            {
               using(System.Data.SqlClient.SqlDataReader reader2 = sqlCommand.ExecuteReader())
               {
                  while(reader2.Read())
                  {
                     gestprojectSessionUserId = reader2.GetInt32(0);
                  };
               };
            };

            //7.Expose an "UserSessionData" instance containing all the obtained data, to determine Rememberable data to be retrieved.

            userSessionData = new UserSessionData();
            userSessionData.CNX_USUARIO = currentUser;
            userSessionData.CNX_PERSONAL = connectedPersonalUserName;
            userSessionData.CNX_EQUIPO = connectionUserDevice;
            userSessionData.CNX_PERFIL = connectedProfile;
            userSessionData.CNX_ID = connectionId;
            userSessionData.CNX_USUARIO = currentUser;
            userSessionData.USU_ID = gestprojectSessionUserId;
         }
         catch(System.Exception exception)
         {
            throw exception;
         }
         finally
         {
            connection.Close();
         };
      }

      private static string GetLatestVersionFolderName(string folderToBeAnalizedPath, string defaultVersionValue = "12.0.0.0")
      {
         DirectoryInfo folderToBeAnalizedInformation = new DirectoryInfo( folderToBeAnalizedPath );
         if(folderToBeAnalizedInformation.Exists)
         {
            System.Collections.SortedList folders = new System.Collections.SortedList();
            foreach(DirectoryInfo directoryInfo in folderToBeAnalizedInformation.GetDirectories())
            {
               folders.Add(directoryInfo.Name, directoryInfo.Name);
            };

            if(folders.Count > 0)
            {
               return Path.Combine(folderToBeAnalizedPath, folders.GetKey(folders.Count - 1).ToString());
            }
            else
            {
               return Path.Combine(folderToBeAnalizedPath, defaultVersionValue);
            };
         }
         else
         {
            return Path.Combine(folderToBeAnalizedPath, defaultVersionValue);
         };
      }
      private void DisposeSensitiveData()
      {
         BaseLocalApplicationGestprojectDataFolder = "";
      }
   }
}
