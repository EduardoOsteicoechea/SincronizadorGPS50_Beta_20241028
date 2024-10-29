using SincronizadorGPS50.GestprojectDataManager;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class UnexsistingClientListWorkflow
   {
      public UnexsistingClientListWorkflow(System.Data.SqlClient.SqlConnection connection, List<GestprojectDataManager.GestprojectCustomer> clientsList, CustomerSyncronizationTableSchema tableSchema)
      {
         List<GestprojectDataManager.GestprojectCustomer> existingClientsList = new List<GestprojectDataManager.GestprojectCustomer> ();
         List<GestprojectDataManager.GestprojectCustomer> unexistingClientsList = new List<GestprojectDataManager.GestprojectCustomer> ();

         for(global::System.Int32 i = 0; i < clientsList.Count; i++)
         {
            GestprojectDataManager.GestprojectCustomer currentClient = clientsList[i];

            Sage50Connector.CustomerComparer customerComparer = new Sage50Connector.CustomerComparer(
               currentClient.fullName,
               currentClient.PAR_CIF_NIF
            );

            if(customerComparer.ClientExists)
            {
               currentClient.sage50_client_code = customerComparer.CustomerCode;
               currentClient.PAR_SUBCTA_CONTABLE = customerComparer.CustomerCode;
               currentClient.sage50_guid_id = customerComparer.CustomerGuid;
               existingClientsList.Add(currentClient);
            }
            else
            {
               unexistingClientsList.Add(currentClient);
            };
         };

         //string aa = "Existing\n\n";
         //foreach(GestprojectCustomer item in existingClientsList)
         //{
         //   aa += item.PAR_NOMBRE + "-" + item.PAR_CIF_NIF + "\n";
         //}
         //aa += "\n\nUnexisting\n\n";
         //foreach(GestprojectCustomer item in unexistingClientsList)
         //{
         //   aa += item.PAR_NOMBRE + "-" + item.PAR_CIF_NIF + "\n";
         //}
         //MessageBox.Show(aa);

         string dialogMessage = "";
         if(existingClientsList.Count > 0 && unexistingClientsList.Count > 0)
         {
            dialogMessage = $"Partiendo de la selección encontramos {existingClientsList.Count} cliente(s) desactualizados y {unexistingClientsList.Count} inexistentes en Sage50.\n\n¿Desea vincular los clientes existentes y crear los faltantes en Sage50?";
         }
         else if(existingClientsList.Count > 0 && unexistingClientsList.Count == 0)
         {
            dialogMessage = $"Partiendo de la selección encontramos {existingClientsList.Count} cliente(s) que ya existen en Sage50.\n\n¿Desea vincularlo(s)?";
         }
         else if(existingClientsList.Count == 0 && unexistingClientsList.Count > 0)
         {
            dialogMessage = $"Partiendo de la selección encontramos {unexistingClientsList.Count} cliente(s) inexistentes en Sage50.\n\n¿Desea crearlos y sincronizar sus datos?";
         };

         DialogResult result = MessageBox.Show(dialogMessage, "Confirmación de actualización y creación", MessageBoxButtons.OKCancel);

         if(result == DialogResult.OK)
         {
            for(global::System.Int32 i = 0; i < existingClientsList.Count; i++)
            {
               GestprojectDataManager.GestprojectCustomer currentClient = existingClientsList[i];

               new LinkClientWorkflow(connection, currentClient, tableSchema);
            };
            for(global::System.Int32 i = 0; i < unexistingClientsList.Count; i++)
            {
               GestprojectDataManager.GestprojectCustomer currentClient = unexistingClientsList[i];
               new CreateClientWorkflow(connection, currentClient, tableSchema);
            };
         };
      }
   }
}
