using sage.ew.db;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SincronizadorGPS50
{
    internal class GetSage50Clients
    {
        internal List<Sage50Client> Sage50ClientClassList {  get; set; } = new List<Sage50Client>();
        internal List<string> Sage50ClientCodeList {  get; set; } = new List<string>();
        internal List<string> Sage50ClientGUID_IDList {  get; set; } = new List<string>();
        internal int LastClientCodeValue {  get; set; }
        internal int NextClientCodeAvailable {  get; set; }

        public GetSage50Clients()
        {
            string getSage50ClientSQLQuery = @"
                SELECT 
                    codigo, 
                    cif, 
                    nombre, 
                    nombre2, 
                    direccion, 
                    codpost, 
                    poblacion, 
                    provincia, 
                    pais, 
                    email, 
                    http, 
                    guid_id 
                FROM " + $"{DB.SQLDatabase("gestion","clientes")}";

            DataTable sage50ClientsDataTable = new DataTable();

            DB.SQLExec(getSage50ClientSQLQuery, ref sage50ClientsDataTable);

            for(int i = 0; i < sage50ClientsDataTable.Rows.Count; i++)
            {
                Sage50Client sage50Client = new Sage50Client();
                sage50Client.CODIGO = sage50ClientsDataTable.Rows[i].ItemArray[0].ToString().Trim();
                sage50Client.CIF = sage50ClientsDataTable.Rows[i].ItemArray[1].ToString().Trim();
                sage50Client.NOMBRE = sage50ClientsDataTable.Rows[i].ItemArray[2].ToString().Trim();
                sage50Client.NOMBRE2 = sage50ClientsDataTable.Rows[i].ItemArray[3].ToString().Trim();
                sage50Client.DIRECCION = sage50ClientsDataTable.Rows[i].ItemArray[4].ToString().Trim();
                sage50Client.CODPOST = sage50ClientsDataTable.Rows[i].ItemArray[5].ToString().Trim();
                sage50Client.POBLACION = sage50ClientsDataTable.Rows[i].ItemArray[6].ToString().Trim();
                sage50Client.PROVINCIA = sage50ClientsDataTable.Rows[i].ItemArray[7].ToString().Trim();
                sage50Client.PAIS = sage50ClientsDataTable.Rows[i].ItemArray[8].ToString().Trim();
                sage50Client.EMAIL = sage50ClientsDataTable.Rows[i].ItemArray[9].ToString().Trim();
                sage50Client.HTTP = sage50ClientsDataTable.Rows[i].ItemArray[10].ToString().Trim();
                sage50Client.GUID_ID = sage50ClientsDataTable.Rows[i].ItemArray[11].ToString().Trim();

                Sage50ClientClassList.Add(sage50Client);
                Sage50ClientCodeList.Add(sage50Client.CODIGO);
                Sage50ClientGUID_IDList.Add(sage50Client.GUID_ID);
            };

            int Sage50HigestCodeNumber = Sage50ClientClassList.First().CODIGO_NUMERO;
            for(int i = 0; i < Sage50ClientClassList.Count; i++)
            {
                if(Sage50ClientClassList[i].CODIGO_NUMERO > Sage50HigestCodeNumber)
                {
                    Sage50HigestCodeNumber = Sage50ClientClassList[i].CODIGO_NUMERO;
                }
            };

            if(Sage50ClientClassList.Count > 0)
            {
                LastClientCodeValue = Sage50HigestCodeNumber;
                NextClientCodeAvailable = Sage50HigestCodeNumber + 1;
            }
            else
            {
                LastClientCodeValue = 1;
                NextClientCodeAvailable = 2;
            };
        }
    }
}
