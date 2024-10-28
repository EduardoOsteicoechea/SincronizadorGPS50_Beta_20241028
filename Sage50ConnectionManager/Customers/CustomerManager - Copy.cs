//using FuzzySharp;
//using sage.ew.db;
//using SincronizadorGPS50.Sage50Connector;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Sage50ConnectionManager
//{
//   public static class CustomerManager
//   {
//      private static Sage50Customer Sage50Customer { get; set; } = new Sage50Customer();

//      private static void PrintMessage(string fileldName, string gpValue, string SValue, string customerName)
//      {

//         MessageBox.Show($"{fileldName}: \"{gpValue}\", era simimilar a: \"{SValue}\" para el cliente {customerName}.\n\nDetuvimos la creación de este cliente en Sage50 parar evitar duplicación de datos.");
//      }
//      public static bool ClientExists
//      (
//         string guidId,
//         string country,
//         string fullName,
//         string cif,
//         string postalCode,
//         string address,
//         string province
//      )
//      {
//         try
//         {
//            Getsage50Client(guidId);

//            if(Sage50Customer != null && IsThisSimilar(fullName, Sage50Customer.NOMBRE, 80))
//            {
//               PrintMessage("El valor de Nombre", fullName, Sage50Customer.NOMBRE, fullName);
//               return true;
//            }

//            if(Sage50Customer != null && IsThisSimilar(cif, Sage50Customer.CIF, 90))
//            {
//               PrintMessage("El valor de CIF-NIF", cif, Sage50Customer.CIF, fullName);
//               return true;
//            }

//            if(ExistsInDatabase(guidId))
//            {
//               MessageBox.Show($"El guid: {guidId} ya existe.");
//               return true;
//            }

//            if(
//               !ExistsInDatabase(guidId)
//               ||
//               Sage50Customer == null
//            )
//            {
//               if(Sage50Customer != null)
//               {
//                  MessageBox.Show($@"El cliente: 
               
//                  {Sage50Customer.PAIS}
//                  {Sage50Customer.NOMBRE}
//                  {Sage50Customer.CIF}
//                  {Sage50Customer.CODPOST}
//                  {Sage50Customer.DIRECCION}
//                  {Sage50Customer.PROVINCIA}

//                  no existe");
//               };
//               return false;
//            }
//            return false;
//         }
//         catch(Exception exception)
//         {
//            throw new Exception($"En:\n\nSage50ConnectionManager.Clients\n.CustomerManager:\n\n{exception.Message}");
//         };
//      }
//      //public static bool ClientExists
//      //(
//      //   string guidId,
//      //   string country,
//      //   string name,
//      //   string cif,
//      //   string postalCode,
//      //   string address,
//      //   string province
//      //)
//      //{
//      //   try
//      //   {
//      //      if(
//      //         guidId != null 
//      //         && guidId != "" 
//      //         //&& guidId != null
//      //         //&& country != null
//      //         //&& name != null
//      //         //&& cif != null
//      //         //&& postalCode != null
//      //         //&& address != null
//      //         //&& province != null
//      //      )
//      //      {
//      //         Getsage50Client(guidId);

//      //         if(Sage50Customer != null && IsThisSimilar(country, Sage50Customer.PAIS, 80))
//      //         {
//      //            PrintMessage("El valor de Pais", country, Sage50Customer.PAIS, name);
//      //            return true;
//      //         }

//      //         if(Sage50Customer != null && IsThisSimilar(name, Sage50Customer.NOMBRE, 80))
//      //         {
//      //            PrintMessage("El valor de Nombre", name, Sage50Customer.NOMBRE, name);
//      //            return true;
//      //         }

//      //         if(Sage50Customer != null && IsThisSimilar(cif, Sage50Customer.CIF, 90))
//      //         {
//      //            PrintMessage("El valor de CIF-NIF", cif, Sage50Customer.CIF, name);
//      //            return true;
//      //         }

//      //         if(Sage50Customer != null && IsThisSimilar(postalCode, Sage50Customer.CODPOST, 90))
//      //         {
//      //            PrintMessage("El valor de Código Postal", postalCode, Sage50Customer.CODPOST, name);
//      //            return true;
//      //         }

//      //         if(Sage50Customer != null && IsThisSimilar(address, Sage50Customer.DIRECCION, 80))
//      //         {
//      //            PrintMessage("El valor de Dirección", address, Sage50Customer.DIRECCION, name);
//      //            return true;
//      //         }

//      //         if(Sage50Customer != null && IsThisSimilar(province, Sage50Customer.PROVINCIA, 80))
//      //         {
//      //            PrintMessage("El valor de Provincia", province, Sage50Customer.PROVINCIA, name);
//      //            return true;
//      //         }

//      //         if(ExistsInDatabase(guidId))
//      //         {
//      //            MessageBox.Show($"El guid: {guidId} ya existe.");
//      //            return true;
//      //         }

//      //         if(
//      //            !ExistsInDatabase(guidId)
//      //            ||
//      //            Sage50Customer == null
//      //         )
//      //         {
//      //            if(Sage50Customer != null)
//      //            {
//      //               MessageBox.Show($@"El cliente: 

//      //            {Sage50Customer.PAIS}
//      //            {Sage50Customer.NOMBRE}
//      //            {Sage50Customer.CIF}
//      //            {Sage50Customer.CODPOST}
//      //            {Sage50Customer.DIRECCION}
//      //            {Sage50Customer.PROVINCIA}

