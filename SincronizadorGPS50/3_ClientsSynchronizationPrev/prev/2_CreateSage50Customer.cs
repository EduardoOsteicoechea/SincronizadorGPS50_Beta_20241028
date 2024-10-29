//using sage.ew.cliente;
//using sage.ew.db;
//using System;
//using System.Data;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.Sage50API
//{
//   internal class CreateSage50Customer
//   {
//      internal string ClientCode { get; set; } = "";
//      internal string GUID_ID { get; set; } = "";
//      internal bool WasSuccessful { get; set; } = false;
//      internal CreateSage50Customer(GestprojectClient gestprojectClient)
//      {
//         int nextAvailableClientCode = new GetSage50Clients().NextClientCodeAvailable;

//         Customer customer = new Customer();
//         clsEntityCustomer clsEntityCustomerInstance = new clsEntityCustomer();

//         if(nextAvailableClientCode < 10000)
//         {
//            if(nextAvailableClientCode < 10)
//            {
//               clsEntityCustomerInstance.codigo = "4300000" + nextAvailableClientCode;
//               ClientCode = clsEntityCustomerInstance.codigo;
//            }
//            else if(nextAvailableClientCode < 100)
//            {
//               clsEntityCustomerInstance.codigo = "430000" + nextAvailableClientCode;
//               ClientCode = clsEntityCustomerInstance.codigo;
//            }
//            else if(nextAvailableClientCode < 1000)
//            {
//               clsEntityCustomerInstance.codigo = "43000" + nextAvailableClientCode;
//               ClientCode = clsEntityCustomerInstance.codigo;
//            }
//            else
//            {
//               clsEntityCustomerInstance.codigo = "4300" + nextAvailableClientCode;
//               ClientCode = clsEntityCustomerInstance.codigo;
//            };

//            clsEntityCustomerInstance.pais = gestprojectClient.PAR_PAIS_1;
//            clsEntityCustomerInstance.nombre = gestprojectClient.PAR_NOMBRE;
//            clsEntityCustomerInstance.cif = gestprojectClient.PAR_CIF_NIF;
//            clsEntityCustomerInstance.direccion = gestprojectClient.PAR_DIRECCION_1;
//            clsEntityCustomerInstance.provincia = gestprojectClient.PAR_PROVINCIA_1;
//            clsEntityCustomerInstance.tipo_iva = "03";

//            clsEntityCustomerInstance.codpos = gestprojectClient.PAR_CP_1;

//            if(customer._Create(clsEntityCustomerInstance))
//            {
//               string getSage50ClientSQLQuery = @"
//                SELECT guid_id FROM " + DB.SQLDatabase("gestion","clientes") + " WHERE codigo = '" + ClientCode + "';";

//               DataTable sage50ClientsDataTable = new DataTable();

//               DB.SQLExec(getSage50ClientSQLQuery, ref sage50ClientsDataTable);

//               GUID_ID = sage50ClientsDataTable.Rows[0].ItemArray[0].ToString().Trim();

//               WasSuccessful = true;
//            }
//            else
//            {
//               MessageBox.Show(
//                   "Error en la creación del cliente empleando estos datos: " + "\n\n" +
//                   "ClientCode: " + ClientCode + "\n" +
//                   "gestprojectClient.PAR_PAIS_1: " + gestprojectClient.PAR_PAIS_1.Replace("-", " ").Replace("ñ", "n") + "\n" +
//                   "gestprojectClient.PAR_LOCALIDAD_1: " + gestprojectClient.PAR_LOCALIDAD_1 + "\n" +
//                   "gestprojectClient.PAR_CP_1: " + gestprojectClient.PAR_CP_1 + "\n" +
//                   "gestprojectClient.PAR_CIF_NIF: " + gestprojectClient.PAR_CIF_NIF + "\n" +
//                   "gestprojectClient.PAR_DIRECCION_1: " + gestprojectClient.PAR_DIRECCION_1.Replace(",", " ") + "\n" +
//                   "gestprojectClient.PAR_PROVINCIA_1: " + gestprojectClient.PAR_PROVINCIA_1.Replace(",", " ") + "\n"
//               );
//            };
//         }
//         else
//         {
//            MessageBox.Show("Sage50 admite un máximo de 9999 clientes por grupo de empresas y su base de clientes de Gestproject supera éste límite.");
//         };
//      }
//   }
//}