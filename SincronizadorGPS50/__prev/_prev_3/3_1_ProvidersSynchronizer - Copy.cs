//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System.Collections.Generic;
//using System.Reflection;

//namespace SincronizadorGPS50
//{
//   internal class ProvidersSynchronizer : IEntitySynchronizer
//   {
//      public void Synchronize
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchema,
//         List<int> selectedIdList
//      )
//      {
//         try
//         {
//            ////////////////////////////////////
//            /// get gestproject and sage50 Providers
//            ////////////////////////////////////

//            List<GestprojectProviderModel> gestProjectProviderList = new GetProvidersFromSynchronizationTable(
//               gestprojectConnectionManager.GestprojectSqlConnection,
//               selectedIdList,
//               tableSchema.TableName,
//               tableSchema.GestprojectProviderNameColumn.ColumnDatabaseName,
//               tableSchema.GestprojectProviderCIFNIFColumn.ColumnDatabaseName,
//               tableSchema.GestprojectProviderAddressColumn.ColumnDatabaseName,
//               tableSchema.GestprojectProviderPostalCodeColumn.ColumnDatabaseName,
//               tableSchema.GestprojectProviderLocalityColumn.ColumnDatabaseName,
//               tableSchema.GestprojectProviderProvinceColumn.ColumnDatabaseName,
//               tableSchema.GestprojectProviderCountryColumn.ColumnDatabaseName,
//               tableSchema.SynchronizationStatusColumn.ColumnDatabaseName,
//               tableSchema.Sage50ProviderCompanyGroupNameColumn.ColumnDatabaseName,
//               tableSchema.Sage50ProviderCompanyGroupCodeColumn.ColumnDatabaseName,
//               tableSchema.Sage50ProviderCompanyGroupMainCodeColumn.ColumnDatabaseName,
//               tableSchema.Sage50ProviderCompanyGroupGuidIdColumn.ColumnDatabaseName,
//               tableSchema.GestprojectProviderIdColumn.ColumnDatabaseName,
//               tableSchema.Sage50ProviderCodeColumn.ColumnDatabaseName,
//               tableSchema.Sage50ProviderGuidIdColumn.ColumnDatabaseName,
//               tableSchema.CommentsColumn.ColumnDatabaseName
//            ).GestprojectEntityList;

//            List<Sage50ProviderModel> sage50ProviderList = new GetSage50Providers().ProviderList;

//            ////////////////////////////////////
//            /// get Providers distributon in sage50 (any exist, some exist, all exist) to determine application flow
//            ////////////////////////////////////

//            List<GestprojectProviderModel> existingProvidersList = new List<GestprojectProviderModel>();
//            List<GestprojectProviderModel> nonExistingProvidersList = new List<GestprojectProviderModel>();
//            List<GestprojectProviderModel> unsynchronizedProviderList = new List<GestprojectProviderModel>();

//            for(int i = 0; i < gestProjectProviderList.Count; i++)
//            {
//               GestprojectProviderModel gestprojectProvider = gestProjectProviderList[i];
//               bool found = false;

//               for(global::System.Int32 j = 0; j < sage50ProviderList.Count; j++)
//               {
//                  Sage50ProviderModel sage50Provider = sage50ProviderList[j];
//                  if(
//                     gestprojectProvider.sage50_guid_id == sage50Provider.GUID_ID
//                  )
//                  {
//                     existingProvidersList.Add(gestprojectProvider);
//                     found = true;
//                     break;
//                  };
//               };

//               if(!found)
//               {
//                  nonExistingProvidersList.Add(gestprojectProvider);
//               };

//               if(gestprojectProvider.synchronization_status != "Sincronizado" && gestprojectProvider.sage50_guid_id != "")
//               {
//                  unsynchronizedProviderList.Add(gestprojectProvider);
//               };
//            };

//            bool someGestprojectProvidersExistsInSage50 = existingProvidersList.Count > 0;
//            bool allGestprojectProvidersExistsInSage50 = existingProvidersList.Count == gestProjectProviderList.Count;
//            bool noGestprojectProvidersExistsInSage50 = existingProvidersList.Count == 0;
//            bool unsynchronizedProvidersExists = unsynchronizedProviderList.Count > 0;

//            ////////////////////////////////////
//            /// execute Provider synchornization flow according to Provider distribution
//            ////////////////////////////////////

//            if(noGestprojectProvidersExistsInSage50)
//            {
//               new UnexsistingProviderListWorkflow
//               (
//                  GestprojectDataHolder.GestprojectDatabaseConnection, 
//                  nonExistingProvidersList, 
//                  tableSchema
//               );
//            }

//            if(allGestprojectProvidersExistsInSage50)
//            {
//               new ExsistingProviderListWorkflow
//               (
//                  GestprojectDataHolder.GestprojectDatabaseConnection, 
//                  gestProjectProviderList, 
//                  unsynchronizedProviderList, 
//                  unsynchronizedProvidersExists, 
//                  tableSchema
//               );
//            }

//            if(someGestprojectProvidersExistsInSage50 && !allGestprojectProvidersExistsInSage50)
//            {
//               if(unsynchronizedProvidersExists)
//               {
//                  new ExsistingProviderListWorkflow
//                  (
//                     GestprojectDataHolder.GestprojectDatabaseConnection, 
//                     existingProvidersList, 
//                     unsynchronizedProviderList, 
//                     unsynchronizedProvidersExists, 
//                     tableSchema
//                  );
//               };

//               new UnexsistingProviderListWorkflow
//               (
//                  GestprojectDataHolder.GestprojectDatabaseConnection, 
//                  nonExistingProvidersList, 
//                  tableSchema
//               );
//            };
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//   }
//}
