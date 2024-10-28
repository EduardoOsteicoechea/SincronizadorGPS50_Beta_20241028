using System.Data.SqlClient;

namespace SincronizadorGPS50.GestprojectConnector
{
   public class ConnectionManager
   {
      public System.Data.SqlClient.SqlConnection Connect()
      {
         try
         {
            new GetLocalDeviceData();

            new GetConnectionData();

            new GenerateConnectionString();

            new ValidateDatabaseConnectionString();

            ConnectionDataHolder.DisposeSensitiveData();

            return new SqlConnection(GestprojectConnectionString.ConnectionString);
         }
         catch(System.Exception exception)
         {
            ConnectionDataHolder.DisposeSensitiveData();
            throw exception;
         };
      }
   }
}
