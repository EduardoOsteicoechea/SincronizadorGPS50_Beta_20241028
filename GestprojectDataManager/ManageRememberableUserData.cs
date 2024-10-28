using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public static class ManageRememberableUserData
   {
      public static string GestprojectSynchronizatorUserDataTableName { get; set; } = "INT_SAGE_USERDATA";

      public static string ID { get; set; } = "ID";
      public static string GP_CNX_ID { get; set; } = "GP_CNX_ID";
      public static string GP_CNX_PERSONAL { get; set; } = "GP_CNX_PERSONAL";
      public static string GP_CNX_USUARIO { get; set; } = "GP_CNX_USUARIO";
      public static string GP_CNX_PERFIL { get; set; } = "GP_CNX_PERFIL";
      public static string GP_CNX_EQUIPO { get; set; } = "GP_CNX_EQUIPO";
      public static string GP_USU_ID { get; set; } = "GP_USU_ID";
      public static string SAGE_50_LOCAL_TERMINAL_PATH { get; set; } = "SAGE_50_LOCAL_TERMINAL_PATH";
      public static string SAGE_50_USER_NAME { get; set; } = "SAGE_50_USER_NAME";
      public static string SAGE_50_PASSWORD { get; set; } = "SAGE_50_PASSWORD";
      public static string SAGE_50_COMPANY_GROUP_NAME { get; set; } = "SAGE_50_COMPANY_GROUP_NAME";
      public static string SAGE_50_COMPANY_GROUP_MAIN_CODE { get; set; } = "SAGE_50_COMPANY_GROUP_MAIN_CODE";
      public static string SAGE_50_COMPANY_GROUP_CODE { get; set; } = "SAGE_50_COMPANY_GROUP_CODE";
      public static string SAGE_50_COMPANY_GROUP_GUID_ID { get; set; } = "SAGE_50_COMPANY_GROUP_GUID_ID";
      public static string REMEMBER { get; set; } = "REMEMBER";
      public static string LAST_UPDATE { get; set; } = "LAST_UPDATE";

      public static bool Save
      (
          System.Data.SqlClient.SqlConnection connection,
          int? gestprojectConnectionId,
          string gestprojectConnectionPersonalName,
          string gestprojectConnectionUser,
          string gestprojectConnectionProfile,
          string gestprojectConnectionDevice,
          int? gestprojectRecordedUserId,
          string sage50LocalTerminalPath,
          string sage50Username,
          string sage50Password,
          string selectedCompanyGroupName,
          string selectedCompanyGroupMainCode,
          string selectedCompanyGroupCode,
          string selectedCompanyGroupGuidId
      )
      {
         try
         {
            PopulateGestprojectUserDataTable(
               connection,
               gestprojectConnectionId,
               gestprojectConnectionPersonalName,
               gestprojectConnectionUser,
               gestprojectConnectionProfile,
               gestprojectConnectionDevice,
               gestprojectRecordedUserId,
               sage50LocalTerminalPath,
               sage50Username,
               sage50Password,
               selectedCompanyGroupName,
               selectedCompanyGroupMainCode,
               selectedCompanyGroupCode,
               selectedCompanyGroupGuidId
            );
            return true;
         }
         catch(SqlException ex)
         {
            MessageBox.Show($"Error: \n\n{ex.Message}");
            return false;
         };
      }

      public static bool CheckIfGestprojectUserDataTableExists(System.Data.SqlClient.SqlConnection connection)
      {
         try
         {
            connection.Open();

            string sqlString = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE \"TABLE_NAME\" = '{GestprojectSynchronizatorUserDataTableName}'";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               int? Sage50SincronizationTableCount = (int)sqlCommand.ExecuteScalar();
               if(Sage50SincronizationTableCount != null)
               {
                  return Sage50SincronizationTableCount > 0;
               }
               else
               {
                  return false;
               };
            };
         }
         catch(SqlException exception)
         {
            throw exception;
         }
         finally
         {
            connection.Close();
         };
      }

      public static bool CreateGestprojectUserDataTable(System.Data.SqlClient.SqlConnection connection)
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            CREATE TABLE 
               {GestprojectSynchronizatorUserDataTableName} 
               (
                  {ID} INT PRIMARY KEY IDENTITY(1,1),
                  {GP_CNX_ID} INT,
                  {GP_CNX_PERSONAL} VARCHAR(MAX),
                  {GP_CNX_USUARIO} VARCHAR(MAX),
                  {GP_CNX_PERFIL} VARCHAR(MAX),
                  {GP_CNX_EQUIPO} VARCHAR(MAX),
                  {GP_USU_ID} VARCHAR(MAX),
                  {SAGE_50_LOCAL_TERMINAL_PATH} VARCHAR(MAX), 
                  {SAGE_50_USER_NAME} VARCHAR(MAX), 
                  {SAGE_50_PASSWORD} VARCHAR(MAX),
                  {SAGE_50_COMPANY_GROUP_NAME} VARCHAR(MAX),
                  {SAGE_50_COMPANY_GROUP_MAIN_CODE} VARCHAR(MAX),
                  {SAGE_50_COMPANY_GROUP_CODE} VARCHAR(MAX),
                  {SAGE_50_COMPANY_GROUP_GUID_ID} VARCHAR(MAX),
                  {REMEMBER} BIT,
                  {LAST_UPDATE} DATETIME DEFAULT GETDATE() NOT NULL
               )
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };

            return true;
         }
         catch(SqlException exception)
         {
            throw exception;
         }
         finally
         {
            connection.Close();
         };
      }

      public static bool PopulateGestprojectUserDataTable
      (
          System.Data.SqlClient.SqlConnection connection,
          int? GestprojectConnectionId,
          string gestprojectConnectionPersonalName,
          string GestprojectConnectionUser,
          string GestprojectConnectionProfile,
          string GestprojectConnectionDevice,
          int? GestprojectRecordedUserId,
          string sage50LocalTerminalPath,
          string sage50Username,
          string sage50Password,
          string selectedCompanyGroupName,
          string selectedCompanyGroupMainCode,
          string selectedCompanyGroupCode,
          string selectedCompanyGroupGuidId
      )
      {
         try
         {
            connection.Open();

            string sqlString1 = $@"DELETE FROM {GestprojectSynchronizatorUserDataTableName} WHERE {GP_CNX_USUARIO}='{GestprojectConnectionUser}' AND {GP_CNX_EQUIPO}='{GestprojectConnectionDevice}' AND {GP_USU_ID}={GestprojectRecordedUserId};";
            using(SqlCommand sqlCommand = new SqlCommand(sqlString1, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };

            string sqlString = $@"
               INSERT INTO {GestprojectSynchronizatorUserDataTableName}
               (
               {GP_CNX_ID},
               {GP_CNX_PERSONAL},
               {GP_CNX_USUARIO},
               {GP_CNX_PERFIL},
               {GP_CNX_EQUIPO},
               {GP_USU_ID},
               {SAGE_50_LOCAL_TERMINAL_PATH},
               {SAGE_50_USER_NAME},
               {SAGE_50_PASSWORD},
               {SAGE_50_COMPANY_GROUP_NAME},
               {SAGE_50_COMPANY_GROUP_MAIN_CODE},
               {SAGE_50_COMPANY_GROUP_CODE},
               {SAGE_50_COMPANY_GROUP_GUID_ID},
               {REMEMBER}
               )
               VALUES
               (
               {GestprojectConnectionId},
               '{gestprojectConnectionPersonalName.Trim()}',
               '{GestprojectConnectionUser.Trim()}',
               '{GestprojectConnectionProfile.Trim()}',
               '{GestprojectConnectionDevice.Trim()}',
               {GestprojectRecordedUserId},
               '{sage50LocalTerminalPath.Trim()}',
               '{sage50Username.Trim()}',
               '{Encryptor.Encrypt(sage50Password.Trim())}',
               '{selectedCompanyGroupName.Trim()}',
               '{selectedCompanyGroupMainCode.Trim()}',
               '{selectedCompanyGroupCode.Trim()}',
               '{selectedCompanyGroupGuidId.Trim()}',
               1
               )
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };

            return true;
         }
         catch(SqlException ex)
         {
            MessageBox.Show($"Error: \n\n{ex.Message}");
            return false;
         }
         finally
         {
            connection.Close();
         };
      }
      public static bool ForgetUserRememberableData
      (
         System.Data.SqlClient.SqlConnection connection,
         string gestprojectConnectionUser,
         string gestprojectConnectionDevice,
         int? gestprojectRecordedUserId
      )
      {
         try
         {
            connection.Open();

            string sqlString1 = $@"DELETE FROM {GestprojectSynchronizatorUserDataTableName}
                WHERE {GP_CNX_USUARIO}='{gestprojectConnectionUser}' 
                AND {GP_CNX_EQUIPO}='{gestprojectConnectionDevice}' 
                AND {GP_USU_ID}={gestprojectRecordedUserId};";
            using(SqlCommand sqlCommand = new SqlCommand(sqlString1, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };

            return true;
         }
         catch(SqlException ex)
         {
            MessageBox.Show($"Error: \n\n{ex.Message}");
            return false;
         }
         finally
         {
            connection.Close();
         };
      }

      public static bool ChangeRememberUserDataFeature
      (
         System.Data.SqlClient.SqlConnection connection,
         string gestprojectConnectionUser,
         string gestprojectConnectionDevice,
         int? gestprojectRecordedUserId,
         int rememberDataFeatureValue
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
                UPDATE {GestprojectSynchronizatorUserDataTableName}
                SET
                  {REMEMBER}={rememberDataFeatureValue}
                WHERE 
                  {GP_CNX_USUARIO}='{gestprojectConnectionUser}' 
                AND 
                  {GP_CNX_EQUIPO}='{gestprojectConnectionDevice}' 
                AND 
                  {GP_USU_ID}={gestprojectRecordedUserId}
                ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };

            return true;
         }
         catch(SqlException ex)
         {
            MessageBox.Show($"Error: \n\n{ex.Message}");
            return false;
         }
         finally
         {
            connection.Close();
         };
      }

      public static bool CheckIfRememberUserDataOptionWasActivated(
         System.Data.SqlClient.SqlConnection connection,
         string gestprojectConnectionUser,
         string gestprojectConnectionDevice,
         int? gestprojectRecordedUserId
      )
      {
         try
         {
            connection.Open();

            //string sqlString = $@"
            //    SELECT
            //        {REMEMBER}
            //    FROM 
            //        {GestprojectSynchronizatorUserDataTableName}
            //    WHERE {GP_CNX_USUARIO}='{gestprojectConnectionUser}' 
            //    AND {GP_CNX_EQUIPO}='{gestprojectConnectionDevice}' 
            //    AND {GP_USU_ID}={gestprojectRecordedUserId}
            //;";

            string sqlString = $"SELECT {REMEMBER} FROM {GestprojectSynchronizatorUserDataTableName} WHERE {GP_CNX_USUARIO}='{gestprojectConnectionUser}' AND {GP_CNX_EQUIPO}='{gestprojectConnectionDevice}' AND {GP_USU_ID}={gestprojectRecordedUserId};";

            //MessageBox.Show("On:\n\nManageRememberableUserData\n.CheckIfRememberUserDataOptionWasActivated\n\n" + sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return Convert.ToInt32(reader.GetValue(0)) == 1;
                  };
               };
            };
            return false;
         }
         catch(SqlException exception)
         {
            throw exception;
         }
         finally
         {
            connection.Close();
         };
      }

      public static SynchronizerUserRememberableDataModel GetSynchronizerUserRememberableDataForConnection(System.Data.SqlClient.SqlConnection connection)
      {
         try
         {
            connection.Open();

            SynchronizerUserRememberableDataModel userRememberableDataModel = new SynchronizerUserRememberableDataModel();

            string sqlString = $@"
                SELECT
                  {GP_CNX_ID},
                  {GP_CNX_PERSONAL},
                  {GP_CNX_USUARIO},
                  {GP_CNX_PERFIL},
                  {GP_CNX_EQUIPO},
                  {GP_USU_ID},
                  {SAGE_50_LOCAL_TERMINAL_PATH},
                  {SAGE_50_USER_NAME},
                  {SAGE_50_PASSWORD},
                  {SAGE_50_COMPANY_GROUP_NAME},
                  {SAGE_50_COMPANY_GROUP_MAIN_CODE},
                  {SAGE_50_COMPANY_GROUP_CODE},
                  {SAGE_50_COMPANY_GROUP_GUID_ID},
                  {REMEMBER}
                FROM 
                    {GestprojectSynchronizatorUserDataTableName};";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     userRememberableDataModel.GP_CNX_ID = Convert.ToInt32(reader.GetValue(0));
                     userRememberableDataModel.GP_CNX_PERSONAL = Convert.ToString(reader.GetValue(1));
                     userRememberableDataModel.GP_CNX_USUARIO = Convert.ToString(reader.GetValue(2));
                     userRememberableDataModel.GP_CNX_PERFIL = Convert.ToString(reader.GetValue(3));
                     userRememberableDataModel.GP_CNX_EQUIPO = Convert.ToString(reader.GetValue(4));
                     userRememberableDataModel.GP_USU_ID = Convert.ToInt32(reader.GetValue(5));
                     userRememberableDataModel.SAGE_50_LOCAL_TERMINAL_PATH = Convert.ToString(reader.GetValue(6));
                     userRememberableDataModel.SAGE_50_USER_NAME = Convert.ToString(reader.GetValue(7));
                     userRememberableDataModel.SAGE_50_PASSWORD = Encryptor.UnEncrypt(Convert.ToString(reader.GetValue(8)));
                     userRememberableDataModel.SAGE_50_COMPANY_GROUP_NAME = Convert.ToString(reader.GetValue(9));
                     userRememberableDataModel.SAGE_50_COMPANY_GROUP_MAIN_CODE = Convert.ToString(reader.GetValue(10));
                     userRememberableDataModel.SAGE_50_COMPANY_GROUP_CODE = Convert.ToString(reader.GetValue(11));
                     userRememberableDataModel.SAGE_50_COMPANY_GROUP_GUID_ID = Convert.ToString(reader.GetValue(12));
                     userRememberableDataModel.REMEMBER = Convert.ToByte(reader.GetValue(13));
                  };
               };
            };

            return userRememberableDataModel;
         }
         catch(SqlException ex)
         {
            MessageBox.Show($"Error: GetSynchronizerUserRememberableDataForConnection");
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
