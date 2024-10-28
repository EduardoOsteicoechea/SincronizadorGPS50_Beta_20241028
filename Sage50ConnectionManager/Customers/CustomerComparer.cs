using FuzzySharp;
using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;

namespace SincronizadorGPS50.Sage50Connector
{
   public class CustomerComparer
   {
      public Sage50Customer Sage50Customer { get; set; } = new Sage50Customer();
      public List<Sage50Customer> Sage50CustomerList { get; set; } = new List<Sage50Customer>();
      public bool ClientExists { get; set; } = false;
      public string CustomerGuid { get; set; } = "";
      public string CustomerCode{ get; set; } = "";
      public CustomerComparer
      (
         string name,
         string cif
      )
      {
         try
         {
            Getsage50Clients();

            foreach(Sage50Customer customer in Sage50CustomerList)
            {

               bool noHayCifEnAlguno = cif == "" || customer.CIF == "";
               bool noHayCifEnNinguno = cif == "" && customer.CIF == "";

               bool coincidenciaCompletaDeNombre = name == customer.NOMBRE;
               //bool coincidenciaCompletaDeCif = (!noHayCifEnNinguno) && (cif == customer.CIF);
               bool coincidenciaCompletaDeCif = cif == customer.CIF;

               bool nombresSimilares = IsThisSimilar(name, customer.NOMBRE, 95);
               bool cifSimilares = !noHayCifEnNinguno && (IsThisSimilar(cif, customer.CIF, 90) || customer.CIF =="");

               bool noHayCifNombreParecido = noHayCifEnNinguno && nombresSimilares;

               bool nombreYCifSimilares = nombresSimilares && cifSimilares;

               if
               (
                  (
                     coincidenciaCompletaDeNombre
                     &&
                     coincidenciaCompletaDeCif
                  )
                  //||
                  //(
                  //   coincidenciaCompletaDeNombre
                  //   &&
                  //   noHayCifEnNinguno
                  //)
                  //||
                  //noHayCifNombreParecido
                  //||
                  //nombreYCifSimilares
               )
               {
                  CustomerGuid = customer.GUID_ID;
                  CustomerCode = customer.CODIGO;
                  ClientExists = true;
               }
            }
         }
         catch(Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }

      public static bool IsThisSimilar
      (
         string value1,
         string value2,
         int minimalToleranceRatio
      )
      {
         try
         {
            //MessageBox.Show($"\"{value1}\" & \"{value2}\" have a: {Fuzz.Ratio(value1, value2)}% match ratio");
            //if(Fuzz.Ratio(value1, value2) > minimalToleranceRatio || Fuzz.Ratio(value1, value2) == 0)
            if(Fuzz.Ratio(value1, value2) > minimalToleranceRatio)
            {
               return true;
            }
            else
               return false;
         }
         catch(Exception exception)
         {
            throw new Exception($"En:\n\nSage50ConnectionManager.Clients\n.CustomerManager\n.IsThisSimilar:\n\n{exception.Message}");
         };
      }



      public void Getsage50Clients()
      {
         try
         {
            string getSage50CustomerSQLQuery = $@"
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
                    guid_id
                FROM 
                  {DB.SQLDatabase("gestion","clientes")}
               ;";

            DataTable sage50CustomersDataTable = new DataTable();

            DB.SQLExec(getSage50CustomerSQLQuery, ref sage50CustomersDataTable);

            if(sage50CustomersDataTable.Rows.Count > 0)
            {
               for(global::System.Int32 i = 0; i < sage50CustomersDataTable.Rows.Count; i++)
               {
                  Sage50Customer customer = new Sage50Customer();
                  customer.CODIGO = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[0].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[0].ToString().Trim());
                  customer.CIF = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[4].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[1].ToString().Trim());
                  customer.NOMBRE = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[2].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[2].ToString().Trim());
                  customer.NOMBRE2 = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[3].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[3].ToString().Trim());
                  customer.DIRECCION = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[4].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[4].ToString().Trim());
                  customer.CODPOST = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[5].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[5].ToString().Trim());
                  customer.POBLACION = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[6].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[6].ToString().Trim());
                  customer.PROVINCIA = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[7].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[7].ToString().Trim());
                  customer.PAIS = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[8].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[8].ToString().Trim());
                  customer.GUID_ID = Convert.ToString(sage50CustomersDataTable.Rows[i].ItemArray[9].ToString().Trim() == "DBNull" ? "" : sage50CustomersDataTable.Rows[i].ItemArray[9].ToString().Trim());

                  //customer.CIF = sage50CustomersDataTable.Rows[i].ItemArray[1].ToString().Trim();
                  //customer.NOMBRE = sage50CustomersDataTable.Rows[i].ItemArray[2].ToString().Trim();
                  //customer.NOMBRE2 = sage50CustomersDataTable.Rows[i].ItemArray[3].ToString().Trim();
                  //customer.DIRECCION = sage50CustomersDataTable.Rows[i].ItemArray[4].ToString().Trim();
                  //customer.CODPOST = sage50CustomersDataTable.Rows[i].ItemArray[5].ToString().Trim();
                  //customer.POBLACION = sage50CustomersDataTable.Rows[i].ItemArray[6].ToString().Trim();
                  //customer.PROVINCIA = sage50CustomersDataTable.Rows[i].ItemArray[7].ToString().Trim();
                  //customer.PAIS = sage50CustomersDataTable.Rows[i].ItemArray[8].ToString().Trim();
                  //customer.GUID_ID = sage50CustomersDataTable.Rows[i].ItemArray[9].ToString().Trim();

                  Sage50CustomerList.Add(customer);
               };
            };
         }
         catch(Exception exception)
         {
            throw new Exception($"En:\n\nSage50ConnectionManager\n.CustomerManager\n.Getsage50Clients:\n\n{exception.Message}");
         };
      }
   }
}
