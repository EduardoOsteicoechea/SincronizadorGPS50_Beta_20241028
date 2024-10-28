//using SincronizadorGPS50.Sage50Connector;
//using System.Collections.Generic;
//using System.Data;

//namespace SincronizadorGPS50
//{
//   internal static class ClientSynchronizationTable
//   {
//      public static DataTable Create()
//      {
//         ///////////////////////////////////
//         /// get both gestproject and sage50 clients
//         ///////////////////////////////////
         
//         List<GestprojectDataManager.GestprojectClient> gestprojectClientList =
//         new GestprojectDataManager
//         .GestprojectClientsManager()
//         .GetClients(
//            GestprojectDataHolder.GestprojectDatabaseConnection
//         );

//         List<Sage50Customer> sage50CustomerList = new GetSage50Customer().CustomerList;

//         ///////////////////////////////////
//         /// create data table
//         ///////////////////////////////////

//         DataTable table = new CreateTableControl().Table;

//         ///////////////////////////////////
//         /// manage synchronization table state
//         ///////////////////////////////////

//         bool Sage50SincronizationTableExists = 
//         new GestprojectDataManager
//         .CheckIfTableExistsOnGestproject(
//            GestprojectDataHolder.GestprojectDatabaseConnection, 
//            "INT_SAGE_SINC_CLIENTE"
//         ).Exists;

//         if(!Sage50SincronizationTableExists)
//         {
//            new GestprojectDataManager.CreateClientSynchronizationTable(GestprojectDataHolder.GestprojectDatabaseConnection);
//         };

//         bool Sage50SynchronizationTableWasJustCreated = !Sage50SincronizationTableExists;

//         ///////////////////////////////////
//         /// process each gestproject database client
//         ///////////////////////////////////

//         for(int i = 0; i < gestprojectClientList.Count; i++)
//         {
//            GestprojectDataManager.GestprojectClient gestprojectClient = gestprojectClientList[i];

//            if(Sage50SynchronizationTableWasJustCreated)
//            {
//               new GestprojectDataManager.RegisterClient(
//                  GestprojectDataHolder.GestprojectDatabaseConnection,
//                  gestprojectClient,
//                  SynchronizationStatusOptions.Nunca_ha_sido_sincronizado,
//                  GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID
//               );
//            }
//            else
//            {
//               GestprojectDataManager.WasGestprojectClientRegistered gestprojectClientRegistrationValidator =
//               ;

//               bool gestprojectClientWasRegistered = gestprojectClientRegistrationValidator.ItIs;

//               if(
//                  !new GestprojectDataManager.WasGestprojectClientRegistered(
//                     GestprojectDataHolder.GestprojectDatabaseConnection,
//                     gestprojectClient
//                  ).ItIs
//               )
//               {
//                  new GestprojectDataManager.RegisterClient(
//                     GestprojectDataHolder.GestprojectDatabaseConnection,
//                     gestprojectClient,
//                     SynchronizationStatusOptions.Nunca_ha_sido_sincronizado,
//                     GestprojectDataHolder.LocalDeviceUserSessionData.USU_ID
//                  );
//               }
//               else
//               {
//                  GestprojectDataManager.GestprojectClient synchronizationTableProjectClient =
//                  new SincronizadorGPS50
//                  .GestprojectDataManager
//                  .GetSingleClientFromSynchronizationTable(
//                     GestprojectDataHolder.GestprojectDatabaseConnection,
//                     gestprojectClient.PAR_ID
//                  ).GestprojectClient;

//                  GestprojectDataManager.GestprojectClient validatedGestprojectClient = 
//                  new ValidateClientSyncronizationStatus(
//                     synchronizationTableProjectClient.sage50_guid_id,
//                     gestprojectClient.PAR_NOMBRE,
//                     gestprojectClient.PAR_CIF_NIF,
//                     gestprojectClient.PAR_CP_1,
//                     gestprojectClient.PAR_DIRECCION_1,
//                     gestprojectClient.PAR_PROVINCIA_1,
//                     gestprojectClient.PAR_PAIS_1,
//                     sage50CustomerList
//                  ).GestprojectClient;

//                  gestprojectClient.synchronization_status = validatedGestprojectClient.synchronization_status;
//                  gestprojectClient.comments = validatedGestprojectClient.comments;
//               };
//            };

//            new GestprojectDataManager.PopulateUnsynchronizedClientRegistrationData(
//               GestprojectDataHolder.GestprojectDatabaseConnection,
//               gestprojectClient
//            );

//            new AddClientToUITable(gestprojectClient, table);
//         };

//         return table;
//      }
//   }
//}










