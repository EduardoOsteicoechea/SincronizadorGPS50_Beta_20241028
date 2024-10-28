//using sage.ew.db;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;

//namespace SincronizadorGPS50.GestprojectAPI
//{
//    internal static class GestprojectClientsActions
//    {
//        public static void GetGestprojectClients()
//        {
//            GestprojectDataHolder.GestprojectDatabaseConnection.Open();

//            string getGestprojectParticipantTypesSQLString = "SELECT * FROM PAR_TPA";

//            SqlCommand getGestprojectParticipantTypesSQLCommand = new SqlCommand(getGestprojectParticipantTypesSQLString, GestprojectDataHolder.GestprojectDatabaseConnection);
//            using(SqlDataReader reader = getGestprojectParticipantTypesSQLCommand.ExecuteReader())
//            {
//                int fieldCount = reader.FieldCount;

//                List<int> gestProjectClientIdList = new List<int>();
//                List<int> gestProjectProviderIdList = new List<int>();

//                while(reader.Read())
//                {
//                    if(reader.GetValue(0).ToString() == "1")
//                    {
//                        gestProjectClientIdList.Add((int)reader.GetValue(1));
//                    }
//                    else if(reader.GetValue(0).ToString() == "12")
//                    {
//                        gestProjectProviderIdList.Add((int)reader.GetValue(1));
//                    };
//                };
//                gestProjectClientIdList.Distinct().ToList();
//                gestProjectProviderIdList.Distinct().ToList();

//                DataHolder.GestprojectClientIdList = gestProjectClientIdList;
//                DataHolder.GestprojectProviderIdList = gestProjectProviderIdList;
//            };

//            string sqlString = "SELECT PAR_ID, PAR_SUBCTA_CONTABLE, PAR_NOMBRE, PAR_NOMBRE_COMERCIAL, PAR_CIF_NIF, PAR_DIRECCION_1, PAR_CP_1, PAR_LOCALIDAD_1, PAR_PROVINCIA_1, PAR_PAIS_1 FROM PARTICIPANTE;";

//            SqlCommand sqlCommand = new SqlCommand(sqlString, GestprojectDataHolder.GestprojectDatabaseConnection);

//            using(SqlDataReader reader = sqlCommand.ExecuteReader())
//            {
//                while(reader.Read())
//                {
//                    if(DataHolder.GestprojectClientIdList.Contains((int)reader.GetValue(0)))
//                    {
//                        GestprojectClient gestprojectClient = new GestprojectClient();
//                        gestprojectClient.PAR_ID = (int)reader.GetValue(0);
//                        gestprojectClient.PAR_SUBCTA_CONTABLE = (string)reader.GetValue(1);
//                        gestprojectClient.PAR_NOMBRE = (string)reader.GetValue(2);
//                        gestprojectClient.PAR_NOMBRE_COMERCIAL = (string)reader.GetValue(3);
//                        gestprojectClient.PAR_CIF_NIF = (string)reader.GetValue(4);
//                        gestprojectClient.PAR_DIRECCION_1 = (string)reader.GetValue(5);
//                        gestprojectClient.PAR_CP_1 = (string)reader.GetValue(6);
//                        gestprojectClient.PAR_LOCALIDAD_1 = (string)reader.GetValue(7);
//                        gestprojectClient.PAR_PROVINCIA_1 = (string)reader.GetValue(8);
//                        gestprojectClient.PAR_PAIS_1 = (string)reader.GetValue(9);

//                        DataHolder.GestprojectClientClassList.Add(gestprojectClient);
//                    }
//                };
//            };

//            GestprojectDataHolder.GestprojectDatabaseConnection.Close();

//            // GetSage50Client
//            // GetSage50Client
//            // GetSage50Client
//            // GetSage50Client
//            // GetSage50Client

//            string getSage50ClientSQLQuery = $"SELECT codigo, cif, nombre, nombre2, direccion, codpost, poblacion, provincia, pais, email, http, guid_id FROM {DB.SQLDatabase("gestion","clientes")}";

//            DataTable sage50ClientsDataTable = new DataTable();

//            DB.SQLExec(getSage50ClientSQLQuery, ref sage50ClientsDataTable);

