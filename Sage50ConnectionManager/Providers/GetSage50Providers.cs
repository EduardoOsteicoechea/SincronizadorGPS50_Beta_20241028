using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SincronizadorGPS50.Sage50Connector
{
   public class GetSage50Providers
   {
      public List<Sage50ProviderModel> ProviderList { get; set; } = new List<Sage50ProviderModel>();
      public List<string> Sage50ProviderCodeList { get; set; } = new List<string>();
      public List<string> Sage50ProviderGUID_IDList { get; set; } = new List<string>();
      public int LastProviderCodeValue { get; set; }
      public int NextProviderCodeAvailable { get; set; }
      public string Codigo { get; set; }
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

                  ProviderList.Add(sage50Provider);
                  Sage50ProviderCodeList.Add(sage50Provider.CODIGO);
                  Sage50ProviderGUID_IDList.Add(sage50Provider.GUID_ID);
               };

               int Sage50HigestCodeNumber = ProviderList.First().CODIGO_NUMERO;
               for(int i = 0; i < ProviderList.Count; i++)
               {
                  if(ProviderList[i].CODIGO_NUMERO > Sage50HigestCodeNumber)
                  {
                     Sage50HigestCodeNumber = ProviderList[i].CODIGO_NUMERO;
                  };
               };

               if(ProviderList.Count > 0)
               {
                  LastProviderCodeValue = Sage50HigestCodeNumber;
                  NextProviderCodeAvailable = Sage50HigestCodeNumber + 1;
               }
               else
               {
                  LastProviderCodeValue = 1;
                  NextProviderCodeAvailable = 2;
               };
            }
            else
            {
               throw new System.Exception("No providers were found in Sage50");
            }
         }
         catch(System.Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}
