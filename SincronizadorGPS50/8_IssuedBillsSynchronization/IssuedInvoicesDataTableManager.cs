using Infragistics.Designers.SqlEditor;
using sage.ew.docventatpv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static sage.ew.docsven.FirmaElectronica;

namespace SincronizadorGPS50
{
   public class IssuedInvoicesDataTableManager : IGridDataSourceGenerator<SynchronizableIssuedInvoiceModel, Sage50IssuedBillModel>
   {
      public string GestprojectEntityTable { get; set; } = "FACTURA_EMITIDA";
      public string GestprojectEntityDetailsTable { get; set; } = "DETALLE_FACTURA_EMITIDA";
      public string SynchornizableEntityDetailsTable { get; set; } = "INT_SAGE_SINC_FACTURA_EMITIDA_DETALLES";
      public List<SynchronizableIssuedInvoiceModel> SynchronizableEntities { get; set; }
      public List<SynchronizableIssuedInvoiceDetailModel> SynchronizableEntitiesDetails { get; set; }
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public DataTable DataTable { get; set; }
      public ewDocVentaTPV Document {get;set;}

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
            SynchronizableEntities = new List<SynchronizableIssuedInvoiceModel>();
            SynchronizableEntitiesDetails = new List<SynchronizableIssuedInvoiceDetailModel>();
            Document = new ewDocVentaTPV();

            ManageEntitySynchronizationTableStatus(TableSchema);
            ManageEntityDetailsSynchronizationTableStatus(new IssuedInvoicesDetailsSynchronizationTableSchemaProvider());

            GetGestprojectIssuedBills();

            foreach(SynchronizableIssuedInvoiceModel entity in SynchronizableEntities)
            {
               if(ValidateIfEntityWasRegistered(entity) == false)
               {
                  GetSynchronizableIssuedBillClientSubaccountableAccount();
                  GetSynchronizableIssuedBillSageCompanyNumber();
                  GetSynchronizableIssuedBillSageTaxCode();
                  GetGestprojectIssuedBillsDetails();
                  RegisterEntityOnSynchronizationTable();
                  RegisterEntityDetailsOnSynchronizationTable();
                  this.AppendSynchronizationTableDataToEntity(entity);
               }
               else
               {
                  this.AppendSynchronizationTableDataToEntity(entity);
               }
            }

            CreateAndDefineDataSource();
            PaintEntitiesOnDataSource();

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

      public void ManageEntitySynchronizationTableStatus(ISynchronizationTableSchemaProvider tableSchemaProvider)
      {
         ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

         bool tableExists = entitySyncronizationTableStatusManager.TableExists(
            GestprojectConnectionManager.GestprojectSqlConnection,
            tableSchemaProvider.TableName
         );

         if(tableExists == false)
         {
            entitySyncronizationTableStatusManager.CreateTable
            (
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchemaProvider
            );
         };
      }

      public void ManageEntityDetailsSynchronizationTableStatus(ISynchronizationTableSchemaProvider tableSchemaProvider)
      {
         ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

         bool tableExists = entitySyncronizationTableStatusManager.TableExists(
            GestprojectConnectionManager.GestprojectSqlConnection,
            tableSchemaProvider.TableName
         );

         if(tableExists == false)
         {
            entitySyncronizationTableStatusManager.CreateTable
            (
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchemaProvider
            );
         };
      }

      public void GetGestprojectIssuedBills()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               FCE_ID
               ,FCE_FECHA
               ,PAR_DAO_ID
               ,PAR_CLI_ID
               ,FCE_IVA_IGIC
               ,FCE_SUBCTA_CONTABLE
            FROM
               {GestprojectEntityTable}
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     SynchronizableIssuedInvoiceModel entity = new SynchronizableIssuedInvoiceModel();
                     entity.FCE_ID = (reader["FCE_ID"] as int?) ?? -1;
                     entity.FCE_FECHA = (reader["FCE_FECHA"] as DateTime?) ?? DateTime.Now;
                     entity.PAR_DAO_ID = (reader["PAR_DAO_ID"] as int?) ?? -1;
                     entity.PAR_CLI_ID = (reader["PAR_CLI_ID"] as int?) ?? -1;
                     entity.FCE_IVA_IGIC = (reader["FCE_IVA_IGIC"] as string) ?? "";
                     entity.FCE_SUBCTA_CONTABLE = (reader["FCE_SUBCTA_CONTABLE"] as string) ?? "";

                     SynchronizableEntities.Add(entity);
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