//            DataHolder.Sage50ClientClassList.Clear();

//            for(int i = 0; i < sage50ClientsDataTable.Rows.Count; i++)
//            {
//                Sage50Client sage50Client = new Sage50Client();
//                sage50Client.CODIGO = sage50ClientsDataTable.Rows[i].ItemArray[0].ToString().Trim();
//                sage50Client.CIF = sage50ClientsDataTable.Rows[i].ItemArray[1].ToString().Trim();
//                sage50Client.NOMBRE = sage50ClientsDataTable.Rows[i].ItemArray[2].ToString().Trim();
//                sage50Client.NOMBRE2 = sage50ClientsDataTable.Rows[i].ItemArray[3].ToString().Trim();
//                sage50Client.DIRECCION = sage50ClientsDataTable.Rows[i].ItemArray[4].ToString().Trim();
//                sage50Client.CODPOST = sage50ClientsDataTable.Rows[i].ItemArray[5].ToString().Trim();
//                sage50Client.POBLACION = sage50ClientsDataTable.Rows[i].ItemArray[6].ToString().Trim();
//                sage50Client.PROVINCIA = sage50ClientsDataTable.Rows[i].ItemArray[7].ToString().Trim();
//                sage50Client.PAIS = sage50ClientsDataTable.Rows[i].ItemArray[8].ToString().Trim();
//                sage50Client.EMAIL = sage50ClientsDataTable.Rows[i].ItemArray[9].ToString().Trim();
//                sage50Client.HTTP = sage50ClientsDataTable.Rows[i].ItemArray[10].ToString().Trim();
//                sage50Client.GUID_ID = sage50ClientsDataTable.Rows[i].ItemArray[11].ToString().Trim();

//                DataHolder.Sage50ClientClassList.Add(sage50Client);

//                //PrintClassProperties.Print(sage50Client);
//            };

//            // GetSage50Client CIF_NIF
//            // GetSage50Client CIF_NIF
//            // GetSage50Client CIF_NIF
//            // GetSage50Client CIF_NIF
//            // GetSage50Client CIF_NIF

//            //for(int i = 0; i < DataHolder.Sage50ClientClassList.Count; i++)
//            //{
//            //    if(DataHolder.Sage50ClientClassList[i].CIF != "" && DataHolder.Sage50ClientClassList[i].CIF != null)
//            //    {
//            //        DataHolder.Sage50CIFList.Add(DataHolder.Sage50ClientClassList[i].CIF);
//            //    }
//            //    else
//            //    {
//            //        DataHolder.Sage50CIFList.Add("Sin valor provisto");
//            //    };
//            //}

//            // GetSage50Client Last CODE
//            // GetSage50Client Last CODE
//            // GetSage50Client Last CODE
//            // GetSage50Client Last CODE
//            // GetSage50Client Last CODE

//            int Sage50HigestCodeNumber = DataHolder.Sage50ClientClassList.First().CODIGO_NUMERO;
//            for(int i = 0; i < DataHolder.Sage50ClientClassList.Count; i++)
//            {
//                if(DataHolder.Sage50ClientClassList[i].CODIGO_NUMERO > Sage50HigestCodeNumber)
//                {
//                    Sage50HigestCodeNumber = DataHolder.Sage50ClientClassList[i].CODIGO_NUMERO;
//                }
//            };

//            int counter = Sage50HigestCodeNumber + 1;

//            // CreateSincronizationTable
//            // CreateSincronizationTable
//            // CreateSincronizationTable
//            // CreateSincronizationTable
//            // CreateSincronizationTable

//            DataTable table = new DataTable();
//            PropertyInfo[] properties = typeof(ClientsSynchronizationTable).GetProperties();

//            for(int i = 0; i < properties.Length; i++)
//            {
//                PropertyInfo property = properties[i];
//                table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
//            };

//            // Check for existing clients in Gestproject Sincronization Database table
//            // Check for existing clients in Gestproject Sincronization Database table
//            // Check for existing clients in Gestproject Sincronization Database table
//            // Check for existing clients in Gestproject Sincronization Database table

//            // Check if Sage50SincronizationTable exist in Gestproject database

//            // Create table

