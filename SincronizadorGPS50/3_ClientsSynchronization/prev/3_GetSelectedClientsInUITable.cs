using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Clients
{
    internal class GetSelectedClientsInUITable
    {
        internal List<GestprojectClient> Clients {  get; set; } = new List<GestprojectClient>();
        internal GetSelectedClientsInUITable(List<int> gestProjectIdList)
        {
            using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
            {
                try
                {
                    connection.Open();

                    for(int i = 0; i < gestProjectIdList.Count; i++)
                    {
                        string sqlString = @"
                        SELECT 
                            PAR_ID,
                            PAR_SUBCTA_CONTABLE,
                            PAR_NOMBRE,
                            PAR_NOMBRE_COMERCIAL,
                            PAR_CIF_NIF,
                            PAR_DIRECCION_1,
                            PAR_CP_1,
                            PAR_LOCALIDAD_1,
                            PAR_PROVINCIA_1,
                            PAR_PAIS_1
                        FROM 
                            PARTICIPANTE 
                        WHERE "
                            +$"PAR_ID={gestProjectIdList[i]};";

                        using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
                        {
                            using(SqlDataReader reader = sqlCommand.ExecuteReader())
                            {
                                while(reader.Read())
                                {
                                    GestprojectClient client = new GestprojectClient();

                                    client.PAR_ID = Convert.ToInt32(reader.GetValue(0));
                                    client.PAR_SUBCTA_CONTABLE = Convert.ToString(reader.GetValue(1));
                                    client.PAR_NOMBRE = Convert.ToString(reader.GetValue(2));
                                    client.PAR_NOMBRE_COMERCIAL = Convert.ToString(reader.GetValue(3));
                                    client.PAR_CIF_NIF = Convert.ToString(reader.GetValue(4));
                                    client.PAR_DIRECCION_1 = Convert.ToString(reader.GetValue(5));
                                    client.PAR_CP_1 = Convert.ToString(reader.GetValue(6));
                                    client.PAR_LOCALIDAD_1 = Convert.ToString(reader.GetValue(7));
                                    client.PAR_PROVINCIA_1 = Convert.ToString(reader.GetValue(8));
                                    client.PAR_PAIS_1 = Convert.ToString(reader.GetValue(9));

                                    Clients.Add(client);
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
