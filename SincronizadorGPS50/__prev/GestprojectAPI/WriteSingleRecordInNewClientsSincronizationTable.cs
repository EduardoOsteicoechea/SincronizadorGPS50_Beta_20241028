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
//    internal class WriteASingleRecordInNewClientsSincronizationTable
//    {
//        internal WriteASingleRecordInNewClientsSincronizationTable
//        (
//            int gestProjectClientId,
//            string Sage50CurrentClientCode,
//            string Sage50ClientId
//        )
//        {
//            string sqlString = $"WRITE INTO INT_SAGE_SINC_CLIENTE (gestproject_id, sage50_code, sage50_guid_id) VALUES ({gestProjectClientId},'{Sage50CurrentClientCode}','{Sage50ClientId}');";

//            using(SqlCommand SQLCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection))
//            {
//                if(SQLCommand.ExecuteNonQuery() > 0)
//                {
//                    MessageBox.Show("Se creó la tabla INT_SAGE_SINC_CLIENTE exitosamente.");
//                }
//                else
//                {
//                    MessageBox.Show("No se logró crear la tabla INT_SAGE_SINC_CLIENTE.");
//                };
//            };
//        }
//    }
//}
