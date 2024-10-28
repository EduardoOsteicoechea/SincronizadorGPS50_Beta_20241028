using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GetSage50Providers
   {
      public List<Sage50ProviderModel> Entities { get; set; } = new List<Sage50ProviderModel>();
      public List<string> Codes { get; set; } = new List<string>();
      public List<string> Guids { get; set; } = new List<string>();
      public int LastCodeValue { get; set; }
      public int NextCodeAvailable { get; set; }
      public string Code { get; set; }
      public string Guid { get; set; }
      public bool Exists { get; set; } = false;
      public GetSage50Providers()
      {
         try
         {
            string getSage50ProviderSQLQuery = @"
                SELECT 
                    codigo, 
                    cif, 
                    nombre, 
                    direccion, 
                    codpost, 
                    poblacion, 
                    provincia, 
                    pais,
                    guid_id 
                FROM " + $"{DB.SQLDatabase("gestion","proveed")}";

            DataTable sage50ProvidersDataTable = new DataTable();

            DB.SQLExec(getSage50ProviderSQLQuery, ref sage50ProvidersDataTable);

            if(sage50ProvidersDataTable.Rows.Count > 0)
            {
               for(int i = 0; i < sage50ProvidersDataTable.Rows.Count; i++)
               {
                  Sage50ProviderModel sage50Provider = new Sage50ProviderModel();

                  sage50Provider.CODIGO = sage50ProvidersDataTable.Rows[i].ItemArray[0].ToString().Trim();
                  sage50Provider.CIF = sage50ProvidersDataTable.Rows[i].ItemArray[1].ToString().Trim();
                  sage50Provider.NOMBRE = sage50ProvidersDataTable.Rows[i].ItemArray[2].ToString().Trim();
                  sage50Provider.DIRECCION = sage50ProvidersDataTable.Rows[i].ItemArray[3].ToString().Trim();
                  sage50Provider.CODPOST = sage50ProvidersDataTable.Rows[i].ItemArray[4].ToString().Trim();
                  sage50Provider.POBLACION = sage50ProvidersDataTable.Rows[i].ItemArray[5].ToString().Trim();
                  sage50Provider.PROVINCIA = sage50ProvidersDataTable.Rows[i].ItemArray[6].ToString().Trim();
                  sage50Provider.PAIS = sage50ProvidersDataTable.Rows[i].ItemArray[7].ToString().Trim();
                  sage50Provider.GUID_ID = sage50ProvidersDataTable.Rows[i].ItemArray[8].ToString().Trim();

                  Entities.Add(sage50Provider);
                  Codes.Add(sage50Provider.CODIGO);
                  Guids.Add(sage50Provider.GUID_ID);
               };

               int Sage50HigestCodeNumber = Entities.First().CODIGO_NUMERO;
               for(int i = 0; i < Entities.Count; i++)
               {
                  if(Entities[i].CODIGO_NUMERO > Sage50HigestCodeNumber)
                  {
                     Sage50HigestCodeNumber = Entities[i].CODIGO_NUMERO;
                  };
               };

               if(Entities.Count > 0)
               {
                  LastCodeValue = Sage50HigestCodeNumber;
                  NextCodeAvailable = Sage50HigestCodeNumber + 1;
               }
               else
               {
                  LastCodeValue = 1;
                  NextCodeAvailable = 2;
               };
            }
            else
            {
               throw new Exception("No providers were found in Sage50");
            }
         }
         catch(Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
   }
}
