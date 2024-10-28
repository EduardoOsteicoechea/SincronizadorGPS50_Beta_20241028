using SincronizadorGPS50.GestprojectConnector;
using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class RunSynchronizeCustomersWorkflow
   {
      public RunSynchronizeCustomersWorkflow
      (
         System.Data.SqlClient.SqlConnection connection,
         List<int> selectedIdList,
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         ////////////////////////////////////
         /// get gestproject and sage50 clients
         ////////////////////////////////////

         List<GestprojectDataManager.GestprojectCustomer> gestProjectClientList =
         new SincronizadorGPS50
         .GestprojectDataManager
         .GetClientsFromSynchronizationTable(
            connection,
            selectedIdList,
            tableSchema
         ).GestprojectClientList;

         List<Sage50Customer> sage50CustomerList = new GetSage50Customer().CustomerList;

         ////////////////////////////////////
         /// get clients distributon in sage50 (any exist, some exist, all exist) to determine application flow
         ////////////////////////////////////

         List<GestprojectDataManager.GestprojectCustomer> existingClientsList = new List<GestprojectDataManager.GestprojectCustomer>();
         List<GestprojectDataManager.GestprojectCustomer> nonExistingClientsList = new List<GestprojectDataManager.GestprojectCustomer>();
         List<GestprojectDataManager.GestprojectCustomer> unsynchronizedClientList = new List<GestprojectDataManager.GestprojectCustomer>();

         for(int i = 0; i < gestProjectClientList.Count; i++)
         {
            GestprojectDataManager.GestprojectCustomer gestprojectClient = gestProjectClientList[i];
            bool found = false;

            for(global::System.Int32 j = 0; j < sage50CustomerList.Count; j++)
            {
               Sage50Customer sage50Customer = sage50CustomerList[j];
               if(
                  gestprojectClient.sage50_guid_id == sage50Customer.GUID_ID
               )
               {
                  existingClientsList.Add(gestprojectClient);
                  found = true;
                  break;
               };
            };

            if(!found)
            {
               nonExistingClientsList.Add(gestprojectClient);
            };

            if(gestprojectClient.synchronization_status != "Sincronizado" && gestprojectClient.sage50_guid_id != "")
            {
               unsynchronizedClientList.Add(gestprojectClient);
            };
         };

         bool someGestprojectClientsExistsInSage50 = existingClientsList.Count > 0;
         bool allGestprojectClientsExistsInSage50 = existingClientsList.Count == gestProjectClientList.Count;
         bool noGestprojectClientsExistsInSage50 = existingClientsList.Count == 0;
         bool unsynchronizedClientsExists = unsynchronizedClientList.Count > 0;

         ////////////////////////////////////
         /// execute client synchornization flow according to client distribution
         ////////////////////////////////////

         if(noGestprojectClientsExistsInSage50)
         {
            new UnexsistingClientListWorkflow(GestprojectDataHolder.GestprojectDatabaseConnection, nonExistingClientsList, tableSchema);
         }

         if(allGestprojectClientsExistsInSage50)
         {
            new ExsistingClientListWorkflow(GestprojectDataHolder.GestprojectDatabaseConnection, gestProjectClientList, unsynchronizedClientList, unsynchronizedClientsExists, tableSchema);
         }

         if(someGestprojectClientsExistsInSage50 && !allGestprojectClientsExistsInSage50)
         {
            if(unsynchronizedClientsExists)
            {
               new ExsistingClientListWorkflow(GestprojectDataHolder.GestprojectDatabaseConnection, existingClientsList, unsynchronizedClientList, unsynchronizedClientsExists, tableSchema);
            };

            new UnexsistingClientListWorkflow(GestprojectDataHolder.GestprojectDatabaseConnection, nonExistingClientsList, tableSchema);
         };
      }
   }
}
