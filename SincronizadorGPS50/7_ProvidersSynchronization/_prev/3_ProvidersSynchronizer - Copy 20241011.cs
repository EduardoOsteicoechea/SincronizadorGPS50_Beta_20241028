//using Infragistics.Designers.SqlEditor;
//using sage.ew.db;
//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
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
//      public ISage50ConnectionManager SageConnectionManager { get; set; }
//      public ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }
//      public ISynchronizationTableSchemaProvider TableSchema { get; set; }

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
//            SageConnectionManager = sage50ConnectionManager;
//            TableSchema = tableSchema;

//            //new VisualizePropertiesAndValues<int>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "selectedIdList",
//            //   selectedIdList
//            //);

//            if(true){
//      //      StoreGestprojectEntityList(
//      //         GestprojectConnectionManager,
//      //         selectedIdList,
//      //         SynchronizationTableSchemaProvider.TableName,
//			   //   SynchronizationTableSchemaProvider.ColumnsTuplesList.Select(x=>(x.columnName,x.columnType)).ToList(),
//      //         (tableSchema.GestprojectId.ColumnDatabaseName, string.Join(",", selectedIdList))
//      //      );

//      //      StoreSage50EntityList(
//      //         tableSchema.SageTableData.dispatcherAndName.sageDispactcherMechanismRoute,
//      //         tableSchema.SageTableData.dispatcherAndName.tableName,
//      //         tableSchema.SageTableData.tableFieldsAlongTypes
//      //      );

//      //      StoreBreakDownGestprojectEntityListByStatus(
//				  // GestprojectEntityList,
//				  // Sage50EntityList
//			   //);

//      //         DetermineEntitySincronizationWorkflow(
//				  // UnexistingGestprojectEntityList,
//				  // ExistingGestprojectEntityList, 
//				  // UnsynchronizedGestprojectEntityList, 
//				  // GestprojectEntityList
//			   //);

//			   //ExecuteSyncronizationWorkflow(
//				  // SomeEntitiesExistsInSage50,
//				  // AllEntitiesExistsInSage50,
//				  // NoEntitiesExistsInSage50,
//				  // UnsynchronizedEntityExists,
//				  // GestprojectConnectionManager,
//				  // Sage50ConnectionManager,
//				  // SynchronizationTableSchemaProvider,
//				  // UnexistingGestprojectEntityList,
//				  // ExistingGestprojectEntityList,
//				  // UnsynchronizedGestprojectEntityList,
//				  // GestprojectEntityList
//			   //);
//            }
//		   }
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

//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "GestprojectEntityList",
//         //   GestprojectEntityList
//         //);
//      }

//      public void StoreSage50EntityList
//      (
//         string sageDispactcherMechanismRoute,
//         string tableName,
//         List<(string, System.Type)> tableFieldsAlongTypes
//      )
//      {
//         string sqlString = $@"
//            SELECT 
//               CODIGO
//               ,CIF
//               ,NOMBRE
//               ,DIRECCION
//               ,CODPOST
//               ,POBLACION
//               ,PROVINCIA
//               ,PAIS
//               ,GUID_ID
//            FROM 
//               {DB.SQLDatabase("gestion","proveed")}
//         ;";

//         DataTable entityDataTable = new DataTable();

//         DB.SQLExec(sqlString, ref entityDataTable);

//         if(entityDataTable.Rows.Count > 0)
//         {
//            for(int i = 0; i < entityDataTable.Rows.Count; i++)
//            {
//               DataRow dataRow = entityDataTable.Rows[i];
//               Sage50ProviderModel entity = new Sage50ProviderModel();

//               entity.CODIGO = dataRow.ItemArray[0].ToString().Trim();
//               entity.CIF = dataRow.ItemArray[1].ToString().Trim();
//               entity.NOMBRE = dataRow.ItemArray[2].ToString().Trim();
//               entity.DIRECCION = dataRow.ItemArray[3].ToString().Trim();
//               entity.CODPOST = dataRow.ItemArray[4].ToString().Trim();
//               entity.POBLACION = dataRow.ItemArray[5].ToString().Trim();
//               entity.PROVINCIA = dataRow.ItemArray[6].ToString().Trim();
//               entity.PAIS = dataRow.ItemArray[7].ToString().Trim();
//               entity.GUID_ID = dataRow.ItemArray[8].ToString().Trim();

//               Sage50EntityList.Add(entity);
//            };
//         };

//         //new VisualizePropertiesAndValues<Sage50ProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "Sage50EntityList",
//         //   Sage50EntityList
//         //);
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
//            bool exists = false;

//            foreach (var sageEntitiy in Sage50EntityList)
//            {
//               if( gestprojectEntity.S50_CODE == sageEntitiy.GUID_ID )
//               {
//                  ExistingGestprojectEntityList.Add(gestprojectEntity);
//                  exists = true;
//                  break;
//               };
//            };

//            if(!exists)
//            {
//               UnexistingGestprojectEntityList.Add(gestprojectEntity);
//            };

//            bool gestprojectEntityHasSynchronizedStatus = gestprojectEntity.SYNC_STATUS == "Sincronizado";
//            bool gestprojectEntityHasSageCode = gestprojectEntity.S50_CODE != "";

//            if(
//               gestprojectEntityHasSynchronizedStatus == false 
//               && 
//               gestprojectEntityHasSageCode == true
//            )
//            {
//               UnsynchronizedGestprojectEntityList.Add(gestprojectEntity);
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
//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "UnexistingGestprojectEntityList",
//         //   UnexistingGestprojectEntityList
//         //);
//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "ExistingGestprojectEntityList",
//         //   ExistingGestprojectEntityList
//         //);
//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "UnsynchronizedGestprojectEntityList",
//         //   UnsynchronizedGestprojectEntityList
//         //);
//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "GestprojectEntityList",
//         //   GestprojectEntityList
//         //);

//         new UnexsistingProvidersSynchronizationWorkflow().Execute
//         (
//            GestprojectConnectionManager,
//            Sage50ConnectionManager,
//            UnexistingGestprojectEntityList,
//            tableSchemaProvider
//         );

//         //if(NoEntitiesExistsInSage50)
//         //{
//         //   new UnexsistingProvidersSynchronizationWorkflow().Execute
//         //   (
//         //      GestprojectConnectionManager,
//         //      Sage50ConnectionManager,
//         //      UnexistingGestprojectEntityList,
//         //      tableSchemaProvider
//         //   );
//         //}

//         //if(AllEntitiesExistsInSage50)
//         //{
//         //   new ExsistingProviderListWorkflow
//         //   (
//         //      GestprojectDataHolder.GestprojectDatabaseConnection,
//         //      gestProjectProviderList,
//         //      unsynchronizedProviderList,
//         //      unsynchronizedProvidersExists,
//         //      tableSchema
//         //   );
//         //}

//         //if(SomeEntitiesExistsInSage50 && !AllEntitiesExistsInSage50)
//         //{
//         //   if(UnsynchronizedEntityExists)
//         //   {
//         //      new ExsistingProviderListWorkflow
//         //      (
//         //         GestprojectDataHolder.GestprojectDatabaseConnection,
//         //         existingProvidersList,
//         //         unsynchronizedProviderList,
//         //         unsynchronizedProvidersExists,
//         //         tableSchema
//         //      );
//         //   };

//         //   new UnexsistingProviderListWorkflow
//         //   (
//         //      GestprojectDataHolder.GestprojectDatabaseConnection,
//         //      nonExistingProvidersList,
//         //      tableSchema
//         //   );
//         //};
//      }
//   }
//}
