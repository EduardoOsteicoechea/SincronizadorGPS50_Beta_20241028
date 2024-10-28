//using System.Collections.Generic;
//using System.Data;
//using System.Reflection;

//namespace SincronizadorGPS50
//{
//   public class ProjectsDataTableManager : IGridDataSourceGenerator<GestprojectProjectModel, Sage50ProjectModel>
//   {
//      public List<GestprojectProjectModel> GestprojectEntities { get; set; }
//      public List<Sage50ProjectModel> Sage50Entities { get; set; }
//      public List<GestprojectProjectModel> ProcessedGestprojectEntities { get; set; }
//      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
//      public System.Data.SqlClient.SqlConnection Connection { get; set; }
//      public ISage50ConnectionManager SageConnectionManager { get; set; }
//      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
//      public DataTable DataTable { get; set; }

//      public System.Data.DataTable GenerateDataTable
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         try
//         {
//            GestprojectConnectionManager = gestprojectConnectionManager;
//            Connection = GestprojectConnectionManager.GestprojectSqlConnection;
//            SageConnectionManager = sage50ConnectionManager;
//            TableSchema = tableSchemaProvider;

//            ManageSynchronizationTableStatus(TableSchema);

//            GetGestprojectEntities();

//            GetAndStoreSage50Entities(tableSchemaProvider);
//            ProccessAndStoreGestprojectEntities(
//               gestprojectConnectionManager,
//               sage50ConnectionManager,
//               tableSchemaProvider,
//               GestprojectEntities,
//               Sage50Entities
//            );
//            CreateAndDefineDataSource(tableSchemaProvider);
//            PaintEntitiesOnDataSource(tableSchemaProvider, ProcessedGestprojectEntities, DataTable);
//            return DataTable;
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

//      public void ManageSynchronizationTableStatus
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         ISynchronizationDatabaseTableManager providersSyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

//         bool tableExists = providersSyncronizationTableStatusManager.TableExists(
//            GestprojectConnectionManager.GestprojectSqlConnection,
//            tableSchemaProvider.TableName
//         );

//         if(tableExists == false)
//         {
//            providersSyncronizationTableStatusManager.CreateTable
//            (
//               GestprojectConnectionManager.GestprojectSqlConnection,
//               tableSchemaProvider
//            );
//         };
//      }

//      public void GetGestprojectEntities()
//      {
//         GestprojectEntities = new GestprojectProjectsManager().GetEntities(
//            GestprojectConnectionManager.GestprojectSqlConnection,
//            "PROYECTO",
//            TableSchema.GestprojectFieldsTupleList
//         );
//      }

//      public void GetAndStoreSage50Entities
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         //Sage50Entities = new GetSage50Projects(tableSchemaProvider.SageTableData).Entities;
//         Sage50Entities = new GetSage50Projects().Entities;
//      }

//      public void ProccessAndStoreGestprojectEntities
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectProjectModel> GestprojectEntities,
//         List<Sage50ProjectModel> Sage50Entities
//      )
//      {
         
//         ISynchronizableEntityProcessor<GestprojectProjectModel, Sage50ProjectModel> gestprojectProvidersProcessor = new GestprojectProjectsProcessor();
//         ProcessedGestprojectEntities = gestprojectProvidersProcessor.ProcessEntityList(
//            gestprojectConnectionManager.GestprojectSqlConnection,
//            sage50ConnectionManager,
//            tableSchemaProvider,
//            GestprojectEntities,
//            Sage50Entities
//         );
//      }

//      public void CreateAndDefineDataSource
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         IDataTableGenerator providersDataTableGenerator = new SyncrhonizationDataTableGenerator();
//         DataTable = providersDataTableGenerator.CreateDataTable(tableSchemaProvider.ColumnsTuplesList);
//      }

//      public void PaintEntitiesOnDataSource
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectProjectModel> ProcessedGestprojectEntities,
//         DataTable dataTable
//      )
//      {
//         ISynchronizableEntityPainter<GestprojectProjectModel> entityPainter = new EntityPainter<GestprojectProjectModel>();
//         entityPainter.PaintEntityListOnDataTable(
//            ProcessedGestprojectEntities,
//            dataTable,
//            tableSchemaProvider.ColumnsTuplesList
//         );
//      }
//   }
//}
