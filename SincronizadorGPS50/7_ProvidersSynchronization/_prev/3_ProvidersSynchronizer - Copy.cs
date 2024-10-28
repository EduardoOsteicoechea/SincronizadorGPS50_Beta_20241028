//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class ProvidersSynchronizer : IEntitySynchronizer<GestprojectProviderModel, Sage50ProviderModel>
//   {
//      public List<GestprojectProviderModel> GestprojectEntityList {get;set;} = new List<GestprojectProviderModel>();
//      public List<Sage50ProviderModel> Sage50EntityList {get;set;} = new List<Sage50ProviderModel> { };
//      public List<GestprojectProviderModel> UnexistingGestprojectEntityList {get;set;} = new List<GestprojectProviderModel>();
//      public List<GestprojectProviderModel> ExistingGestprojectEntityList {get;set; } = new List<GestprojectProviderModel>();
//      public List<GestprojectProviderModel> UnsynchronizedGestprojectEntityList {get;set; } = new List<GestprojectProviderModel>();
//      public bool SomeEntitiesExistsInSage50 {get;set;}
//      public bool AllEntitiesExistsInSage50 {get;set;}
//      public bool NoEntitiesExistsInSage50 {get;set;}
//      public bool UnsynchronizedEntityExists {get;set;}
//      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
//      public ISage50ConnectionManager Sage50ConnectionManager { get; set; }
//      public ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }

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
//            GestprojectConnectionManager = gestprojectConnectionManager;
//            Sage50ConnectionManager = sage50ConnectionManager;
//            SynchronizationTableSchemaProvider = tableSchema;

//            StoreGestprojectEntityList
//            (
//               GestprojectConnectionManager,
//               selectedIdList,
//               SynchronizationTableSchemaProvider.TableName,
//               new List<(string, System.Type)>()
//               {
//                  (tableSchema.Name.ColumnDatabaseName, tableSchema.Name.ColumnValueType),
//                  (tableSchema.Cif.ColumnDatabaseName, tableSchema.Cif.ColumnValueType),
//                  (tableSchema.Address.ColumnDatabaseName, tableSchema.Address.ColumnValueType),
//                  (tableSchema.PostalCode.ColumnDatabaseName, tableSchema.PostalCode.ColumnValueType),
//                  (tableSchema.Locality.ColumnDatabaseName, tableSchema.Locality.ColumnValueType),
//                  (tableSchema.Province.ColumnDatabaseName, tableSchema.Province.ColumnValueType),
//                  (tableSchema.Country.ColumnDatabaseName, tableSchema.Country.ColumnValueType),
//                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, tableSchema.SynchronizationStatus.ColumnValueType),
//                  (tableSchema.CompanyGroupName.ColumnDatabaseName, tableSchema.CompanyGroupName.ColumnValueType),
//                  (tableSchema.CompanyGroupCode.ColumnDatabaseName, tableSchema.CompanyGroupCode.ColumnValueType),
//                  (tableSchema.CompanyGroupMainCode.ColumnDatabaseName, tableSchema.CompanyGroupMainCode.ColumnValueType),
//                  (tableSchema.CompanyGroupGuidId.ColumnDatabaseName, tableSchema.CompanyGroupGuidId.ColumnValueType),
//                  (tableSchema.GestprojectId.ColumnDatabaseName, tableSchema.GestprojectId.ColumnValueType),
//                  (tableSchema.Sage50Code.ColumnDatabaseName, tableSchema.Sage50Code.ColumnValueType),
//                  (tableSchema.Sage50GuidId.ColumnDatabaseName, tableSchema.Sage50GuidId.ColumnValueType),
//                  (tableSchema.Comments.ColumnDatabaseName, tableSchema.Comments.ColumnValueType)
//               },
//               (
//                  tableSchema.GestprojectId.ColumnDatabaseName,
//                  string.Join(",", selectedIdList)
//               )
//            );

//            StoreSage50EntityList
//            (
//               "proveed",
//               new List<(string, System.Type)>()
//               {
//                  ("CODIGO", typeof(string)),
//                  ("CIF", typeof(string)),
//                  ("NOMBRE", typeof(string)),
//                  ("DIRECCION", typeof(string)),
//                  ("CODPOST", typeof(string)),
//                  ("POBLACION", typeof(string)),
//                  ("PROVINCIA", typeof(string)),
//                  ("PAIS", typeof(string)),
//                  ("GUID_ID", typeof(string))
//               }
//            );

//            StoreBreakDownGestprojectEntityListByStatus(GestprojectEntityList, Sage50EntityList);

//            DetermineEntitySincronizationWorkflow(UnexistingGestprojectEntityList, ExistingGestprojectEntityList, UnsynchronizedGestprojectEntityList, GestprojectEntityList);

//            ExecuteSyncronizationWorkflow
//            (
//               SomeEntitiesExistsInSage50,
//               AllEntitiesExistsInSage50,
//               NoEntitiesExistsInSage50,
//               UnsynchronizedEntityExists,
//               GestprojectConnectionManager,
//               Sage50ConnectionManager,
//               SynchronizationTableSchemaProvider,
//               UnexistingGestprojectEntityList,
//               ExistingGestprojectEntityList,
//               UnsynchronizedGestprojectEntityList,
//               GestprojectEntityList
//            );
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

