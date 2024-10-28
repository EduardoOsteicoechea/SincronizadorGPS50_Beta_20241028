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
   internal class ExsistingProjectListWorkflow
   {
      public ExsistingProjectListWorkflow
      (
         System.Data.SqlClient.SqlConnection connection, 
         List<GestprojectDataManager.GestprojectCustomer> entityList, 
         List<GestprojectDataManager.GestprojectCustomer> unsynchronizedEntityList, 
         bool unsynchronizedEntitysExists, 
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
         {
            if(unsynchronizedEntitysExists)
            {
               DialogResult result = MessageBox.Show($"Partiendo de la selección encontramos {unsynchronizedEntityList.Count} Entitye(s) desactualizados.\n\n¿Desea sincronizarlo(s)?", "Confirmación de actualización", MessageBoxButtons.OKCancel);

               if(result == DialogResult.OK)
               {
                  for(global::System.Int32 i = 0; i < entityList.Count; i++)
                  {
                     if(unsynchronizedEntityList.Contains(entityList[i]))
                     {
                        //new UpdateEntityWorkflow(GestprojectDataHolder.GestprojectDatabaseConnection, EntitysList[i], tableSchema);
                     };
                  };
               };
            }
            else
            {
               MessageBox.Show($"Los {entityList.Count} Entitye(s) están sincronizados.");
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
