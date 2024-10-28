

namespace SincronizadorGPS50.GestprojectConnector
{
   internal class GenerateConnectionString
   {
      public GenerateConnectionString()
      {
         try
         {
            string connectionString = "";
            connectionString += $@"Data Source={ConnectionDataHolder.Server.Trim()}\{ConnectionDataHolder.DatabaseInstance.Trim().ToUpper()};";
            connectionString += $"Initial Catalog={ConnectionDataHolder.DatabaseName.Trim()};";
            connectionString += $"Persist Security Info=True;";
            connectionString += $"User ID={ConnectionDataHolder.DatabaseUser.Trim()};";
            connectionString += $"Password={ConnectionDataHolder.DatabasePassword.Trim()};";
            connectionString += $"Connection Timeout=5;";

            ConnectionDataHolder.GestprojectConnectionString = connectionString;

            connectionString = null;
         }
         catch(System.Exception exception)
         {
            throw new System.Exception($"Error: \n\n{exception.ToString()}. \n\nProcederemos a detener la aplicación. Contacte a nuestro servicio de atención al cliente para reportar el error y recibir servicio técnico al respecto.");
         };
      }
   }
}