//using System.Collections.Generic;
//using System.Data;
//using System.Reflection;
//using System.Text;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class IssuedBillsDataTableManager : IGridDataSourceGenerator<GestprojectIssuedBillModel, Sage50IssuedBillModel>
//   {
//      public List<GestprojectIssuedBillModel> GestprojectEntities { get; set; }
//      public List<Sage50IssuedBillModel> Sage50Entities { get; set; }
//      public List<GestprojectIssuedBillModel> ProcessedGestprojectEntities { get; set; }
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
//            ManageSynchronizationTableStatus(gestprojectConnectionManager, tableSchemaProvider);
//            GetAndStoreGestprojectEntities(gestprojectConnectionManager, tableSchemaProvider);
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

//      public void ManageSynchronizationTableStatus ( IGestprojectConnectionManager gestprojectConnectionManager, ISynchronizationTableSchemaProvider tableSchemaProvider )
//      {
//         ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

//         bool tableExists = entitySyncronizationTableStatusManager.TableExists(
//            gestprojectConnectionManager.GestprojectSqlConnection,
//            tableSchemaProvider.TableName
//         );

//         if(tableExists == false)
//         {
//            entitySyncronizationTableStatusManager.CreateTable
//            (
//               gestprojectConnectionManager.GestprojectSqlConnection,
//               tableSchemaProvider
//            );
//         };
//      }

//      public void GetAndStoreGestprojectEntities ( IGestprojectConnectionManager gestprojectConnectionManager, ISynchronizationTableSchemaProvider tableSchemaProvider )
//      {
//         GestprojectEntities = new GestprojectIssuedBillsManager().GetEntities(
//            gestprojectConnectionManager.GestprojectSqlConnection,
//            "FACTURA_EMITIDA",
//            tableSchemaProvider.GestprojectFieldsTupleList
//         );

//         //foreach(var item in GestprojectEntities)
//         //{
//         //   StringBuilder stringBuilder = new StringBuilder();
//         //   foreach(var prpoertyInfo in item.GetType().GetProperties())
//         //   {
//         //      stringBuilder.Append($"{prpoertyInfo.Name}: {prpoertyInfo.GetValue(item)}\n");
//         //   }  
//         //   MessageBox.Show( stringBuilder.ToString() );
//         //}
//      }

//      public void GetAndStoreSage50Entities ( ISynchronizationTableSchemaProvider tableSchemaProvider )
//      {
//         Sage50Entities = new GetSage50IssuedBills().Entities;
//      }

//      public void ProccessAndStoreGestprojectEntities
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectIssuedBillModel> GestprojectEntities,
//         List<Sage50IssuedBillModel> Sage50Entities
//      )
//      {

//         ISynchronizableEntityProcessor<GestprojectIssuedBillModel, Sage50IssuedBillModel> gestprojectProvidersProcessor = new GestprojectIssuedBillsProcessor();
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
//         IDataTableGenerator entityDataTableGenerator = new SyncrhonizationDataTableGenerator();
//         DataTable = entityDataTableGenerator.CreateDataTable(tableSchemaProvider.ColumnsTuplesList);
//      }

//      public void PaintEntitiesOnDataSource
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectIssuedBillModel> ProcessedGestprojectEntities,
//         DataTable dataTable
//      )
//      {
//         //foreach(var entity in ProcessedGestprojectEntities)
//         //{
//         //   StringBuilder stringBuilder = new StringBuilder();
//         //   foreach(var item in entity.GetType().GetProperties())
//         //   {
//         //      stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
//         //   };
//         //   MessageBox.Show(stringBuilder.ToString());
//         //};

//         ISynchronizableEntityPainter<GestprojectIssuedBillModel> entityPainter = new EntityPainter<GestprojectIssuedBillModel>();
//         entityPainter.PaintEntityListOnDataTable(
//            ProcessedGestprojectEntities,
//            dataTable,
//            tableSchemaProvider.ColumnsTuplesList
//         );
//      }
//   }
//}
