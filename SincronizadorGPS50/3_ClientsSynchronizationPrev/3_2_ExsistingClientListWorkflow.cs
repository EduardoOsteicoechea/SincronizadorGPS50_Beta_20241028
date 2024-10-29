using SincronizadorGPS50.GestprojectDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SincronizadorGPS50
{
   internal class ExsistingClientListWorkflow
   {
      public ExsistingClientListWorkflow(System.Data.SqlClient.SqlConnection connection, List<GestprojectDataManager.GestprojectCustomer> clientsList, List<GestprojectDataManager.GestprojectCustomer> unsynchronizedClientList, bool unsynchronizedClientsExists, CustomerSyncronizationTableSchema tableSchema)
      {
         if(unsynchronizedClientsExists)
         {
            DialogResult result = MessageBox.Show($"Partiendo de la selección encontramos {unsynchronizedClientList.Count} cliente(s) desactualizados.\n\n¿Desea sincronizarlo(s)?", "Confirmación de actualización", MessageBoxButtons.OKCancel);

            if(result == DialogResult.OK)
            {
               for(global::System.Int32 i = 0; i < clientsList.Count; i++)
               {
                  if(unsynchronizedClientList.Contains(clientsList[i]))
                  {
                     new UpdateClientWorkflow(GestprojectDataHolder.GestprojectDatabaseConnection, clientsList[i], tableSchema);
                  };
               };
            };
         }
         else
         {
            MessageBox.Show($"Los {clientsList.Count} cliente(s) están sincronizados.");
         };
      }
   }
}
