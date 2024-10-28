using SincronizadorGPS50.GestprojectAPI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Clients
{
    internal static class GestprojectClients
    {
        internal static List<GestprojectClient> Get(List<int> gestProjectIdList)
        {
            List<GestprojectClient> gestprojectClientClassList = new List<GestprojectClient>();

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

                                    client.PAR_ID = (int)reader.GetValue(0);
                                    client.PAR_SUBCTA_CONTABLE = (string)reader.GetValue(1);
                                    client.PAR_NOMBRE = (string)reader.GetValue(2);
                                    client.PAR_NOMBRE_COMERCIAL = (string)reader.GetValue(3);
                                    client.PAR_CIF_NIF = (string)reader.GetValue(4);
                                    client.PAR_DIRECCION_1 = (string)reader.GetValue(5);
                                    client.PAR_CP_1 = (string)reader.GetValue(6);
                                    client.PAR_LOCALIDAD_1 = (string)reader.GetValue(7);
                                    client.PAR_PROVINCIA_1 = (string)reader.GetValue(8);
                                    client.PAR_PAIS_1 = (string)reader.GetValue(9);

                                    gestprojectClientClassList.Add(client);
                                };
                            };
                        };
                    };
                    return gestprojectClientClassList;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Error during data retrieval: \n\n{ex.Message}");
                    return gestprojectClientClassList;
                }
                finally
                {
                    connection.Close();
                };
            };
        }

        internal static List<GestprojectClient> Get()
        {
            List<int> gestProjectClientIdList = new List<int>();
            List<int> gestProjectProviderIdList = new List<int>();
            List<GestprojectClient> gestprojectClientClassList = new List<GestprojectClient>();

            using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
            {
                connection.Open();
                try
                {
                    string sqlString1 = "SELECT * FROM PAR_TPA";

                    using(SqlCommand sqlCommand = new SqlCommand(sqlString1, connection))
                    {
                        using(SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                int fieldCount = reader.FieldCount;

                                while(reader.Read())
                                {
                                    if(reader.GetValue(0).ToString() == "1")
                                    {
                                        gestProjectClientIdList.Add((int)reader.GetValue(1));
                                    }
                                    else if(reader.GetValue(0).ToString() == "12")
                                    {
                                        gestProjectProviderIdList.Add((int)reader.GetValue(1));
                                    };
                                };
                                gestProjectClientIdList.Distinct().ToList();
                                gestProjectProviderIdList.Distinct().ToList();
                            };
                        };
                    };

                    string sqlString2 = @"
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
                    ;";

                    using(SqlCommand sqlCommand = new SqlCommand(sqlString2, connection))
                    {
                        using(SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                //MessageBox.Show(reader.GetValue(3).GetType().Name);
                                if(reader.GetValue(0).GetType().Name != "DBNull")
                                {
                                    if(gestProjectClientIdList.Contains((int)reader.GetValue(0)))
                                    {
                                        GestprojectClient gestprojectClient = new GestprojectClient();

                                        //gestprojectClient.PAR_ID = (int)reader.GetValue(0);
                                        //gestprojectClient.PAR_SUBCTA_CONTABLE = (string)reader.GetValue(1) != null ? (string)reader.GetValue(1) : "";
                                        //gestprojectClient.PAR_NOMBRE = (string)reader.GetValue(2) != null ? (string)reader.GetValue(2) : "";
                                        //gestprojectClient.PAR_NOMBRE_COMERCIAL = (string)reader.GetValue(3) != null ? (string)reader.GetValue(3) : "";
                                        //gestprojectClient.PAR_CIF_NIF = (string)reader.GetValue(4) != null ? (string)reader.GetValue(4) : "";
                                        //gestprojectClient.PAR_DIRECCION_1 = (string)reader.GetValue(5) != null ? (string)reader.GetValue(5) : "";
                                        //gestprojectClient.PAR_CP_1 = (string)reader.GetValue(6) != null ? (string)reader.GetValue(6) : "";
                                        //gestprojectClient.PAR_LOCALIDAD_1 = (string)reader.GetValue(7) != null ? (string)reader.GetValue(7) : "";
                                        //gestprojectClient.PAR_PROVINCIA_1 = (string)reader.GetValue(8) != null ? (string)reader.GetValue(8) : "";
                                        //gestprojectClient.PAR_PAIS_1 = (string)reader.GetValue(9) != null ? (string)reader.GetValue(9) : "";
                                        gestprojectClient.PAR_ID = Convert.ToInt32(reader.GetValue(0));
                                        gestprojectClient.PAR_SUBCTA_CONTABLE = Convert.ToString(reader.GetValue(1));
                                        gestprojectClient.PAR_NOMBRE = Convert.ToString(reader.GetValue(2));
                                        gestprojectClient.PAR_NOMBRE_COMERCIAL = Convert.ToString(reader.GetValue(3));
                                        gestprojectClient.PAR_CIF_NIF = Convert.ToString(reader.GetValue(4));
                                        gestprojectClient.PAR_DIRECCION_1 = Convert.ToString(reader.GetValue(5));
                                        gestprojectClient.PAR_CP_1 = Convert.ToString(reader.GetValue(6));
                                        gestprojectClient.PAR_LOCALIDAD_1 = Convert.ToString(reader.GetValue(7));
                                        gestprojectClient.PAR_PROVINCIA_1 = Convert.ToString(reader.GetValue(8));
                                        gestprojectClient.PAR_PAIS_1 = Convert.ToString(reader.GetValue(9));

                                        gestprojectClientClassList.Add(gestprojectClient);
                                    };
                                };
                            };
                        };
                    };
                    return gestprojectClientClassList;
                }
                catch(SqlException ex)
                {
                    MessageBox.Show($"Error during data retrieval: \n\n{ex.Message}");
                    return gestprojectClientClassList;
                }
                finally
                {
                    connection.Close();
                };
            };
        }
    }
}
