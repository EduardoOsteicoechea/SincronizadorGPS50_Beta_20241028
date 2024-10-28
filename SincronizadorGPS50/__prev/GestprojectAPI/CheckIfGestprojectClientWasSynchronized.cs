using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class CheckIfGestprojectClientWasSynchronized
    {
        internal bool ItIs {  get; set; } = false;
        internal CheckIfGestprojectClientWasSynchronized(GestprojectClient client) 
        {
            using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
            {
                try
                {
                    connection.Open();

                    string sqlString = $"SELECT sage50_guid_id FROM INT_SAGE_SINC_CLIENTE WHERE gestproject_id={client.PAR_ID};";

                    using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
                    {
                        using(SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                if((string)reader.GetValue(0) != "" && (string)reader.GetValue(0) != null)
                                {
                                    ItIs = true;
                                    break;
                                }
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
