//using sage.ew.db;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class GetSage50Projects
//   {
//      public List<Sage50ProjectModel> Entities { get; set; } = new List<Sage50ProjectModel>();
//      public List<string> Codes { get; set; } = new List<string>();
//      public List<string> Guids { get; set; } = new List<string>();
//      public int LastCodeValue { get; set; }
//      public int NextCodeAvailable { get; set; }
//      public string Code { get; set; }
//      public string Guid { get; set; }
//      public bool Exists { get; set; } = false;
//      public GetSage50Projects()
//      {
//         try
//         {
//            string getSage50ProviderSQLQuery = @"
//                SELECT 
//                    codigo,
//                    nombre, 
//                    direccion, 
//                    codpost, 
//                    poblacion, 
//                    provincia,
//                    guid_id 
//                FROM " + $"{DB.SQLDatabase("comunes","obra")}";

//            DataTable sage50ProvidersDataTable = new DataTable();

//            DB.SQLExec(getSage50ProviderSQLQuery, ref sage50ProvidersDataTable);

//            if(sage50ProvidersDataTable.Rows.Count > 0)
//            {
//               for(int i = 0; i < sage50ProvidersDataTable.Rows.Count; i++)
//               {
//                  Sage50ProjectModel sage50Provider = new Sage50ProjectModel();

//                  sage50Provider.CODIGO = sage50ProvidersDataTable.Rows[i].ItemArray[0].ToString().Trim();
//                  sage50Provider.NOMBRE = sage50ProvidersDataTable.Rows[i].ItemArray[1].ToString().Trim();
//                  sage50Provider.DIRECCION = sage50ProvidersDataTable.Rows[i].ItemArray[2].ToString().Trim();
//                  sage50Provider.CODPOST = sage50ProvidersDataTable.Rows[i].ItemArray[4].ToString().Trim();
//                  sage50Provider.POBLACION = sage50ProvidersDataTable.Rows[i].ItemArray[4].ToString().Trim();
//                  sage50Provider.PROVINCIA = sage50ProvidersDataTable.Rows[i].ItemArray[5].ToString().Trim();
//                  sage50Provider.GUID_ID = sage50ProvidersDataTable.Rows[i].ItemArray[6].ToString().Trim();

//                  Entities.Add(sage50Provider);
//                  Codes.Add(sage50Provider.CODIGO);
//                  Guids.Add(sage50Provider.GUID_ID);
//               };

//               int Sage50HigestCodeNumber = int.Parse(Entities.Last().CODIGO);
//               Sage50HigestCodeNumber++;

//               if(Entities.Count > 0)
//               {
//                  LastCodeValue = Sage50HigestCodeNumber;
//                  NextCodeAvailable = Sage50HigestCodeNumber + 1;
//               }
//               else
//               {
//                  LastCodeValue = 1;
//                  NextCodeAvailable = 2;
//               };
//            };
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//   }
//}