//      public void StoreGestprojectEntityList
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager, 
//         List<int> selectedIdList, 
//         string tableName, 
//         List<(string, System.Type)> fieldsToBeRetrieved, 
//         (string condition1ColumnName, string condition1Value) condition1Data
//      )
//      {
//         GestprojectEntityList = new GestprojectEntities<GestprojectProviderModel>().GetAll(
//            gestprojectConnectionManager.GestprojectSqlConnection,
//            selectedIdList,
//            tableName,
//            fieldsToBeRetrieved,
//            condition1Data
//         );
//      }

//      public void StoreSage50EntityList
//      (
//         string tableName, 
//         List<(string, System.Type)> fieldsToBeRetrieved
//      )
//      {
//         Sage50EntityList = new Sage50Entities<Sage50ProviderModel>().GetAll(
//            tableName,
//            fieldsToBeRetrieved
//         );
//      }
      
//      public void StoreBreakDownGestprojectEntityListByStatus
//      (
//         List<GestprojectProviderModel> GestprojectEntityList, 
//         List<Sage50ProviderModel> Sage50EntityList
//      )
//      {
//         for(int i = 0; i < GestprojectEntityList.Count; i++)
//         {
//            var gestprojectEntity = GestprojectEntityList[i];
//            bool found = false;

//            for(global::System.Int32 j = 0; j < Sage50EntityList.Count; j++)
//            {
//               var sage50Entity = Sage50EntityList[j];
//               if(
//                  gestprojectEntity.S50_CODE == sage50Entity.GUID_ID
//               )
//               {
//               ExistingGestprojectEntityList.Add(gestprojectEntity);
//                  found = true;
//                  break;
//               };
//            };

//            if(!found)
//            {
//            UnexistingGestprojectEntityList.Add(gestprojectEntity);
//            };

//            if(gestprojectEntity.SYNC_STATUS != "Sincronizado" && gestprojectEntity.S50_CODE != "")
//            {
//            UnsynchronizedGestprojectEntityList.Add(gestprojectEntity);
//            };
//         };
//      }

//      public void DetermineEntitySincronizationWorkflow
//      (
//         List<GestprojectProviderModel> UnexistingGestprojectEntityList, 
//         List<GestprojectProviderModel> ExistingGestprojectEntityList, 
//         List<GestprojectProviderModel> UnsynchronizedGestprojectEntityList,
//         List<GestprojectProviderModel> GestprojectEntityList
//      )
//      {
//         SomeEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count > 0;
//         AllEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count == GestprojectEntityList.Count;
//         NoEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count == 0;
//         UnsynchronizedEntityExists = UnsynchronizedGestprojectEntityList.Count > 0;
//      }

//      public void ExecuteSyncronizationWorkflow
//      (
//         bool SomeEntitiesExistsInSage50, 
//         bool AllEntitiesExistsInSage50, 
//         bool NoEntitiesExistsInSage50, 
//         bool UnsynchronizedEntityExists, 
//         IGestprojectConnectionManager GestprojectConnectionManager, 
//         ISage50ConnectionManager Sage50ConnectionManager, 
//         ISynchronizationTableSchemaProvider tableSchemaProvider, 
//         List<GestprojectProviderModel> UnexistingGestprojectEntityList, 
//         List<GestprojectProviderModel> ExistingGestprojectEntityList, 
//         List<GestprojectProviderModel> UnsynchronizedGestprojectEntityList,
//         List<GestprojectProviderModel> GestprojectEntityList
//      )
//      {
//         //var aa = "";
//         //foreach (var item in GestprojectEntityList)
//         //{
//         //   aa += $"{item.NOMBRE_COMPLETO}\n";
//         //};
//         //MessageBox.Show(aa);

//         if (NoEntitiesExistsInSage50)
//         {
//            new UnexsistingProvidersSynchronizationWorkflow().Execute
//            (
//               GestprojectConnectionManager,
//               Sage50ConnectionManager,
//               UnexistingGestprojectEntityList,
//               tableSchemaProvider
//            );
//         }

//         if(AllEntitiesExistsInSage50)
//         {
//            //new ExsistingProviderListWorkflow
//            //(
//            //   GestprojectDataHolder.GestprojectDatabaseConnection,
//            //   gestProjectProviderList,
//            //   unsynchronizedProviderList,
//            //   unsynchronizedProvidersExists,
//            //   tableSchema
//            //);
//         }

//         if(SomeEntitiesExistsInSage50 && !AllEntitiesExistsInSage50)
//         {
//            if(UnsynchronizedEntityExists)
//            {
//               //new ExsistingProviderListWorkflow
//               //(
//               //   GestprojectDataHolder.GestprojectDatabaseConnection,
//               //   existingProvidersList,
//               //   unsynchronizedProviderList,
//               //   unsynchronizedProvidersExists,
//               //   tableSchema
//               //);
//            };

//            //new UnexsistingProviderListWorkflow
//            //(
//            //   GestprojectDataHolder.GestprojectDatabaseConnection,
//            //   nonExistingProvidersList,
//            //   tableSchema
//            //);
//         };
//      }
//   }
//}
