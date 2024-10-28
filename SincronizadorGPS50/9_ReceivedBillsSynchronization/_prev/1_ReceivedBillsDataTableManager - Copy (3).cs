//using Infragistics.Designers.SqlEditor;
//using Infragistics.Win.UltraWinGrid;
//using Infragistics.Win.UltraWinSchedule;
//using sage.ew.articulo;
//using sage.ew.cliente;
//using sage.ew.contabilidad;
//using sage.ew.db;
//using sage.ew.docscompra;
//using sage.ew.docscompra.Forms;
//using sage.ew.docscompra.UserControls;
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
//using System.Linq;
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
//            GestprojectEntities = new List<GestprojectReceivedBillModel>();
//            Sage50Entities = new List<Sage50ReceivedBillModel>();

//            ManageReceivedBillsSynchronizationTableStatus(TableSchema);
//            ManageReceivedBillsDetailsSynchronizationTableStatus(new ReceivedBillsDetailsSynchronizationTableSchemaProvider());

//            //GetAndStoreGestprojectEntities();
//            ////GetAndStoreSage50Entities();
//            ////ProccessAndStoreGestprojectEntities();
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

//      public void ManageReceivedBillsSynchronizationTableStatus(ISynchronizationTableSchemaProvider tableSchema)
//      {
//         try
//         {
//            ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

//            bool tableExists = entitySyncronizationTableStatusManager.TableExists(
//               GestprojectConnectionManager.GestprojectSqlConnection,
//               tableSchema.TableName
//            );

//            if(tableExists == false)
//            {
//               entitySyncronizationTableStatusManager.CreateTable
//               (
//                  GestprojectConnectionManager.GestprojectSqlConnection,
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

//      public void ManageReceivedBillsDetailsSynchronizationTableStatus(ISynchronizationTableSchemaProvider tableSchema)
//      {         
//         try
//         {
//            ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

//            bool tableExists = entitySyncronizationTableStatusManager.TableExists(
//               GestprojectConnectionManager.GestprojectSqlConnection,
//               tableSchema.TableName
//            );

//            if(tableExists == false)
//            {
//               entitySyncronizationTableStatusManager.CreateTable
//               (
//                  GestprojectConnectionManager.GestprojectSqlConnection,
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

//      public void GetAndStoreGestprojectEntities()
//      {
//         try
//         {
//            List<SageReceivedInvoiceModel> receivedInvoiceCodesAndProvidersCodes = GetSageReceivedInvoiceCodesAndProvidersCodes();

//            //new VisualizePropertiesAndValues<SageReceivedInvoiceModel>(
//            //  MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //  "receivedInvoiceCodesAndProvidersCodes",
//            //  receivedInvoiceCodesAndProvidersCodes
//            //);

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

//      public List<SageReceivedInvoiceModel> GetSageReceivedInvoiceCodesAndProvidersCodes()
//      {
//         try
//         {
//            List<SageReceivedInvoiceModel> receivedInvoiceCodesAndProvidersCodes = new List<SageReceivedInvoiceModel>();

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

//                  SageReceivedInvoiceModel entity = new SageReceivedInvoiceModel();

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
//         SageReceivedInvoiceModel entity
//      )
//      {
//         try
//         {
//            //////////////////////////////////
//            // Use the instance that created the connection
//            //////////////////////////////////

//            ConnectionActions.Sage50ConnectionManager._LoadGlobalVariables();
//            ConnectionActions.Sage50ConnectionManager._LoadEnvironmentCompany();

//            ewDocCompraFACTURA purchaseInvoice = new ewDocCompraFACTURA();
//            purchaseInvoice._Load(
//               entity.CompanyNumber,
//               entity.Number,
//               entity.ProviderCode
//            );

//            //////////////////////////////////
//            // Generate the received invoice model to be synchronized (or transferred)
//            //////////////////////////////////

//            SincronizadorGP50ReceivedInvoiceModel gestprojectReceivedBillModel = new SincronizadorGP50ReceivedInvoiceModel();
//            //gestprojectReceivedBillModel.FCP_ID = ;
            
//            sage.ew.cliente.Obra obra = new sage.ew.cliente.Obra();
//            obra._Codigo = purchaseInvoice._Cabecera._Obra;
//            obra._Load();

//            //gestprojectReceivedBillModel.PAR_DAO_ID = GetSynchronizationTableProjectIdByGuidId(obra._Guid_Id); /////////!!!!!!!!!!!!ERROR esta es la empresa, creo que ID de empresa
//            gestprojectReceivedBillModel.FCP_NUM_FACTURA = purchaseInvoice._Cabecera._Factura;
//            gestprojectReceivedBillModel.FCP_FECHA = purchaseInvoice._Cabecera._FechaFac;
//            //gestprojectReceivedBillModel.PAR_PRO_ID =  ; purchaseInvoice._Cabecera._Proveedor -> Obtener PAR_ID de la tabla de sincronización de proveedores usando éste valor (subctacontable)
//            gestprojectReceivedBillModel.FCP_SUBCTA_CONTABLE = purchaseInvoice._Lineas.FirstOrDefault()._Cuenta;
//            gestprojectReceivedBillModel.FCP_BASE_IMPONIBLE = purchaseInvoice._Pie._TotalBase;
//            //gestprojectReceivedBillModel.FCP_VALOR_IVA = ; // De la consulta a la tabla c_factucom, en el campo IVA, sumar los valores del campo "_ImpIva" de todos los elementos
//            //gestprojectReceivedBillModel.FCP_IVA = ; // De la consulta a la tabla c_factucom, en el campo IVA, seleccionar el campo "_PrcIva" del primer objeto del array
//            gestprojectReceivedBillModel.FCP_VALOR_IRPF = purchaseInvoice._Pie._RetencionDoc;
//            gestprojectReceivedBillModel.FCP_IRPF = purchaseInvoice._Pie._RetencionDocPorcen;
//            gestprojectReceivedBillModel.FCP_TOTAL_FACTURA = purchaseInvoice._Pie._TotalDocumento;
//            gestprojectReceivedBillModel.FCP_OBSERVACIONES = purchaseInvoice._Cabecera._Observacio;
//            //gestprojectReceivedBillModel.PROYECTO = purchaseInvoice._Cabecera._Obra; ?? Name or id
//            //gestprojectReceivedBillModel.TIPO = ; // Excluded in this version
//            gestprojectReceivedBillModel.S50_GUID_ID = entity.GuidId;

