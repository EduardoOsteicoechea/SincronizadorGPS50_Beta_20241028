using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static sage.ew.docsven.FirmaElectronica;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class IsClientUpToDateWithLastGestProjectRecord
    {
        internal bool ItIs {  get; set; } = false;
        internal string Comment {  get; set; }
        internal Dictionary<string, bool> SynchronizationStatusDictionary { get; set; } = new Dictionary<string, bool>();
        internal IsClientUpToDateWithLastGestProjectRecord(GestprojectClient client) 
        {
            string sqlString = @"
            SELECT 
                * 
            FROM 
                INT_SAGE_SINC_CLIENTE_IMAGEN 
            WHERE 
                PAR_ID=" + client.PAR_ID + ";";

            SqlCommand sqlCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection);

            using(SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                int client_counter = 0;
                while(reader.Read())
                {
                    string database_par_id = Convert.ToString(reader.GetValue(1));
                    string database_par_subcta_contable = (string)reader.GetValue(2);
                    string database_par_nombre = (string)reader.GetValue(3);
                    string database_par_nombre_comercial = (string)reader.GetValue(4);
                    string database_par_cif_nif = (string)reader.GetValue(5);
                    string database_par_direccion_1 = (string)reader.GetValue(6);
                    string database_par_cp_1 = (string)reader.GetValue(7);
                    string database_par_localidad_1 = (string)reader.GetValue(8);
                    string database_par_provincia_1 = (string)reader.GetValue(9);
                    string database_par_pais_1 = (string)reader.GetValue(10);
                    string database_synchronization_status = (string)reader.GetValue(11);
                    string database_sage50_client_code = (string)reader.GetValue(12);
                    string database_sage50_guid_id = (string)reader.GetValue(13);
                    string database_sage50_instance = (string)reader.GetValue(14);

                    List<string> clientFields = new List<string>
                    {
                        Convert.ToString(client.PAR_ID),
                        client.PAR_SUBCTA_CONTABLE,
                        client.PAR_NOMBRE,
                        client.PAR_NOMBRE_COMERCIAL,
                        client.PAR_CIF_NIF,
                        client.PAR_DIRECCION_1,
                        client.PAR_CP_1,
                        client.PAR_LOCALIDAD_1,
                        client.PAR_PROVINCIA_1,
                        client.PAR_PAIS_1,
                        client.synchronization_status,
                        client.sage50_client_code,
                        client.sage50_guid_id,
                        client.sage50_instance
                    };

                    List<string> databaseFields = new List<string>
                    {
                        database_par_id,
                        database_par_subcta_contable,
                        database_par_nombre,
                        database_par_nombre_comercial,
                        database_par_cif_nif,
                        database_par_direccion_1,
                        database_par_cp_1,
                        database_par_localidad_1,
                        database_par_provincia_1,
                        database_par_pais_1,
                        database_synchronization_status,
                        database_sage50_client_code,
                        database_sage50_guid_id,
                        database_sage50_instance
                    };

                    List<string> fieldNames = new List<string>
                    {
                        "id",
                        "subcta_contable",
                        "mombre",
                        "nombre_comercial",
                        "cif_nif",
                        "direccion_1",
                        "cp_1",
                        "localidad_1",
                        "provincia_1",
                        "pais_1",
                        "synchronization_status",
                        "sage50_client_code",
                        "sage50_guid_id",
                        "sage50_instance"
                    };

                    int ClientErrorQuantity = 0;

                    for (global::System.Int32 i = 0; i < clientFields.Count; i++)
                    {
                        SynchronizationStatusDictionary.Add(fieldNames[i] + (client_counter + 1), clientFields[i] == databaseFields[i]);
                        if(clientFields[i] != databaseFields[i])
                            ClientErrorQuantity++;
                    };

                    if(ClientErrorQuantity > 0) 
                    {
                        Comment = "Identificamos " + ClientErrorQuantity + " errores en este cliente. Estos son los campos afectados: \n";

                        for(global::System.Int32 i = 0; i < SynchronizationStatusDictionary.Count; i++)
                        {
                            if(!SynchronizationStatusDictionary.ElementAt(i).Value && i > SynchronizationStatusDictionary.Count - 1)
                            {
                                Comment += SynchronizationStatusDictionary.ElementAt(i).Key + ".";
                            }
                            else if(!SynchronizationStatusDictionary.ElementAt(i).Value)
                            {
                                Comment += SynchronizationStatusDictionary.ElementAt(i).Key + ", ";
                            };
                        };
                    }
                    else
                    {
                        ItIs = true;
                    };
                    client_counter++;
                };
            };
        }
    }
}
