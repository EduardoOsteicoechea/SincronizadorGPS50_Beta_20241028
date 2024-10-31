using sage.ew.db;
using sage.ew.docscompra;
using SincronizadorGPS50.Sage50Connector;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace SincronizadorGPS50
{
   public class ReceivedBillsDataTableManager : IGridDataSourceGenerator<GestprojectReceivedBillModel, Sage50ReceivedBillModel>
   {
      public List<GestprojectReceivedBillModel> GestprojectEntities { get; set; }
      public List<Sage50ReceivedBillModel> Sage50Entities { get; set; }
      public List<SageReceivedInvoiceEnrichedModel> SageReceivedInvoicesEnrichedModels { get; set; }
      public List<SincronizadorGP50ReceivedInvoiceModel> SynchronizadorGPS50ReceivedInvoices { get; set; }
      public List<SincronizadorGPS50ReceivedInvoiceDetailModel> SynchronizadorGPS50ReceivedInvoicesDetails { get; set; }
      public List<GestprojectReceivedBillModel> ProcessedGestprojectEntities { get; set; }
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public DataTable DataTable { get; set; }

      public System.Data.DataTable GenerateDataTable
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchemaProvider
      )
      {
         try
         {
            GestprojectConnectionManager = gestprojectConnectionManager;
            Connection = GestprojectConnectionManager.GestprojectSqlConnection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchemaProvider;
            GestprojectEntities = new List<GestprojectReceivedBillModel>();
            SynchronizadorGPS50ReceivedInvoices = new List<SincronizadorGP50ReceivedInvoiceModel>();
            SynchronizadorGPS50ReceivedInvoicesDetails = new List<SincronizadorGPS50ReceivedInvoiceDetailModel>();

            ManageReceivedBillsSynchronizationTableStatus(TableSchema);
            ManageReceivedBillsDetailsSynchronizationTableStatus(new ReceivedBillsDetailsSynchronizationTableSchemaProvider());
            GetSageData();
            StructureSynchronizationData();
            AppendMissingSynchronizationData();
            RegisterSynchronizationData();
            CreateAndDefineDataSource();
            PaintEntitiesOnDataSource(SynchronizadorGPS50ReceivedInvoices);

            return DataTable;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public void ManageReceivedBillsSynchronizationTableStatus(ISynchronizationTableSchemaProvider tableSchema)
      {
         try
         {
            ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

            bool tableExists = entitySyncronizationTableStatusManager.TableExists(
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchema.TableName
            );

            if(tableExists == false)
            {
               entitySyncronizationTableStatusManager.CreateTable
               (
                  GestprojectConnectionManager.GestprojectSqlConnection,
                  tableSchema
               );
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public void ManageReceivedBillsDetailsSynchronizationTableStatus(ISynchronizationTableSchemaProvider tableSchema)
      {         
         try
         {
            ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

            bool tableExists = entitySyncronizationTableStatusManager.TableExists(
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchema.TableName
            );

            if(tableExists == false)
            {
               entitySyncronizationTableStatusManager.CreateTable
               (
                  GestprojectConnectionManager.GestprojectSqlConnection,
                  tableSchema
               );
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public void GetSageData()
      {
         try
         {
            SageReceivedInvoicesEnrichedModels = new List<SageReceivedInvoiceEnrichedModel>();
            List<SageReceivedInvoiceModel> sageReceivedInvoicesBaseModels = GetSageReceivedBillsBaseModels();

            foreach(SageReceivedInvoiceModel baseReceivedInvoiceModel in sageReceivedInvoicesBaseModels)
            {
               List<SageReceivedInvoiceBaseModelTaxModel> sageReceivedInvoiceBaseModelTaxModelList = ExtractSageReceivedBillBaseModelTaxModel(baseReceivedInvoiceModel);

               ewDocCompraFACTURA sageReceivedInvoice = GetSageReceivedBill(baseReceivedInvoiceModel);

               SynchronizableCompanyModel sageReceivedBillCompany = GetReceivedBillCompanySynchronizationRegistry(baseReceivedInvoiceModel);

               GestprojectProviderModel sageReceivedBillProvider = GetReceivedBillProviderSynchronizationRegistry(baseReceivedInvoiceModel);

               GestprojectProjectModel sageReceivedBillProject = GetReceivedBillProjectSynchronizationRegistry(sageReceivedInvoice);

               SageReceivedInvoiceEnrichedModel sageReceivedBillsEnrichedModel = GenerateSageReceivedBillEnrichedModel(
                  baseReceivedInvoiceModel,
                  sageReceivedInvoiceBaseModelTaxModelList,
                  sageReceivedInvoice,
                  sageReceivedBillCompany,
                  sageReceivedBillProvider,
                  sageReceivedBillProject
               );

               SageReceivedInvoicesEnrichedModels.Add(sageReceivedBillsEnrichedModel);
            };

            //new VisualizePropertiesAndValues<SageReceivedInvoiceEnrichedModel>(
            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
            //   "SageReceivedInvoicesEnrichedModels",
            //   SageReceivedInvoicesEnrichedModels
            //);
         }
         catch(System.Exception exception) 
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }


      public List<SageReceivedInvoiceModel> GetSageReceivedBillsBaseModels()
      {
         try
         {
            List<SageReceivedInvoiceModel> entities = new List<SageReceivedInvoiceModel>();

            string sqlString = $@"
            SELECT 
               empresa
               ,numero
               ,proveedor
               ,guid_id
               ,iva
            FROM 
               {DB.SQLDatabase("gestion","c_factucom")}
            ;";

            DataTable enentiesDataTable = new DataTable();
            DB.SQLExec(sqlString, ref enentiesDataTable);

            if(enentiesDataTable.Rows.Count > 0)
            {
               for(int i = 0; i < enentiesDataTable.Rows.Count; i++)
               {
                  DataRow row = enentiesDataTable.Rows[i];

                  SageReceivedInvoiceModel entity = new SageReceivedInvoiceModel();

                  entity.CompanyNumber = row.ItemArray[0].ToString().Trim();
                  entity.Number = row.ItemArray[1].ToString().Trim();
                  entity.ProviderCode = row.ItemArray[2].ToString().Trim();
                  entity.GuidId = row.ItemArray[3].ToString().Trim();
                  entity.IvaObject = row.ItemArray[4].ToString().Trim();

                  entities.Add(entity);
               };
            };

            return entities;
         }
         catch(System.Exception exception) 
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
      public List<SageReceivedInvoiceBaseModelTaxModel> ExtractSageReceivedBillBaseModelTaxModel
      (
         SageReceivedInvoiceModel sage50ReceivedInvoiceModel
      )
      {
         try
         {
            List<SageReceivedInvoiceBaseModelTaxModel> entities = JsonConvert.DeserializeObject<List<SageReceivedInvoiceBaseModelTaxModel>>(sage50ReceivedInvoiceModel.IvaObject);

            //new VisualizePropertiesAndValues<SageReceivedInvoiceBaseModelTaxModel>(
            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
            //   "List<SageReceivedInvoiceBaseModelTaxModel>",
            //   entities
            //);

            return entities;
         }
         catch(System.Exception exception) 
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public ewDocCompraFACTURA GetSageReceivedBill
      (
         SageReceivedInvoiceModel sage50ReceivedInvoiceModel
      )
      {
         try
         {
            ConnectionActions.Sage50ConnectionManager._LoadGlobalVariables();
            ConnectionActions.Sage50ConnectionManager._LoadEnvironmentCompany();

            ewDocCompraFACTURA entity = new ewDocCompraFACTURA();

            entity._Load(
               sage50ReceivedInvoiceModel.CompanyNumber,
               sage50ReceivedInvoiceModel.Number,
               sage50ReceivedInvoiceModel.ProviderCode
            );

            return entity;
         }
         catch(System.Exception exception) 
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public SynchronizableCompanyModel GetReceivedBillCompanySynchronizationRegistry
      (
         SageReceivedInvoiceModel sage50ReceivedInvoiceModel
      )
      {
         try
         {
            Connection.Open();
            
            SynchronizableCompanyModel entity = new SynchronizableCompanyModel();

            string sqlString = $@"
               SELECT
                  PAR_ID
                  ,PAR_NOMBRE
                  ,PAR_CIF_NIF
                  ,SageCompanyNumber
                  ,S50_GUID_ID
               FROM
                  INT_SAGE_SINC_EMPRESAS
               WHERE
                  SageCompanyNumber=@SageCompanyNumber
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@SageCompanyNumber",sage50ReceivedInvoiceModel.CompanyNumber);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.PAR_ID = (reader["PAR_ID"] as int?) ?? -1;
                     entity.PAR_NOMBRE = (reader["PAR_NOMBRE"] as string) ?? "";
                     entity.PAR_CIF_NIF = (reader["PAR_CIF_NIF"] as string) ?? "";
                     entity.SageCompanyNumber = (reader["SageCompanyNumber"] as string) ?? "";
                     entity.S50_GUID_ID = (reader["S50_GUID_ID"] as string) ?? "";
                  };
               };
            };

            return entity;
         }
         catch(System.Exception exception) 
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public GestprojectProviderModel GetReceivedBillProviderSynchronizationRegistry
      (
         SageReceivedInvoiceModel sage50ReceivedInvoiceModel
      )
      {
         try
         {
            Connection.Open();
            
            GestprojectProviderModel entity = new GestprojectProviderModel();

            string sqlString = $@"
               SELECT
                  PAR_ID
                  ,S50_GUID_ID
               FROM
                  INT_SAGE_SINC_PROVEEDORES
               WHERE
                  PAR_SUBCTA_CONTABLE_2=@PAR_SUBCTA_CONTABLE_2
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE_2",sage50ReceivedInvoiceModel.ProviderCode);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.PAR_ID = (reader["PAR_ID"] as int?) ?? -1;
                     entity.S50_GUID_ID = (reader["S50_GUID_ID"] as string) ?? "";
                  };
               };
            };

            return entity;
         }
         catch(System.Exception exception) 
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public GestprojectProjectModel GetReceivedBillProjectSynchronizationRegistry
      (
         ewDocCompraFACTURA sageReceivedInvoiceModel
      )
      {
         try
         {
            Connection.Open();
            
            GestprojectProjectModel entity = new GestprojectProjectModel();
            
            sage.ew.cliente.Obra InvocesProject = new sage.ew.cliente.Obra();
            InvocesProject._Codigo = sageReceivedInvoiceModel._Cabecera._Obra;
            InvocesProject._Load();

            string sqlString = $@"
               SELECT
                  PRY_ID
                  ,S50_GUID_ID
               FROM
                  INT_SAGE_SINC_PROYECTOS
               WHERE
                  PRY_NOMBRE=@PRY_NOMBRE
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PRY_NOMBRE",InvocesProject._Nombre.Trim());

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.PRY_ID = (reader["PRY_ID"] as int?) ?? -1;
                     entity.S50_GUID_ID = (reader["S50_GUID_ID"] as string) ?? "";
                  };
               };
            };

            return entity;
         }
         catch(System.Exception exception) 
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public SageReceivedInvoiceEnrichedModel GenerateSageReceivedBillEnrichedModel
      (
         SageReceivedInvoiceModel baseReceivedInvoiceModel,
         List<SageReceivedInvoiceBaseModelTaxModel> sageReceivedInvoiceBaseModelTaxModelList,
         ewDocCompraFACTURA sageReceivedInvoice,
         SynchronizableCompanyModel sageReceivedBillCompany,
         GestprojectProviderModel sageReceivedBillProvider,
         GestprojectProjectModel sageReceivedBillProject
      )
      {
         try
         {
            SageReceivedInvoiceEnrichedModel entity = new SageReceivedInvoiceEnrichedModel();

            entity.BaseReceivedInvoiceModel = baseReceivedInvoiceModel;
            entity.SageReceivedInvoiceBaseModelTaxModelList = sageReceivedInvoiceBaseModelTaxModelList;
            entity.SageReceivedInvoice = sageReceivedInvoice;
            entity.SageReceivedInvoiceCompany = sageReceivedBillCompany;
            entity.SageReceivedInvoiceProvider = sageReceivedBillProvider;
            entity.SageReceivedInvoiceProject = sageReceivedBillProject;

            return entity;
         }
         catch(System.Exception exception) 
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public void StructureSynchronizationData()
      {
         try
         {
            foreach (SageReceivedInvoiceEnrichedModel sageEnrichedModel in SageReceivedInvoicesEnrichedModels)
            {
               SincronizadorGP50ReceivedInvoiceModel entity = new SincronizadorGP50ReceivedInvoiceModel(sageEnrichedModel, SageConnectionManager, GestprojectConnectionManager);

               SynchronizadorGPS50ReceivedInvoices.Add(entity);

               foreach (ewDocCompraLinFACTURA receivedInvoiceLine in sageEnrichedModel.SageReceivedInvoice._Lineas)
               {
                  SincronizadorGPS50ReceivedInvoiceDetailModel detailEntity = new SincronizadorGPS50ReceivedInvoiceDetailModel(sageEnrichedModel, receivedInvoiceLine, SageConnectionManager, GestprojectConnectionManager);

                  GetDetailGuid(receivedInvoiceLine, detailEntity);

                  SynchronizadorGPS50ReceivedInvoicesDetails.Add(detailEntity);
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }

      public void GetDetailGuid
      (
         ewDocCompraLinFACTURA receivedInvoiceLine, 
         SincronizadorGPS50ReceivedInvoiceDetailModel detailEntity
      )
      {
         try
         {
            string sqlString = $@"
            SELECT
               GUID_ID
            FROM 
               {DB.SQLDatabase("gestion","d_albcom")}
            WHERE
               EMPRESA='{receivedInvoiceLine._Empresa}'
            AND
               DOC_NUM='{receivedInvoiceLine._Doc_Num}'
            AND
               NUMERO='{receivedInvoiceLine._Numero}'
            AND
               DEFINICION='{receivedInvoiceLine._Definicion}'
            ;";

            DataTable entiesDataTable = new DataTable();
            DB.SQLExec(sqlString, ref entiesDataTable);

            detailEntity.S50_GUID_ID = entiesDataTable.Rows[0].ItemArray[0].ToString().Trim();
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }
      

      public void AppendMissingSynchronizationData()
      {
         try
         {
            foreach (SincronizadorGP50ReceivedInvoiceModel entity in SynchronizadorGPS50ReceivedInvoices)
            {

               if(ValidateIfSynchornizationEntityExist(entity, "INT_SAGE_SINC_FACTURA_RECIBIDA") == true)
               {
                  Connection.Open();

                  string sqlString = $@"
                     SELECT
                        FCP_ID
                        ,SYNC_STATUS
                     FROM
                        {TableSchema.TableName}
                     WHERE
                        S50_GUID_ID=@S50_GUID_ID
                  ";

                  using(SqlCommand command = new SqlCommand(sqlString, Connection))
                  {
                     command.Parameters.Clear();
                     command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

                     using(SqlDataReader reader = command.ExecuteReader())
                     {
                        while(reader.Read())
                        {
                           entity.FCP_ID = reader["FCP_ID"] as int?;
                           entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                           break;
                        };
                     };
                  };

                  foreach(
                     SincronizadorGPS50ReceivedInvoiceDetailModel entityDetail in SynchronizadorGPS50ReceivedInvoicesDetails
                     .Where(entityDetail => entityDetail.INVOICE_GUID_ID == entity.S50_GUID_ID)
                  )
                  {
                     string sqlString2 = $@"
                        SELECT
                           DFP_ID
                        FROM
                           INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES
                        WHERE
                           S50_GUID_ID=S50_GUID_ID
                     ";

                     using(SqlCommand command = new SqlCommand(sqlString2, Connection))
                     {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@S50_GUID_ID", entityDetail.S50_GUID_ID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                           while(reader.Read())
                           {
                              entityDetail.DFP_ID = reader["DFP_ID"] as int?;
                              entityDetail.FCP_ID = entity.FCP_ID;
                              break;
                           };
                        };
                     };
                  };          

               Connection.Close();        
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public void RegisterSynchronizationData()
      {
         try
         {
            foreach(SincronizadorGP50ReceivedInvoiceModel entity in SynchronizadorGPS50ReceivedInvoices)
            {
               if(ValidateIfSynchornizationEntityExist(entity, "INT_SAGE_SINC_FACTURA_RECIBIDA") == false)
               {
                  RegisterSynchornizationEntity(entity, "INT_SAGE_SINC_FACTURA_RECIBIDA");
                  GetEntitySynchronizationId(entity, "INT_SAGE_SINC_FACTURA_RECIBIDA");

                  foreach(
                     SincronizadorGPS50ReceivedInvoiceDetailModel entityDetail in SynchronizadorGPS50ReceivedInvoicesDetails
                     .Where(entityDetail => entityDetail.INVOICE_GUID_ID == entity.S50_GUID_ID)
                  )
                  {
                     RegisterSynchornizationEntityDetail(entityDetail, "INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES");
                     GetEntityDetailSynchronizationId(entityDetail, "INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES");
                  };
               }
               else
               {
                  UpdateSynchornizationEntity(entity, "INT_SAGE_SINC_FACTURA_RECIBIDA");
                  GetEntitySynchronizationId(entity, "INT_SAGE_SINC_FACTURA_RECIBIDA");

                  //foreach(
                  //   SincronizadorGPS50ReceivedInvoiceDetailModel entityDetail in SynchronizadorGPS50ReceivedInvoicesDetails
                  //   .Where(entityDetail => entityDetail.INVOICE_GUID_ID == entity.S50_GUID_ID)
                  //)
                  //{
                  //   RegisterSynchornizationEntityDetail(entityDetail, "INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES");
                  //   GetEntityDetailSynchronizationId(entityDetail, "INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES");
                  //};



               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public bool ValidateIfSynchornizationEntityExist
      (
         SincronizadorGP50ReceivedInvoiceModel entity, 
         string tableName
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               ID
            FROM
               {tableName}
            WHERE
               S50_GUID_ID=@S50_GUID_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString,Connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID",entity.S50_GUID_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return true;
                  };
               };
            };

            return false;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public void RegisterSynchornizationEntity(SincronizadorGP50ReceivedInvoiceModel entity, string tableName)
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            INSERT INTO
               {tableName}
            (
               SYNC_STATUS
               ,FCP_ID
               ,PAR_DAO_ID
               ,FCP_NUM_FACTURA
               ,FCP_FECHA
               ,PAR_PRO_ID
               ,FCP_SUBCTA_CONTABLE
               ,FCP_BASE_IMPONIBLE
               ,FCP_VALOR_IVA
               ,FCP_IVA
               ,FCP_VALOR_IRPF
               ,FCP_IRPF
               ,FCP_TOTAL_FACTURA
               ,FCP_OBSERVACIONES
               ,PRY_ID
               ,FCP_EJERCICIO
               ,S50_GUID_ID
               ,S50_COMPANY_GROUP_NAME
               ,S50_COMPANY_GROUP_CODE
               ,S50_COMPANY_GROUP_MAIN_CODE
               ,S50_COMPANY_GROUP_GUID_ID
               ,GP_USU_ID
               ,COMMENTS
            )
            VALUES
            (
               @SYNC_STATUS
               ,@FCP_ID
               ,@PAR_DAO_ID
               ,@FCP_NUM_FACTURA
               ,@FCP_FECHA
               ,@PAR_PRO_ID
               ,@FCP_SUBCTA_CONTABLE
               ,@FCP_BASE_IMPONIBLE
               ,@FCP_VALOR_IVA
               ,@FCP_IVA
               ,@FCP_VALOR_IRPF
               ,@FCP_IRPF
               ,@FCP_TOTAL_FACTURA
               ,@FCP_OBSERVACIONES
               ,@PRY_ID
               ,@FCP_EJERCICIO
               ,@S50_GUID_ID
               ,@S50_COMPANY_GROUP_NAME
               ,@S50_COMPANY_GROUP_CODE
               ,@S50_COMPANY_GROUP_MAIN_CODE
               ,@S50_COMPANY_GROUP_GUID_ID
               ,@GP_USU_ID
               ,@COMMENTS
            )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString,Connection))
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.NoTransferido;

               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
               command.Parameters.AddWithValue("@FCP_ID", entity.FCP_ID);
               command.Parameters.AddWithValue("@PAR_DAO_ID", entity.PAR_DAO_ID);
               command.Parameters.AddWithValue("@FCP_NUM_FACTURA", entity.FCP_NUM_FACTURA);
               command.Parameters.AddWithValue("@FCP_FECHA", entity.FCP_FECHA);
               command.Parameters.AddWithValue("@PAR_PRO_ID", entity.PAR_PRO_ID);
               command.Parameters.AddWithValue("@FCP_SUBCTA_CONTABLE", entity.FCP_SUBCTA_CONTABLE);
               command.Parameters.AddWithValue("@FCP_BASE_IMPONIBLE", entity.FCP_BASE_IMPONIBLE);
               command.Parameters.AddWithValue("@FCP_VALOR_IVA", entity.FCP_VALOR_IVA);
               command.Parameters.AddWithValue("@FCP_IVA", entity.FCP_IVA);
               command.Parameters.AddWithValue("@FCP_VALOR_IRPF", entity.FCP_VALOR_IRPF);
               command.Parameters.AddWithValue("@FCP_IRPF", entity.FCP_IRPF);
               command.Parameters.AddWithValue("@FCP_TOTAL_FACTURA", entity.FCP_TOTAL_FACTURA);
               command.Parameters.AddWithValue("@FCP_OBSERVACIONES", entity.FCP_OBSERVACIONES);
               command.Parameters.AddWithValue("@PRY_ID", entity.PRY_ID);
               command.Parameters.AddWithValue("@FCP_EJERCICIO", entity.FCP_EJERCICIO);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_NAME", entity.S50_COMPANY_GROUP_NAME);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_CODE", entity.S50_COMPANY_GROUP_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_MAIN_CODE", entity.S50_COMPANY_GROUP_MAIN_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_GUID_ID", entity.S50_COMPANY_GROUP_GUID_ID);
               command.Parameters.AddWithValue("@GP_USU_ID", entity.GP_USU_ID);
               command.Parameters.AddWithValue("@COMMENTS", entity.COMMENTS);

               command.ExecuteNonQuery();
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }
      
      public void UpdateSynchornizationEntity(SincronizadorGP50ReceivedInvoiceModel entity, string tableName)
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            UPDATE
               {tableName}
            SET
               PAR_PRO_ID=@PAR_PRO_ID
            WHERE
               FCP_ID=@FCP_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString,Connection))
            {
               command.Parameters.AddWithValue("@FCP_ID", entity.FCP_ID);
               command.Parameters.AddWithValue("@PAR_PRO_ID", entity.PAR_PRO_ID);

               command.ExecuteNonQuery();
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public void GetEntitySynchronizationId
      (
         SincronizadorGP50ReceivedInvoiceModel entity, 
         string tableName
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               ID
            FROM
               {tableName}
            WHERE
               S50_GUID_ID=@S50_GUID_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString,Connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID",entity.S50_GUID_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.ID = reader["ID"] as int?;
                     break;
                  };
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public void RegisterSynchornizationEntityDetail
      (
         SincronizadorGPS50ReceivedInvoiceDetailModel entityDetail, string tableName
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            INSERT INTO
               {tableName}
            (
               SYNC_STATUS
               ,DFP_ID
               ,DFP_CONCEPTO
               ,DFP_PRECIO_UNIDAD
               ,DFP_UNIDADES
               ,DFP_SUBTOTAL
               ,PRY_ID
               ,FCP_ID
               ,DFP_ESTRUCTURAL
               ,INVOICE_GUID_ID
               ,S50_GUID_ID
               ,S50_COMPANY_GROUP_NAME
               ,S50_COMPANY_GROUP_CODE
               ,S50_COMPANY_GROUP_MAIN_CODE
               ,S50_COMPANY_GROUP_GUID_ID
               ,GP_USU_ID
               ,COMMENTS
            )
            VALUES
            (
               @SYNC_STATUS
               ,@DFP_ID
               ,@DFP_CONCEPTO
               ,@DFP_PRECIO_UNIDAD
               ,@DFP_UNIDADES
               ,@DFP_SUBTOTAL
               ,@PRY_ID
               ,@FCP_ID
               ,@DFP_ESTRUCTURAL
               ,@INVOICE_GUID_ID
               ,@S50_GUID_ID
               ,@S50_COMPANY_GROUP_NAME
               ,@S50_COMPANY_GROUP_CODE
               ,@S50_COMPANY_GROUP_MAIN_CODE
               ,@S50_COMPANY_GROUP_GUID_ID
               ,@GP_USU_ID
               ,@COMMENTS
            )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString,Connection))
            {
               command.Parameters.AddWithValue("@SYNC_STATUS", entityDetail.SYNC_STATUS);
               command.Parameters.AddWithValue("@DFP_ID", entityDetail.DFP_ID);
               command.Parameters.AddWithValue("@DFP_CONCEPTO", entityDetail.DFP_CONCEPTO);
               command.Parameters.AddWithValue("@DFP_PRECIO_UNIDAD", entityDetail.DFP_PRECIO_UNIDAD);
               command.Parameters.AddWithValue("@DFP_UNIDADES", entityDetail.DFP_UNIDADES);
               command.Parameters.AddWithValue("@DFP_SUBTOTAL", entityDetail.DFP_SUBTOTAL);
               command.Parameters.AddWithValue("@PRY_ID", entityDetail.PRY_ID);
               command.Parameters.AddWithValue("@FCP_ID", entityDetail.FCP_ID);
               command.Parameters.AddWithValue("@DFP_ESTRUCTURAL", entityDetail.DFP_ESTRUCTURAL);
               command.Parameters.AddWithValue("@INVOICE_GUID_ID", entityDetail.INVOICE_GUID_ID);
               command.Parameters.AddWithValue("@S50_GUID_ID", entityDetail.S50_GUID_ID);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_NAME", entityDetail.S50_COMPANY_GROUP_NAME);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_CODE", entityDetail.S50_COMPANY_GROUP_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_MAIN_CODE", entityDetail.S50_COMPANY_GROUP_MAIN_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_GUID_ID", entityDetail.S50_COMPANY_GROUP_GUID_ID);
               command.Parameters.AddWithValue("@GP_USU_ID", entityDetail.GP_USU_ID);
               command.Parameters.AddWithValue("@COMMENTS", entityDetail.COMMENTS);

               command.ExecuteNonQuery();
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public void GetEntityDetailSynchronizationId(SincronizadorGPS50ReceivedInvoiceDetailModel entityDetail, string tableName)
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               ID
            FROM
               {tableName}
            WHERE
               S50_GUID_ID=@S50_GUID_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString,Connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID",entityDetail.S50_GUID_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entityDetail.ID = reader["ID"] as int?;
                     break;
                  };
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public void CreateAndDefineDataSource()
      {                  
         try
         {
            IDataTableGenerator entityDataTableGenerator = new SyncrhonizationDataTableGenerator();
            DataTable = entityDataTableGenerator.CreateDataTable(TableSchema.ColumnsTuplesList);
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public void PaintEntitiesOnDataSource(dynamic finalEntity)
      {         
         try
         {
            if(finalEntity.Count > 0)
            {
               ISynchronizableEntityPainter<SincronizadorGP50ReceivedInvoiceModel> entityPainter = new EntityPainter<SincronizadorGP50ReceivedInvoiceModel>();
               entityPainter.PaintEntityListOnDataTable(
                  finalEntity,
                  DataTable,
                  TableSchema.ColumnsTuplesList
               );
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
   }
}