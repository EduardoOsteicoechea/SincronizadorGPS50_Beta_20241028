//using Infragistics.Win.UltraWinSchedule;
//using sage.ew.db;
//using sage.ew.docscompra;
//using SincronizadorGPS50.Workflows.Sage50Connection;
//using System.Collections.Generic;
//using System.Data;
//using System.Reflection;
//using System.Text;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class ReceivedBillsDataTableManager : IGridDataSourceGenerator<GestprojectReceivedBillModel, Sage50ReceivedBillModel>
//   {
//      public List<GestprojectReceivedBillModel> GestprojectEntities { get; set; }
//      public List<Sage50ReceivedBillModel> Sage50Entities { get; set; }
//      public List<GestprojectReceivedBillModel> ProcessedGestprojectEntities { get; set; }
//      public IGestprojectConnectionManager GestprojectConnectionManager  { get; set; }
//      public ISage50ConnectionManager SageConnectionManager  { get; set; }
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
//            SageConnectionManager = sage50ConnectionManager;
//            TableSchema = tableSchemaProvider;

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
//         //GestprojectEntities = new GestprojectReceivedBillsManager().GetEntities(
//         //   gestprojectConnectionManager.GestprojectSqlConnection,
//         //   "FACTURA_PROVEEDOR",
//         //   tableSchemaProvider.GestprojectFieldsTupleList
//         //);

//         List<(string invoiceCode, string providerCode)> receivedInvoiceCodesAndProvidersCodes = GetSageReceivedInvoiceCodesAndProvidersCodes();


//         // Get Receivde invoice codes,
//         // Get invoices provider
//         // load Invoide
//         // Get Lines
//         // Record Lines in GP
//         // Record Invoice in GP

//         ewDocCompraFACTURA loFraComp = new ewDocCompraFACTURA();
//         if(
//            loFraComp._Existe(
//               SageConnectionManager.CompanyGroupData.CompanyName, 
//               codigoDeFactura, 
//               codigoDeProveedor
//            )
//         )
//         {
//            loFraComp._Load(
//               SageConnectionManager.CompanyGroupData.CompanyName, 
//               codigoDeFactura, 
//               codigoDeProveedor
//            );

//            List<ewDocCompraLinFACTURA> invoiceDetailList = new List<ewDocCompraLinFACTURA>();

//            if(!string.IsNullOrWhiteSpace(loFraComp._Cabecera._Obra))
//            {
//               foreach(ewDocCompraLinFACTURA invoiceDetail in loFraComp._Lineas)
//               {
//                  invoiceDetailList.Add(invoiceDetail);
//               }
//            }
//         }
//      }

//      public class ReceivedInvoiceModel 
//      {
//         public string CompanyNumber {get;set;} = "";
//         public string Number {get;set;} = "";
//         public string ProviderCode {get;set;} = "";
//         public string GuidId {get;set;} = "";
//         public string IvaObject {get;set;} = "";
//      }

//      public List<ReceivedInvoiceModel> GetSageReceivedInvoiceCodesAndProvidersCodes()
//      {
//         try
//         {
//            List<ReceivedInvoiceModel> receivedInvoiceCodesAndProvidersCodes = new List<ReceivedInvoiceModel>();

//            string sqlString = $@"
//            SELECT 
//               empresa
//               ,numero
//               ,proveedor
//               ,guid_id
//               ,iva
//            FROM 
//               {DB.SQLDatabase("gestion","factucom")}
//            ;";

//            DataTable enentiesDataTable = new DataTable();

//            DB.SQLExec(sqlString, ref enentiesDataTable);

//            if(enentiesDataTable.Rows.Count > 0)
//            {
//               for(int i = 0; i < enentiesDataTable.Rows.Count; i++)
//               {
//                  DataRow row = enentiesDataTable.Rows[i];
//                  ReceivedInvoiceModel entity = new ReceivedInvoiceModel();

//                  entity.CompanyNumber = row.ItemArray[0].ToString().Trim();
//                  entity.CompanyNumber = row.ItemArray[1].ToString().Trim();
//                  entity.CompanyNumber = row.ItemArray[2].ToString().Trim();
//                  entity.CompanyNumber = row.ItemArray[3].ToString().Trim();
//                  entity.CompanyNumber = row.ItemArray[4].ToString().Trim();

//                  receivedInvoiceCodesAndProvidersCodes.Add(entity);
//               };
//            };

//            return receivedInvoiceCodesAndProvidersCodes;
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

//      public void GetAndStoreSage50Entities ( ISynchronizationTableSchemaProvider tableSchemaProvider )
//      {
//         Sage50Entities = new GetSage50ReceivedBills().Entities;
//      }

//      public void ProccessAndStoreGestprojectEntities
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectReceivedBillModel> GestprojectEntities,
//         List<Sage50ReceivedBillModel> Sage50Entities
//      )
//      {

//         ISynchronizableEntityProcessor<GestprojectReceivedBillModel, Sage50ReceivedBillModel> gestprojectProvidersProcessor = new GestprojectReceivedBillsProcessor();
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
//         List<GestprojectReceivedBillModel> ProcessedGestprojectEntities,
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

//         ISynchronizableEntityPainter<GestprojectReceivedBillModel> entityPainter = new EntityPainter<GestprojectReceivedBillModel>();
//         entityPainter.PaintEntityListOnDataTable(
//            ProcessedGestprojectEntities,
//            dataTable,
//            tableSchemaProvider.ColumnsTuplesList
//         );
//      }
//   }
//}
