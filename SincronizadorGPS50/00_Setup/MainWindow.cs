//using Dinq.Gestproject;
//using SincronizadorGPS50.Workflows.Sage50Connection;
//using System;
//using System.IO;
//using System.Reflection;
//using System.Security.Principal;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   internal class MainWindow : ApplicationContext
//   {
//      public static string LatestVersionFolderNameManager { get; set; } = new GetLatestVersionFolderName(
//               BaseCommonApplicationGestprojectDataFolder
//            ).FolderName;
//      private static string BaseCommonApplicationGestprojectDataFolder { get; set; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Micad\Gestproject");
//      public static string BaseLocalApplicationGestprojectDataFolder { get; set; } =
//      System.IO.Path.Combine(
//         Environment.GetFolderPath(
//            Environment.SpecialFolder.LocalApplicationData
//         ),
//         @"Micad\Gestproject"
//      );
//      public string GestprojectDATUserSettingsFilePath { get; set; } = System.IO.Path.Combine(LatestVersionFolderNameManager, "_USERSETTINGS.DAT");
//      private static string GestprojectStylesFolderPath { get; set; } =
//      System.IO.Path.Combine(
//         Environment.GetFolderPath(
//            Environment.SpecialFolder.ProgramFilesX86
//         ),
//         @"Micad\Gestproject 2020\Styles"
//      );

//      public LocalDeviceUserData LocalDeviceUserData { get; set; }
//      public GestprojectConnectionManager2 GestprojectConnectionManager { get; set; }

//      public MainWindow()
//      {
//         try
//         {
//            GestprojectConnectionManager = new GestprojectConnectionManager2();
//            SetWindowInitialSettings();
//            GetLocalDeviceUserData();
//            ApplyGestprojectCurrentStyle();
//            new GenerateMainWindow();
//            new GenerateMainWindowUI();
//            new ConnectionTab(GestprojectConnectionManager);
//            MainWindowUIHolder.MainWindow.Show();
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//      }

//      public void SetWindowInitialSettings()
//      {
//         try
//         {
//            System.Windows.Forms.Application.EnableVisualStyles();
//            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//      }

//      internal void ApplyGestprojectCurrentStyle()
//      {
//         try
//         {
//            Infragistics.Win.AppStyling.StyleManager.Load(LocalDeviceUserData.StyleFilePath);
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//      }

//      public void GetLocalDeviceUserData()
//      {
//         try
//         {
//            LocalDeviceUserData = new LocalDeviceUserData();

//            dynamic userSettings = Serializer.DeserializeObject(GestprojectDATUserSettingsFilePath);

//            LocalDeviceUserData.LastUser = userSettings.LastUser;
//            LocalDeviceUserData.StyleFileName = userSettings.StyleFileName;
//            LocalDeviceUserData.StyleFilePath = System.IO.Path.Combine(GestprojectStylesFolderPath, userSettings.StyleFileName);
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }

//      public void LaunchSage50ConnectionTab()
//      {
//         try
//         {
//            new GenerateSage50ConnectionTabPageUI();
//            new GenerateCenterRowUI();

//            ///////////////////////////////////////
//            // Get user Rememberlable Data
//            ///////////////////////////////////////

//            GestprojectDataHolder.LocalDeviceUserSessionData = new GestprojectSessionSettings(
//               GestprojectConnectionManager.GestprojectSqlConnection,
//               GestprojectConnectionManager.GestprojectLocalDeviceUserData.LastUser
//            ).userSessionData;

//            string currentLocalDevice = WindowsIdentity.GetCurrent().Name.Split('\\')[0];
//            string gestprojectSessionDevice = GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO;
//            //bool userIsInRememberedAndApprovedDevice = gestprojectSessionDevice == currentLocalDevice;
//            //////////////////////
//            //////////////////////
//            // CHANGE THIS FOR PRODUCTION !!!!!!!!!!!!!!!!!!!!!!!!!!!!
//            //////////////////////
//            //////////////////////
//            bool userIsInRememberedAndApprovedDevice = true;

//            ///////////////////////////////////////
//            // Evaluate Basic User Rememberlable assets
//            ///////////////////////////////////////

//            bool gestprojectUserDataTableExists = ManageRememberableUserData
//               .CheckIfGestprojectUserDataTableExists(
//                  GestprojectDataHolder.GestprojectDatabaseConnection
//               );

//            bool rememberUserDataOptionWasActivated = false;
//            if(gestprojectUserDataTableExists)
//            {
//               rememberUserDataOptionWasActivated = ManageRememberableUserData.CheckIfRememberUserDataOptionWasActivated(
//                  GestprojectDataHolder.GestprojectDatabaseConnection,
//                  GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO,
//                  GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO,
//                  GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID
//               );
//            };

//            ///////////////////////////////////////
//            // Create Sage50Connection conditional controls
//            ///////////////////////////////////////

//            if(!gestprojectUserDataTableExists)
//            {
//               ManageRememberableUserData.CreateGestprojectUserDataTable(
//                  GestprojectDataHolder.GestprojectDatabaseConnection
//               );
//            }

//            if(
//               (!gestprojectUserDataTableExists) ||
//               (gestprojectUserDataTableExists && !rememberUserDataOptionWasActivated) ||
//               (gestprojectUserDataTableExists && rememberUserDataOptionWasActivated && !userIsInRememberedAndApprovedDevice)
//            )
//            {
//               Sage50ConnectionUIHolder.Sage50ConnectionUIManagerInstance = new Sage50ConnectionUIManager(
//                  Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.Controls,
//                  "stateless"
//               );
//            };

//            if(gestprojectUserDataTableExists && rememberUserDataOptionWasActivated && userIsInRememberedAndApprovedDevice)
//            {
//               Sage50ConnectionUIHolder.Sage50ConnectionUIManagerInstance = new Sage50ConnectionUIManager(
//                  Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.Controls,
//                  "stateful"
//               );
//            };

//            ///////////////////////////////////////
//            // InitialLaunchForSage50Connection
//            ///////////////////////////////////////

//            MainWindowUIHolder.MainWindow.Show();
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//   }
//}
