//using Infragistics.Designers.SqlEditor;
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
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Reflection;
//using System.Text;
//using System.Windows.Forms;
//using static sage.ew.articulo.Articulo.Precios;
//using static sage.ew.docsven.FirmaElectronica;

//namespace SincronizadorGPS50
//{
//   public class ReceivedBillsDataTableManager : IGridDataSourceGenerator<GestprojectReceivedBillModel, Sage50ReceivedBillModel>
//   {
//      public List<GestprojectReceivedBillModel> GestprojectEntities { get; set; }
//      public List<Sage50ReceivedBillModel> Sage50Entities { get; set; }
//      public List<GestprojectReceivedBillModel> ProcessedGestprojectEntities { get; set; }
//      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
//      public SqlConnection Connection { get; set; }
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

//            ManageSynchronizationTableStatus();
//            GetAndStoreGestprojectEntities();
//            //GetAndStoreSage50Entities();
//            //ProccessAndStoreGestprojectEntities();
//            CreateAndDefineDataSource();
//            PaintEntitiesOnDataSource();

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

//      public void ManageSynchronizationTableStatus()
//      {
//         ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

//         bool tableExists = entitySyncronizationTableStatusManager.TableExists(
//            GestprojectConnectionManager.GestprojectSqlConnection,
//            TableSchema.TableName
//         );

//         if(tableExists == false)
//         {
//            entitySyncronizationTableStatusManager.CreateTable
//            (
//               GestprojectConnectionManager.GestprojectSqlConnection,
//               TableSchema
//            );
//         };
//      }

//      public void GetAndStoreGestprojectEntities()
//      {
//         try
//         {
//            List<ReceivedInvoiceModel> receivedInvoiceCodesAndProvidersCodes = GetSageReceivedInvoiceCodesAndProvidersCodes();

//            new VisualizePropertiesAndValues<ReceivedInvoiceModel>(
//              MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//              "receivedInvoiceCodesAndProvidersCodes",
//              receivedInvoiceCodesAndProvidersCodes
//            );

//            GestprojectEntities = GenerateGestprojectEntitiesFromReceivedInvoiceModels(receivedInvoiceCodesAndProvidersCodes);
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

//            //ewDocCompraALBARAN albaranDeCompra = new ewDocCompraALBARAN();
//            //ewDocCompraCabALBARAN cabeceraAlbaran = new ewDocCompraCabALBARAN();

//            //cabeceraAlbaran.

//            //albaranDeCompra._Cabecera._Load
//            ////albaranDeCompra.obra

//            //sage.ew.cliente.Obra obra = new sage.ew.cliente.Obra();
//            //obra.al

//            //ewDocCompraCabFACTURA cabeceraFactura = new ewDocCompraCabFACTURA();
//            ////cabeceraFactura.prov

//            //ew



//            ewDocCompraFACTURA purchaseInvoice = new ewDocCompraFACTURA();
//            //if(
//            //   loFraComp._Existe(
//            //   entity.CompanyNumber,  
//            //   entity.Number, 
//            //   entity.ProviderCode
//            //   )
//            //)
//            //{
//            purchaseInvoice._Load(
//               entity.CompanyNumber,
//               entity.Number,
//               entity.ProviderCode
//            );

//            new VisualizePropertiesAndValues<ewDocCompraCabFACTURA>(
//              MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//              "purchaseInvoice._Cabecera",
//              purchaseInvoice._Cabecera
//            );

//            new VisualizePropertiesAndValues<ewDocCompraPieFACTURA>(
//              MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//              "purchaseInvoice._Pie",
//              purchaseInvoice._Pie
//            );

//            //sage.ew.docscompra.Proveedor currentReceivedInvoiceProvider = new Proveedor();
//            //currentReceivedInvoiceProvider._Codigo = entity.ProviderCode;
//            //currentReceivedInvoiceProvider._Load();
//            //string providerGuidId = currentReceivedInvoiceProvider._Guid_Id;

//            List<ReceivedInvoiceDetailModel> invoiceDetailList = new List<ReceivedInvoiceDetailModel>();

//            if(!string.IsNullOrWhiteSpace(purchaseInvoice._Cabecera._Obra))
//            {
//               foreach(sage.ew.docscompra.ewDocCompraLinFACTURA invoiceDetail in purchaseInvoice._Lineas)
//               {
//                  ReceivedInvoiceDetailModel entityDetail = new ReceivedInvoiceDetailModel();

//                  //entityDetail.User = invoiceDetail.;
//                  entityDetail.CompanyNumber = invoiceDetail._Empresa.Trim();
//                  entityDetail.InvoiceNumber = invoiceDetail._Numero.Trim();
//                  entityDetail.Article = invoiceDetail._Articulo.Trim();
//                  entityDetail.Definition = invoiceDetail._Definicion.Trim();
//                  entityDetail.Units = invoiceDetail._Unidades;
//                  entityDetail.Price = invoiceDetail._Precio;
//                  entityDetail.Discount1 = invoiceDetail._Dto1;
//                  entityDetail.Discount2 = invoiceDetail._Dto2;
//                  entityDetail.Import = invoiceDetail._Importe;
//                  entityDetail.IvaType = invoiceDetail._TipoIva;
//                  entityDetail.Cost = invoiceDetail._Coste;
//                  entityDetail.Account = invoiceDetail._Cuenta.Trim();
//                  entityDetail.Date = invoiceDetail._Fecha;
//                  entityDetail.LineNumber = invoiceDetail._Linea;
//                  entityDetail.ProviderCode = invoiceDetail._Proveedor.Trim();
//                  entityDetail.CurrencyPrice = invoiceDetail._PrecioDivisa;
//                  entityDetail.CurrencyImport = invoiceDetail._ImporteDivisa;

                  
//               // DFP_ESTRUCTURAL = 0 (falso, porque sí tiene proyecto)
//               // PRY_ID = id de obra (obtener obra (cabecera de factura recibida -> codigoDeObra -> id de Gestproject de obra en la tabla de sincronizacion) -> obtener id de obra)

