using System.Collections.Generic;

namespace SincronizadorGPS50
{
   internal static class GestprojectDataHolder
   {
      internal static System.Data.SqlClient.SqlConnection GestprojectDatabaseConnection { get; set; } = null;
      internal static List<GestprojectDataManager.GestprojectCustomer> GestprojectClientList { get; set; } = null;
      internal static GestprojectConnector.UserSessionData LocalDeviceUserSessionData { get; set; } = null;
      internal static GestprojectConnector.LocalDeviceUserData GestprojectLocalDeviceUserData { get; set; } = null;
   }
}
