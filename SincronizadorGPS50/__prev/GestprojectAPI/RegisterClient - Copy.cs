//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data.SqlTypes;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.GestprojectAPI
//{
//    internal class RegisterClient
//    {
//        public RegisterClient( GestprojectClient client, string synchronizationStatus ) 
//        {
//            string sqlString = @"
//                INSERT INTO 
//                    INT_SAGE_SINC_CLIENTE_IMAGEN 
//                    (
//                        PAR_ID, 
//                        PAR_SUBCTA_CONTABLE, 
//                        PAR_NOMBRE, 
//                        PAR_NOMBRE_COMERCIAL, 
//                        PAR_CIF_NIF, 
//                        PAR_DIRECCION_1, 
//                        PAR_CP_1, 
//                        PAR_LOCALIDAD_1, 
//                        PAR_PROVINCIA_1, 
//                        PAR_PAIS_1, 
//                        synchronization_status, 
//                        sage50_client_code, 
//                        sage50_guid_id, 
//                        sage50_instance,
//                        comments
//                    ) 
//                VALUES " + $"({client.PAR_ID}, '{client.PAR_SUBCTA_CONTABLE}', '{client.PAR_NOMBRE}', '{client.PAR_NOMBRE_COMERCIAL}','{client.PAR_CIF_NIF}', '{client.PAR_DIRECCION_1}', '{client.PAR_CP_1}', '{client.PAR_LOCALIDAD_1}', '{client.PAR_PROVINCIA_1}', '{client.PAR_PAIS_1}', '{synchronizationStatus}', '{client.sage50_client_code}', '{client.sage50_guid_id}', '{client.sage50_instance}', '{client.comments}');";

//            using(SqlCommand SQLCommand = new SqlCommand(sqlString, DataHolder.GestprojectSQLConnection))
//            {
//                SQLCommand.ExecuteNonQuery();
//            };

//            string sqlString2 = @"
//                INSERT INTO 
//                    INT_SAGE_SINC_CLIENTE 
//                    (
//                        synchronization_status, 
//                        gestproject_id, 
//                        sage50_code, 
//                        sage50_guid_id, 
//                        sage50_instance
//                    ) 
//                VALUES " + $"('{synchronizationStatus}', {client.PAR_ID}, '{client.sage50_client_code}', '{client.sage50_guid_id}', '{client.sage50_instance}');";

//            using(SqlCommand SQLCommand = new SqlCommand(sqlString2, DataHolder.GestprojectSQLConnection))
//            {
//                SQLCommand.ExecuteNonQuery();
//            };
//        }
//    }
//}
