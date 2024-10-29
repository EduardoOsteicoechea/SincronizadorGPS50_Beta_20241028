//using System.Collections.Generic;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   internal class UnexsistingClientListWorkflow
//   {
//      public UnexsistingClientListWorkflow(System.Data.SqlClient.SqlConnection connection, List<GestprojectDataManager.GestprojectCustomer> clientsList)
//      {
//         DialogResult result = MessageBox.Show($"¿Desea sincronizar {clientsList.Count} cliente(s) con Sage50?", "Confirmación para actualización", MessageBoxButtons.OKCancel);

//         if(result == DialogResult.OK)
//         {
//            for(global::System.Int32 i = 0; i < clientsList.Count; i++)
//            {
//               GestprojectDataManager.GestprojectCustomer currentClient = clientsList[i];

//               Sage50ConnectionManager.CustomerManager customerManager = new Sage50ConnectionManager.CustomerManager(
//                  currentClient.fullName,
//                  currentClient.PAR_CIF_NIF
//               );

//               if(customerManager.ClientExists)
//               {
//                  //AksIfDuplicationOrUpdateIsDesired aksIfDuplicationOrUpdateIsDesired = new AksIfDuplicationOrUpdateIsDesired("cliente",currentClient.PAR_NOMBRE, customerManager.MatchMessage);

//                  //if(!aksIfDuplicationOrUpdateIsDesired.WasOperationCanceled)
//                  //{
//                     currentClient.sage50_guid_id = customerManager.CustomerGuid;
//                     currentClient.sage50_client_code = customerManager.CustomerCode;
//                     currentClient.PAR_SUBCTA_CONTABLE = customerManager.CustomerCode;

//                     //if(aksIfDuplicationOrUpdateIsDesired.IsDuplicationDesired)
//                     //{
//                     //   new CreateClientWorkflow(connection, currentClient);
//                     //}
//                     //else
//                     //{
//                        new UpdateClientWorkflow(connection, currentClient);
//                  //   };
//                  //};
//               }
//               else
//               {
//                  new CreateClientWorkflow(connection, clientsList[i]);
//               };
//            }
//         };
//      }
//   }
//}
