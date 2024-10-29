//using Infragistics.Documents.Reports.HTML;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.GestprojectAPI
//{
//    internal class IsGestprojectClientSynchronized
//    {
//        internal bool ItIs { get; set; } = false;
//        internal string Comment { get; set; } = "";
//        internal Dictionary<string, bool> SynchronizationStatusDictionary { get; set; } = new Dictionary<string, bool>();
//        internal IsGestprojectClientSynchronized(GestprojectClient client) 
//        {
//            using(System.Data.SqlClient.SqlConnection connection = GestprojectDatabase.Connect())
//            {
//                try
//                {
//                    connection.Open();

//                    string sqlString = $"SELECT [sage50_code],[sage50_guid_id] FROM [GESTPROJECT2020].[dbo].[INT_SAGE_SINC_CLIENTE] WHERE [gestproject_id]={client.PAR_ID};";

//                    using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
//                    {
//                        using(SqlDataReader reader = sqlCommand.ExecuteReader())
//                        {
//                            while(reader.Read())
//                            {
//                                if(
//                                    ((string)reader.GetValue(0) != "" && (string)reader.GetValue(0) != null)
//                                    &&
//                                    ((string)reader.GetValue(1) != "" && (string)reader.GetValue(1) != null)
//                                )
//                                {
//                                    //GetSage50Client sage50Clients = new GetSage50Client(client);
//                                    //if(sage50Clients.Exists)
//                                    //{
//                                    //    List<string> GestprojectClientData = new List<string>
//                                    //    {
//                                    //        client.PAR_SUBCTA_CONTABLE,
//                                    //        client.PAR_NOMBRE,
//                                    //        client.PAR_CIF_NIF,
//                                    //        client.PAR_DIRECCION_1,
//                                    //        client.PAR_CP_1,
//                                    //        client.PAR_LOCALIDAD_1,
//                                    //        client.PAR_PROVINCIA_1,
//                                    //        client.PAR_PAIS_1,
//                                    //    };

//                                    //    List<string> Sage50ClientData = new List<string>
//                                    //    {
//                                    //        sage50Clients.Codigo,
//                                    //        sage50Clients.Nombre,
//                                    //        sage50Clients.Cif,
//                                    //        sage50Clients.Direccion,
//                                    //        sage50Clients.Codpost,
//                                    //        sage50Clients.Poblacion,
//                                    //        sage50Clients.Provincia,
//                                    //        sage50Clients.Pais,
//                                    //    };

//                                    //    List<string> SynchronizationTableFieldsNames = new List<string>
//                                    //    {
//                                    //        "Subcuenta_Contable",
//                                    //        "Nombre",
//                                    //        "CIF_NIF",
//                                    //        "Direccion",
//                                    //        "Codigo_Postal",
//                                    //        "Localidad",
//                                    //        "Provincia",
//                                    //        "Pais",
//                                    //    };

//                                    //    int ClientErrorQuantity = 0;

//                                    //    for(global::System.Int32 i = 0; i < SynchronizationTableFieldsNames.Count; i++)
//                                    //    {
//                                    //        SynchronizationStatusDictionary.Add(SynchronizationTableFieldsNames[i], GestprojectClientData[i] == Sage50ClientData[i]);
//                                    //        if(GestprojectClientData[i] != Sage50ClientData[i])
//                                    //        {
//                                    //            ClientErrorQuantity++;
//                                    //        };
//                                    //    };

//                                    //    if(ClientErrorQuantity > 0)
//                                    //    {
//                                    //        Comment += "Identificamos " + ClientErrorQuantity + " errores en este cliente. Estos son los campos afectados: \n";

//                                    //        for(global::System.Int32 i = 0; i < SynchronizationStatusDictionary.Count; i++)
//                                    //        {
//                                    //            string errorMessage = SynchronizationStatusDictionary.ElementAt(i).Key + $" (su valor en Sage50 es: {Sage50ClientData[i]})";

//                                    //            if(!SynchronizationStatusDictionary.ElementAt(i).Value && i > SynchronizationStatusDictionary.Count - 1)
//                                    //            {
//                                    //                Comment += errorMessage + ".";
//                                    //            }
//                                    //            else if(!SynchronizationStatusDictionary.ElementAt(i).Value)
//                                    //            {
//                                    //                Comment += errorMessage + ", ";
//                                    //            };
//                                    //        };
//                                    //    }
//                                    //    else
//                                    //    {
//                                    //        ItIs = true;
//                                    //    };
//                                    };
//                                };
//                            };
//                        };
//                    };
//                }
//                catch(SqlException ex)
//                {
//                    MessageBox.Show($"Error during data retrieval: \n\n{ex.Message}");
//                }
//                finally
//                {
//                    connection.Close();
//                };
//            };
//        }
//    }
//}
