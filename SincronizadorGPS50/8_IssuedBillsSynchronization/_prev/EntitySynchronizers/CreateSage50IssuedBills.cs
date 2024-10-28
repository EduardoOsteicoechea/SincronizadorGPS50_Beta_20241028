using sage.ew.db;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50.Sage50Connector
{
   public class CreateSage50IssuedBills
   {
      public string ClientCode { get; set; } = "";
      public string GUID_ID { get; set; } = "";
      public CreateSage50IssuedBills
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
            int nextCodeAvailable = new SincronizadorGPS50.Sage50Connector.GetSage50Providers().NextProviderCodeAvailable;

            Customer entity = new Customer();
            clsEntityProvider entityInstance = new clsEntityProvider();

            if(nextCodeAvailable < 10000)
            {
               if(nextCodeAvailable < 10)
               {
                  entityInstance.codigo = "4300000" + nextCodeAvailable;
                  ClientCode = entityInstance.codigo;
               }
               else if(nextCodeAvailable < 100)
               {
                  entityInstance.codigo = "430000" + nextCodeAvailable;
                  ClientCode = entityInstance.codigo;
               }
               else if(nextCodeAvailable < 1000)
               {
                  entityInstance.codigo = "43000" + nextCodeAvailable;
                  ClientCode = entityInstance.codigo;
               }
               else
               {
                  entityInstance.codigo = "4300" + nextCodeAvailable;
                  ClientCode = entityInstance.codigo;
               };

               entityInstance.pais = country.Trim();
               entityInstance.nombre = name.Trim();
               entityInstance.codpos = postalCode.Trim();
               entityInstance.cif = cif.Trim();
               entityInstance.direccion = address.Trim();
               entityInstance.provincia = province.Trim();
               entityInstance.tipo_iva = ivaType.Trim();

               if(entity._Create(entityInstance))
               {
                  string getSage50ClientSQLQuery = $@"
                  SELECT 
                     guid_id 
                  FROM 
                     {DB.SQLDatabase("gestion","proveed")}
                  WHERE 
                     codigo='{ClientCode}'
                  ;";

                  DataTable sage50EntityDataTable = new DataTable();

                  DB.SQLExec(getSage50ClientSQLQuery, ref sage50EntityDataTable);

                  GUID_ID = sage50EntityDataTable.Rows[0].ItemArray[0].ToString().Trim();
               }
               //else
               //{
               //   MessageBox.Show(
               //       "Error en la creación del cliente empleando estos datos: " + "\n\n" +
               //       "ClientCode: " + clsEntityCustomerInstance.codigo + "\n" +
               //       "country: " + clsEntityCustomerInstance.pais + "\n" +
               //       "name: " + clsEntityCustomerInstance.nombre + "\n" +
               //       "postalCode: " + clsEntityCustomerInstance.codpos + "\n" +
               //       "cif: " + clsEntityCustomerInstance.cif + "\n" +
               //       "address: " + clsEntityCustomerInstance.direccion + "\n" +
               //       "province: " + clsEntityCustomerInstance.provincia + "\n" +
               //       "ivaType: " + clsEntityCustomerInstance.tipo_iva + "\n"
               //   );
               //};
            }
            //else
            //{
            //   MessageBox.Show("Sage50 admite un máximo de 9999 clientes por grupo de empresas y su base de clientes de Gestproject supera éste límite.");
            //};
         }
         catch(System.Exception exception)
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