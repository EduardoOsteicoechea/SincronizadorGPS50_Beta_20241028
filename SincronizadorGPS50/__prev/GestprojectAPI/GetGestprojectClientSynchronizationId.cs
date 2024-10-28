using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class GetGestprojectClientSynchronizationId
    {
        internal int Value { get; set; }
        internal GetGestprojectClientSynchronizationId (GestprojectClient gestprojectClient) 
        {
            string sqlString = $"SELECT id FROM INT_SAGE_SINC_CLIENTE WHERE gestproject_id={gestprojectClient.PAR_ID};";

            SqlCommand sqlCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection);

            using(SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while(reader.Read())
                {
                    Value = (int)reader.GetValue(0);
                };
            };
        }   
    }
}
