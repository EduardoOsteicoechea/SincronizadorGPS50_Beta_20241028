using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class CheckIfGestprojectClientWasRegistered
    {
        internal bool ItIs {  get; set; } = false;
        internal CheckIfGestprojectClientWasRegistered(GestprojectClient client) 
        {
            using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
            {
                try
                {
                    connection.Open();

                    string sqlString = $"SELECT * FROM INT_SAGE_SINC_CLIENTE_IMAGEN WHERE PAR_ID={client.PAR_ID};";

                    using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
                    {
                        using(SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                if((int)reader.GetValue(0) == client.PAR_ID)
                                {
                                    ItIs = true;
                                    break;
                                };
                            };
                        };
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