//                  string sqlString2 = $@"
//                     SELECT
//                        guid_id
//                     FROM 
//                        {DB.SQLDatabase("gestion","d_albcom")}
//                     WHERE
//                        empresa='{entityDetail.CompanyNumber}'
//                     AND
//                        definicion='{entityDetail.Definition}'
//                  ;";

//                  DataTable entiesDataTable2 = new DataTable();
//                  DB.SQLExec(sqlString2, ref entiesDataTable2);

//                  entityDetail.GuidId = entiesDataTable2.Rows[0].ItemArray[0].ToString().Trim();

//                  new VisualizePropertiesAndValues<ReceivedInvoiceDetailModel>(
//                     MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//                     "entityDetail",
//                     entityDetail
//                  );

//                  invoiceDetailList.Add(entityDetail);
//               }
//            }
//            else
//            {
//               // DFP_ESTRUCTURAL = 1 (verdadero, porque no tiene proyecto)
//               // PRY_ID = null
//            };

//            entity.Details = invoiceDetailList;

//            //new VisualizePropertiesAndValues<ReceivedInvoiceDetailModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "invoiceDetailList",
//            //   invoiceDetailList
//            //);
//            //}
//            //else
//            //{
//            //   MessageBox.Show($@"
//            //      The entity wasn't found
//            //         entity.CompanyNumber: {entity.CompanyNumber}  
//            //         entity.Number: {entity.Number}
//            //         entity.ProviderCode: {entity.ProviderCode}
//            //   ");
//            //}
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

//      public List<GestprojectReceivedBillModel> GenerateGestprojectEntitiesFromReceivedInvoiceModels(List<ReceivedInvoiceModel> receivedInvoiceCodesAndProvidersCodes)
//      {
//         try
//         {
//            List<GestprojectReceivedBillModel> gestprojectEntities = new List<GestprojectReceivedBillModel>();

//            foreach(ReceivedInvoiceModel item in receivedInvoiceCodesAndProvidersCodes)
//            {
//               GetReceivedInvoiceModelProvider(item.ProviderCode);

//               //   GestprojectReceivedBillModel entity = new GestprojectReceivedBillModel();

//               //   entity.FCP_ID = -1;
//               //   entity.PAR_DAO_ID = -1;
//               //   entity.FCP_NUM_FACTURA = item.Number;
//               //   entity.FCP_FECHA = item.;
//               //   entity.PAR_PRO_ID = -1;
//               //   entity.FCP_BASE_IMPONIBLE = item.;
//               //   entity.FCP_VALOR_IVA = item.;
//               //   entity.FCP_IVA = item.;
//               //   entity.FCP_VALOR_IRPF = item.;
//               //   entity.FCP_IRPF = item.;
//               //   entity.FCP_TOTAL_FACTURA = item.tota;
//               //   entity.FCP_OBSERVACIONES = "";
//               //   entity.PROYECTO = item.;
//               //   entity.TIPO = item.;

//               //   gestprojectEntities.Add(entity);
//            }

//            return gestprojectEntities;
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

      
//      public GestprojectProviderModel GetReceivedInvoiceModelProvider(string providerCode)
//      {
//         try
//         {
//            Connection.Open();

//            GestprojectProviderModel entity = new GestprojectProviderModel();

//            //string sqlString = "";
//            //sqlString = "SELECT" + " ";
//            // FCP_ID
//            // ,PAR_DAO_ID
//            // ,FCP_NUM_FACTURA
//            // ,FCP_FECHA
//            // ,PAR_PRO_ID
//            // ,FCP_BASE_IMPONIBLE
//            // ,FCP_VALOR_IVA
//            // ,FCP_IVA
//            // ,FCP_VALOR_IRPF
//            // ,FCP_IRPF
//            // ,FCP_TOTAL_FACTURA
//            // ,FCP_OBSERVACIONES
//            // ,PROYECTO
//            // ,TIPO

//            return entity;
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//         finally
//         {
//            Connection.Close();
//         };
//      }





//      //public void GetAndStoreSage50Entities()
//      //{
//      //   Sage50Entities = new GetSage50ReceivedBills().Entities;
//      //}

//      public void ProccessAndStoreGestprojectEntities()
//      {
//         ISynchronizableEntityProcessor<GestprojectReceivedBillModel, Sage50ReceivedBillModel> gestprojectProvidersProcessor = new GestprojectReceivedBillsProcessor();
//         ProcessedGestprojectEntities = gestprojectProvidersProcessor.ProcessEntityList(
//            GestprojectConnectionManager.GestprojectSqlConnection,
//            SageConnectionManager,
//            TableSchema,
//            GestprojectEntities,
//            Sage50Entities
//         );
//      }

//      public void CreateAndDefineDataSource()
//      {
//         IDataTableGenerator entityDataTableGenerator = new SyncrhonizationDataTableGenerator();
//         DataTable = entityDataTableGenerator.CreateDataTable(TableSchema.ColumnsTuplesList);
//      }

//      public void PaintEntitiesOnDataSource()
//      {
//         ISynchronizableEntityPainter<GestprojectReceivedBillModel> entityPainter = new EntityPainter<GestprojectReceivedBillModel>();
//         entityPainter.PaintEntityListOnDataTable(
//            GestprojectEntities,
//            DataTable,
//            TableSchema.ColumnsTuplesList
//         );
//      }
//   }
//}
