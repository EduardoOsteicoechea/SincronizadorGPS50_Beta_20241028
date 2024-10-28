//using sage.ew.db;
//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using SincronizadorGPS50.Workflows.Sage50Connection;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class ProjectsSynchronizer : IEntitySynchronizer<GestprojectProjectModel, Sage50ProjectModel>
//   {
//      public List<GestprojectProjectModel> GestprojectEntityList {get;set;} = new List<GestprojectProjectModel>();
//      public List<Sage50ProjectModel> Sage50EntityList {get;set;} = new List<Sage50ProjectModel> { };
//      public List<GestprojectProjectModel> UnexistingGestprojectEntityList {get;set;} = new List<GestprojectProjectModel>();
//      public List<GestprojectProjectModel> ExistingGestprojectEntityList {get;set; } = new List<GestprojectProjectModel>();
//      public List<GestprojectProjectModel> UnsynchronizedGestprojectEntityList {get;set; } = new List<GestprojectProjectModel>();
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
//               tableSchema.ColumnsTuplesList.Select(x=> (x.columnName,x.columnType)).ToList(),
//               ( tableSchema.GestprojectId.ColumnDatabaseName, string.Join(",", selectedIdList) )
//            );

//            StoreSage50EntityList
//            (
//               tableSchema.SageTableData.dispatcherAndName.sageDispactcherMechanismRoute,
//               tableSchema.SageTableData.dispatcherAndName.tableName,
//               tableSchema.SageTableData.tableFieldsAlongTypes
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
//         try
//         {
//            GestprojectEntityList = new GestprojectEntities<GestprojectProjectModel>().GetAll(
//               gestprojectConnectionManager.GestprojectSqlConnection,
//               selectedIdList,
//               tableName,
//               fieldsToBeRetrieved,
//               condition1Data
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

//      public void StoreSage50EntityList
//      (
//         string sageDispactcherMechanismRoute,
//         string tableName,
//         List<(string, System.Type)> tableFieldsAlongTypes
//      )
//      {
//         try
//         {
//            //Sage50EntityList = new Sage50Entities<Sage50ProjectModel>().GetAll(
//            //   sageDispactcherMechanismRoute,
//            //   tableName,
//            //   tableFieldsAlongTypes
//            //);

            
//            string sqlString = $@"
//               SELECT 
//                  CODIGO
//                  ,CIF
//                  ,NOMBRE
//                  ,DIRECCION
//                  ,CODPOST
//                  ,POBLACION
//                  ,PROVINCIA
//                  ,PAIS
//                  ,GUID_ID
//               FROM 
//                  {DB.SQLDatabase("gestion","proveed")}
//            ;";

//            DataTable entityDataTable = new DataTable();

//            DB.SQLExec(sqlString, ref entityDataTable);

//            if(entityDataTable.Rows.Count > 0)
//            {
//               for(int i = 0; i < entityDataTable.Rows.Count; i++)
//               {
//                  DataRow dataRow = entityDataTable.Rows[i];
//                  Sage50ProviderModel entity = new Sage50ProviderModel();

//                  entity.CODIGO = dataRow.ItemArray[0].ToString().Trim();
//                  entity.CIF = dataRow.ItemArray[1].ToString().Trim();
//                  entity.NOMBRE = dataRow.ItemArray[2].ToString().Trim();
//                  entity.DIRECCION = dataRow.ItemArray[3].ToString().Trim();
//                  entity.CODPOST = dataRow.ItemArray[4].ToString().Trim();
//                  entity.POBLACION = dataRow.ItemArray[5].ToString().Trim();
//                  entity.PROVINCIA = dataRow.ItemArray[6].ToString().Trim();
//                  entity.PAIS = dataRow.ItemArray[7].ToString().Trim();
//                  entity.GUID_ID = dataRow.ItemArray[8].ToString().Trim();

//                  Sage50EntityList.Add(entity);
//               };
//            };

//            new VisualizePropertiesAndValues<Sage50ProjectModel>(
//               MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               "Sage50ProjectModel",
//               Sage50EntityList
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
      
//      public void StoreBreakDownGestprojectEntityListByStatus
//      (
//         List<GestprojectProjectModel> GestprojectEntityList, 
//         List<Sage50ProjectModel> Sage50EntityList
//      )
//      {
//         try
//         {
//            for(int i = 0; i < GestprojectEntityList.Count; i++)
//            {
//               var gestprojectEntity = GestprojectEntityList[i];
//               bool found = false;

