using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class AddAccountableSubacountValueToClient
    {
        internal AddAccountableSubacountValueToClient(int gestprojectClientid, string sage50ClientCode) 
        {
            string tableName = "PARTICIPANTE";
            string sqlString = $"UPDATE {tableName} SET PAR_SUBCTA_CONTABLE = {sage50ClientCode} WHERE PAR_ID = {gestprojectClientid};";

            using(SqlCommand SQLCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection))
            {
                SQLCommand.ExecuteNonQuery();
            };
        }
    }
}
