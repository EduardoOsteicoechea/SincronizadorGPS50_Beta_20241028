using SincronizadorGPS50.Workflows.Sage50Connection;
using System.Security.Principal;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class SetupAndConnections : ApplicationContext
   {
      public SetupAndConnections()
      {
         ///////////////////////////////////////
         // Set initial windows forms application settings
         ///////////////////////////////////////

         new WindowsFormsApplicationSettings().SetInitialSettings();

         ///////////////////////////////////////
         // Get and leverage user device Gestproject data For Global Styling and Database Connection
         ///////////////////////////////////////

         GestprojectDataHolder.GestprojectDatabaseConnection =
            new SincronizadorGPS50.GestprojectConnector.ConnectionManager().Connect();

         GestprojectDataHolder.GestprojectLocalDeviceUserData =
            new GestprojectConnector.GestprojectLocalDeviceUserData().Get();

         new ApplicationGlobalStyles().ApplyGestprojectCurrentStyle();

         ///////////////////////////////////////
         // Create Global UI
         ///////////////////////////////////////

         new GenerateMainWindow();

         new GenerateMainWindowUI();

         new GenerateSage50ConnectionTabPageUI();

         new GenerateCenterRowUI();

         ///////////////////////////////////////
         // Get user Rememberlable Data
         ///////////////////////////////////////

         GestprojectDataHolder.LocalDeviceUserSessionData =
            new SincronizadorGPS50.GestprojectConnector.GestprojectSessionSettings(
               GestprojectDataHolder.GestprojectDatabaseConnection,
               GestprojectDataHolder.GestprojectLocalDeviceUserData.LastUser
            ).userSessionData;

         string currentLocalDevice = WindowsIdentity.GetCurrent().Name.Split('\\')[0];
         string gestprojectSessionDevice = GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO;
         //bool userIsInRememberedAndApprovedDevice = gestprojectSessionDevice == currentLocalDevice;

         //////////////////////
         //////////////////////
         //////////////////////
         //////////////////////
         //////////////////////
         //////////////////////
         //////////////////////
         // CHANGE THIS FOR PRODUCTION !!!!!!!!!!!!!!!!!!!!!!!!!!!!
         //////////////////////
         //////////////////////
         bool userIsInRememberedAndApprovedDevice = true;

         ///////////////////////////////////////
         // Evaluate Basic User Rememberlable assets
         ///////////////////////////////////////

         bool gestprojectUserDataTableExists =
            GestprojectDataManager
            .ManageRememberableUserData
            .CheckIfGestprojectUserDataTableExists(
               GestprojectDataHolder.GestprojectDatabaseConnection
            );

         bool rememberUserDataOptionWasActivated = false;
         if(gestprojectUserDataTableExists)
         {
            rememberUserDataOptionWasActivated =
            GestprojectDataManager
            .ManageRememberableUserData
            .CheckIfRememberUserDataOptionWasActivated(
               GestprojectDataHolder.GestprojectDatabaseConnection,
               GestprojectDataHolder.LocalDeviceUserSessionData.CNX_USUARIO,
               GestprojectDataHolder.LocalDeviceUserSessionData.CNX_EQUIPO,
               GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID
            );
         };

         ///////////////////////////////////////
         // Create Sage50Connection conditional controls
         ///////////////////////////////////////

         if(!gestprojectUserDataTableExists)
         {
            GestprojectDataManager
            .ManageRememberableUserData
            .CreateGestprojectUserDataTable(
               GestprojectDataHolder.GestprojectDatabaseConnection
            );
         }

         if(
            (!gestprojectUserDataTableExists) ||
            (gestprojectUserDataTableExists && !rememberUserDataOptionWasActivated) ||
            (gestprojectUserDataTableExists && rememberUserDataOptionWasActivated && !userIsInRememberedAndApprovedDevice)
         ){
            Sage50ConnectionUIHolder.Sage50ConnectionUIManagerInstance = new Sage50ConnectionUIManager(
               Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.Controls,
               "stateless"
            );
         };

         if(gestprojectUserDataTableExists && rememberUserDataOptionWasActivated && userIsInRememberedAndApprovedDevice)
         {
            Sage50ConnectionUIHolder.Sage50ConnectionUIManagerInstance = new Sage50ConnectionUIManager(
               Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.Controls,
               "stateful"
            );
         };

         ///////////////////////////////////////
         // InitialLaunchForSage50Connection
         ///////////////////////////////////////

         MainWindowUIHolder.MainWindow.Show();
      }
   }
}