//            //////////////////////////////////
//            // Load the invoice's provider
//            //////////////////////////////////

//            sage.ew.docscompra.Proveedor currentReceivedInvoiceProvider = new Proveedor();
//            currentReceivedInvoiceProvider._Codigo = entity.ProviderCode;
//            currentReceivedInvoiceProvider._Load();

//            //new VisualizePropertiesAndValues<ewDocCompraFACTURA>(
//            //  MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //  "purchaseInvoice",
//            //  purchaseInvoice
//            //);

//            //new VisualizePropertiesAndValues<ewDocCompraCabFACTURA>(
//            //  MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //  "purchaseInvoice._Cabecera",
//            //  purchaseInvoice._Cabecera
//            //);

//            //new VisualizePropertiesAndValues<ewDocCompraPieFACTURA>(
//            //  MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //  "purchaseInvoice._Pie",
//            //  purchaseInvoice._Pie
//            //);

//            //new VisualizePropertiesAndValues<sage.ew.docscompra.Proveedor>(
//            //  MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //  "currentReceivedInvoiceProvider",
//            //  currentReceivedInvoiceProvider
//            //);

//            //new VisualizePropertiesAndValues<SageReceivedInvoiceModel>(
//            //  MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //  "entity",
//            //  entity
//            //);

//            List<SincronizadorGPS50ReceivedInvoiceDetailModel> invoiceDetailList = new List<SincronizadorGPS50ReceivedInvoiceDetailModel>();

//            foreach(sage.ew.docscompra.ewDocCompraLinFACTURA invoiceDetail in purchaseInvoice._Lineas)
//            {
//               SincronizadorGPS50ReceivedInvoiceDetailModel gestprojectEntityDetail = new SincronizadorGPS50ReceivedInvoiceDetailModel();

//               //gestprojectEntityDetail.FCP_ID = ; // Obtener el id de la factura en GP
//               gestprojectEntityDetail.DFP_CONCEPTO = invoiceDetail._Definicion.ToString().Trim();
//               gestprojectEntityDetail.DFP_PRECIO_UNIDAD = invoiceDetail._Precio;
//               gestprojectEntityDetail.DFP_UNIDADES = invoiceDetail._Unidades;
//               //gestprojectEntityDetail.DFP_SUBTOTAL = invoiceDetail.;
//               if(string.IsNullOrWhiteSpace(purchaseInvoice._Cabecera._Obra) == true)
//               {
//                  gestprojectEntityDetail.PRY_ID = null;
//                  gestprojectEntityDetail.DFP_ESTRUCTURAL = "1";
//               }
//               else
//               {
//                  //gestprojectEntityDetail.PRY_ID = null; // id de obra (obtener obra (cabecera de factura recibida -> codigoDeObra -> id de Gestproject de obra en la tabla de sincronizacion) -> obtener id de obra)
//                  gestprojectEntityDetail.DFP_ESTRUCTURAL = "0";
//               };                  

//               string sqlString2 = $@"
//                  SELECT
//                     guid_id
//                  FROM 
//                     {DB.SQLDatabase("gestion","d_albcom")}
//                  WHERE
//                     empresa='{invoiceDetail._Empresa}'
//                  AND
//                     definicion='{invoiceDetail._Definicion}'
//               ;";

//               DataTable entiesDataTable2 = new DataTable();
//               DB.SQLExec(sqlString2, ref entiesDataTable2);

//               gestprojectEntityDetail.S50_CODE = "";
//               gestprojectEntityDetail.S50_GUID_ID = entiesDataTable2.Rows[0].ItemArray[0].ToString().Trim();

//               //new VisualizePropertiesAndValues<GestprojectReceivedInvoiceDetailModel>(
//               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               //   "gestprojectEntityDetail",
//               //   gestprojectEntityDetail
//               //);

//               invoiceDetailList.Add(gestprojectEntityDetail);
//            };

//            entity.Details = invoiceDetailList;
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

//      public List<GestprojectReceivedBillModel> GenerateGestprojectEntitiesFromReceivedInvoiceModels(List<SageReceivedInvoiceModel> receivedInvoiceCodesAndProvidersCodes)
//      {
//         try
//         {
//            List<GestprojectReceivedBillModel> gestprojectEntities = new List<GestprojectReceivedBillModel>();

//            foreach(SageReceivedInvoiceModel item in receivedInvoiceCodesAndProvidersCodes)
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
//         try
//         {
//            IDataTableGenerator entityDataTableGenerator = new SyncrhonizationDataTableGenerator();
//            DataTable = entityDataTableGenerator.CreateDataTable(TableSchema.ColumnsTuplesList);
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

//      public void PaintEntitiesOnDataSource()
//      {         
//         try
//         {
//            if(GestprojectEntities.Count > 0)
//            {
//               ISynchronizableEntityPainter<GestprojectReceivedBillModel> entityPainter = new EntityPainter<GestprojectReceivedBillModel>();
//               entityPainter.PaintEntityListOnDataTable(
//                  GestprojectEntities,
//                  DataTable,
//                  TableSchema.ColumnsTuplesList
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
