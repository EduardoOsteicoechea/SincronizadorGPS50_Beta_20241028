using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static sage.ew.docsven.FirmaElectronica;

namespace SincronizadorGPS50.GestprojectAPI
{
    internal class IsClientUpToDateWithSage50
    {
        internal bool ItIs {  get; set; } = false;
        internal string Comment {  get; set; }
        internal Dictionary<string, bool> SynchronizationStatusDictionary { get; set; } = new Dictionary<string, bool>();
        internal IsClientUpToDateWithSage50(GestprojectClient client)
        {
            string sqlString = $"SELECT sage50_guid_id FROM INT_SAGE_SINC_CLIENTE_IMAGEN WHERE PAR_ID='{client.PAR_ID}';";

            SqlCommand sqlCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection);

            string clientRegisteredGUID = "";

            using(SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while(reader.Read())
                {
                    clientRegisteredGUID = (string)reader.GetValue(0);
                };
            };

            string getSage50ClientSQLQuery = $"SELECT codigo, nombre, cif, direccion, codpost, poblacion, provincia, pais FROM {DB.SQLDatabase("gestion","clientes")} WHERE guid_id='{clientRegisteredGUID}';";

            DataTable sage50ClientsDataTable = new DataTable();

            DB.SQLExec(getSage50ClientSQLQuery, ref sage50ClientsDataTable);

            string database_sage50_client_code = sage50ClientsDataTable.Rows[0].ItemArray[0].ToString().Trim();
            string database_par_nombre = sage50ClientsDataTable.Rows[0].ItemArray[1].ToString().Trim();
            string database_par_cif_nif = sage50ClientsDataTable.Rows[0].ItemArray[2].ToString().Trim();
            string database_par_direccion_1 = sage50ClientsDataTable.Rows[0].ItemArray[3].ToString().Trim();
            string database_par_cp_1 = sage50ClientsDataTable.Rows[0].ItemArray[4].ToString().Trim();
            string database_par_localidad_1 = sage50ClientsDataTable.Rows[0].ItemArray[5].ToString().Trim();
            string database_par_provincia_1 = sage50ClientsDataTable.Rows[0].ItemArray[6].ToString().Trim();
            string database_par_pais_1 = sage50ClientsDataTable.Rows[0].ItemArray[7].ToString().Trim();

            List<string> clientFields = new List<string>
            {
                client.PAR_SUBCTA_CONTABLE,
                client.PAR_NOMBRE,
                client.PAR_CIF_NIF,
                client.PAR_DIRECCION_1,
                client.PAR_CP_1,
                client.PAR_LOCALIDAD_1,
                client.PAR_PROVINCIA_1,
                client.PAR_PAIS_1,
            };

            List<string> databaseFields = new List<string>
            {
                database_sage50_client_code,
                database_par_nombre,
                database_par_cif_nif,
                database_par_direccion_1,
                database_par_cp_1,
                database_par_localidad_1,
                database_par_provincia_1,
                database_par_pais_1,
            };

            List<string> fieldNames = new List<string>
            {
                "subcta_contable",
                "mombre",
                "cif_nif",
                "direccion_1",
                "cp_1",
                "localidad_1",
                "provincia_1",
                "pais_1",
            };

            int ClientErrorQuantity = 0;

            for(global::System.Int32 i = 0; i < clientFields.Count; i++)
            {
                SynchronizationStatusDictionary.Add(fieldNames[i], clientFields[i] == databaseFields[i]);
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
        }
    }
}
