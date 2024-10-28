using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class GetGestprojectClients
    {
        internal GetGestprojectClients()
        {
            new GetGestprojectParticipants();

            DataHolder.GestprojectClientClassList.Clear();

            string sqlString = "SELECT PAR_ID, PAR_SUBCTA_CONTABLE, PAR_NOMBRE, PAR_NOMBRE_COMERCIAL, PAR_CIF_NIF, PAR_DIRECCION_1, PAR_CP_1, PAR_LOCALIDAD_1, PAR_PROVINCIA_1, PAR_PAIS_1 FROM PARTICIPANTE;";

            SqlCommand sqlCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection);

            using(SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while(reader.Read())
                {
                    if(DataHolder.GestprojectClientIdList.Contains((int)reader.GetValue(0)))
                    {
                        GestprojectClient gestprojectClient = new GestprojectClient();
                        gestprojectClient.PAR_ID = (int)reader.GetValue(0);
                        gestprojectClient.PAR_SUBCTA_CONTABLE = (string)reader.GetValue(1);
                        gestprojectClient.PAR_NOMBRE = (string)reader.GetValue(2);
                        gestprojectClient.PAR_NOMBRE_COMERCIAL = (string)reader.GetValue(3);
                        gestprojectClient.PAR_CIF_NIF = (string)reader.GetValue(4);
                        gestprojectClient.PAR_DIRECCION_1 = (string)reader.GetValue(5);
                        gestprojectClient.PAR_CP_1 = (string)reader.GetValue(6);
                        gestprojectClient.PAR_LOCALIDAD_1 = (string)reader.GetValue(7);
                        gestprojectClient.PAR_PROVINCIA_1 = (string)reader.GetValue(8);
                        gestprojectClient.PAR_PAIS_1 = (string)reader.GetValue(9);

                        DataHolder.GestprojectClientClassList.Add(gestprojectClient);
                    }
                };
            };
        }
    }
}
