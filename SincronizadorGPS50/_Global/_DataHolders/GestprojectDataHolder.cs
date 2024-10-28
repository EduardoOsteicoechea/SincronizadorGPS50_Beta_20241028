using SincronizadorGPS50.GestprojectConnector;
using System.Collections.Generic;

namespace SincronizadorGPS50
{
    internal static class GestprojectDataHolder
    {

        internal static System.Data.SqlClient.SqlConnection GestprojectDatabaseConnection { get; set; } = null;
        internal static List<GestprojectDataManager.GestprojectCustomer> GestprojectClientList { get; set; } = null;
        //internal static List<GestprojectDataManager.GestprojectProviderModel> GestprojectProviderList { get; set; } = null; 
        internal static UserSessionData LocalDeviceUserSessionData { get; set; } = null;

        internal static LocalDeviceUserData GestprojectLocalDeviceUserData { get; set; } = null;
   }
}
