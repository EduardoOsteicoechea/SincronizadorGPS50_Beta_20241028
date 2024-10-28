//using sage.ew.db;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.Sage50Connector
//{
//   internal class GetSage50Customer
//   {
//      internal List<Sage50Customer> Sage50ClientClassList { get; set; } = new List<Sage50Customer>();
//      internal List<string> Sage50ClientCodeList { get; set; } = new List<string>();
//      internal List<string> Sage50ClientGUID_IDList { get; set; } = new List<string>();
//      internal int LastClientCodeValue { get; set; }
//      internal int NextClientCodeAvailable { get; set; }
//      internal string Codigo { get; set; }
//      internal string Cif { get; set; }
//      internal string Nombre { get; set; }
//      internal string Direccion { get; set; }
//      internal string Codpost { get; set; }
//      internal string Poblacion { get; set; }
//      internal string Provincia { get; set; }
//      internal string Pais { get; set; }
//      internal bool Exists { get; set; } = false;
//      public GetSage50Customer
//      (
//         string guidId = ""
//      )
//      {
//         try
//         {
//            string getSage50ClientSQLQuery = $@"
//         SELECT 
//            codigo,
//            cif,
//            nombre,
//            direccion,
//            codpost,
//            poblacion,
//            provincia,
//            pais
//         FROM 
//            {DB.SQLDatabase("gestion","clientes")}
//         WHERE
//            codigo LIKE '43%';";

//            DataTable sage50ClientsDataTable = new DataTable();

//            DB.SQLExec(getSage50ClientSQLQuery, ref sage50ClientsDataTable);

//            if(sage50ClientsDataTable.Rows.Count > 0)
//            {
//               Sage50Customer sage50Customer = new Sage50Customer();

//               for(int i = 0; i < sage50ClientsDataTable.Rows.Count; i++)
//               {
//                  Exists = true;
//                  Codigo = sage50ClientsDataTable.Rows[i].ItemArray[0].ToString().Trim();
//                  Cif = sage50ClientsDataTable.Rows[i].ItemArray[1].ToString().Trim();
//                  Nombre = sage50ClientsDataTable.Rows[i].ItemArray[2].ToString().Trim();
//                  Direccion = sage50ClientsDataTable.Rows[i].ItemArray[3].ToString().Trim();
//                  Codpost = sage50ClientsDataTable.Rows[i].ItemArray[4].ToString().Trim();
//                  Poblacion = sage50ClientsDataTable.Rows[i].ItemArray[5].ToString().Trim();
//                  Provincia = sage50ClientsDataTable.Rows[i].ItemArray[6].ToString().Trim();
//                  Pais = sage50ClientsDataTable.Rows[i].ItemArray[7].ToString().Trim();

//                  Exists = true;
//                  sage50Customer.CODIGO = sage50ClientsDataTable.Rows[i].ItemArray[0].ToString().Trim();
//                  sage50Customer.CIF = sage50ClientsDataTable.Rows[i].ItemArray[1].ToString().Trim();
//                  sage50Customer.NOMBRE = sage50ClientsDataTable.Rows[i].ItemArray[2].ToString().Trim();
//                  sage50Customer.DIRECCION = sage50ClientsDataTable.Rows[i].ItemArray[3].ToString().Trim();
//                  sage50Customer.CODPOST = sage50ClientsDataTable.Rows[i].ItemArray[4].ToString().Trim();
//                  sage50Customer.POBLACION = sage50ClientsDataTable.Rows[i].ItemArray[5].ToString().Trim();
//                  sage50Customer.PROVINCIA = sage50ClientsDataTable.Rows[i].ItemArray[6].ToString().Trim();
//                  sage50Customer.PAIS = sage50ClientsDataTable.Rows[i].ItemArray[7].ToString().Trim();

//                  Sage50ClientClassList.Add(sage50Customer);
//               };

//               int Sage50HigestCodeNumber = Sage50ClientClassList.First().CODIGO_NUMERO;
//               for(int i = 0; i < Sage50ClientClassList.Count; i++)
//               {
//                  if(Sage50ClientClassList[i].CODIGO_NUMERO > Sage50HigestCodeNumber)
//                  {
//                     Sage50HigestCodeNumber = Sage50ClientClassList[i].CODIGO_NUMERO;
//                  }
//               };

//               if(Sage50ClientClassList.Count > 0)
//               {
//                  LastClientCodeValue = Sage50HigestCodeNumber;
//                  NextClientCodeAvailable = Sage50HigestCodeNumber + 1;
//               }
//               else
//               {
//                  LastClientCodeValue = 1;
//                  NextClientCodeAvailable = 2;
//               };
//            }
//            else
//            {
//               throw new System.Exception();
//            }
//         }
//         catch (System.Exception exception)
//         {
//            throw new Exception($"En:\n\nSincronizadorGPS50.Sage50Connector\n.GetSage50Customer:\n\n{exception.Message}");
//         };
//      }
//   }
//}