      public void GetSynchronizableIssuedBillClientSubaccountableAccount()
      {
         try
         {
            Connection.Open();

            foreach(SynchronizableIssuedInvoiceModel entity in SynchronizableEntities)
            {
               string sqlString = $@"
               SELECT
                  PAR_SUBCTA_CONTABLE
               FROM
                  INT_SAGE_SINC_CLIENTES
               WHERE
                  PAR_ID=@PAR_ID
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  command.Parameters.AddWithValue("@PAR_ID", entity.PAR_CLI_ID);

                  using(SqlDataReader reader = command.ExecuteReader())
                  {
                     while(reader.Read())
                     {
                        entity.PAR_SUBCTA_CONTABLE = (reader["PAR_SUBCTA_CONTABLE"] as string) ?? "";
                     };
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


      public void GetSynchronizableIssuedBillSageCompanyNumber()
      {
         try
         {
            Connection.Open();

            foreach(SynchronizableIssuedInvoiceModel entity in SynchronizableEntities)
            {
               string sqlString = $@"
               SELECT
                  SageCompanyNumber
               FROM
                  INT_SAGE_SINC_EMPRESAS
               WHERE
                  PAR_ID=@PAR_ID
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  command.Parameters.AddWithValue("@PAR_ID", entity.PAR_DAO_ID);

                  using(SqlDataReader reader = command.ExecuteReader())
                  {
                     while(reader.Read())
                     {
                        entity.SageCompanyNumber = (reader["SageCompanyNumber"] as string) ?? "";
                     };
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


      public void GetSynchronizableIssuedBillSageTaxCode()
      {
         try
         {
            Connection.Open();

            foreach(SynchronizableIssuedInvoiceModel entity in SynchronizableEntities)
            {
               string sqlString = $@"
               SELECT
                  S50_CODE
               FROM
                  INT_SAGE_SINC_IMPUESTOS
               WHERE
                  IMP_NOMBRE=@IMP_NOMBRE
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  command.Parameters.AddWithValue("@IMP_NOMBRE", entity.FCE_IVA_IGIC);

                  using(SqlDataReader reader = command.ExecuteReader())
                  {
                     while(reader.Read())
                     {
                        entity.TaxCode = (reader["S50_CODE"] as string) ?? "";
                     };
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


      public void GetGestprojectIssuedBillsDetails()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               DFE_CONCEPTO
               ,DFE_ID
               ,DFE_PRECIO_UNIDAD
               ,DFE_UNIDADES
               ,DFE_SUBTOTAL
               ,PRY_ID
               ,FCE_ID
               ,DFE_SUBTOTAL_BASE
            FROM
               {GestprojectEntityDetailsTable}
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     SynchronizableIssuedInvoiceDetailModel entity = new SynchronizableIssuedInvoiceDetailModel();

                     entity.DFE_CONCEPTO = reader["DFE_CONCEPTO"] as string;
                     entity.DFE_ID = reader["DFE_ID"] as int?;
                     entity.DFE_PRECIO_UNIDAD = reader["DFE_PRECIO_UNIDAD"] as decimal?;
                     entity.DFE_UNIDADES = reader["DFE_UNIDADES"] as decimal?;
                     entity.DFE_SUBTOTAL = reader["DFE_SUBTOTAL"] as decimal?;
                     entity.PRY_ID = reader["PRY_ID"] as int?;
                     entity.FCE_ID = reader["FCE_ID"] as int?;
                     entity.DFE_SUBTOTAL_BASE = reader["DFE_SUBTOTAL_BASE"] as decimal?;

                     SynchronizableEntitiesDetails.Add(entity);
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


      public void GetSynchronizableIssuedBillSageProjectCode()
      {
         try
         {
            Connection.Open();

            foreach(SynchronizableIssuedInvoiceModel invoice in SynchronizableEntities)
            {
               List<SynchronizableIssuedInvoiceDetailModel> invoiceDetails = 
               SynchronizableEntitiesDetails
               .Where(detail => detail.FCE_ID == invoice.FCE_ID).ToList();

               string sqlString = $@"
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  using(SqlDataReader reader = command.ExecuteReader())
                  {
                     while(reader.Read())
                     {

                     };
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
         }
      }


      public void RegisterEntityOnSynchronizationTable()
      {
         try
         {
            Connection.Open();

            foreach ( SynchronizableIssuedInvoiceModel entity in SynchronizableEntities )
            {
               string sqlString = $@"
               IF NOT EXISTS (SELECT 1 FROM {TableSchema.TableName} WHERE FCE_ID = @FCE_ID)
                  BEGIN
                     INSERT INTO
                        {TableSchema.TableName}
                     (
                        SYNC_STATUS
                        ,FCE_ID
                        ,PAR_DAO_ID
                        ,FCE_REFERENCIA
                        ,FCE_FECHA
                        ,PAR_CLI_ID
                        ,FCE_BASE_IMPONIBLE
                        ,FCE_VALOR_IVA
                        ,FCE_IVA
                        ,FCE_VALOR_IRPF
                        ,FCE_IRPF
                        ,FCE_TOTAL_SUPLIDO
                        ,FCE_TOTAL_FACTURA
                        ,FCE_OBSERVACIONES
                        ,FCE_IVA_IGIC
                        ,PAR_SUBCTA_CONTABLE
                        ,SageCompanyNumber
                        ,TaxCode
                        ,FCE_SUBCTA_CONTABLE
                     )
                     VALUES
                     (
                        @SYNC_STATUS
                        ,@FCE_ID
                        ,@PAR_DAO_ID
                        ,@FCE_REFERENCIA
                        ,@FCE_FECHA
                        ,@PAR_CLI_ID
                        ,@FCE_BASE_IMPONIBLE
                        ,@FCE_VALOR_IVA
                        ,@FCE_IVA
                        ,@FCE_VALOR_IRPF
                        ,@FCE_IRPF
                        ,@FCE_TOTAL_SUPLIDO
                        ,@FCE_TOTAL_FACTURA
                        ,@FCE_OBSERVACIONES
                        ,@FCE_IVA_IGIC
                        ,@PAR_SUBCTA_CONTABLE
                        ,@SageCompanyNumber
                        ,@TaxCode
                        ,@FCE_SUBCTA_CONTABLE
                     )
                  END
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  entity.SYNC_STATUS = SynchronizationStatusOptions.NoTransferido;

                  command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
                  command.Parameters.AddWithValue("@FCE_ID", entity.FCE_ID ?? -1);
                  command.Parameters.AddWithValue("@PAR_DAO_ID", entity.PAR_DAO_ID ?? -1);
                  command.Parameters.AddWithValue("@FCE_REFERENCIA", entity.FCE_REFERENCIA ?? "");
                  command.Parameters.AddWithValue("@FCE_FECHA", entity.FCE_FECHA ?? DateTime.Now);
                  command.Parameters.AddWithValue("@PAR_CLI_ID", entity.PAR_CLI_ID ?? -1);
                  command.Parameters.AddWithValue("@FCE_BASE_IMPONIBLE", entity.FCE_BASE_IMPONIBLE ?? 0);
                  command.Parameters.AddWithValue("@FCE_VALOR_IVA", entity.FCE_VALOR_IVA ?? 0);
                  command.Parameters.AddWithValue("@FCE_IVA", entity.FCE_IVA ?? 0);
                  command.Parameters.AddWithValue("@FCE_VALOR_IRPF", entity.FCE_VALOR_IRPF ?? 0);
                  command.Parameters.AddWithValue("@FCE_IRPF", entity.FCE_IRPF ?? 0);
                  command.Parameters.AddWithValue("@FCE_TOTAL_SUPLIDO", entity.FCE_TOTAL_SUPLIDO ?? 0);
                  command.Parameters.AddWithValue("@FCE_TOTAL_FACTURA", entity.FCE_TOTAL_FACTURA ?? 0);
                  command.Parameters.AddWithValue("@FCE_OBSERVACIONES", entity.FCE_OBSERVACIONES ?? "");
                  command.Parameters.AddWithValue("@FCE_IVA_IGIC", entity.FCE_IVA_IGIC ?? "");
                  command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE", entity.PAR_SUBCTA_CONTABLE ?? "");
                  command.Parameters.AddWithValue("@SageCompanyNumber", entity.SageCompanyNumber ?? "");
                  command.Parameters.AddWithValue("@TaxCode", entity.TaxCode ?? "");
                  command.Parameters.AddWithValue("@FCE_SUBCTA_CONTABLE", entity.FCE_SUBCTA_CONTABLE ?? "");

                  command.ExecuteNonQuery();
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
         }
      }


      public void RegisterEntityDetailsOnSynchronizationTable()
      {
         try
         {
            Connection.Open();

            foreach ( SynchronizableIssuedInvoiceDetailModel entity in SynchronizableEntitiesDetails )
            {
               string sqlString = $@"
               IF NOT EXISTS (SELECT 1 FROM {SynchornizableEntityDetailsTable} WHERE DFE_ID = @DFE_ID)
                  BEGIN
                     INSERT INTO
                        {SynchornizableEntityDetailsTable}
                     (  
                        SYNC_STATUS
                        ,DFE_ID
                        ,DFE_CONCEPTO
                        ,DFE_PRECIO_UNIDAD
                        ,DFE_UNIDADES
                        ,DFE_SUBTOTAL
                        ,PRY_ID
                        ,FCE_ID
                        ,DFE_SUBTOTAL_BASE
                     )
                     VALUES
                     (
                        @SYNC_STATUS
                        ,@DFE_ID
                        ,@DFE_CONCEPTO
                        ,@DFE_PRECIO_UNIDAD
                        ,@DFE_UNIDADES
                        ,@DFE_SUBTOTAL
                        ,@PRY_ID
                        ,@FCE_ID
                        ,@DFE_SUBTOTAL_BASE
                     )
                  END
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  entity.SYNC_STATUS = SynchronizationStatusOptions.NoTransferido;

                  command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS ?? "");
                  command.Parameters.AddWithValue("@DFE_ID", entity.DFE_ID ?? -1);
                  command.Parameters.AddWithValue("@DFE_CONCEPTO", entity.DFE_CONCEPTO ?? "");
                  command.Parameters.AddWithValue("@DFE_PRECIO_UNIDAD", entity.DFE_PRECIO_UNIDAD ?? 0);
                  command.Parameters.AddWithValue("@DFE_UNIDADES", entity.DFE_UNIDADES ?? 0);
                  command.Parameters.AddWithValue("@DFE_SUBTOTAL", entity.DFE_SUBTOTAL ?? 0);
                  command.Parameters.AddWithValue("@PRY_ID", entity.PRY_ID ?? -1);
                  command.Parameters.AddWithValue("@FCE_ID", entity.FCE_ID ?? -1);
                  command.Parameters.AddWithValue("@DFE_SUBTOTAL_BASE", entity.DFE_SUBTOTAL_BASE ?? 0);

                  command.ExecuteNonQuery();
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
         }
      }


      public bool ValidateIfEntityWasRegistered
      (
         SynchronizableIssuedInvoiceModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            IF EXISTS (SELECT 1 FROM {TableSchema.TableName})
            BEGIN
               SELECT 
                  ID
               FROM
                  {TableSchema.TableName}
               WHERE
                  FCE_ID=@FCE_ID
            END
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@FCE_ID", entity.FCE_ID);

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
         }
      }


      public void AppendSynchronizationTableDataToEntity
      (
         SynchronizableIssuedInvoiceModel entity
      )
      {
         try
         {
            Connection.Open();
         
            string sqlString = $@"
            IF EXISTS (SELECT 1 FROM {TableSchema.TableName})
            BEGIN
               SELECT
                  ID
                  ,SYNC_STATUS
                  ,S50_CODE
                  ,S50_GUID_ID
                  ,S50_COMPANY_GROUP_NAME
                  ,S50_COMPANY_GROUP_CODE
                  ,S50_COMPANY_GROUP_MAIN_CODE
                  ,S50_COMPANY_GROUP_GUID_ID
                  ,LAST_UPDATE
                  ,GP_USU_ID
                  ,COMMENTS
               FROM
                  {TableSchema.TableName}
               WHERE
                  FCE_ID=@FCE_ID
            END
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@FCE_ID",entity.FCE_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = (reader["SYNC_STATUS"] as string) ?? "";
                     entity.S50_CODE = (reader["S50_CODE"] as string) ?? "";
                     entity.S50_GUID_ID = (reader["S50_GUID_ID"] as string) ?? "";
                     entity.S50_COMPANY_GROUP_NAME = (reader["S50_COMPANY_GROUP_NAME"] as string) ?? "";
                     entity.S50_COMPANY_GROUP_CODE = (reader["S50_COMPANY_GROUP_CODE"] as string) ?? "";
                     entity.S50_COMPANY_GROUP_MAIN_CODE = (reader["S50_COMPANY_GROUP_MAIN_CODE"] as string) ?? "";
                     entity.S50_COMPANY_GROUP_GUID_ID = (reader["S50_COMPANY_GROUP_GUID_ID"] as string) ?? "";
                     entity.LAST_UPDATE = (reader["LAST_UPDATE"] as DateTime?) ?? DateTime.Now;
                     entity.GP_USU_ID = (reader["GP_USU_ID"] as int?)  ?? -1;
                     entity.COMMENTS = (reader["COMMENTS"] as string) ?? "";
                  }
               }
            }
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
         }
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
         }
      }

      public void PaintEntitiesOnDataSource()
      {
         try
         {
            ISynchronizableEntityPainter<SynchronizableIssuedInvoiceModel> entityPainter = new EntityPainter<SynchronizableIssuedInvoiceModel>();
            entityPainter.PaintEntityListOnDataTable(
               SynchronizableEntities,
               DataTable,
               TableSchema.ColumnsTuplesList
            );
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
   }
}
