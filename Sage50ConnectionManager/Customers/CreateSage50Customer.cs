using sage.ew.db;
using System;
using System.Data;
using System.Windows.Forms;

namespace SincronizadorGPS50.Sage50Connector
{
   public class CreateSage50Customer
   {
      public string ClientCode { get; set; } = "";
      public string GUID_ID { get; set; } = "";
      public CreateSage50Customer
      (
         string country,
         string name,
         string cif,
         string postalCode,
         string address,
         string province,
         string ivaType = "03"
      )
      {
         try
         {
            int nextAvailableClientCode = new GetSage50Customer().NextCustomerCodeAvailable;

            Customer customer = new Customer();
            clsEntityCustomer clsEntityCustomerInstance = new clsEntityCustomer();

            if(nextAvailableClientCode < 10000)
            {
               if(nextAvailableClientCode < 10)
               {
                  clsEntityCustomerInstance.codigo = "4300000" + nextAvailableClientCode;
                  ClientCode = clsEntityCustomerInstance.codigo;
               }
               else if(nextAvailableClientCode < 100)
               {
                  clsEntityCustomerInstance.codigo = "430000" + nextAvailableClientCode;
                  ClientCode = clsEntityCustomerInstance.codigo;
               }
               else if(nextAvailableClientCode < 1000)
               {
                  clsEntityCustomerInstance.codigo = "43000" + nextAvailableClientCode;
                  ClientCode = clsEntityCustomerInstance.codigo;
               }
               else
               {
                  clsEntityCustomerInstance.codigo = "4300" + nextAvailableClientCode;
                  ClientCode = clsEntityCustomerInstance.codigo;
               };

               clsEntityCustomerInstance.pais = country.Trim();
               clsEntityCustomerInstance.nombre = name.Trim();
               clsEntityCustomerInstance.codpos = postalCode.Trim();
               clsEntityCustomerInstance.cif = cif.Trim();
               clsEntityCustomerInstance.direccion = address.Trim();
               clsEntityCustomerInstance.provincia = province.Trim();
               clsEntityCustomerInstance.tipo_iva = ivaType.Trim();

               if(customer._Create(clsEntityCustomerInstance))
               {
                  string getSage50ClientSQLQuery = $@"
                  SELECT 
                     guid_id 
                  FROM 
                     {DB.SQLDatabase("gestion","clientes")}
                  WHERE 
                     codigo='{ClientCode}'
                  ;";

                  DataTable sage50ClientsDataTable = new DataTable();

                  DB.SQLExec(getSage50ClientSQLQuery, ref sage50ClientsDataTable);

                  GUID_ID = sage50ClientsDataTable.Rows[0].ItemArray[0].ToString().Trim();
               }
               else
               {
                  MessageBox.Show(
                      "Error en la creación del cliente empleando estos datos: " + "\n\n" +
                      "ClientCode: " + clsEntityCustomerInstance.codigo + "\n" +
                      "country: " + clsEntityCustomerInstance.pais + "\n" +
                      "name: " + clsEntityCustomerInstance.nombre + "\n" +
                      "postalCode: " + clsEntityCustomerInstance.codpos + "\n" +
                      "cif: " + clsEntityCustomerInstance.cif + "\n" +
                      "address: " + clsEntityCustomerInstance.direccion + "\n" +
                      "province: " + clsEntityCustomerInstance.provincia + "\n" +
                      "ivaType: " + clsEntityCustomerInstance.tipo_iva + "\n"
                  );
               };
            }
            else
            {
               MessageBox.Show("Sage50 admite un máximo de 9999 clientes por grupo de empresas y su base de clientes de Gestproject supera éste límite.");
            };
         }
         catch(System.Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}