//               for(global::System.Int32 j = 0; j < Sage50EntityList.Count; j++)
//               {
//                  var sage50Entity = Sage50EntityList[j];
//                  if(
//                     gestprojectEntity.S50_CODE == sage50Entity.GUID_ID
//                  )
//                  {
//                     ExistingGestprojectEntityList.Add(gestprojectEntity);
//                     found = true;
//                     break;
//                  };
//               };

//               if(!found)
//               {
//                  UnexistingGestprojectEntityList.Add(gestprojectEntity);
//               };

//               if(gestprojectEntity.SYNC_STATUS != "Sincronizado" && gestprojectEntity.S50_CODE != "")
//               {
//                  UnsynchronizedGestprojectEntityList.Add(gestprojectEntity);
//               };
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

//      public void DetermineEntitySincronizationWorkflow
//      (
//         List<GestprojectProjectModel> UnexistingGestprojectEntityList, 
//         List<GestprojectProjectModel> ExistingGestprojectEntityList, 
//         List<GestprojectProjectModel> UnsynchronizedGestprojectEntityList,
//         List<GestprojectProjectModel> GestprojectEntityList
//      )
//      {
//         try
//         {
//            SomeEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count > 0;
//            AllEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count == GestprojectEntityList.Count;
//            NoEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count == 0;
//            UnsynchronizedEntityExists = UnsynchronizedGestprojectEntityList.Count > 0;
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

//      public void ExecuteSyncronizationWorkflow
//      (
//         bool SomeEntitiesExistsInSage50, 
//         bool AllEntitiesExistsInSage50, 
//         bool NoEntitiesExistsInSage50, 
//         bool UnsynchronizedEntityExists, 
//         IGestprojectConnectionManager GestprojectConnectionManager, 
//         ISage50ConnectionManager Sage50ConnectionManager, 
//         ISynchronizationTableSchemaProvider tableSchemaProvider, 
//         List<GestprojectProjectModel> UnexistingGestprojectEntityList, 
//         List<GestprojectProjectModel> ExistingGestprojectEntityList, 
//         List<GestprojectProjectModel> UnsynchronizedGestprojectEntityList,
//         List<GestprojectProjectModel> GestprojectEntityList
//      )
//      {
//         try
//         {
//            //new VisualizeData<GestprojectProjectModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "UnexistingGestprojectEntityList",
//            //   UnexistingGestprojectEntityList, 
//            //   new GestprojectProjectModel().GetType().GetProperties()[2]
//            //);
//            //new VisualizeData<GestprojectProjectModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "ExistingGestprojectEntityList",
//            //   ExistingGestprojectEntityList, 
//            //   new GestprojectProjectModel().GetType().GetProperties()[2]
//            //);
//            //new VisualizeData<GestprojectProjectModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "UnsynchronizedGestprojectEntityList",
//            //   UnsynchronizedGestprojectEntityList, 
//            //   new GestprojectProjectModel().GetType().GetProperties()[2]
//            //);
//            //new VisualizeData<GestprojectProjectModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "GestprojectEntityList",
//            //   GestprojectEntityList, 
//            //   new GestprojectProjectModel().GetType().GetProperties()[2]
//            //);

//            if (NoEntitiesExistsInSage50)
//            {
//               new UnexsistingProjectListWorkflow
//               (
//                  GestprojectConnectionManager,
//                  Sage50ConnectionManager,
//                  tableSchemaProvider,
//                  UnexistingGestprojectEntityList
//               );
//            }

//            if(AllEntitiesExistsInSage50)
//            {
//               //new ExsistingProviderListWorkflow
//               //(
//               //   GestprojectDataHolder.GestprojectDatabaseConnection,
//               //   gestProjectProviderList,
//               //   unsynchronizedProviderList,
//               //   unsynchronizedProvidersExists,
//               //   tableSchema
//               //);
//            }

//            if(SomeEntitiesExistsInSage50 && !AllEntitiesExistsInSage50)
//            {
//               if(UnsynchronizedEntityExists)
//               {
//                  //new ExsistingProviderListWorkflow
//                  //(
//                  //   GestprojectDataHolder.GestprojectDatabaseConnection,
//                  //   existingProvidersList,
//                  //   unsynchronizedProviderList,
//                  //   unsynchronizedProvidersExists,
//                  //   tableSchema
//                  //);
//               };

//               //new UnexsistingProviderListWorkflow
//               //(
//               //   GestprojectDataHolder.GestprojectDatabaseConnection,
//               //   nonExistingProvidersList,
//               //   tableSchema
//               //);
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
