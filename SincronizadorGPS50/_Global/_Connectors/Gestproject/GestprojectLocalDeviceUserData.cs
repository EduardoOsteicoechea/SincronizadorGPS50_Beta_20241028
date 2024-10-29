using Dinq.Gestproject;
using System;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class GestprojectLocalDeviceUserData
   {
      private static string BaseLocalApplicationGestprojectDataFolder { get; set; } =
      System.IO.Path.Combine(
         Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData
         ),
         @"Micad\Gestproject"
      );
      public string GestprojectDATUserSettingsFilePath { get; set; } =
      System.IO.Path.Combine(
          new GetLatestVersionFolderName(
              BaseLocalApplicationGestprojectDataFolder
          ).FolderName,
          "_USERSETTINGS.DAT"
      );
      private static string GestprojectStylesFolderPath { get; set; } =
      System.IO.Path.Combine(
         Environment.GetFolderPath(
            Environment.SpecialFolder.ProgramFilesX86
         ),
         @"Micad\Gestproject 2020\Styles"
      );

      public LocalDeviceUserData Get()
      {
         try
         {
            LocalDeviceUserData localDeviceUserData = new LocalDeviceUserData();

            dynamic userSettings = Serializer.DeserializeObject(GestprojectDATUserSettingsFilePath);

            localDeviceUserData.LastUser = userSettings.LastUser;
            localDeviceUserData.StyleFileName = userSettings.StyleFileName;
            localDeviceUserData.StyleFilePath = System.IO.Path.Combine(GestprojectStylesFolderPath, userSettings.StyleFileName);

            return localDeviceUserData;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }
   }
}