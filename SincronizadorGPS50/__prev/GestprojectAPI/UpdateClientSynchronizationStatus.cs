using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class UpdateClientSynchronizationStatus
    {
        public UpdateClientSynchronizationStatus
        (
            GestprojectClient client, 
            string synchronizationStatus
        )
        {
            using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
            {
                try
                {
                    connection.Open();

                    string sqlString = $"UPDATE INT_SAGE_SINC_CLIENTE SET synchronization_status='{synchronizationStatus}';";

                    client.synchronization_status = synchronizationStatus;

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
