using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Clients
{
    internal class EndPointsData
    {
        internal bool Matches { get; set; } = false;
        internal string Comment { get; set; } = "";
        internal Dictionary<string, bool> SynchronizationStatusDictionary { get; set; } = new Dictionary<string, bool>();
        internal List<(int Id, string fieldName, string gestprojectFieldValue, string sage50FieldValue)> DataMismatchTuple { get; set; } = new List<(int Id, string fieldName, string gestprojectFieldValue, string sage50FieldValue)>();
        internal List<Sage50Client> Sage50ClientClassList { get; set; } = new List<Sage50Client>();
        internal List<string> Sage50ClientCodeList { get; set; } = new List<string>();
        internal List<string> Sage50ClientGUID_IDList { get; set; } = new List<string>();
        internal EndPointsData(GestprojectClient client) 
        {
            string getSage50ClientSQLQuery = @"
            SELECT 
                codigo,
                nombre,
                cif,
                direccion,
                codpost,
                poblacion,
                provincia,
                pais
            FROM " + $"{DB.SQLDatabase("gestion","clientes")} WHERE guid_id='{client.sage50_guid_id}'";

            DataTable sage50ClientsDataTable = new DataTable();

            DB.SQLExec(getSage50ClientSQLQuery, ref sage50ClientsDataTable);

            if(sage50ClientsDataTable.Rows.Count > 0)
            {
                Sage50Client sage50Client = new Sage50Client();
                sage50Client.CODIGO = sage50ClientsDataTable.Rows[0].ItemArray[0].ToString().Trim();
                sage50Client.NOMBRE = sage50ClientsDataTable.Rows[0].ItemArray[1].ToString().Trim();
                sage50Client.CIF = sage50ClientsDataTable.Rows[0].ItemArray[2].ToString().Trim();
                sage50Client.DIRECCION = sage50ClientsDataTable.Rows[0].ItemArray[3].ToString().Trim();
                sage50Client.CODPOST = sage50ClientsDataTable.Rows[0].ItemArray[4].ToString().Trim();
                sage50Client.POBLACION = sage50ClientsDataTable.Rows[0].ItemArray[5].ToString().Trim();
                sage50Client.PROVINCIA = sage50ClientsDataTable.Rows[0].ItemArray[6].ToString().Trim();
                sage50Client.PAIS = sage50ClientsDataTable.Rows[0].ItemArray[7].ToString().Trim();

                List<string> GestprojectClientData = new List<string>
                {
                    client.PAR_SUBCTA_CONTABLE.Trim(),
                    client.PAR_NOMBRE.Trim(),
                    client.PAR_CIF_NIF.Trim(),
                    client.PAR_DIRECCION_1.Trim(),
                    client.PAR_CP_1.Trim(),
                    client.PAR_LOCALIDAD_1.Trim(),
                    client.PAR_PROVINCIA_1.Trim(),
                    client.PAR_PAIS_1.Trim(),
                };

                List<string> Sage50ClientData = new List<string>
                {
                    sage50Client.CODIGO,
                    sage50Client.NOMBRE,
                    sage50Client.CIF,
                    sage50Client.DIRECCION,
                    sage50Client.CODPOST,
                    sage50Client.POBLACION,
                    sage50Client.PROVINCIA,
                    sage50Client.PAIS,
                };

                List<string> SynchronizationTableFieldsNames = new List<string>
                {
                    "SUBCUENTA_CONTABLE",
                    "NOMBRE",
                    "CIF_NIF",
                    "DIRECCION",
                    "CODIGO_POSTAL",
                    "POBLACION",
                    "PROVINCIA",
                    "PAIS",
                };

                int ClientErrorQuantity = 0;

                for(global::System.Int32 i = 0; i < SynchronizationTableFieldsNames.Count; i++)
                {
                    SynchronizationStatusDictionary.Add(SynchronizationTableFieldsNames[i], GestprojectClientData[i] == Sage50ClientData[i]);
                    if(GestprojectClientData[i] != Sage50ClientData[i])
                    {
                        ClientErrorQuantity++;
                        DataMismatchTuple.Add((ClientErrorQuantity, SynchronizationTableFieldsNames[i], GestprojectClientData[i], Sage50ClientData[i]));
                    };
                };

                if(ClientErrorQuantity > 0)
                {
                    Comment += ClientErrorQuantity + " errores en este cliente: ";

                    for(global::System.Int32 i = 0; i < DataMismatchTuple.Count; i++)
                    {

                        var dataMismatchTupleElement = DataMismatchTuple[i];
                        int id = dataMismatchTupleElement.Id;
                        string fieldName = dataMismatchTupleElement.fieldName;
                        string sage50FieldValue = dataMismatchTupleElement.sage50FieldValue;
                        string gestprojectFieldValue = dataMismatchTupleElement.gestprojectFieldValue;

                        if(sage50FieldValue == "" && gestprojectFieldValue != "")
                        {
                            Comment += fieldName + " fue rechazado por Sage50. ";
                        }
                        else
                        {
                            Comment += fieldName + $" difiere (El valor en sage es: {sage50FieldValue}). ";
                        }
                    };

                    client.comments = Comment;

                    List<string> checkProblematicFieldList = new List<string>();

                    for(global::System.Int32 i = 0; i < DataMismatchTuple.Count; i++)
                    {
                        var dataMismatchTupleElement = DataMismatchTuple[i];
                        int id = dataMismatchTupleElement.Id;

                        string fieldName = dataMismatchTupleElement.fieldName;

                        string sage50FieldValue = dataMismatchTupleElement.sage50FieldValue;

                        string gestprojectFieldValue = dataMismatchTupleElement.gestprojectFieldValue;

                        if(fieldName == "POBLACION" || fieldName == "PAIS")
                        {
                            checkProblematicFieldList.Add(fieldName);
                        };
                    };

                    if(checkProblematicFieldList.Count > 0)
                    {
                        Matches = true;
                    }
                }
                else
                {
                    Matches = true;
                };
            } 
            //else 
            //{
            //    MessageBox.Show("El cliente fue eliminado de la base de datos de sincronización");
            //};
        }
    }
}
