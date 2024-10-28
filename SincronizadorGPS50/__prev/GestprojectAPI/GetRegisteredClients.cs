using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class GetRegisteredClients
    {
        internal List<GestprojectClient> RegisteredClientsList { get; set; } = new List<GestprojectClient>();
        public GetRegisteredClients() 
        {
            string sqlString = @"
            SELECT 
                synchronization_status,
                id, 
                PAR_ID,
                sage50_client_code,
                sage50_guid_id,
                PAR_NOMBRE,
                PAR_NOMBRE_COMERCIAL,
                PAR_CIF_NIF,
                PAR_DIRECCION_1,
                PAR_CP_1,
                PAR_LOCALIDAD_1,
                PAR_PROVINCIA_1,
                PAR_PAIS_1,
                sage50_instance,
                comments,
                last_record
            FROM 
                INT_SAGE_SINC_CLIENTE_IMAGEN 
            ;";

            SqlCommand sqlCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection);

            using(SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while(reader.Read())
                {
                    GestprojectClient client = new GestprojectClient();

                    client.synchronization_status = (string)reader.GetValue(0);
                    client.synchronization_table_id = (int)reader.GetValue(1);
                    client.PAR_ID = (int)reader.GetValue(2);
                    client.sage50_client_code = (string)reader.GetValue(3);
                    client.sage50_guid_id = (string)reader.GetValue(4);
                    client.PAR_NOMBRE = (string)reader.GetValue(5);
                    client.PAR_NOMBRE_COMERCIAL = (string)reader.GetValue(6);
                    client.PAR_CIF_NIF = (string)reader.GetValue(7);
                    client.PAR_DIRECCION_1 = (string)reader.GetValue(8);
                    client.PAR_CP_1 = (string)reader.GetValue(9);
                    client.PAR_LOCALIDAD_1 = (string)reader.GetValue(10);
                    client.PAR_PROVINCIA_1 = (string)reader.GetValue(11);
                    client.PAR_PAIS_1 = (string)reader.GetValue(12);
                    client.sage50_instance = (string)reader.GetValue(13);
                    client.comments = (string)reader.GetValue(14);
                    client.last_record = (System.DateTime)reader.GetValue(15);

                    RegisteredClientsList.Add(client);
                };
            };
        }
    }
}
