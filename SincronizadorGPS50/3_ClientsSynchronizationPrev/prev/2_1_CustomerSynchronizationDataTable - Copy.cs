//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   internal static class CustomerSynchronizationDataTable
//   {
//      public static DataTable Create
//      (
//         System.Data.SqlClient.SqlConnection connection,
//         CompanyGroup sage50CompanyGroupData,
//         CustomerSyncronizationTableSchema tableSchema
//      )
//      {
//         ///////////////////////////////////
//         /// manage synchronization table state
//         ///////////////////////////////////

//         bool CustomerSincronizationTableExists = new GestprojectDataManager.CheckIfTableExistsOnGestproject(
//            connection,
//            "INT_SAGE_SINC_CLIENTE"
//         ).Exists;

//         if(!CustomerSincronizationTableExists)
//            new GestprojectDataManager.CreateClientSynchronizationTable(connection, tableSchema);

//         ///////////////////////////////////
//         /// get both gestproject and sage50 clients
//         ///////////////////////////////////

//         List<GestprojectDataManager.GestprojectCustomer> gestprojectCustomerList = new GestprojectClientsManager().GetClients(connection);

//         List<Sage50Customer> sage50CustomerList = new GetSage50Customer().CustomerList;

//         ///////////////////////////////////
//         /// create ui data source
//         ///////////////////////////////////

//         DataTable table = new CreateTableControl(tableSchema).Table;

//         ///////////////////////////////////
//         /// process each Gestproject customer
//         ///////////////////////////////////

//         for(int i = 0; i < gestprojectCustomerList.Count; i++)
//         {
//            GestprojectDataManager.GestprojectCustomer gestprojectCustomer = gestprojectCustomerList[i];

//            ///////////////////////////////////
//            /// add current Gestproject customer synchronization table data if any
//            ///////////////////////////////////

//            new GestprojectDataManager.AddSynchronizationTableCustomerData(
//               connection,
//               gestprojectCustomer,
//               tableSchema
//            );

//            ///////////////////////////////////
//            /// Dispatch Customer to corresponding proccess
//            ///////////////////////////////////

//            bool mustRegister = !CustomerSincronizationTableExists || !new GestprojectDataManager.WasGestprojectClientRegistered(
//                  connection,
//                  gestprojectCustomer,
//                  sage50CompanyGroupData.CompanyGuidId,
//                  tableSchema
//               ).ItIs;

//            bool registeredInDifferentCompanyGroup = gestprojectCustomer.sage50_company_group_guid_id != "" && sage50CompanyGroupData.CompanyGuidId != gestprojectCustomer.sage50_company_group_guid_id;

//            bool neverSynchronized = gestprojectCustomer.sage50_company_group_guid_id == "";

//            bool synchronizedInThePast = gestprojectCustomer.sage50_company_group_guid_id != "" && sage50CompanyGroupData.CompanyGuidId == gestprojectCustomer.sage50_company_group_guid_id;

//            if(mustRegister)
//            {
//               new GestprojectDataManager.RegisterClient(
//                  connection,
//                  gestprojectCustomer, 
//                  tableSchema
//               );
//            }
//            else
//            {
//               if(registeredInDifferentCompanyGroup)
//               {
//                  continue;
//               }
//               else if(neverSynchronized || synchronizedInThePast)
//               {
//                  new GestprojectDataManager.UpdateClientState(
//                     connection,
//                     gestprojectCustomer,
//                     tableSchema
//                  );
//               };
//            };

//            ///////////////////////////////////
//            /// add current Gestproject customer synchronization table data if any
//            ///////////////////////////////////

//            //new GestprojectDataManager.AddSynchronizationTableCustomerData(
//            //   connection,
//            //   gestprojectCustomer,
//            //   tableSchema
//            //);

//            ///////////////////////////////////
//            /// get Gestproject client current synchronization state
//            ///////////////////////////////////

//            ValidateClientSyncronizationStatus clientSyncronizationStatusValidator = new ValidateClientSyncronizationStatus(
//               gestprojectCustomer,
//               sage50CustomerList,
//               tableSchema
//            );

//            if(clientSyncronizationStatusValidator.MustBeDeleted)
//            {
//               ///////////////////////////////////
//               /// delete sage50 unexisting customer from synchronization table
//               /// and register it again as a never synchronized customer
//               ///////////////////////////////////

//               new GestprojectDataManager.DeleteFromSynchronizationTable(
//                  connection,
//                  gestprojectCustomer,
//                  tableSchema
//               );

//               new GestprojectDataManager.ClearCustomerSynchronizationData(
//                  gestprojectCustomer
//               );

//               gestprojectCustomer.synchronization_status = "Nunca ha sido sincronizado";
//               new GestprojectDataManager.RegisterClient(
//                  connection,
//                  gestprojectCustomer,
//                  tableSchema
//               );

//               new GestprojectDataManager.AddSynchronizationTableCustomerData(
//                  connection,
//                  gestprojectCustomer,
//                  tableSchema
//               );
//            }
//            else if(clientSyncronizationStatusValidator.NeverWasSynchronized)
//            {
//               gestprojectCustomer.synchronization_status = "Nunca ha sido sincronizado";
//               new GestprojectDataManager.UpdateClientState(
//                  connection,
//                  gestprojectCustomer,
//                  tableSchema
//               );
//            }
//            else if(!clientSyncronizationStatusValidator.IsSynchronized)
//            {
//               gestprojectCustomer.synchronization_status = "Desincronizado";
//               new GestprojectDataManager.UpdateClientState(
//                  connection,
//                  gestprojectCustomer,
//                  tableSchema
//               );
//            };

//            ///////////////////////////////////
//            /// add stateful Gestproject client to ui data source
//            ///////////////////////////////////

//            new AddClientToUITable(gestprojectCustomer, table);
//         };

//         ///////////////////////////////////
//         /// return ui data source
//         ///////////////////////////////////

//         return table;
//      }
//   }
//}










