using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SincronizadorGPS50.Sage50Connector
{
   public class GetSage50Customer
   {
      public List<Sage50Customer> CustomerList { get; set; } = new List<Sage50Customer>();
      public List<string> Sage50CustomerCodeList { get; set; } = new List<string>();
      public List<string> Sage50CustomerGUID_IDList { get; set; } = new List<string>();
      public int LastCustomerCodeValue { get; set; }
      public int NextCustomerCodeAvailable { get; set; }
      public string Codigo { get; set; }
      public string Cif { get; set; }
      public string Nombre { get; set; }
      public string Direccion { get; set; }
      public string Codpost { get; set; }
      public string Poblacion { get; set; }
      public string Provincia { get; set; }
      public string Pais { get; set; }
      public bool Exists { get; set; } = false;
      public GetSage50Customer
      (
         string guidId = ""
      )
      {
         try
         {
            string getSage50CustomerSQLQuery = @"
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

            DataTable sage50CustomersDataTable = new DataTable();

            DB.SQLExec(getSage50CustomerSQLQuery, ref sage50CustomersDataTable);

            if(sage50CustomersDataTable.Rows.Count > 0)
            {
               for(int i = 0; i < sage50CustomersDataTable.Rows.Count; i++)
               {
                  Sage50Customer sage50Customer = new Sage50Customer();

                  sage50Customer.CODIGO = sage50CustomersDataTable.Rows[i].ItemArray[0].ToString().Trim();
                  sage50Customer.CIF = sage50CustomersDataTable.Rows[i].ItemArray[1].ToString().Trim();
                  sage50Customer.NOMBRE = sage50CustomersDataTable.Rows[i].ItemArray[2].ToString().Trim();
                  sage50Customer.NOMBRE2 = sage50CustomersDataTable.Rows[i].ItemArray[3].ToString().Trim();
                  sage50Customer.DIRECCION = sage50CustomersDataTable.Rows[i].ItemArray[4].ToString().Trim();
                  sage50Customer.CODPOST = sage50CustomersDataTable.Rows[i].ItemArray[5].ToString().Trim();
                  sage50Customer.POBLACION = sage50CustomersDataTable.Rows[i].ItemArray[6].ToString().Trim();
                  sage50Customer.PROVINCIA = sage50CustomersDataTable.Rows[i].ItemArray[7].ToString().Trim();
                  sage50Customer.PAIS = sage50CustomersDataTable.Rows[i].ItemArray[8].ToString().Trim();
                  sage50Customer.EMAIL = sage50CustomersDataTable.Rows[i].ItemArray[9].ToString().Trim();
                  sage50Customer.HTTP = sage50CustomersDataTable.Rows[i].ItemArray[10].ToString().Trim();
                  sage50Customer.GUID_ID = sage50CustomersDataTable.Rows[i].ItemArray[11].ToString().Trim();

                  CustomerList.Add(sage50Customer);
                  Sage50CustomerCodeList.Add(sage50Customer.CODIGO);
                  Sage50CustomerGUID_IDList.Add(sage50Customer.GUID_ID);
               };

               int Sage50HigestCodeNumber = CustomerList.First().CODIGO_NUMERO;
               for(int i = 0; i < CustomerList.Count; i++)
               {
                  if(CustomerList[i].CODIGO_NUMERO > Sage50HigestCodeNumber)
                  {
                     Sage50HigestCodeNumber = CustomerList[i].CODIGO_NUMERO;
                  };
               };

               if(CustomerList.Count > 0)
               {
                  LastCustomerCodeValue = Sage50HigestCodeNumber;
                  NextCustomerCodeAvailable = Sage50HigestCodeNumber + 1;
               }
               else
               {
                  LastCustomerCodeValue = 1;
                  NextCustomerCodeAvailable = 2;
               };
            }
            else
            {
               throw new System.Exception("No clients were found in Sage50");
            }
         }
         catch(System.Exception exception)
         {
            throw new Exception($"En:\n\nSincronizadorGPS50.Sage50Connector\n.GetSage50Customer:\n\n{exception.Message}");
         };
      }
   }
}
