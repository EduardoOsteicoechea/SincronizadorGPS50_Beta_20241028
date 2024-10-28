//using SincronizadorGPS50.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data.SqlTypes;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SincronizadorGPS50.GestprojectAPI
//{
//    internal class CheckIfGestprojectClientIsSynchronized
//    {
//        internal List<int> GestprojectSynchronizedClientsIdList {  get; set; } = new List<int>();
//        internal SynchronizedClient SynchronizedClient {  get; set; }
//        internal bool IsSynchronized {  get; set; } = false;
//        internal CheckIfGestprojectClientIsSynchronized
//        (
//            int GestprojectClientId,
//            List<string> Sage50ClientCodeList,
//            List<string> Sage50GUID_IDList,
//            string sage50_instance_terminal
//        ) 
//        {
//            string sqlString = $"SELECT sage50_guid_id, sage50_code, sage50_instance_terminal FROM INT_SAGE_SINC_CLIENTE WHERE \"gestproject_id\"='{GestprojectClientId}'";

//            using(SqlCommand SQLCommand = new SqlCommand(sqlString, DataHolder.GestprojectSQLConnection))
//            {
//                using(SqlDataReader reader = SQLCommand.ExecuteReader())
//                {
//                    while(reader.Read())
//                    {
//                        if(
//                            Sage50ClientCodeList.Contains((string)reader.GetValue(0))
//                            &&
//                            Sage50GUID_IDList.Contains((string)reader.GetValue(0))
//                            &&
//                            sage50_instance_terminal == DataHolder.Sage50LocalTerminalPath
//                        )
//                        {
//                            IsSynchronized = true;
//                            GestprojectSynchronizedClientsIdList.Add(GestprojectClientId);
//                            SynchronizedClient = new SynchronizedClient();
//                            SynchronizedClient.GestprojectId = GestprojectClientId;
//                            SynchronizedClient.Sage50Code = (string)reader.GetValue(1);
//                            SynchronizedClient.Sage50GUID_ID = (string)reader.GetValue(0);
//                        };
//                    };
//                };
//            };
//        }
//    }
//}
