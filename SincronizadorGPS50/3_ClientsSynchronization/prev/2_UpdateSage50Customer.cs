//using System;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.Sage50API
//{
//    internal class UpdateSage50Customer
//    {
//        internal bool WasSuccessful { get; set; } = false;
//        internal UpdateSage50Customer(GestprojectClient gestprojectClient)
//        {
//            Customer customer = new Customer();
//            clsEntityCustomer clsEntityCustomerInstance = new clsEntityCustomer();

//            if(Convert.ToInt32(gestprojectClient.PAR_SUBCTA_CONTABLE) <= 43009999)
//            {
//                clsEntityCustomerInstance.codigo = gestprojectClient.PAR_SUBCTA_CONTABLE;
//                clsEntityCustomerInstance.pais = gestprojectClient.PAR_PAIS_1;
//                clsEntityCustomerInstance.nombre = gestprojectClient.PAR_NOMBRE;
//                clsEntityCustomerInstance.cif = gestprojectClient.PAR_CIF_NIF;
//                clsEntityCustomerInstance.direccion = gestprojectClient.PAR_DIRECCION_1;
//                clsEntityCustomerInstance.provincia = gestprojectClient.PAR_PROVINCIA_1;
//                clsEntityCustomerInstance.tipo_iva = "03";
//                clsEntityCustomerInstance.codpos = gestprojectClient.PAR_CP_1;

//                if(customer._Update(clsEntityCustomerInstance))
//                {

//                    WasSuccessful = true;
//                }
//                else
//                {
//                    MessageBox.Show(
//                        "Error en la creación del cliente empleando estos datos: " + "\n\n" +
//                        "ClientCode: " + gestprojectClient.PAR_SUBCTA_CONTABLE + "\n" +
//                        "gestprojectClient.PAR_PAIS_1: " + gestprojectClient.PAR_PAIS_1.Replace("-", " ").Replace("ñ", "n") + "\n" +
//                        "gestprojectClient.PAR_LOCALIDAD_1: " + gestprojectClient.PAR_LOCALIDAD_1 + "\n" +
//                        "gestprojectClient.PAR_CP_1: " + gestprojectClient.PAR_CP_1 + "\n" +
//                        "gestprojectClient.PAR_CIF_NIF: " + gestprojectClient.PAR_CIF_NIF + "\n" +
//                        "gestprojectClient.PAR_DIRECCION_1: " + gestprojectClient.PAR_DIRECCION_1.Replace(",", " ") + "\n" +
//                        "gestprojectClient.PAR_PROVINCIA_1: " + gestprojectClient.PAR_PROVINCIA_1.Replace(",", " ") + "\n"
//                    );
//                };
//            }
//            else
//            {
//                MessageBox.Show("Sage50 admite un máximo de 9999 clientes por grupo de empresas y su base de clientes de Gestproject supera éste límite.");
//            };
//        }
//    }
//}

