using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class GetGestprojectParticipants
    {
        internal GetGestprojectParticipants() 
        {
            DataHolder.GestprojectClientIdList.Clear();
            DataHolder.GestprojectProviderIdList.Clear();

            string getGestprojectParticipantTypesSQLString = "SELECT * FROM PAR_TPA";

            SqlCommand getGestprojectParticipantTypesSQLCommand = new SqlCommand(getGestprojectParticipantTypesSQLString, GestprojectDataHolder.GestprojectDatabaseConnection);

            using(SqlDataReader reader = getGestprojectParticipantTypesSQLCommand.ExecuteReader())
            {
                int fieldCount = reader.FieldCount;

                List<int> gestProjectClientIdList = new List<int>();
                List<int> gestProjectProviderIdList = new List<int>();

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

                DataHolder.GestprojectClientIdList = gestProjectClientIdList;
                DataHolder.GestprojectProviderIdList = gestProjectProviderIdList;
            };
        }
    }
}
