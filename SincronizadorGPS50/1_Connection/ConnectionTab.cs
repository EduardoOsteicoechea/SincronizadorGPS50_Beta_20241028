//using SincronizadorGPS50.Workflows.Sage50Connection;
//using System.Security.Principal;

//namespace SincronizadorGPS50
//{
//   internal class ConnectionTab
//   {
//      public ConnectionTab
//      (
//         GestprojectConnectionManager2 gestprojectConnectionManager
//      )
//      {
//         new GenerateSage50ConnectionTabPageUI();
//         new GenerateCenterRowUI();

//         ///////////////////////////////////////
//         // Get user Rememberlable Data
//         ///////////////////////////////////////

//         GestprojectDataHolder.LocalDeviceUserSessionData = new GestprojectSessionSettings(
//            gestprojectConnectionManager.GestprojectSqlConnection,
//            gestprojectConnectionManager.GestprojectLocalDeviceUserData.LastUser
//         ).userSessionData;

//         string currentLocalDevice = WindowsIdentity.GetCurrent().Name.Split('\\')[0];
//         string gestprojectSessionDevice = GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO;
//         //bool userIsInRememberedAndApprovedDevice = gestprojectSessionDevice == currentLocalDevice;
//         //////////////////////
//         //////////////////////
//         // CHANGE THIS FOR PRODUCTION !!!!!!!!!!!!!!!!!!!!!!!!!!!!
//         //////////////////////
//         //////////////////////
//         bool userIsInRememberedAndApprovedDevice = true;

//         ///////////////////////////////////////
//         // Evaluate Basic User Rememberlable assets
//         ///////////////////////////////////////

//         bool gestprojectUserDataTableExists = ManageRememberableUserData
//               .CheckIfGestprojectUserDataTableExists(
//                  GestprojectDataHolder.GestprojectDatabaseConnection
//               );

//         bool rememberUserDataOptionWasActivated = false;
//         if(gestprojectUserDataTableExists)
//         {
//            rememberUserDataOptionWasActivated = ManageRememberableUserData.CheckIfRememberUserDataOptionWasActivated(
//               GestprojectDataHolder.GestprojectDatabaseConnection,
//               GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO,
//               GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO,
//               GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID
//            );
//         };

//         ///////////////////////////////////////
//         // Create Sage50Connection conditional controls
//         ///////////////////////////////////////

//         if(!gestprojectUserDataTableExists)
//         {
//            ManageRememberableUserData.CreateGestprojectUserDataTable(
//               GestprojectDataHolder.GestprojectDatabaseConnection
//            );
//         }

//         if(
//            (gestprojectUserDataTableExists == false) 
//            ||
//            (gestprojectUserDataTableExists && rememberUserDataOptionWasActivated == false) 
//            ||
//            (gestprojectUserDataTableExists && rememberUserDataOptionWasActivated && userIsInRememberedAndApprovedDevice == false)
//         )
//         {
//            Sage50ConnectionUIHolder.Sage50ConnectionUIManagerInstance = new Sage50ConnectionUIManager(
//               Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.Controls,
//               "stateless"
//            );
//         };

//         if(gestprojectUserDataTableExists && rememberUserDataOptionWasActivated && userIsInRememberedAndApprovedDevice)
//         {
//            Sage50ConnectionUIHolder.Sage50ConnectionUIManagerInstance = new Sage50ConnectionUIManager(
//               Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.Controls,
//               "stateful"
//            );
//         };
//      }
//   }
//}