//      //            no existe");
//      //            };
//      //            return false;
//      //         }
//      //         return false;
//      //      }
//      //      else
//      //      {
//      //         return false;
//      //      };
//      //   }
//      //   catch(Exception exception)
//      //   {
//      //      throw new Exception($"En:\n\nSage50ConnectionManager.Clients\n.CustomerManager:\n\n{exception.Message}");
//      //   };
//      //}
//      public static bool IsThisSimilar
//      (
//         string value1,
//         string value2,
//         int minimalToleranceRatio
//      )
//      {
//         //MessageBox.Show($"\"{value1}\" & \"{value2}\" have a: {Fuzz.Ratio(value1, value2)}% match ratio");
//         if(Fuzz.Ratio(value1, value2) > minimalToleranceRatio)
//         {
//            return true;
//         }
//         else
//            return false;
//      }
//      public static bool ExistsInDatabase
//      (
//         string guidId
//      )
//      {
//         try
//         {
//            string getSage50CustomerSQLQuery = $@"
//                SELECT 
//                    codigo, 
//                    cif, 
//                    nombre, 
//                    nombre2, 
//                    direccion, 
//                    codpost, 
//                    poblacion, 
//                    provincia, 
//                    pais
//                FROM 
//                  {DB.SQLDatabase("gestion","clientes")}
//                WHERE guid_id='{guidId}'
//               ;";

//            DataTable sage50CustomersDataTable = new DataTable();

//            DB.SQLExec(getSage50CustomerSQLQuery, ref sage50CustomersDataTable);

//            if(sage50CustomersDataTable.Rows.Count > 0)
//            {
//               Sage50Customer.CODIGO = sage50CustomersDataTable.Rows[0].ItemArray[0].ToString().Trim();
//               Sage50Customer.CIF = sage50CustomersDataTable.Rows[0].ItemArray[1].ToString().Trim();
//               Sage50Customer.NOMBRE = sage50CustomersDataTable.Rows[0].ItemArray[2].ToString().Trim();
//               Sage50Customer.NOMBRE2 = sage50CustomersDataTable.Rows[0].ItemArray[3].ToString().Trim();
//               Sage50Customer.DIRECCION = sage50CustomersDataTable.Rows[0].ItemArray[4].ToString().Trim();
//               Sage50Customer.CODPOST = sage50CustomersDataTable.Rows[0].ItemArray[5].ToString().Trim();
//               Sage50Customer.POBLACION = sage50CustomersDataTable.Rows[0].ItemArray[6].ToString().Trim();
//               Sage50Customer.PROVINCIA = sage50CustomersDataTable.Rows[0].ItemArray[7].ToString().Trim();
//               Sage50Customer.PAIS = sage50CustomersDataTable.Rows[0].ItemArray[8].ToString().Trim();
//               Sage50Customer.GUID_ID = guidId;
//               return true;
//            }
//            else
//               return false;

//         }
//         catch(Exception exception)
//         {
//            throw new Exception($"En:\n\nSage50ConnectionManager\n.CustomerManager\n.ExistsInDatabase:\n\n{exception.Message}");
//         };
//      }

//      public static void Getsage50Client
//      (
//         string guidId
//      )
//      {
//         try
//         {
//            if(guidId != null && guidId != "")
//            {
//               string getSage50CustomerSQLQuery = $@"
//                SELECT 
//                    codigo, 
//                    cif, 
//                    nombre, 
//                    nombre2, 
//                    direccion, 
//                    codpost, 
//                    poblacion, 
//                    provincia, 
//                    pais
//                FROM 
//                  {DB.SQLDatabase("gestion","clientes")}
//                WHERE guid_id='{guidId}'
//               ;";

//               DataTable sage50CustomersDataTable = new DataTable();

//               DB.SQLExec(getSage50CustomerSQLQuery, ref sage50CustomersDataTable);

//               if(sage50CustomersDataTable.Rows.Count > 0)
//               {
//                  Sage50Customer.CODIGO = sage50CustomersDataTable.Rows[0].ItemArray[0].ToString().Trim();
//                  Sage50Customer.CIF = sage50CustomersDataTable.Rows[0].ItemArray[1].ToString().Trim();
//                  Sage50Customer.NOMBRE = sage50CustomersDataTable.Rows[0].ItemArray[2].ToString().Trim();
//                  Sage50Customer.NOMBRE2 = sage50CustomersDataTable.Rows[0].ItemArray[3].ToString().Trim();
//                  Sage50Customer.DIRECCION = sage50CustomersDataTable.Rows[0].ItemArray[4].ToString().Trim();
//                  Sage50Customer.CODPOST = sage50CustomersDataTable.Rows[0].ItemArray[5].ToString().Trim();
//                  Sage50Customer.POBLACION = sage50CustomersDataTable.Rows[0].ItemArray[6].ToString().Trim();
//                  Sage50Customer.PROVINCIA = sage50CustomersDataTable.Rows[0].ItemArray[7].ToString().Trim();
//                  Sage50Customer.PAIS = sage50CustomersDataTable.Rows[0].ItemArray[8].ToString().Trim();
//                  Sage50Customer.GUID_ID = guidId;
//               };
//            };
//         }
//         catch(Exception exception)
//         {
//            throw new Exception($"En:\n\nSage50ConnectionManager\n.CustomerManager\n.ExistsInDatabase:\n\n{exception.Message}");
//         };
//      }
//   }
//}
