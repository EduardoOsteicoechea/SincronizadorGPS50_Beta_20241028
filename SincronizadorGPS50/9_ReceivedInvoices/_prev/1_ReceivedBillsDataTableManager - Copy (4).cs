//using sage.ew.db;
//using sage.ew.docscompra;
//using SincronizadorGPS50.Sage50Connector;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using Newtonsoft.Json;

//namespace SincronizadorGPS50
//{
//   public class ReceivedBillsDataTableManager : IGridDataSourceGenerator<GestprojectReceivedBillModel, Sage50ReceivedBillModel>
//   {
//      public List<GestprojectReceivedBillModel> GestprojectEntities { get; set; }
//      public List<Sage50ReceivedBillModel> Sage50Entities { get; set; }
//      public List<SageReceivedInvoiceEnrichedModel> SageReceivedInvoicesEnrichedModels { get; set; }
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

//            GetAllSage50Data();
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

//      public void GetAllSage50Data()
//      {
//         try
//         {
//            SageReceivedInvoicesEnrichedModels = new List<SageReceivedInvoiceEnrichedModel>();
//            List<SageReceivedInvoiceModel> sageReceivedInvoicesBaseModels = GetSageReceivedBillsBaseModels();

//            foreach(SageReceivedInvoiceModel baseReceivedInvoiceModel in sageReceivedInvoicesBaseModels)
//            {
//               List<SageReceivedInvoiceBaseModelTaxModel> sageReceivedInvoiceBaseModelTaxModelList = ExtractSageReceivedBillBaseModelTaxModel(baseReceivedInvoiceModel);

//               ewDocCompraFACTURA sageReceivedInvoice = GetSageReceivedBill(baseReceivedInvoiceModel);

//               SincronizadorGP50CompanyModel sageReceivedBillCompany = GetReceivedBillCompanySynchronizationRegistry(baseReceivedInvoiceModel);

//               GestprojectProviderModel sageReceivedBillProvider = GetReceivedBillProviderSynchronizationRegistry(baseReceivedInvoiceModel);

//               GestprojectProjectModel sageReceivedBillProject = GetReceivedBillProjectSynchronizationRegistry(sageReceivedInvoice);

//               //new VisualizePropertiesAndValues<SageReceivedInvoiceModel>(
//               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               //   "baseReceivedInvoiceModel",
//               //   baseReceivedInvoiceModel
//               //);

//               //new VisualizePropertiesAndValues<SageReceivedInvoiceBaseModelTaxModel>(
//               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               //   "sageReceivedInvoiceBaseModelTaxModelList",
//               //   sageReceivedInvoiceBaseModelTaxModelList
//               //);

//               //new VisualizePropertiesAndValues<ewDocCompraFACTURA>(
//               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               //   "sageReceivedInvoice",
//               //   sageReceivedInvoice
//               //);

//               //new VisualizePropertiesAndValues<SincronizadorGP50CompanyModel>(
//               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               //   "sageReceivedBillCompany",
//               //   sageReceivedBillCompany
//               //);

//               //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               //   "sageReceivedBillProvider",
//               //   sageReceivedBillProvider
//               //);

//               //new VisualizePropertiesAndValues<GestprojectProjectModel>(
//               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               //   "sageReceivedBillProject",
//               //   sageReceivedBillProject
//               //);

//               SageReceivedInvoiceEnrichedModel sageReceivedBillsEnrichedModel = GenerateSageReceivedBillEnrichedModel(
//                  baseReceivedInvoiceModel,
//                  sageReceivedInvoiceBaseModelTaxModelList,
//                  sageReceivedInvoice,
//                  sageReceivedBillCompany,
//                  sageReceivedBillProvider,
//                  sageReceivedBillProject
//               );
//               SageReceivedInvoicesEnrichedModels.Add(sageReceivedBillsEnrichedModel);
//            };

//            new VisualizePropertiesAndValues<SageReceivedInvoiceEnrichedModel>(
//               MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               "SageReceivedInvoicesEnrichedModels",
//               SageReceivedInvoicesEnrichedModels
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


//      public List<SageReceivedInvoiceModel> GetSageReceivedBillsBaseModels()
//      {
//         try
//         {
//            List<SageReceivedInvoiceModel> entities = new List<SageReceivedInvoiceModel>();

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
//                  entities.Add(entity);
//               };
//            };

//            return entities;
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
//      public List<SageReceivedInvoiceBaseModelTaxModel> ExtractSageReceivedBillBaseModelTaxModel
//      (
//         SageReceivedInvoiceModel sage50ReceivedInvoiceModel
//      )
//      {
//         try
//         {
//            List<SageReceivedInvoiceBaseModelTaxModel> entities = JsonConvert.DeserializeObject<List<SageReceivedInvoiceBaseModelTaxModel>>(sage50ReceivedInvoiceModel.IvaObject);

//            //new VisualizePropertiesAndValues<SageReceivedInvoiceBaseModelTaxModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "List<SageReceivedInvoiceBaseModelTaxModel>",
//            //   entities
//            //);

//            return entities;
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

