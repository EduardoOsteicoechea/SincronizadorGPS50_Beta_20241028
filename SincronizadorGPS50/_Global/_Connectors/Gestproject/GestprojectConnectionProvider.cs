using SincronizadorGPS50.GestprojectDataManager;

namespace SincronizadorGPS50
{
   internal class GestprojectConnectionManager : IGestprojectConnectionManager
   {
      public System.Data.SqlClient.SqlConnection GestprojectSqlConnection { get; set; }
      public SynchronizerUserRememberableDataModel GestprojectUserRememberableData { get; set; }
      public GestprojectConnectionManager()
      {
         GestprojectSqlConnection = new SincronizadorGPS50.GestprojectConnector.ConnectionManager().Connect();

         GestprojectUserRememberableData = ManageRememberableUserData.GetSynchronizerUserRememberableDataForConnection(
            GestprojectDataHolder.GestprojectDatabaseConnection
         );
      }
   }
}
