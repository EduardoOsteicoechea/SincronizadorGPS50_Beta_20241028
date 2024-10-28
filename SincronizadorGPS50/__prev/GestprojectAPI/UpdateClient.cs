using SincronizadorGPS50.Workflows.Clients;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class UpdateClient
    {
        public UpdateClient( GestprojectClient client, string synchronizationStatus ) 
        {
            using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
            {
                try
                {
                    connection.Open();

                    string sqlString = $"UPDATE INT_SAGE_SINC_CLIENTE SET synchronization_status='{synchronizationStatus}', sage50_code='{client.sage50_client_code}', sage50_guid_id='{client.sage50_guid_id}', sage50_instance='{client.sage50_instance}' WHERE gestproject_id={client.PAR_ID};";

                    using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    };
                }
                catch(SqlException ex)
                {
                    MessageBox.Show($"Error during data retrieval: \n\n{ex.Message}");
                }
                finally
                {
                    connection.Close();
                };
            };
        }
    }
}