//      public ewDocCompraFACTURA GetSageReceivedBill
//      (
//         SageReceivedInvoiceModel sage50ReceivedInvoiceModel
//      )
//      {
//         try
//         {
//            ConnectionActions.Sage50ConnectionManager._LoadGlobalVariables();
//            ConnectionActions.Sage50ConnectionManager._LoadEnvironmentCompany();

//            ewDocCompraFACTURA entity = new ewDocCompraFACTURA();

//            entity._Load(
//               sage50ReceivedInvoiceModel.CompanyNumber,
//               sage50ReceivedInvoiceModel.Number,
//               sage50ReceivedInvoiceModel.ProviderCode
//            );

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
//         };
//      }

//      public SincronizadorGP50CompanyModel GetReceivedBillCompanySynchronizationRegistry
//      (
//         SageReceivedInvoiceModel sage50ReceivedInvoiceModel
//      )
//      {
//         try
//         {
//            Connection.Open();
            
//            SincronizadorGP50CompanyModel entity = new SincronizadorGP50CompanyModel();

//            string sqlString = $@"
//               SELECT
//                  PAR_ID
//                  ,PAR_NOMBRE
//                  ,PAR_CIF_NIF
//                  ,SageCompanyNumber
//                  ,S50_GUID_ID
//               FROM
//                  INT_SAGE_SINC_EMPRESAS
//               WHERE
//                  SageCompanyNumber=@SageCompanyNumber
//            ";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               command.Parameters.AddWithValue("@SageCompanyNumber",sage50ReceivedInvoiceModel.CompanyNumber);

//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     entity.PAR_ID = reader["PAR_ID"] as int?;
//                     entity.PAR_NOMBRE = reader["PAR_NOMBRE"] as string;
//                     entity.PAR_CIF_NIF = reader["PAR_CIF_NIF"] as string;
//                     entity.SageCompanyNumber = reader["SageCompanyNumber"] as string;
//                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
//                  };
//               };
//            };

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

//      public GestprojectProviderModel GetReceivedBillProviderSynchronizationRegistry
//      (
//         SageReceivedInvoiceModel sage50ReceivedInvoiceModel
//      )
//      {
//         try
//         {
//            Connection.Open();
            
//            GestprojectProviderModel entity = new GestprojectProviderModel();

//            string sqlString = $@"
//               SELECT
//                  PAR_ID
//                  ,S50_GUID_ID
//               FROM
//                  INT_SAGE_SINC_PROVEEDORES
//               WHERE
//                  PAR_SUBCTA_CONTABLE_2=@PAR_SUBCTA_CONTABLE_2
//            ";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE_2",sage50ReceivedInvoiceModel.ProviderCode);

//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     entity.PAR_ID = reader["PAR_ID"] as int?;
//                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
//                  };
//               };
//            };

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

//      public GestprojectProjectModel GetReceivedBillProjectSynchronizationRegistry
//      (
//         ewDocCompraFACTURA sageReceivedInvoiceModel
//      )
//      {
//         try
//         {
//            Connection.Open();
            
//            GestprojectProjectModel entity = new GestprojectProjectModel();
            
//            sage.ew.cliente.Obra InvocesProject = new sage.ew.cliente.Obra();
//            InvocesProject._Codigo = sageReceivedInvoiceModel._Cabecera._Obra;
//            InvocesProject._Load();

//            string sqlString = $@"
//               SELECT
//                  PRY_ID
//                  ,S50_GUID_ID
//               FROM
//                  INT_SAGE_SINC_PROYECTOS
//               WHERE
//                  PRY_NOMBRE=@PRY_NOMBRE
//            ";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               command.Parameters.AddWithValue("@PRY_NOMBRE",InvocesProject._Nombre.Trim());

//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     entity.PRY_ID = reader["PRY_ID"] as int?;
//                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
//                  };
//               };
//            };

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

//      public SageReceivedInvoiceEnrichedModel GenerateSageReceivedBillEnrichedModel
//      (
//         SageReceivedInvoiceModel baseReceivedInvoiceModel,
//         List<SageReceivedInvoiceBaseModelTaxModel> sageReceivedInvoiceBaseModelTaxModelList,
//         ewDocCompraFACTURA sageReceivedInvoice,
//         SincronizadorGP50CompanyModel sageReceivedBillCompany,
//         GestprojectProviderModel sageReceivedBillProvider,
//         GestprojectProjectModel sageReceivedBillProject
//      )
//      {
//         try
//         {
//            SageReceivedInvoiceEnrichedModel entity = new SageReceivedInvoiceEnrichedModel();

//            entity.BaseReceivedInvoiceModel = baseReceivedInvoiceModel;
//            entity.SageReceivedInvoiceBaseModelTaxModelList = sageReceivedInvoiceBaseModelTaxModelList;
//            entity.SageReceivedInvoice = sageReceivedInvoice;
//            entity.SageReceivedInvoiceCompany = sageReceivedBillCompany;
//            entity.SageReceivedInvoiceProvider = sageReceivedBillProvider;
//            entity.SageReceivedInvoiceProject = sageReceivedBillProject;

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
