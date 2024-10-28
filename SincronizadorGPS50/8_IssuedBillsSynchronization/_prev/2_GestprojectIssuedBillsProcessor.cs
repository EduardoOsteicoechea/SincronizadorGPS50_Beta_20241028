using SincronizadorGPS50.Workflows.Sage50Connection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GestprojectIssuedBillsProcessor : ISynchronizableEntityProcessor<SynchronizableIssuedInvoiceModel, Sage50IssuedBillModel>
   {
      public List<SynchronizableIssuedInvoiceModel> ProcessedEntities { get; set; }
      public bool MustBeRegistered { get; set; } = false;
      public bool MustBeSkipped { get; set; } = false;
      public bool MustBeUpdated { get; set; } = false;
      public bool MustBeDeleted { get; set; } = false;
      public bool NevesWasSynchronized { get; set; } = false;

      public List<SynchronizableIssuedInvoiceModel> ProcessEntityList
      (
         System.Data.SqlClient.SqlConnection connection,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<SynchronizableIssuedInvoiceModel> gestprojectEntites,
         List<Sage50IssuedBillModel> sage50Entities
      )
      {
         try
         {
            ProcessedEntities = new List<SynchronizableIssuedInvoiceModel>();

            for(int i = 0; i < gestprojectEntites.Count; i++)
            {
               SynchronizableIssuedInvoiceModel entity = gestprojectEntites[i];

               AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
               DetermineEntityWorkflow(connection, sage50ConnectionManager, tableSchema, entity);

               if(MustBeSkipped)
               {
                  //MessageBox.Show(entity.FCE_ID + " MustBeSkipped");
                  continue;
               }
               else if(MustBeRegistered)
               {
                  //MessageBox.Show(entity.FCE_ID + " MustBeRegistered");
                  RegisterEntity(connection, tableSchema, entity);
               }
               else if(MustBeUpdated)
               {
                  //MessageBox.Show(entity.FCE_ID + " MustBeUpdated");
                  UpdateEntity(connection, tableSchema, entity);
               };

               ValidateEntitySynchronizationStatus(connection, tableSchema, sage50Entities, entity);

               if(MustBeDeleted)
               {
                  DeleteEntity(connection, tableSchema, gestprojectEntites, entity);
                  RegisterEntity(connection, tableSchema, entity);
               };

               UpdateEntity(connection, tableSchema, entity);

               ProcessedEntities.Add(entity);
            };

            return ProcessedEntities;
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


      public void AppendSynchronizationTableDataToEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, SynchronizableIssuedInvoiceModel entity)
      {
         try
         {
            new EntitySynchronizationTable<SynchronizableIssuedInvoiceModel>().AppendTableDataToEntity
            (
               connection,
               tableSchema.TableName,
               tableSchema.SynchronizationFieldsTupleList,
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID),
               entity,               
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCE_ID)
            );
            
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach(var item in entity.GetType().GetProperties())
            //{
            //   stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
            //};
            //MessageBox.Show(stringBuilder.ToString());
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
      public void DetermineEntityWorkflow(SqlConnection connection, ISage50ConnectionManager sage50ConnectionManager, ISynchronizationTableSchemaProvider tableSchema, SynchronizableIssuedInvoiceModel entity)
      {
         try
         {
            MustBeRegistered = !new WasIssuedBillRegistered(
               connection,
               tableSchema.TableName,
               tableSchema.GestprojectId.ColumnDatabaseName,
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCE_ID),
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
            ).ItWas;

            bool registeredInDifferentCompanyGroup =
            entity.S50_COMPANY_GROUP_GUID_ID != ""
            &&
            sage50ConnectionManager.CompanyGroupData.CompanyGuidId != entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeSkipped = registeredInDifferentCompanyGroup;

            bool neverSynchronized = entity.S50_COMPANY_GROUP_GUID_ID == "";
            NevesWasSynchronized = neverSynchronized;

            bool synchronizedInThePast =
            entity.S50_COMPANY_GROUP_GUID_ID != ""
            &&
            sage50ConnectionManager.CompanyGroupData.CompanyGuidId == entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeUpdated = neverSynchronized || synchronizedInThePast;

            //MessageBox.Show(
            //   "MustBeRegistered: " + MustBeRegistered + "\n" +
            //   "MustBeSkipped: " + MustBeSkipped + "\n" +
            //   "MustBeUpdated: " + MustBeUpdated + "\n"
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
      public void RegisterEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, SynchronizableIssuedInvoiceModel entity)
      {
         try
         {
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach(var item in entity.GetType().GetProperties())
            //{
            //   stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
            //};
            //MessageBox.Show(stringBuilder.ToString());

            new RegisterEntity
            (
               connection,
               tableSchema.TableName,
               new List<(string, dynamic)>()
               {
                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, SynchronizationStatusOptions.Desincronizado),
                  (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCE_ID),
                  (tableSchema.GestprojectDaoId.ColumnDatabaseName, entity.PAR_DAO_ID),
                  (tableSchema.GestprojectReference.ColumnDatabaseName, entity.FCE_REFERENCIA),
                  (tableSchema.GestprojectDate.ColumnDatabaseName, entity.FCE_FECHA),
                  (tableSchema.GestprojectCliId.ColumnDatabaseName, entity.PAR_CLI_ID),
                  (tableSchema.GestprojectTaxableBase.ColumnDatabaseName, entity.FCE_BASE_IMPONIBLE),
                  (tableSchema.GestprojectIvaValue.ColumnDatabaseName, entity.FCE_VALOR_IVA),
                  (tableSchema.GestprojectIvaValueInEuros.ColumnDatabaseName, entity.FCE_IVA),
                  (tableSchema.GestprojectIrpfValue.ColumnDatabaseName, entity.FCE_VALOR_IRPF),
                  (tableSchema.GestprojectIrpfValueInEuros.ColumnDatabaseName, entity.FCE_IRPF),
                  (tableSchema.GestprojectTotalInvoiced.ColumnDatabaseName, entity.FCE_TOTAL_SUPLIDO),
                  (tableSchema.GestprojectBillTotal.ColumnDatabaseName, entity.FCE_TOTAL_FACTURA),
                  (tableSchema.GestprojectBillObservations.ColumnDatabaseName, entity.FCE_OBSERVACIONES)
               }
            );

            //StringBuilder stringBuilder = new StringBuilder();
            //foreach(var item in entity.GetType().GetProperties())
            //{
            //   stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
            //};
            //MessageBox.Show(stringBuilder.ToString());

            AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
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
      public void UpdateEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, SynchronizableIssuedInvoiceModel entity)
      {
         try
         {
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach(var item in entity.GetType().GetProperties())
            //{
            //   stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
            //}
            //MessageBox.Show(stringBuilder.ToString());

            new UpdateEntity
            (
               connection,
               tableSchema.TableName,
               new List<(string, dynamic)>()
               {
                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, entity.SYNC_STATUS),
                  (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCE_ID),
                  (tableSchema.GestprojectDaoId.ColumnDatabaseName, entity.PAR_DAO_ID),
                  (tableSchema.GestprojectReference.ColumnDatabaseName, entity.FCE_REFERENCIA),
                  (tableSchema.GestprojectDate.ColumnDatabaseName, entity.FCE_FECHA),
                  (tableSchema.GestprojectCliId.ColumnDatabaseName, entity.PAR_CLI_ID),
                  (tableSchema.GestprojectTaxableBase.ColumnDatabaseName, entity.FCE_BASE_IMPONIBLE),
                  (tableSchema.GestprojectIvaValue.ColumnDatabaseName, entity.FCE_VALOR_IVA),
                  (tableSchema.GestprojectIvaValueInEuros.ColumnDatabaseName, entity.FCE_IVA),
                  (tableSchema.GestprojectIrpfValue.ColumnDatabaseName, entity.FCE_VALOR_IRPF),
                  (tableSchema.GestprojectIrpfValueInEuros.ColumnDatabaseName, entity.FCE_IRPF),
                  (tableSchema.GestprojectTotalInvoiced.ColumnDatabaseName, entity.FCE_TOTAL_SUPLIDO),
                  (tableSchema.GestprojectBillTotal.ColumnDatabaseName, entity.FCE_TOTAL_FACTURA),
                  (tableSchema.GestprojectBillObservations.ColumnDatabaseName, entity.FCE_OBSERVACIONES)
               },
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCE_ID),
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
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
         };
      }
      public void ValidateEntitySynchronizationStatus(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<Sage50IssuedBillModel> sage50Entities, SynchronizableIssuedInvoiceModel entity)
      {
         try
         {
            //ValidateIssuedBillSyncronizationStatus ProviderSyncronizationStatusValidator = new ValidateIssuedBillSyncronizationStatus(
            //   entity,
            //   sage50Entities,
            //   (tableSchema.GestprojectDaoId.ColumnDatabaseName),
            //   (tableSchema.GestprojectReference.ColumnDatabaseName),
            //   (tableSchema.GestprojectDate.ColumnDatabaseName),
            //   (tableSchema.GestprojectCliId.ColumnDatabaseName),
            //   (tableSchema.GestprojectTaxableBase.ColumnDatabaseName),
            //   (tableSchema.GestprojectBillTotal.ColumnDatabaseName),
            //   (tableSchema.GestprojectBillObservations.ColumnDatabaseName),
            //   NevesWasSynchronized
            //);

            //MustBeDeleted = ProviderSyncronizationStatusValidator.MustBeDeleted;
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
      public void DeleteEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<SynchronizableIssuedInvoiceModel> gestprojectEntites, SynchronizableIssuedInvoiceModel entity)
      {
         try
         {
            //new DeleteEntityFromSynchronizationTable(
            //   connection,
            //   tableSchema.TableName,
            //   (tableSchema.GestprojectId.ColumnDatabaseName, entity.IMP_ID),
            //   (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
            //);

            //new ClearEntityDataInGestproject(
            //      connection,
            //      "IMPUESTO_CONFIG",
            //      new List<string>(){
            //      tableSchema.AccountableSubaccount.ColumnDatabaseName
            //   },
            //   (tableSchema.GestprojectId.ColumnDatabaseName, entity.IMP_ID),
            //   (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
            //);

            ClearEntitySynchronizationData(entity, tableSchema.SynchronizationFieldsDefaultValuesTupleList);
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

      public void ClearEntitySynchronizationData(SynchronizableIssuedInvoiceModel entity, List<(string propertyName, dynamic defaultValue)> entityPropertiesValuesTupleList)
      {
         try
         {
            for(global::System.Int32 i = 0; i < entityPropertiesValuesTupleList.Count; i++)
            {
               typeof(SynchronizableIssuedInvoiceModel).GetProperty(entityPropertiesValuesTupleList[i].propertyName).SetValue(entity, entityPropertiesValuesTupleList[i].defaultValue);
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