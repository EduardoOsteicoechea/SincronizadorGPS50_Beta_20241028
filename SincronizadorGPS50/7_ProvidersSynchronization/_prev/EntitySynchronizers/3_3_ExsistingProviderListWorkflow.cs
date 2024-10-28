using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class ExsistingProviderListWorkflow
   {
      public ExsistingProviderListWorkflow
      (
         System.Data.SqlClient.SqlConnection connection, 
         List<GestprojectDataManager.GestprojectCustomer> clientsList, 
         List<GestprojectDataManager.GestprojectCustomer> unsynchronizedClientList, 
         bool unsynchronizedClientsExists, 
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
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
