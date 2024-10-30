//using Infragistics.Win.UltraWinGrid;
//using Infragistics.Win.UltraWinSchedule;
//using sage.ew.articulo;
//using sage.ew.contabilidad;
//using sage.ew.db;
//using sage.ew.docscompra;
//using sage.ew.empresa;
//using sage.ew.interficies;
//using sage.ew.listados.Listados;
//using sage.ew.txtbox.UserControls;
//using sage.ew.usuario;
//using SincronizadorGPS50.Sage50Connector;
//using SincronizadorGPS50.Workflows.Sage50Connection;
//using System.Collections.Generic;
//using System.Data;
//using System.Reflection;
//using System.Text;
//using System.Windows.Forms;
//using static sage.ew.articulo.Articulo.Precios;

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
//         try
//         {
//             List<ReceivedInvoiceModel> receivedInvoiceCodesAndProvidersCodes = GetSageReceivedInvoiceCodesAndProvidersCodes();

//             new VisualizePropertiesAndValues<ReceivedInvoiceModel>(
//               MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               "receivedInvoiceCodesAndProvidersCodes",
//               receivedInvoiceCodesAndProvidersCodes
//             );            
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
//               {DB.SQLDatabase("gestion","c_factucom")}
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
//                  entity.Number = row.ItemArray[1].ToString().Trim();
//                  entity.ProviderCode = row.ItemArray[2].ToString().Trim();
//                  entity.GuidId = row.ItemArray[3].ToString().Trim();
//                  entity.IvaObject = row.ItemArray[4].ToString().Trim();
//                  GetAndAddEntityDetails(entity);
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

//      public void GetAndAddEntityDetails
//      (
//         ReceivedInvoiceModel entity
//      )
//      {
//         try
//         {
//            // Use the instance that created the connection

//            ConnectionActions.Sage50ConnectionManager._LoadGlobalVariables();
//            ConnectionActions.Sage50ConnectionManager._LoadEnvironmentCompany();

//            ewDocCompraFACTURA loFraComp = new ewDocCompraFACTURA();
//            if(
//               loFraComp._Existe(
//               entity.CompanyNumber,  
//               entity.Number, 
//               entity.ProviderCode
//               )
//            )
//            {
//               loFraComp._Load(
//               entity.CompanyNumber,  
//               entity.Number, 
//               entity.ProviderCode
//               );

//               List<ReceivedInvoiceDetailModel> invoiceDetailList = new List<ReceivedInvoiceDetailModel>();

//               if(!string.IsNullOrWhiteSpace(loFraComp._Cabecera._Obra))
//               {
//                  foreach(sage.ew.docscompra.ewDocCompraLinFACTURA invoiceDetail in loFraComp._Lineas)
//                  {
//                     ReceivedInvoiceDetailModel entityDetail = new ReceivedInvoiceDetailModel();
//                     //entityDetail.User = invoiceDetail.;
//                     entityDetail.CompanyNumber = invoiceDetail._Empresa.Trim();
//                     entityDetail.InvoiceNumber = invoiceDetail._Doc_Num.Trim();
//                     entityDetail.Article = invoiceDetail._Articulo.Trim();
//                     entityDetail.Definition = invoiceDetail._Definicion.Trim();
//                     entityDetail.Units = invoiceDetail._Unidades;
//                     entityDetail.Price = invoiceDetail._Precio;
//                     entityDetail.Discount1 = invoiceDetail._Dto1;
//                     entityDetail.Discount2 = invoiceDetail._Dto2;
//                     entityDetail.Import = invoiceDetail._Importe;
//                     entityDetail.IvaType = invoiceDetail._TipoIva;
//                     entityDetail.Cost = invoiceDetail._Coste;
//                     entityDetail.Account = invoiceDetail._Cuenta.Trim();
//                     entityDetail.Date = invoiceDetail._Fecha;
//                     entityDetail.LineNumber = invoiceDetail._Doc_Lin;
//                     entityDetail.ProviderCode = invoiceDetail._Proveedor.Trim();
//                     entityDetail.CurrencyPrice = invoiceDetail._PrecioDivisa;
//                     entityDetail.CurrencyImport = invoiceDetail._ImporteDivisa;

//                     string sqlString2 = $@"
//                     SELECT
//                        guid_id
//                     FROM 
//                        {DB.SQLDatabase("gestion","c_factucom")}
//                     WHERE
//                        numero='{entityDetail.InvoiceNumber}',
//                        definicion='{entityDetail.Definition}'
//                     ;";

//                     DataTable entiesDataTable2 = new DataTable();
//                     DB.SQLExec(sqlString2, ref entiesDataTable2);
                           
//                     entityDetail.GuidId = entiesDataTable2.Rows[0].ToString().Trim();

//                     invoiceDetailList.Add(entityDetail);
//                  }
//               };

//               entity.Details = invoiceDetailList;

//               new VisualizePropertiesAndValues<ReceivedInvoiceDetailModel>(
//                  MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//                  "invoiceDetailList",
//                  invoiceDetailList
//               );
//            }
//            else
//            {
//               MessageBox.Show($@"
//                  The entity wasn't found
//                     entity.CompanyNumber: {entity.CompanyNumber}  
//                     entity.Number: {entity.Number}
//                     entity.ProviderCode: {entity.ProviderCode}
//               ");
//            }
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
//         ISynchronizableEntityPainter<GestprojectReceivedBillModel> entityPainter = new EntityPainter<GestprojectReceivedBillModel>();
//         entityPainter.PaintEntityListOnDataTable(
//            ProcessedGestprojectEntities,
//            dataTable,
//            tableSchemaProvider.ColumnsTuplesList
//         );
//      }
//   }
//}