//            // if created, just record

//            // else, check for guid,

//            // if no GUID_ID, create

//            //if guid, next iteration

//            // CreateSage50Client
//            // CreateSage50Client
//            // CreateSage50Client
//            // CreateSage50Client
//            // CreateSage50Client

//            for(int i = 0; i < DataHolder.GestprojectClientClassList.Count; i++)
//            {
//                GestprojectClient gestProjectClient = DataHolder.GestprojectClientClassList[i];

//                //if(!DataHolder.Sage50CIFList.Contains(gestProjectClient.PAR_CIF_NIF))
//                //{
//                Customer customer = new Customer();
//                clsEntityCustomer clsEntityCustomerInstance = new clsEntityCustomer();
//                string currentClientCode = "";

//                if(counter < 10)
//                {
//                    clsEntityCustomerInstance.codigo = "4300000" + counter;
//                    currentClientCode = clsEntityCustomerInstance.codigo;
//                }
//                else if(counter < 100)
//                {
//                    clsEntityCustomerInstance.codigo = "430000" + counter;
//                    currentClientCode = clsEntityCustomerInstance.codigo;
//                }
//                else
//                {
//                    clsEntityCustomerInstance.codigo = "43000" + counter;
//                    currentClientCode = clsEntityCustomerInstance.codigo;
//                };

//                clsEntityCustomerInstance.pais = gestProjectClient.PAR_CP_1;
//                clsEntityCustomerInstance.nombre = gestProjectClient.PAR_NOMBRE;
//                clsEntityCustomerInstance.cif = gestProjectClient.PAR_CIF_NIF;
//                clsEntityCustomerInstance.direccion = gestProjectClient.PAR_DIRECCION_1;
//                clsEntityCustomerInstance.provincia = gestProjectClient.PAR_PROVINCIA_1;
//                clsEntityCustomerInstance.tipo_iva = "03";

//                customer._Create(clsEntityCustomerInstance);

//                counter++;
//                //};

//                // Get new Sage50Client guid_id
//                // Get new Sage50Client guid_id
//                // Get new Sage50Client guid_id
//                // Get new Sage50Client guid_id
//                // Get new Sage50Client guid_id

//                string Sage50NewClientSQLQuery = $"SELECT guid_id FROM {DB.SQLDatabase("gestion","clientes")} WHERE \"codigo\"={currentClientCode}";
//                DataTable Sage50NewClientDataTable = new DataTable();
//                DB.SQLExec(Sage50NewClientSQLQuery, ref Sage50NewClientDataTable);
//                string Sage50ClientId = Sage50NewClientDataTable.Rows[0].ItemArray[0].ToString().Trim();

//                // Generate SynchronizationTableData
//                // Generate SynchronizationTableData
//                // Generate SynchronizationTableData
//                // Generate SynchronizationTableData
//                // Generate SynchronizationTableData

//                GestprojectClient client = DataHolder.GestprojectClientClassList[i];
//                DataRow row = table.NewRow();
//                foreach(PropertyInfo prop in properties)
//                {
//                    row[0] = "No Sincronizado";
//                    row[1] = i + 1;
//                    row[2] = client.PAR_ID;
//                    row[3] = currentClientCode;
//                    row[4] = Sage50ClientId;
//                    row[5] = client.PAR_SUBCTA_CONTABLE;
//                    row[6] = client.PAR_NOMBRE;
//                    row[7] = client.PAR_NOMBRE_COMERCIAL;
//                    row[8] = client.PAR_CIF_NIF;
//                    row[9] = client.PAR_DIRECCION_1;
//                    row[10] = client.PAR_CP_1;
//                    row[11] = client.PAR_LOCALIDAD_1;
//                    row[12] = client.PAR_PROVINCIA_1;
//                    row[13] = client.PAR_PAIS_1;
//                };
//                table.Rows.Add(row);
//            }

//            // Move the sincronized table to a Global scope
//            // Move the sincronized table to a Global scope
//            // Move the sincronized table to a Global scope
//            // Move the sincronized table to a Global scope
//            // Move the sincronized table to a Global scope

//            DataHolder.GestprojectClientsTable = table;
//        }
//    }
//}
