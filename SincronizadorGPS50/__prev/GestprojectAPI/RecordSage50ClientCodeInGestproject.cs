using System.Data.SqlClient;
using System.Windows.Forms;
namespace SincronizadorGPS50.GestprojectAPI
{
    internal class RecordSage50ClientCodeInGestproject
    {
        internal RecordSage50ClientCodeInGestproject(int gestprojectClientid, string sage50ClientCode)
        {
            using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
            {
                try
                {
                    connection.Open();

                    string sqlString = $"UPDATE PARTICIPANTE SET PAR_SUBCTA_CONTABLE = {sage50ClientCode} WHERE PAR_ID = {gestprojectClientid};";

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
