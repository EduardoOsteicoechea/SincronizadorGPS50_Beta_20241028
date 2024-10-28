

namespace SincronizadorGPS50.GestprojectConnector
{
    internal class ValidateDatabaseConnectionString
    {
        public ValidateDatabaseConnectionString()
        {
            try
            {
                using(System.Data.SqlClient.SqlConnection testConnection = new System.Data.SqlClient.SqlConnection(ConnectionDataHolder.GestprojectConnectionString))
                {
                    testConnection.Open();
                    testConnection.Close();
                };

                GestprojectConnectionString.ConnectionString = ConnectionDataHolder.GestprojectConnectionString;
            }
            catch(System.Exception exception)
            {
                throw new System.Exception($"Error: \n\n{exception.Message}. \n\nProcederemos a detener la aplicación. Contacte a nuestro servicio de atención al cliente para reportar el error y recibir servicio técnico al respecto.");
            };
        }
    }
}